namespace ContactApp.Domain.DTOs.Contact
{
    public class CreateContactDto
    {
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int CountryId { get; set; }
    }
}
