namespace Finance.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; init; }
        public T? Value { get; init; }
        public AppError? Error { get; init; }

        public static Result<T> Success(T value)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Value = value
            };
        }

        public static Result<T> Failure(string code, string message)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Error = new AppError(code, message)
            };
        }
    }
}
