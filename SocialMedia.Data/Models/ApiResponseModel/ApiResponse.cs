

namespace SocialMedia.Data.Models.ApiResponseModel
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = null!;
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T? ResponseObject { get; set; }
    }
}
