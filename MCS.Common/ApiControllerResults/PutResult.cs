namespace MCS.Common.ApiControllerResults
{
    public class PutResult : BaseResult
    {
        public int? Id { get; set; }
        public static PutResult Create(StatusCode statusCode)
        {
            return new PutResult
            {
                StatusCode = statusCode
            };
        }

        public static PutResult Create(StatusCode statusCode, int? id)
        {
            return new PutResult
            {
                StatusCode = statusCode,
                Id = id
            };
        }
    }
}
