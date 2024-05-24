public class Result
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    private Result(bool success, string errorMessage)
    {
        IsSuccess = success;
        ErrorMessage = errorMessage;
    }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Error(string errorMessage)
    {
        return new Result(false, errorMessage);
    }
}
