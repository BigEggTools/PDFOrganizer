using BigEgg.Tools.ConsoleExtension.Parameters;

namespace BigEgg.PDFOrganizer.Parameters
{
    [Command("merge", "Merge the PDF files in source directory")]
    public class MergeParameter
    {
        [StringProperty("source", "s", "The source directory", Required = true)]
        public string Source { get; set; }

        [StringProperty("target", "t", "The target PDF file name")]
        public string Target { get; set; }

        [StringProperty("prefix", "p", "The prefix filter in the source directory")]
        public string Prefix { get; set; }

        [BooleanProperty("view", "v", "Open the merged file after merge complete")]
        public bool ViewAfterComplete { get; set; }
    }
}
