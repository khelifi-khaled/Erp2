using Carter;
using Erp.Api.Contracts;
using Erp.Api.Entities;
using Erp.Api.Infrastrucutre;
using Erp.Tools.CL;
using FluentValidation;
using Mapster;
using MediatR;
using System.Data;

namespace Erp.Api.Features.Items;

public static class CreateItem
{
    public class Command : IRequest<IResults>
    {
        public Guid VatTypeId { get; set; }
        public string SupplierItemNumber { get; set; } = string.Empty;
        public string ItemBarcode { get; set; } = string.Empty;
        public string ItemNumber { get; set; } = string.Empty;
        public string ItemDiscription { get; set; } = string.Empty;
        public double ItemPrice { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.VatTypeId).NotEmpty();
            RuleFor(x => x.SupplierItemNumber).NotEmpty();
            RuleFor(x => x.ItemBarcode).NotEmpty();
            RuleFor(x => x.ItemNumber).NotEmpty();
            RuleFor(x => x.ItemDiscription).NotEmpty();
            RuleFor(x => x.ItemPrice).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, IResults>
    {
        private readonly IDbConnection _dbConnection;
        private readonly IValidator<Command> _validator;
        public Handler(IDbConnection dbConnection, IValidator<Command> validator)
        {
            _dbConnection = dbConnection;
            _validator = validator;
        }

        

        public Task<IResults> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var concatenatedErrors = string.Join(", ", errorMessages);
                return Task.FromResult(Result.Failure(concatenatedErrors));
            }

            var item = new Item
            {
                Id = Guid.NewGuid(),
                VatType = new VatTypes { Id = request.VatTypeId },
                SupplierItemNumber = request.SupplierItemNumber,
                ItemBarcode = request.ItemBarcode,
                ItemNumber = request.ItemNumber,
                ItemDiscription = request.ItemDiscription,
                ItemPrice = request.ItemPrice
            };

             _dbConnection.ExecuteNonQuery(@"
                            INSERT INTO items 
                            (id,vat_type_id,item_number,supplier_item_number,item_barcode,item_discription,item_price) VALUES 
                            (@id,@vat_type_id,@item_number,@supplier_item_number,@item_barcode,@item_discription,@item_price)", 
                            isStoredProcedure : false,
                            parameters : new
                            {
                                id = item.Id,
                                vat_type_id = item.VatType.Id,
                                item_number = item.ItemNumber,
                                supplier_item_number = item.SupplierItemNumber,
                                item_barcode = item.ItemBarcode,
                                item_discription = item.ItemDiscription,
                                item_price = item.ItemPrice
                            });

            return Task.FromResult(Result.Success(data: item.Id));
        }

    }
}

public class CreateItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/items", async (CreateItemRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateItem.Command>();

            var result = await sender.Send(command);

            if(result.IsFailure)
            {
                return Results.BadRequest(result.Message);
            }

            return Results.Ok(result.Data);
        });
    }
}
