using Erp.Api.Entities;
using Erp.Api.Infrastrucutre;
using Erp.Api.Mappers;
using Erp.Tools.CL;
using FluentValidation;
using MediatR;
using System.Data;

namespace Erp.Api.Features.Items;

public class GetItem 
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
                _dbConnection.ExecuteReader(@"SELECT * FROM Items WHERE Id = @Id", dr => dr.ToItem(),false, new { request.Id }).FirstOrDefault();

            if (item is null)
            {
                return Task.FromResult(Result.Failure("Item not found"));
            }

            return Task.FromResult(Result.Success(data: );
        }
    }
}

