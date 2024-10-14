using Microsoft.EntityFrameworkCore;
using RSSreader.Models;

namespace RSSreader.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new RSSreaderContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<RSSreaderContext>>());
        
        context.Database.EnsureCreated();

        // Look for any feeds.
        if (context.Feed.Any())
        {
            return; // DB has been seeded
        }
        
        context.Feed.AddRange(
                new Feed
                {
                    FeedTitle = "NYT > World News",
                    FeedLink = "https://rss.nytimes.com/services/xml/rss/nyt/World.xml",
                    Articles = new List<Article>
                    {
                        new()
                        {
                            Title = "Live Updates: Nobel Peace Prize Is Awarded to Japanese Group of Atomic Bomb Survivors",
                            Summary =
                                "Nihon Hidankyo is a grass-roots movement of survivors from Hiroshima and Nagasaki. The group’s efforts have helped establish a “nuclear taboo,” the Nobel committee said.",
                            PublishDate = DateTime.Parse("Fri, 11 Oct 2024 10:55:13 +0000"),
                            Link = "https://www.nytimes.com/2024/10/11/world/asia/nobel-peace-prize-nihon-hidanyuo-atomic-bomb.html"
                        },
                        new()
                        {
                            Title = "A Woman Won South Korea’s First Literature Nobel. That Says a Lot.",
                            Summary =
                                "While Han Kang’s victory was celebrated as a crowning cultural achievement for her country, her work also represents a form of rebellion against its culture.",
                            PublishDate = DateTime.Parse("Fri, 11 Oct 2024 06:52:01 +0000"),
                            Link = "https://www.nytimes.com/2024/10/11/world/asia/han-kang-nobel-south-korea.html"
                        },
                        new()
                        {
                            Title = "Rescuers Search for Survivors After Israel Strikes Central Beirut",
                            Summary =
                                "At least 22 people were killed in the attack, Lebanese officials said, as Israel’s bombing campaign against Hezbollah militants continued.",
                            PublishDate = DateTime.Parse("Fri, 11 Oct 2024 10:56:33 +0000"),
                            Link = "https://www.nytimes.com/2024/10/11/world/middleeast/israel-strikes-beirut-lebanon.html"
                        }
                    }
                }
            );
        
        context.SaveChanges();
    }
}