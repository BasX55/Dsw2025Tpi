namespace Dsw2025Tpi.Domain.Entities
{
    public class Customer : EntityBase
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        [Obsolete]

        public ICollection<Order> Orders { get; set; } = new List<Order>();


    }
}
