using BigEgg.PDFOrganizer.ArgumentHandlers;
using BigEgg.PDFOrganizer.Parameters;
using BigEgg.Tools.ConsoleExtension.Parameters;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace BigEgg.PDFOrganizer
{
    class Program
    {
        private static CompositionContainer container;
        private static AggregateCatalog catalog;

        static void Main(string[] args)
        {
            Initialize();

            var parameter = new Parser(
                    container,
                    ParserSettings.Builder().WithDefault().CaseSensitive(false).ComputeDisplayWidth().Build())
                .Parse(args,
                    typeof(SplitParameter),
                    typeof(MergeParameter));
            if (parameter == null) { return; }

            var handlers = container.GetExportedValues<IArgumentHandler>();
            var handler = handlers.First(h => h.CanHandle(parameter));

            Console.Write("Find Handle to Handle this Parameter.");
            handler.Handle(parameter).Wait();
            Console.Write("Done.");
        }

        private static void Initialize()
        {
            catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Parser).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);
        }
    }
}
