

using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.GenericReturn
{
    public static class StatusCodeReturn<T>
    {

        public static ApiResponse<T> _200_Success(string msg)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = msg,
                StatusCode = 200
            };
        }
        public static ApiResponse<T> _200_Success(string msg, T Object)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = msg,
                StatusCode = 200,
                ResponseObject = Object
            };
        }

        public static ApiResponse<T> _201_Created(string msg)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = msg,
                StatusCode = 201,
            };
        }

        public static ApiResponse<T> _201_Created(string msg, T Object)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = msg,
                StatusCode = 201,
                ResponseObject = Object
            };
        }

        public static ApiResponse<T> _404_NotFound(string msg)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = msg,
                StatusCode = 404
            };
        }

        public static ApiResponse<T> _404_NotFound()
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = "Page not found",
                StatusCode = 404
            };
        }

        public static ApiResponse<T> _403_Forbidden()
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = $"Forbidden",
                StatusCode = 403
            };
        }

        public static ApiResponse<T> _403_Forbidden(string msg)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = msg,
                StatusCode = 403
            };
        }

        public static ApiResponse<T> _401_UnAuthorized()
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = $"UnAuthorized",
                StatusCode = 401
            };
        }

        public static ApiResponse<T> _500_ServerError(string msg)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = msg,
                StatusCode = 500
            };
        }

        public static ApiResponse<T> _400_BadRequest(string msg)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = msg,
                StatusCode = 400
            };
        }

        public static ApiResponse<T> _406_NotAcceptable()
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = "Not Acceptable",
                StatusCode = 406
            };
        }


    }
}
