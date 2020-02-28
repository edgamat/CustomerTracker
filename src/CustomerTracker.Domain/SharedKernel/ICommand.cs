using System.Threading.Tasks;

namespace CustomerTracker.Domain.SharedKernel
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task<Result> HandleAsync(TCommand command);
    }
}