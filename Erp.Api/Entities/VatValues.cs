namespace Erp.Api.Entities;

public class VatValues
{
    public Guid Id { get; set; }
    public int VatValue { get; set; }
    public VatTypes VatType { get; set; }
}
