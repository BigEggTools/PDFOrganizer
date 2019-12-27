using BigEgg.Tools.ConsoleExtension.Parameters;

namespace BigEgg.PDFOrganizer.Parameters
{
    [Command("split-block", "Split the DPF file to blocks by setting")]
    public class SplitBlockParameter
    {
        [StringProperty("source", "s", "The source PDF file", Required = true)]
        public string Source { get; set; }

        [StringProperty("target", "t", "The target directory, same as source PDF file if not specific")]
        public string Target { get; set; }

        [StringProperty("config", "j", "The config file", Required = true)]
        public string Setting { get; set; }
    }
}
