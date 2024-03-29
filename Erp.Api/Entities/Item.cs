﻿namespace Erp.Api.Entities;

public class Item
{
    public Guid Id { get; set; }
    public VatTypes VatType { get; set; } = new VatTypes();
    public string SupplierItemNumber { get; set; } = string.Empty;
    public string ItemBarcode { get; set; } = string.Empty;
    public string ItemNumber { get; set;} = string.Empty;
    public string ItemDiscription { get; set; } = string.Empty;
    public double ItemPrice { get; set; }
}
