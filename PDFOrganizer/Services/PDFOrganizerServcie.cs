using BigEgg.PDFOrganizer.Models;
using BigEgg.Progress;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BigEgg.PDFOrganizer.Services
{
    [Export(typeof(IPDFOrganizerServcie))]
    public class PDFOrganizerServcie : IPDFOrganizerServcie
    {
        public Task Merge(string sourceDirectory, string targetFile, string filterPrefix, bool viewFinalOutput, IProgress<IProgressReport> progress)
        {
            Preconditions.NotNullOrWhiteSpace(sourceDirectory, nameof(sourceDirectory));
            Preconditions.Check(Directory.Exists(sourceDirectory), "Source sould be an existed directory.");

            return Task.Factory.StartNew(() =>
            {
                Trace.Indent();
                Trace.TraceInformation($"Process the parameters target: {targetFile}.");
                if (string.IsNullOrWhiteSpace(targetFile)) { targetFile = "Merge_File.pdf"; }
                if (!targetFile.EndsWith(".pdf")) { targetFile += ".pdf"; }
                Trace.TraceInformation($"Target hand been process to {targetFile}.");

                // Get the files
                Trace.TraceInformation($"Get the files from {sourceDirectory} with prefix {filterPrefix}.");
                var files = string.IsNullOrWhiteSpace(filterPrefix)
                    ? Directory.GetFiles(sourceDirectory, "*.pdf", SearchOption.TopDirectoryOnly)
                    : Directory.GetFiles(sourceDirectory, $"{filterPrefix}*.pdf", SearchOption.TopDirectoryOnly);

                files = files.OrderBy(file => file).ToArray();

                // Open the output document
                PdfDocument outputDocument = new PdfDocument();

                // Iterate files
                Console.WriteLine($"Start merge the files. Total number: {files.Length}.");
                int mergedFileCount = 0;
                foreach (string file in files)
                {
                    ReportProgress(progress, new ProgressReport(mergedFileCount, files.Length));

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
                ReportProgress(progress, new ProgressReport(files.Length, files.Length));

                // Save the document
                Trace.TraceInformation($"Save the output file {targetFile}.");
                outputDocument.Save(targetFile);

                if (viewFinalOutput)   // start a viewer. 
                {
                    Console.WriteLine($"Show the merged file {targetFile}.");
                    Process.Start(targetFile);
                }

                Trace.Unindent();
            });
        }

        public Task Split(string sourceFile, string targetDirectory, string namePrefix, IProgress<IProgressReport> progress)
        {
            Preconditions.NotNullOrWhiteSpace(sourceFile, nameof(sourceFile));
            Preconditions.Check(File.Exists(sourceFile), "Source should be an existed file.");
            Preconditions.Check(sourceFile.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase), "Source should be a file with pdf as extension.");

            return Task.Factory.StartNew(() =>
            {
                Trace.Indent();
                Trace.TraceInformation($"Process the parameters prefix: {namePrefix}.");
                if (!targetDirectory.EndsWith("/")) { targetDirectory += "/"; }
                if (string.IsNullOrWhiteSpace(namePrefix)) { namePrefix = Path.GetFileNameWithoutExtension(sourceFile); }
                Trace.TraceInformation($"Prefix hand been process to {namePrefix}.");
                if (!Directory.Exists(targetDirectory)) { Directory.CreateDirectory(targetDirectory); }

                // Open the file
                PdfDocument inputDocument = PdfReader.Open(sourceFile, PdfDocumentOpenMode.Import);

                Console.WriteLine($"Start split the file. Total page number: {inputDocument.PageCount}.");
                for (int idx = 0; idx < inputDocument.PageCount; idx++)
                {
                    ReportProgress(progress, new ProgressReport(idx, inputDocument.PageCount));

                    // Create new document
                    PdfDocument outputDocument = new PdfDocument();
                    outputDocument.Version = inputDocument.Version;
                    outputDocument.Info.Title = $"{inputDocument.Info.Title} - Page {idx + 1} of {inputDocument.PageCount}";
                    outputDocument.Info.Creator = inputDocument.Info.Creator;

                    // Add the page and save it
                    outputDocument.AddPage(inputDocument.Pages[idx]);
                    outputDocument.Save($"{targetDirectory}{namePrefix} - Page {idx + 1}_{inputDocument.PageCount}.pdf");
                }
                ReportProgress(progress, new ProgressReport(inputDocument.PageCount, inputDocument.PageCount));

                Trace.Unindent();
            });
        }

        public Task SplitBlock(string sourceFile, string targetDirectory, string settingFile, IProgress<IProgressReport> progress)
        {
            Preconditions.NotNullOrWhiteSpace(sourceFile, nameof(sourceFile));
            Preconditions.Check(File.Exists(sourceFile), "Source should be an existed file.");
            Preconditions.Check(sourceFile.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase), "Source should be a file with pdf as extension.");

            Preconditions.NotNullOrWhiteSpace(settingFile, nameof(settingFile));
            Preconditions.Check(File.Exists(settingFile), "Setting should be an existed file.");
            Preconditions.Check(settingFile.EndsWith(".json", StringComparison.OrdinalIgnoreCase), "Setting should be a file with json as extension.");

            return Task.Factory.StartNew(() =>
            {
                Trace.Indent();

                // Open the file
                PdfDocument inputDocument = PdfReader.Open(sourceFile, PdfDocumentOpenMode.Import);

                if (!Directory.Exists(targetDirectory)) { Directory.CreateDirectory(targetDirectory); }
                if (!targetDirectory.EndsWith("/")) { targetDirectory += "/"; }

                List<SplitBlockModel> settings = null;
                try
                {
                    Trace.TraceInformation($"Read setting from file {settingFile}.");
                    settings = JsonConvert.DeserializeObject<List<SplitBlockModel>>(File.ReadAllText(settingFile));

                    if (settings.Any(s => s.StartPage > s.EndPage))
                    {
                        Console.WriteLine("Setting not valid, start page index should not larger than end page index.");
                        return;
                    }

                    if (settings.Any(s => s.StartPage > inputDocument.PageCount || s.StartPage <= 0))
                    {
                        Console.WriteLine("Setting not valid, start page index should larger than 0 and smaller or equal to source file page count.");
                        return;
                    }

                    if (settings.Any(s => s.EndPage > inputDocument.PageCount || s.EndPage <= 0))
                    {
                        Console.WriteLine("Setting not valid, end page index should larger than 0 and smaller or equal to source file page count.");
                        return;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Cannot read the settings, please check the document and fix the setting file.");
                    return;
                }

                Console.WriteLine($"Start split the file to {settings.Count} block. Total page number: {inputDocument.PageCount}.");
                string name = Path.GetFileNameWithoutExtension(sourceFile);
                for (int idx = 0; idx < settings.Count; idx++)
                {
                    var setting = settings[idx];

                    ReportProgress(progress, new ProgressReport(idx, settings.Count));

                    // Create new document
                    PdfDocument outputDocument = new PdfDocument();
                    outputDocument.Version = inputDocument.Version;
                    outputDocument.Info.Title = $"{setting.Name}";
                    outputDocument.Info.Creator = inputDocument.Info.Creator;

                    // Add the pages and save it
                    for (int pageId = setting.StartPage; pageId <= setting.EndPage; pageId++)
                        outputDocument.AddPage(inputDocument.Pages[pageId - 1]);

                    outputDocument.Save($"{targetDirectory}{setting.Name}.pdf");
                }
                ReportProgress(progress, new ProgressReport(settings.Count, settings.Count));

                Trace.Unindent();
            });
        }

        public Task Compress(string sourceFile, string targetFile, bool viewFinalOutput, Progress<IProgressReport> progress)
        {
            Preconditions.NotNullOrWhiteSpace(sourceFile, nameof(sourceFile));
            Preconditions.Check(File.Exists(sourceFile), "Source should be an existed file.");
            Preconditions.Check(sourceFile.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase), "Source should be a file with pdf as extension.");

            return Task.Factory.StartNew(() =>
            {
                Trace.Indent();
                Trace.TraceInformation($"Process the parameters target: {targetFile}.");
                if (string.IsNullOrWhiteSpace(targetFile)) { targetFile = "Merge_File.pdf"; }
                if (!targetFile.EndsWith(".pdf")) { targetFile += ".pdf"; }
                Trace.TraceInformation($"Target hand been process to {targetFile}.");

                // Open the file
                PdfDocument inputDocument = PdfReader.Open(sourceFile, PdfDocumentOpenMode.Import);

                // Create new document
                PdfDocument outputDocument = new PdfDocument();
                outputDocument.Version = inputDocument.Version;
                outputDocument.Info.Title = inputDocument.Info.Title;
                outputDocument.Info.Creator = inputDocument.Info.Creator;
                outputDocument.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
                outputDocument.Options.UseFlateDecoderForJpegImages = PdfUseFlateDecoderForJpegImages.Automatic;
                outputDocument.Options.NoCompression = false;
                outputDocument.Options.CompressContentStreams = true;

                Console.WriteLine($"Start compress the file. Total page number: {inputDocument.PageCount}.");
                for (int idx = 0; idx < inputDocument.PageCount; idx++)
                {
                    ReportProgress(progress, new ProgressReport(idx, inputDocument.PageCount));

                    // Add the page and save it
                    outputDocument.AddPage(inputDocument.Pages[idx]);
                }
                ReportProgress(progress, new ProgressReport(inputDocument.PageCount, inputDocument.PageCount));

                // Save the document
                Trace.TraceInformation($"Save the output file {targetFile}.");
                outputDocument.Save(targetFile);

                if (viewFinalOutput)   // start a viewer. 
                {
                    Console.WriteLine($"Show the merged file {targetFile}.");
                    Process.Start(targetFile);
                }

                Trace.Unindent();
            });
        }

        private void ReportProgress(IProgress<IProgressReport> progress, IProgressReport report)
        {
            if (progress != null)
            {
                progress.Report(report);
            }
        }
    }
}
