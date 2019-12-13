using BigEgg.Tools.ConsoleExtension.Parameters;

namespace PDFOrganizer.Parameters
{
    [Command("generate", "Generate the markdown file for one folder")]
    public class MergeParameter
    {
        [StringProperty("source", "s", "The source directory", Required = true)]
        public string Source { get; set; }

        [StringProperty("target", "t", "The target PDF file name")]
        public string Target { get; set; }

        [StringProperty("prefix", "p", "The prefix filter in the source directory")]
        public string Prefix { get; set; }
    }
}
