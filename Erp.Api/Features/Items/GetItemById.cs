using Carter;
using Erp.Api.Infrastrucutre;
using Erp.Api.Mappers;
using Erp.Tools.CL;
using FluentValidation;
using MediatR;
using System.Data;

namespace Erp.Api.Features.Items;

public class GetItemById 
{
    public class Query : IRequest<IResults>
    {
        public Guid Id { get; set; }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }

    internal sealed class Handler : IRequestHandler<Query, IResults>
    {
        private readonly IDbConnection _dbConnection;
        private readonly IValidator<Query> _validator;
        public Handler(IDbConnection dbConnection, IValidator<Query> validator)
        {
            _dbConnection = dbConnection;
            _validator = validator;
        }

        public Task<IResults> Handle(Query request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var concatenatedErrors = string.Join(", ", errorMessages);
                return Task.FromResult(Result.Failure(concatenatedErrors));
            }

            var item = 
                _dbConnection.ExecuteReader(@"SELECT i.id 
	                                        , i.vat_type_id
	                                        , vt.vat_type 
	                                        , i.supplier_item_number
	                                        , i.item_barcode
	                                        , i.item_number
	                                        , i.item_discription
	                                        ,i.item_price 
	                                        FROM items i join vat_types vt ON vt.id = i.vat_type_id WHERE i.Id = @Id", 
                                            dr => dr.ToItem(),
                                            false, 
                                            new { request.Id }).FirstOrDefault();

            if (item is null)
            {
                return Task.FromResult(Result.Failure("Item not found"));
            }

            return Task.FromResult(Result.Success(data: item));
        }
    }
}

public class GetItemEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/items/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetItemById.Query() { Id = id };

            var result = await sender.Send(query);

            if(result.IsFailure)
            {
                return Results.BadRequest(result.Message);
            }

            return Results.Ok(result.Data);

        });
    }
}


