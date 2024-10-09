using Microsoft.AspNetCore.Mvc;
using RSSreader.Models;

namespace RSSreader.Controllers;

public class FeedController : Controller
{
    private readonly ILogger<FeedController> _logger;

    public FeedController(ILogger<FeedController> logger)
    {
        _logger = logger;
    }

    public IActionResult Homepage()
    {
        ViewData["Feeds"] = GetFeeds();
        ViewData["FeedsSize"] = GetFeeds().Count;
        return View(GetFeeds());
    }

    public List<FeedViewModel> GetFeeds()
    {
        var feeds = new List<FeedViewModel>();
        feeds.Add(new FeedViewModel { FeedLink = "https://rss.nytimes.com/services/xml/rss/nyt/World.xml"});
        feeds.Add(new FeedViewModel { FeedLink = "https://hnrss.org/newest"});
        return feeds;
    }
}