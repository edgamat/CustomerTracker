using System.Threading.Tasks;

namespace CustomerTracker.Domain.SharedKernel
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResult>
        where TCommand : ICommand
    {
        Task<Result<TResult>> HandleAsync(TCommand command);
    }
}