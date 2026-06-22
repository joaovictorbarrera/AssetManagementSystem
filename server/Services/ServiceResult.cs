namespace AssetManagementSystem.Services
{
    public enum ServiceErrorType
    {
        NotFound,
        Forbidden,
        BadRequest
    }

    // Non-generic result for operations that return no payload on success.
    public class ServiceResult
    {
        public bool Succeeded { get; private set; }
        public ServiceErrorType? ErrorType { get; private set; }
        public string? ErrorMessage { get; private set; }

        private ServiceResult() { }

        public static ServiceResult Success() =>
            new() { Succeeded = true };

        private static ServiceResult Fail(ServiceErrorType errorType, string message) =>
            new() { Succeeded = false, ErrorType = errorType, ErrorMessage = message };

        public static ServiceResult NotFound(string message = "Resource not found") =>
            Fail(ServiceErrorType.NotFound, message);

        public static ServiceResult Forbidden(string message) =>
            Fail(ServiceErrorType.Forbidden, message);

        public static ServiceResult BadRequest(string message) =>
            Fail(ServiceErrorType.BadRequest, message);
    }

    // Generic result for operations that return a payload on success.
    public class ServiceResult<T>
    {
        public bool Succeeded { get; private set; }
        public T? Value { get; private set; }
        public ServiceErrorType? ErrorType { get; private set; }
        public string? ErrorMessage { get; private set; }

        private ServiceResult() { }

        public static ServiceResult<T> Success(T value) =>
            new() { Succeeded = true, Value = value };

        public static ServiceResult<T> Fail(ServiceErrorType errorType, string message) =>
            new() { Succeeded = false, ErrorType = errorType, ErrorMessage = message };

        public static ServiceResult<T> NotFound(string message = "Resource not found") =>
            Fail(ServiceErrorType.NotFound, message);

        public static ServiceResult<T> Forbidden(string message) =>
            Fail(ServiceErrorType.Forbidden, message);

        public static ServiceResult<T> BadRequest(string message) =>
            Fail(ServiceErrorType.BadRequest, message);
    }
}