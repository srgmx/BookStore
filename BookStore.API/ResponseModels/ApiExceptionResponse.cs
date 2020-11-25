namespace BookStore.API.ResponseModels
{
    public class ApiExceptionResponse : ApiBaseResponse
    {
        public string ExceptionType { get; set; }

        public string ExceptionStackTrace { get; set; }
    }
}
