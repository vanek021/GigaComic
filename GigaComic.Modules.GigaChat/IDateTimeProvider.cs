namespace GigaComic.Modules.GigaChat
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime FromUtcMilliseconds(long seconds);
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        DateTime IDateTimeProvider.FromUtcMilliseconds(long seconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds).LocalDateTime;
        }
    }
}
