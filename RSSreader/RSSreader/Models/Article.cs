using System.ComponentModel.DataAnnotations;

namespace RSSreader.Models;

public class Article
{
    public int ArticleId { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    [DataType(DataType.Date)]
    public DateTime PublishDate { get; set; }
    // public string? ImageUrl { get; set; }
    public string Link { get; set; }
    // public string? LinkUrl { get; set; }
    // public string? LinkText { get; set; }
}