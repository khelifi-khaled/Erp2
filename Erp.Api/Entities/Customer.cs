namespace Erp.Api.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public CustomerCategory CustomerCategory { get; set; } = new();
    public CustomerPayementTerm CustomerPayementTerm { get; set; } = new();
    public int CustomerNumber { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhoneNumber { get; set; } = string.Empty;
    public string CustomerFaxNumber { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerVatNumber { get; set; } = string.Empty;
    public bool RegisteredVat { get; set; }
}
