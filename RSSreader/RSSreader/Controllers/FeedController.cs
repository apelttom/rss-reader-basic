using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RSSreader.Data;
using RSSreader.Models;

namespace RSSreader.Controllers
{
    public class FeedController : Controller
    {
        private readonly RSSreaderContext _context;

        public FeedController(RSSreaderContext context)
        {
            _context = context;
        }

        // GET: Feeds
        public async Task<IActionResult> Index()
        {
            return View(await _context.Feed.Include(feed => feed.Articles).ToListAsync());
        }

        // GET: Feeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feed
                .FirstOrDefaultAsync(m => m.FeedId == id);
            if (feed == null)
            {
                return NotFound();
            }

            return View(feed);
        }

        // GET: Feeds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedId,FeedTitle,FeedLink")] Feed feed)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feed);
        }

        // GET: Feeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feed.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }
            return View(feed);
        }

        // POST: Feeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedId,FeedTitle,FeedLink")] Feed feed)
        {
            if (id != feed.FeedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedExists(feed.FeedId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(feed);
        }

        // GET: Feeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feed
                .FirstOrDefaultAsync(m => m.FeedId == id);
            if (feed == null)
            {
                return NotFound();
            }

            return View(feed);
        }

        // POST: Feeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feed = await _context.Feed.FindAsync(id);
            if (feed != null)
            {
                _context.Feed.Remove(feed);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedExists(int id)
        {
            return _context.Feed.Any(e => e.FeedId == id);
        }

        public async Task<IActionResult> ReloadArticles()
        {
            var feeds = await _context.Feed.ToListAsync(); // load feeds from DB
            foreach (var feed in feeds)
            {
                var feedLink = feed.FeedLink;
                // get online version of each feed
                var onlineFeed = await RssChannelLoader.LoadFeed(feedLink);
                if (onlineFeed.FeedTitle != feed.FeedTitle)
                    Console.WriteLine($"Feed under URL: {feedLink} changed title from {feed.FeedTitle} to {onlineFeed.FeedTitle}");
                feed.Articles = onlineFeed.Articles; // update articles for given feed 
                _context.Update(feed); // send update to DB
            }
            await _context.SaveChangesAsync(); // write all updates to the DB
            return RedirectToAction(nameof(Index));
        }
    }
}
