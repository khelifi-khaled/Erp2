using Erp.Api.Entities;
using System.Data;

namespace Erp.Api.Mappers;

public static class CustomerMapper
{
    internal static Customer ToCustomer(this IDataRecord record)
    {
        return new Customer
        {
           Id= (Guid)record["id"],
            CustomerNumber = (int)record["customer_number"],
            CustomerPhoneNumber = (string)record["phone_number"],
            CustomerFaxNumber = (string)record["fax_number"],
            CustomerEmail = (string)record["mail"],
            CustomerVatNumber = (string)record["vat_number"],
            RegisteredVat = (bool)record["registered_vat"],
            CustomerName = (string)record["customer_name"],
            CustomerCategory = new CustomerCategory
            {
                Id = (Guid)record["catId"],
                Category = (string)record["category"]
            },
            CustomerPayementTerm = new CustomerPayementTerm
            {
                Id = (Guid)record["payment_term_id"],
                PayementTerm = (string)record["payment_term"],
                PayementTermCode = (string)record["payment_term_code"]
            }
        };
    }
}
