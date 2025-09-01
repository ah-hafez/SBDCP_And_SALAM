namespace MCS.Common.ApiControllerResults
{
    public class GetResultExtraData<T> : BaseResult
        {
            public T Result { get; set; }
            public int? RowsCount { get; set; }
            public T ExtraData;

            public static GetResultExtraData<T> Create(StatusCode statusCode, T result, T extraData, int? rowsCount)
            {
                return new GetResultExtraData<T>
                {
                    StatusCode = statusCode,
                    Result = result,
                    RowsCount = rowsCount,
                    ExtraData = extraData
                };
            }
        }
}
