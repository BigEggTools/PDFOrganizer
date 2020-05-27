using BigEgg.PDFOrganizer.Parameters;
using BigEgg.PDFOrganizer.Services;
using BigEgg.Progress;
using BigEgg.Tools.ConsoleExtension.ProgressBar;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace BigEgg.PDFOrganizer.ArgumentHandlers
{
    [Export(typeof(IArgumentHandler))]
    public class ConvertImageHandler : IArgumentHandler
    {
        private readonly IPDFOrganizerServcie pdfOrganizerService;

        [ImportingConstructor]
        public ConvertImageHandler(IPDFOrganizerServcie pdfOrganizerService)
        {
            this.pdfOrganizerService = pdfOrganizerService;
        }

        public bool CanHandle(object parameter)
        {
            return parameter is ConvertImageParameter;
        }

        public async Task Handle(object parameterObj)
        {
            var parameter = parameterObj as ConvertImageParameter;
            Console.WriteLine($"Start convert the Files from: {parameter.Source}");
            Console.WriteLine($"Target File: {parameter.Target}");

            var progress = new Progress<IProgressReport>(report =>
            {
                TextProgressBar.Draw(report.Current, report.Total);
            });

            await pdfOrganizerService.ConvertImage(parameter.Source, parameter.Target, parameter.Landscape, parameter.Letter, parameter.ViewAfterComplete, progress);
        }
    }
}
