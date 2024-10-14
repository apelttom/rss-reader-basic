using System.Globalization;
using System.Xml.Linq;
using RSSreader.Models;

namespace RSSreader.Controllers;

public static class RssChannelLoader
{
    public static async Task<Feed> LoadFeed(string url)
    {
        var feed = new Feed();
        try
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var doc = XDocument.Parse(await response.Content.ReadAsStringAsync());

            // Expecting RSS 2.0 format with channel and item tags
            var channelDescendants = doc.Descendants("channel").ToList();

            var feedFeedTitle = channelDescendants.FirstOrDefault()?.Descendants("title").FirstOrDefault()?.Value;
            feed.FeedTitle = feedFeedTitle ?? "No Feed Title";

            var feedLink = channelDescendants.FirstOrDefault()?.Descendants("link").FirstOrDefault()?.Value;
            feed.FeedLink = feedLink ?? "No Feed Link";

            feed.Articles = new List<Article>();
            var items = doc.Descendants("item").ToList();
            foreach (var item in items)
            {
                var pubDateString = item.Descendants("pubDate").FirstOrDefault()?.Value;
                var pubDate = pubDateString != null ? DateTime.Parse(pubDateString) : DateTime.MinValue;
                var article = new Article()
                {
                    Title = item.Descendants("title").FirstOrDefault()?.Value ?? "No Article Title",
                    Summary = item.Descendants("description").FirstOrDefault()?.Value ?? "No Article Summary",
                    PublishDate = pubDate,
                    Link = item.Descendants("link").FirstOrDefault()?.Value ?? "No Article Link"
                };
                feed.Articles.Add(article);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return feed;
    }
}