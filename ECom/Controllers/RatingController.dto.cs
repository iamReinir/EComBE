namespace ECom.Controllers
{
    public class RatingRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
    }
}
