namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public string Detail { get; set; }
        public ApiException(int statusCode, string message, string detail) : base(statusCode, message)
        {
            Detail = detail;
        }
    }
}