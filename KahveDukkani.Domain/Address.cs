namespace KahveDukkani.Domain;

public record Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string ZipCode { get; init; }

    public Address(string street, string city, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(city))
        {
            throw new Exception("Adres bilgileri eksik olamaz!");
        }

        Street = street;
        City = city;
        ZipCode = zipCode;
    }
}