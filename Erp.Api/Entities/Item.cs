namespace Erp.Api.Entities;

public class Item
{
    public Guid Id { get; set; }

    public VatTypes VatType { get; set; }

    public string SupplierItemNumber { get; set; }

    public string ItemBarcode { get; set;}

    public string ItemDiscription { get; set; }

    public double ItemPrice { get; set; }
}
