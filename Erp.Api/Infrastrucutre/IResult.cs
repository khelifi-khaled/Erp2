namespace Erp.Api.Infrastrucutre;

public interface IResults
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    string? Message { get; }
    object? Data { get; }
}
