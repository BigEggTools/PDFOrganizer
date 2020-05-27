using BigEgg.Tools.ConsoleExtension.Parameters;

namespace BigEgg.PDFOrganizer.Parameters
{
    [Command("convert-image", "Convert image to PDF file")]
    public class ConvertImageParameter
    {
        [StringProperty("source", "s", "The source image file", Required = true)]
        public string Source { get; set; }

        [StringProperty("target", "t", "The PDF file name, same as source image file if not specific")]
        public string Target { get; set; }

        [BooleanProperty("landscape", "l", "The PDF page orientation is landscape, will use portrait if not specific")]
        public bool Landscape { get; set; }

        [BooleanProperty("USSize", "u", "The PDF page size will be US Letter, will use A4 if not specific")]
        public bool Letter { get; set; }

        [BooleanProperty("view", "v", "Open the merged file after merge complete")]
        public bool ViewAfterComplete { get; set; }
    }
}
