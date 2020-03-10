using System.Threading.Tasks;
using CustomerTracker.Domain.SharedKernel;

namespace CustomerTracker.Domain
{
    public interface IAccountingGateway
    {
        Task<Result> RegisterCustomerAsync(RegisterCustomerRequest request);
    }
}