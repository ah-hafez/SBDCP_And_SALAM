namespace MCS.Common.ApiControllerResults
{
    public class PostResult : BaseResult
    {
        public int? Id { get; set; }

        public object Result { get; set; }

        public static PostResult Create(StatusCode statusCode, int? id)
        {
            return new PostResult
            {
                StatusCode = statusCode,
                Id = id
            };
        }

        public static PostResult Create(StatusCode statusCode, object result)
        {
            return new PostResult
            {
                StatusCode = statusCode,
                Result = result
            };
        }
    }
}

