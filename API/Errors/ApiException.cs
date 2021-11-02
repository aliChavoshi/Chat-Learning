namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public string Detail { get; set; }
        public ApiException(int statusCode, string message = null, string detail = null) : base(statusCode, message)
        {
            Detail = detail;
        }
    }
}