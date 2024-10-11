namespace RSSreader.Models;

public class Feed
{
    public int FeedId { get; set; }
    public string FeedTitle { get; set; }
    public string FeedLink { get; set; }
    public ICollection<Article> Articles { get; set; }
}