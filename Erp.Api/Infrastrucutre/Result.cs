namespace Erp.Api.Infrastrucutre;

public class Result : IResults
{
    public static IResults Success(string? message = null, object? data = null)
    {
        return new Result(true, message, data);
    }

    public static IResults Failure(string message)
    {
        return new Result(false, message);
    }

    public bool IsSuccess { get; init; }
    public bool IsFailure { get => !IsSuccess; }
    public string? Message { get; init; }
    public object? Data { get; init; }

    private Result(bool isSucces, string? message = null , object? data = null)
    {
        IsSuccess = isSucces;
        Message = message;
        Data = data;
    }
}
