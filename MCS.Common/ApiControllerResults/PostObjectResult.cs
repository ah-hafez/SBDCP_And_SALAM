namespace MCS.Common.ApiControllerResults
{
    public class PostObjectResult<T> : BaseResult where T : class
    {
        public T Result { get; set; }

        public static PostObjectResult<T> Create(StatusCode statusCode, T result)
        {
            return new PostObjectResult<T>
            {
                StatusCode = statusCode,
                Result = result
            };
        }
    }
}
