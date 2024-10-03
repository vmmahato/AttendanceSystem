namespace AttendanceSystem.ExcelResponse
{
   public class ExcelResponse<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public static ExcelResponse<T> GetResult(int code, string Message, T data = default(T))
        {
            return new ExcelResponse<T>
            {
                Code = code,
                Message = Message,
                Data = data
            };
        }
    }
}
