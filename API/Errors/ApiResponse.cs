namespace API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "مشکلی زخ داده است لطفا مجدد تلاش کنید",
                401 => "دسترسی به این بخش ندارید",
                404 => "صفحه ی مورد نظر یافت نشد ",
                _ => "مشکلی رخ داده است خطا شناسایی نشد"
            };
        }
    }
}