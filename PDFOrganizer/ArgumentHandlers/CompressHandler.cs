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
    public class CompressHandler : IArgumentHandler
    {
        private readonly IPDFOrganizerServcie pdfOrganizerService;

        [ImportingConstructor]
        public CompressHandler(IPDFOrganizerServcie pdfOrganizerService)
        {
            this.pdfOrganizerService = pdfOrganizerService;
        }

        public bool CanHandle(object parameter)
        {
            return parameter is CompressParameter;
        }

        public async Task Handle(object parameterObj)
        {
            var parameter = parameterObj as CompressParameter;
            Console.WriteLine($"Start Compress the File: {parameter.Source}");
            Console.WriteLine($"Target File: {parameter.Target}");

            var progress = new Progress<IProgressReport>(report =>
            {
                TextProgressBar.Draw(report.Current, report.Total);
            });

            await pdfOrganizerService.Compress(parameter.Source, parameter.Target, parameter.ViewAfterComplete, progress);
        }
    }

}
