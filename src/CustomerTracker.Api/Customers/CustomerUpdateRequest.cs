using System.ComponentModel.DataAnnotations;

namespace CustomerTracker.Api.Customers
{
    public class CustomerUpdateRequest
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        [Required]
        public bool? IsActive { get; set; }
    }
}