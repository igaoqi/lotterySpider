namespace LotterySpider.Common.Dtos.Lottery168
{
    public class Json168Root<T>
    {
        public int errorCode { get; set; }
        public string message { get; set; }
        public Result<T> result { get; set; }
    }

    public class Result<T>
    {
        public int businessCode { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}