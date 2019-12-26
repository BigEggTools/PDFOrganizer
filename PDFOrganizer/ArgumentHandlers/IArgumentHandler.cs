using System.Threading.Tasks;

namespace BigEgg.PDFOrganizer.ArgumentHandlers
{
    public interface IArgumentHandler
    {
        bool CanHandle(object parameter);

        Task Handle(object parameterObj);
    }
}
