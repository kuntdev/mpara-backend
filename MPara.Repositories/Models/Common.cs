using System;
namespace MPara.Repositories.Models
{
    public class ApiResponse<T>
    {
        public bool IsErrorOccured { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
        public T Response { get; set; }

        public ApiResponse(ResponseType type, T response, string message = "")
        {
            IsErrorOccured = true;
            Code = (int)type;
            Response = response;

            switch (type)
            {
                case ResponseType.NullParameter:
                    Message = String.IsNullOrEmpty(message) ? "Parametre boş gönderilemez." : message;
                    break;
                case ResponseType.Conflict:
                    Message = String.IsNullOrEmpty(message) ? "Çakışma oluştu. Yeni bilgiler ile kayıt oluşturunuz." : message;
                    break;
                case ResponseType.Success:
                    Message = String.IsNullOrEmpty(message) ? "Başarılı" : message;
                    IsErrorOccured = false;
                    break;
                case ResponseType.Exception:
                    Message = String.IsNullOrEmpty(message) ? "Beklenmedik bir hata oluştu." : message;
                    break;
                case ResponseType.NotFound:
                    Message = String.IsNullOrEmpty(message) ? "Kayıt bulunamadı." : message;
                    break;
                case ResponseType.Undone:
                    Message = String.IsNullOrEmpty(message) ? "İşlem gerçekleştirilemedi." : message;
                    break;
                default:
                    break;
            }
        }
    }

    public enum ResponseType
    {
        NotFound = 100,
        Success = 200,
        Conflict = 300,
        NullParameter = 400,
        Exception = 500,
        Undone = 600
    }

    public class ClaimModel
    {
        public string Username { get; set; }
        public int ApiUserId { get; set; }
    }
}


