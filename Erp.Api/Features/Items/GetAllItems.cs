using Carter;
using Erp.Api.Contracts;
using Erp.Api.Infrastrucutre;
using Erp.Api.Mappers;
using Erp.Tools.CL;
using MediatR;
using System.Data;

namespace Erp.Api.Features.Items;


public class GetAllItems
{
    public class Query : IRequest<IResults>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, IResults>
    {
        private readonly IDbConnection _dbConnection;
        public Handler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<IResults> Handle(Query request, CancellationToken cancellationToken)
        {
            var items =
                _dbConnection.ExecuteReader(@"SELECT i.id 
	                                        , i.vat_type_id
	                                        , vt.vat_type 
	                                        , i.supplier_item_number
	                                        , i.item_barcode
	                                        , i.item_number
	                                        , i.item_discription
	                                        ,i.item_price 
	                                        FROM items i join vat_types vt ON vt.id = i.vat_type_id",
                                            dr => dr.ToItem(),
                                            false);

            if (items is null)
            {
                return Task.FromResult(Result.Failure("Items not found"));
            }

            return Task.FromResult(Result.Success(data: items));
        }
    }
}

public class GetAllItemsEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/items", async (ISender sender) =>
        {
            var query = new GetAllItems.Query();

            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Message);
            }

            return Results.Ok(result.Data);

        });
    }
}
