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
    public class MergePDFHandler : IArgumentHandler
    {
        private readonly IPDFOrganizerServcie pdfOrganizerService;

        [ImportingConstructor]
        public MergePDFHandler(IPDFOrganizerServcie pdfOrganizerService)
        {
            this.pdfOrganizerService = pdfOrganizerService;
        }

        public bool CanHandle(object parameter)
        {
            return parameter is MergeParameter;
        }

        public async Task Handle(object parameterObj)
        {
            var parameter = parameterObj as MergeParameter;
            Console.WriteLine($"Start Merge the Files from: {parameter.Source}");
            Console.WriteLine($"Target File: {parameter.Target}" + (string.IsNullOrWhiteSpace(parameter.Prefix) ? "" : $" with prefix {parameter.Prefix}"));

            var progress = new Progress<IProgressReport>(report =>
            {
                TextProgressBar.Draw(report.Current, report.Total);
            });

            await pdfOrganizerService.Merge(parameter.Source, parameter.Target, parameter.Prefix, parameter.ViewAfterComplete, progress);
        }
    }
}
