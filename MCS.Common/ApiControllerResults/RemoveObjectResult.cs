namespace MCS.Common.ApiControllerResults
{
    public class RemoveObjectResult<T> : BaseResult where T : class
    {
        public T Result { get; set; }

        public static RemoveObjectResult<T> Create(StatusCode statusCode, T result)
        {
            return new RemoveObjectResult<T>
            {
                StatusCode = statusCode,
                Result = result
            };
        }
    }
}
