using BigEgg.Tools.ConsoleExtension.Parameters;

namespace PDFOrganizer.Parameters
{
    [Command("generate", "Generate the markdown file for one folder")]
    public class SplitParameter
    {
        [StringProperty("source", "s", "The source PDF file", Required = true)]
        public string Source { get; set; }

        [StringProperty("target", "t", "The target directory, same as source PDF file if not specific")]
        public string Target { get; set; }

        [StringProperty("prefix", "p", "The prefix of the result files")]
        public string Prefix { get; set; }
    }
}
