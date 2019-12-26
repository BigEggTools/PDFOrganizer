using BigEgg.Progress;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BigEgg.PDFOrganizer.Services
{
    [Export(typeof(IPDFOrganizerServcie))]
    public class PDFOrganizerServcie : IPDFOrganizerServcie
    {
        public Task Merge(string source, string target, string prefix, bool view, IProgress<IProgressReport> progress)
        {
            Preconditions.NotNullOrWhiteSpace(source, nameof(source));
            Preconditions.Check(Directory.Exists(source), "Source sould be an existed directory.");

            return Task.Factory.StartNew(() =>
            {
                Trace.Indent();
                Trace.TraceInformation($"Process the parameters target: {target}.");
                if (string.IsNullOrWhiteSpace(target)) { target = "Merge_File.pdf"; }
                if (!target.EndsWith(".pdf")) { target += ".pdf"; }
                Trace.TraceInformation($"Target hand been process to {target}.");

                // Get the files
                Trace.TraceInformation($"Get the files from {source} with prefix {prefix}.");
                var files = string.IsNullOrWhiteSpace(prefix)
                    ? Directory.GetFiles(source, "*.pdf", SearchOption.TopDirectoryOnly)
                    : Directory.GetFiles(source, $"{prefix}*.pdf", SearchOption.TopDirectoryOnly);

                // Open the output document
                PdfDocument outputDocument = new PdfDocument();

                // Iterate files
                Console.WriteLine($"Start merge the files. Total number: {files.Length}.");
                int mergedFileCount = 0;
                foreach (string file in files)
                {
                    reportProgress(progress, new ProgressReport(mergedFileCount, files.Length));

                    // Open the document to import pages from it.
                    PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        // Get the page from the external document...
                        PdfPage page = inputDocument.Pages[idx];
                        // ...and add it to the output document.
                        outputDocument.AddPage(page);
                    }
                }
                reportProgress(progress, new ProgressReport(files.Length, files.Length));

                // Save the document
                Trace.TraceInformation($"Save the output file {target}.");
                outputDocument.Save(target);

                if (view)   // start a viewer. 
                {
                    Console.WriteLine($"Show the merged file {target}.");
                    Process.Start(target);
                }

                Trace.Unindent();
            });
        }

        public Task Split(string source, string target, string prefix, IProgress<IProgressReport> progress)
        {
            Preconditions.NotNullOrWhiteSpace(source, nameof(source));
            Preconditions.Check(File.Exists(source), "Source should be an existed file.");
            Preconditions.Check(!source.EndsWith(".pdf"), "Source should be a file with pdf as extension.");

            return Task.Factory.StartNew(() =>
            {
                Trace.Indent();
                Trace.TraceInformation($"Process the parameters prefix: {prefix}.");
                if (!target.EndsWith("/")) { target += "/"; }
                if (string.IsNullOrWhiteSpace(prefix)) { prefix = source.Substring(0, source.Length - 4); }
                Trace.TraceInformation($"Prefix hand been process to {prefix}.");

                // Open the file
                PdfDocument inputDocument = PdfReader.Open(source, PdfDocumentOpenMode.Import);

                Console.WriteLine($"Start split the file. Total page number: {inputDocument.PageCount}.");
                string name = Path.GetFileNameWithoutExtension(source);
                for (int idx = 0; idx < inputDocument.PageCount; idx++)
                {
                    reportProgress(progress, new ProgressReport(idx, inputDocument.PageCount));

                    // Create new document
                    PdfDocument outputDocument = new PdfDocument();
                    outputDocument.Version = inputDocument.Version;
                    outputDocument.Info.Title = $"{inputDocument.Info.Title} - Page {idx + 1} of {inputDocument.PageCount}";
                    outputDocument.Info.Creator = inputDocument.Info.Creator;

                    // Add the page and save it
                    outputDocument.AddPage(inputDocument.Pages[idx]);
                    outputDocument.Save($"{target}{prefix} - Page {idx + 1}_inputDocument.PageCount.pdf");
                }
                reportProgress(progress, new ProgressReport(inputDocument.PageCount, inputDocument.PageCount));

                Trace.Unindent();
            });
        }

        private void reportProgress(IProgress<IProgressReport> progress, IProgressReport report)
        {
            if (progress != null)
            {
                progress.Report(report);
            }
        }
    }
}
