using BigEgg.Tools.ConsoleExtension.Parameters;

namespace BigEgg.PDFOrganizer.Parameters
{
    [Command("compress", "Compress the PDF file to a smaller one")]
    class CompressParameter
    {
        [StringProperty("source", "s", "The source PDF file", Required = true)]
        public string Source { get; set; }

        [StringProperty("target", "t", "The target PDF file name")]
        public string Target { get; set; }

        [BooleanProperty("view", "v", "Open the merged file after merge complete")]
        public bool ViewAfterComplete { get; set; }
    }
}
