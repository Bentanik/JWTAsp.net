namespace Repository.ViewModels.Responses
{
    public class BaseResponse
    {
        public BaseResponse(int error, string message)
        {
            Error = error;
            Message = message;
        }

        public int Error { get; set; }
        public string Message { get; set; }
    }
}
