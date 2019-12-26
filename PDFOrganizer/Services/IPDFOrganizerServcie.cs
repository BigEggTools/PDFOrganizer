using BigEgg.Progress;
using System;
using System.Threading.Tasks;

namespace BigEgg.PDFOrganizer.Services
{
    public interface IPDFOrganizerServcie
    {
        Task Merge(string source, string target, string prefix, bool view, IProgress<IProgressReport> progress);

        Task Split(string source, string target, string prefix, IProgress<IProgressReport> progress);
    }
}
