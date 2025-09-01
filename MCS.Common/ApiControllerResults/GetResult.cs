namespace MCS.Common.ApiControllerResults
{
    public class GetResult<T> : BaseResult
    {
        public T Result { get; set; }
        public int? RowsCount { get; set; }

        public static GetResult<T> Create(StatusCode statusCode, T result, int? rowsCount)
        {
            return new GetResult<T>
            {
                StatusCode = statusCode,
                Result = result,
                RowsCount = rowsCount
            };
        }
    }
}
