namespace Dsw2025Tpi.Application.Dtos
{
    public record CustomerModel
    {
        public record Request(string Email, string Name, int PhoneNumber);

        public record Response(Guid Id, string Name, string Email, string PhoneNumber);
    }
}
