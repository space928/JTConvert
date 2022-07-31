﻿using System.Text;
using System.Runtime.CompilerServices;
using Joveler.Compression.ZLib;
using Joveler.Compression.XZ;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace JTConvert.JTCodec
{
    public class JTLoader : ICADLoader
    {
        private static Stopwatch stopwatch;
        // Only populated if we load the whole file into memory
        private byte[] loadedFile;

        public void LoadFile(JTConvertSettings settings)
        {
            Logger.Log($"Loading {settings.inputFile}");
            stopwatch = Stopwatch.StartNew();

            Stream fileStream;
            if(settings.loadWholeFile)
            {
                Logger.Log("Reading whole file into memory...");
                loadedFile = File.ReadAllBytes(settings.inputFile);
                fileStream = new MemoryStream(loadedFile, false);
            } else
            {
                fileStream = File.OpenRead(settings.inputFile);
            }
            using BinaryJTReader reader = new(fileStream);

            var jtFile = new JTFile();
            jtFile.version = reader.ReadString(80);
            if (!jtFile.version.StartsWith("Version "))
            {
                Logger.Log($"Malformed JT header: {jtFile.version}!", Logger.VerbosityLevel.ERROR);
                return;
            }
            ExtractVersionNumber(jtFile);
            jtFile.byteOrder = reader.ReadByte() == 1;
            reader.BigEndian = jtFile.byteOrder;
            jtFile.emptyField = reader.ReadInt32();
            Logger.Log($"Version: {jtFile.version.Trim()} length: {reader.BaseStream.Length} byteOrder: {(jtFile.byteOrder ? "BE" : "LE")}");
            Logger.Log($"Loading TOC...");
            long tocOffset;
            if (jtFile.versionMajor >= 10)
                tocOffset = reader.ReadInt64();
            else
                tocOffset = reader.ReadInt32();
            jtFile.toc = LoadTOC(reader, tocOffset, jtFile.versionMajor, settings);
            jtFile.lsgSegmentID = reader.ReadGUID();
            jtFile.lsgSegment = jtFile.toc[jtFile.lsgSegmentID];
        }

        private static void ExtractVersionNumber(JTFile jtFile)
        {
            // The version string allways starts with "Version " so we ignore those characters.
            int vNumEnd;
            for (vNumEnd = 8; vNumEnd < 20; vNumEnd++)
                if (!char.IsDigit(jtFile.version[vNumEnd]))
                    break;
            jtFile.versionMajor = int.Parse(jtFile.version[8..vNumEnd]);
        }

        private Dictionary<GUID, JTSegment> LoadTOC(BinaryJTReader reader, long offset, int jtVersion, JTConvertSettings settings)
        {
            long lastPos = reader.BaseStream.Position;
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            int tocCount = reader.ReadInt32();
            // Double the capacity to account for sparse dictionary packing.
            var toc = new Dictionary<GUID, JTSegment>(tocCount * 2);
            var segments = new JTTOCEntry[tocCount];

            //TODO: Parallel?
            for (int i = 0; i < tocCount; i++)
            {
                JTTOCEntry segment = LoadTOCEntry(reader, jtVersion);

                segments[i] = segment;
                toc.Add(segment.guid, null);
            }

            Logger.Log($"Loaded {tocCount} segment entries in {stopwatch.Elapsed.TotalMilliseconds} ms.");
            Logger.Log("Loading segment data...");
            stopwatch.Restart();

            // Just for logging failures
            ConcurrentDictionary<JTSegmentType, int> unimplementedSegments = new();

            // Deffer segment loading until after TOC reading to reduce file seeking.
            if(settings.parallel)
            {
                // TODO: Profile and add partitioning if needed.
                // TODO: Add task local state for MemoryStream/BinaryJTReader
                // https://docs.microsoft.com/en-us/previous-versions/msp-n-p/ff963552(v=pandp.10)#using-task-local-state-in-a-loop-body
               Parallel.ForEach(Partitioner.Create(segments),
                    new ParallelOptions(),
                    () =>
                    {
                        var ms = new MemoryStream(loadedFile);
                        BinaryJTReader nreader = new(ms, reader.BigEndian);
                        return nreader;
                    },
                    (segment, loopState, nreader) =>
                {
                    var seg = LoadSegment(nreader, segment, jtVersion);
                    toc[segment.guid] = seg;
                    if (seg == null)
                    {
                        var typ = (JTSegmentType)(segment.attributes >> 24);
                        unimplementedSegments.AddOrUpdate(typ, 1, (k, v) => v + 1);
                    }
                    return nreader;
                },
                (nreader) => nreader.Dispose());
            }
            else
            {
                foreach(var segment in segments)
                { 
                    var seg = LoadSegment(reader, segment, jtVersion); ;
                    toc[segment.guid] = seg;
                    if (seg == null)
                    {
                        var typ = (JTSegmentType)(segment.attributes >> 24);
                        unimplementedSegments.AddOrUpdate(typ, 1, (k, v) => v + 1);
                    }
                }
            }

            // Log errros loading segments
            foreach (var seg in unimplementedSegments)
            {
                Logger.Log($"Decoder not yet implemented for {seg.Value} segment(s) with type: {seg.Key}", Logger.VerbosityLevel.WARN);
            }

            Logger.Log($"Loaded {segments.Length} segments in {stopwatch.Elapsed.TotalMilliseconds} ms!");
            stopwatch.Restart();

            reader.BaseStream.Seek(lastPos, SeekOrigin.Begin);
            return toc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static JTTOCEntry LoadTOCEntry(BinaryJTReader reader, int jtVersion)
        {
            var segment = new JTTOCEntry();
            segment.guid = reader.ReadGUID();
            if (jtVersion >= 10)
                segment.offset = reader.ReadUInt64();
            else
                segment.offset = reader.ReadUInt32();
            segment.length = reader.ReadUInt32();
            segment.attributes = reader.ReadUInt32();
            return segment;
        }

        /// <summary>
        /// Loads a single segment from it's TOC entry.
        /// WARNING: Mutates the position of the reader without resetting it.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="jTTOCEntry"></param>
        /// <returns></returns>
        private static JTSegment LoadSegment(BinaryJTReader reader, JTTOCEntry jTTOCEntry, int jtVersion)
        {
            reader.BaseStream.Seek((long)jTTOCEntry.offset, SeekOrigin.Begin);
            JTSegment segment;

            var segmentType = (JTSegmentType)(jTTOCEntry.attributes >> 24);
            switch (segmentType)
            {
                case JTSegmentType.LogicalSceneGraph:
                    segment = JTLSGSegment.LoadSegment(ref reader, jtVersion);
                    break;
                case JTSegmentType.Shape:
                case JTSegmentType.ShapeLOD0:
                case JTSegmentType.ShapeLOD1:
                case JTSegmentType.ShapeLOD2:
                case JTSegmentType.ShapeLOD3:
                case JTSegmentType.ShapeLOD4:
                case JTSegmentType.ShapeLOD5:
                case JTSegmentType.ShapeLOD6:
                case JTSegmentType.ShapeLOD7:
                case JTSegmentType.ShapeLOD8:
                case JTSegmentType.ShapeLOD9:
                    segment = JTShapeLODSegment.LoadSegment(ref reader, jtVersion);
                    break;
                case JTSegmentType.MetaData:
                case JTSegmentType.PMIData:
                    // For now we just return null to reduce the number of errors logged
                    segment = null;
                    break;
                default:
                    var typ = (JTSegmentType)(jTTOCEntry.attributes >> 24);
                    Logger.Debug($"Decoder not yet implemented for segment {jTTOCEntry.guid} with type: {typ}");
                    segment = null;
                    break;
            }

            return segment;
        }
    }
}