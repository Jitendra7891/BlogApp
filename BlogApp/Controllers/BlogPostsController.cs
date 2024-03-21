
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        public BlogPostsController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts(int? page)
        {
            int pageSize = 3;
            int pageIndex = page ?? 1;

            var totalPosts = await _databaseContext.BlogPosts.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

            var blogPosts = await _databaseContext.BlogPosts
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex = pageIndex;

            return View(blogPosts);
        }


        public ActionResult CreateBlogPost()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult<BlogPost>> CreateBlogPost(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                _databaseContext.BlogPosts.Add(blogPost);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetBlogPosts));
            }
            return View(blogPost);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var blogPost = await _databaseContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }


        [HttpGet]
        public async Task<ActionResult<BlogPost>> Edit(int id)
        {
            var blogPost = await _databaseContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {

                _databaseContext.Update(blogPost);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetBlogPosts));
            }
            return View(blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var blogPost = await _databaseContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _databaseContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            _databaseContext.BlogPosts.Remove(blogPost);
            await _databaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(GetBlogPosts)); 
        }

    }
}
