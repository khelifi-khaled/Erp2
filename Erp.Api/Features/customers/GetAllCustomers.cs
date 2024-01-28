using Carter;
using Erp.Api.Infrastrucutre;
using Erp.Api.Mappers;
using Erp.Tools.CL;
using MediatR;
using System.Data;

namespace Erp.Api.Features.customers;

public class GetAllCustomers
{
    public class Query : IRequest<IResults>
    {

    }
    public class Handeler : IRequestHandler<Query, IResults>
    {
        private readonly IDbConnection _dbConnection;

        public Handeler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<IResults> Handle(Query request, CancellationToken cancellationToken)
        {
            var customers =
                 _dbConnection.ExecuteReader(@"SELECT c.id, 
	                                            c.customer_name , 
	                                            c.customer_number,
	                                            c.phone_number,
	                                            c.fax_number,
	                                            c.mail,
	                                            c.vat_number,
	                                            c.registered_vat,
	                                            c.cathegory_id as catId,
	                                            cat.category,
	                                            c.payment_term_id,
	                                            pt.payment_term, 
	                                            pt.payment_term_code
	                                            FROM customers c join payment_terms pt ON c.payment_term_id = pt.Id
	                                            join categories cat ON c.cathegory_id = cat.Id;",
                                             dr => dr.ToCustomer(),
                                             false);

            if (customers is null)
            {
                return Task.FromResult(Result.Failure("Customers not found"));
            }

            return Task.FromResult(Result.Success(data: customers));
        }

    }
}


public class GetAllCostomersEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/customers", async (ISender sender) =>
        {
            var query = new GetAllCustomers.Query();

            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Message);
            }

            return Results.Ok(result.Data);

        });
    }
}

