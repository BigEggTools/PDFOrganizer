using BigEgg.Progress;
using System;
using System.Threading.Tasks;

namespace BigEgg.PDFOrganizer.Services
{
    public interface IPDFOrganizerServcie
    {
        Task Merge(string sourceDirectory, string targetFile, string filterPrefix, bool viewFinalOutput, IProgress<IProgressReport> progress);

        Task Split(string sourceFile, string targetDirectory, string namePrefix, IProgress<IProgressReport> progress);
    }
}
