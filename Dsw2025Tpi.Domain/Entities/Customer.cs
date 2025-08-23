namespace Dsw2025Tpi.Domain.Entities
{
    public class Customer : EntityBase
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        
    }
}
