namespace GigaComic.Modules.GigaChat
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime FromUtcMilliseconds(long milliseconds);
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        DateTime IDateTimeProvider.FromUtcMilliseconds(long milliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).LocalDateTime;
        }
    }
}
