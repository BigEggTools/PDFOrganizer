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
    public class SplitBlockHandler : IArgumentHandler
    {
        private readonly IPDFOrganizerServcie pdfOrganizerService;

        [ImportingConstructor]
        public SplitBlockHandler(IPDFOrganizerServcie pdfOrganizerService)
        {
            this.pdfOrganizerService = pdfOrganizerService;
        }

        public bool CanHandle(object parameter)
        {
            return parameter is SplitBlockParameter;
        }

        public async Task Handle(object parameterObj)
        {
            var parameter = parameterObj as SplitBlockParameter;
            Console.WriteLine($"Start Merge the Files from: {parameter.Source}");
            Console.WriteLine($"Target File: {parameter.Target}");
            Console.WriteLine($"Setting File: {parameter.Setting}");

            var progress = new Progress<IProgressReport>(report =>
            {
                TextProgressBar.Draw(report.Current, report.Total);
            });

            await pdfOrganizerService.SplitBlock(parameter.Source, parameter.Target, parameter.Setting, progress);
        }
    }
}
