﻿using Erp.Api.Entities;
using System.Data;

namespace Erp.Api.Mappers;

public static class ItemMapper
{
    internal static Item ToItem(this IDataRecord record)
    {
        return new Item()
        {
            Id = (Guid)record["id"],
            VatType = new VatTypes()
            {
                Id = (Guid)record["vat_type_id"],
                VatType = (string)record["vat_type"]
            },
            SupplierItemNumber = (string)record["supplier_item_number"],
            ItemBarcode = (string)record["item_barcode"],
            ItemNumber = (string)record["item_number"],
            ItemDiscription = (string)record["item_discription"],
            ItemPrice = Convert.ToDouble(record["item_price"])
        };
    }
}
