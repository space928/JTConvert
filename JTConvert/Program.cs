using JTConvert.JTCodec;
using System;

namespace JTConvert
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WindowHeight = 40;
            Console.WindowWidth = 200;
            Console.WriteLine("##### JTConvert by Thomas M. #####");
            Console.WriteLine();

            //Logger.Verbosity = Logger.VerbosityLevel.ERROR;

            if (args.Length == 0)
            {
                // Start gui...
                args = new string[4];
                Console.WriteLine("No command line arguments detected! Enter conversion parameters...");
                Console.WriteLine("Input file path: ");
                args[0] = "-i";
                args[1] = Console.ReadLine().Trim();
                Console.WriteLine("Output file path: ");
                args[2] = "-o";
                args[3] = Console.ReadLine().Trim();
            }

            var settings = ParseArgs(args);
            if(settings.inputType == CADFileType.None)
            {
                Console.WriteLine("Input file type not supported!");
                return;
            }
            if(settings.outputType == CADFileType.None)
            {
                Console.WriteLine("Output file type not supported!");
                return;
            }

            switch(settings.inputType)
            {
                case CADFileType.JT:
                    var loader = new JTLoader();
                    CompressionInitialiser.InitNativeLibrary();
                    loader.LoadFile(settings);
                    break;
            }
        }

        private static JTConvertSettings ParseArgs(string[] args)
        {
            JTConvertSettings ret = new();
            for(int i = 0; i < args.Length; i++)
            {
                switch(args[i])
                {
                    case "-i":
                        ret.inputFile = args[++i];
                        ret.inputType = JTConvertSettings.FileTypeFromExtension(Path.GetExtension(ret.inputFile));
                        break;
                    case "-o":
                        ret.outputFile = args[++i];
                        ret.outputType = JTConvertSettings.FileTypeFromExtension(Path.GetExtension(ret.outputFile));
                        break;
                    case "-p":
                        ret.loadWholeFile = true;
                        ret.parallel = true;
                        break;
                }
            }

            return ret;
        }
    }
}