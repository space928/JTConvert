namespace JTConvert
{
    public struct JTConvertSettings
    {
        public string inputFile;
        public string outputFile;
        public CADFileType inputType;
        public CADFileType outputType;
        /// <summary>
        /// Whether or not to load the whole file into memory at once.
        /// </summary>
        public bool loadWholeFile;
        /// <summary>
        /// Wehther or not to attempt to parse the file using multiple threads. Only works with loadWholeFile enabled!
        /// </summary>
        public bool parallel;

        public static CADFileType FileTypeFromExtension(string extension) 
            => extension.ToLowerInvariant() switch
        {
            ".jt" => CADFileType.JT,
            ".obj" => CADFileType.OBJ,
            ".usda" => CADFileType.USD,
            _ => CADFileType.None,
        };
    }

    public enum CADFileType
    {
        None,
        JT,
        OBJ,
        USD
    }
}