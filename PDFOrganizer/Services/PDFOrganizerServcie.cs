using BigEgg.Progress;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace BigEgg.PDFOrganizer.Services
{
    [Export(typeof(IPDFOrganizerServcie))]
    public class PDFOrganizerServcie : IPDFOrganizerServcie
    {
        public Task Merge(string source, string target, string prefix, IProgress<IProgressReport> progress)
        {
            throw new NotImplementedException();
        }

        public Task Split(string source, string target, string prefix, IProgress<IProgressReport> progress)
        {
            throw new NotImplementedException();
        }
    }
}
