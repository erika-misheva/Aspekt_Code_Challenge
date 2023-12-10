namespace Domain.Models;

public class Contact : BaseModel
{
    public int CountryId { get; set; }
    public Country Country { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
