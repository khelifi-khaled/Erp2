namespace Erp.Api.Entities;

public class CustomerPayementTerm
{
    public Guid Id { get; set; }
    public string PayementTerm { get; set; } = string.Empty;
    public string PayementTermCode { get; set; } = string.Empty;
}
