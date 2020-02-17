namespace CustomerTracker.Api.Customers
{
    public class CustomerUpdateRequest
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }
    }
}