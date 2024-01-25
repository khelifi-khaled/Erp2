namespace Erp.Api.Infrastrucutre;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    string? Message { get; }
}
