namespace MCS.Common.ApiControllerResults
{
    public class DeleteResult : BaseResult
    {
        public static DeleteResult Create(StatusCode statusCode)
        {
            return new DeleteResult
            {
                StatusCode = statusCode
            };
        }
    }
}
