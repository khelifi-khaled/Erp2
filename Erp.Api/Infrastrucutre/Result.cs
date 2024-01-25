namespace Erp.Api.Infrastrucutre;

public class Result : IResult
{
    public static IResult Success(string? message = null)
    {
        return new Result(true, message);
    }

    public static IResult Failure(string message)
    {
        return new Result(false, message);
    }

    public bool IsSuccess { get; init; }
    public bool IsFailure { get => !IsSuccess; }
    public string? Message { get; init; }

    private Result(bool isSucces, string? message = null)
    {
        IsSuccess = isSucces;
        Message = message;
    }
}
