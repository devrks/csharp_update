namespace Models.Options
{
    public class ArchiveOptions
    {
        public string SourceFolder { get; set; }
        public string TargetFolder { get; set; }
        public Ionic.Zlib.CompressionLevel CompressingLevel { get; set; }
    }
}