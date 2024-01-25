namespace Erp.Api.Contracts;

public class CreateItemRequest
{
    public Guid VatTypeId { get; set; }
    public string SupplierItemNumber { get; set; } = string.Empty;
    public string ItemBarcode { get; set; } = string.Empty;
    public string ItemNumber { get; set; } = string.Empty;
    public string ItemDiscription { get; set; } = string.Empty;
    public double ItemPrice { get; set; }
}
