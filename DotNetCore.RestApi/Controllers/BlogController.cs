using DotNetCore.RestApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DotNetCore.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public BlogController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        var blogs = await _appDbContext.Blogs.ToListAsync();
        return Ok(blogs);
    }

    [HttpGet("{blogId}")]
    public async Task<IActionResult> GetBlog(int blogId)
    {
        try
        {
            var blog = await _appDbContext.Blogs.FirstOrDefaultAsync(x => x.BlogId == blogId);
            if (blog == null)
            {
                return NotFound("No data not found");
            }
            return Ok(blog);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateBlog(BlogDataModel reqModel)
    {
        _appDbContext.Blogs.Add(reqModel);
        var result = await _appDbContext.SaveChangesAsync();
        var message = result > 0 ? "Saving Successful" : "Saving failed";
        return Ok(message);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateBlog(int blogId, BlogDataModel reqModel)
    {
        var blog = await _appDbContext.Blogs.FirstOrDefaultAsync(x => x.BlogId == blogId);
        if (blog == null)
        {
            return NotFound("No data not found");
        }

        if (string.IsNullOrEmpty(blog.BlogTitle))
        {
            return BadRequest("Blog title is required");
        }
        if (string.IsNullOrEmpty(blog.BlogAuthor))
        {
            return BadRequest("Blog author is required");
        }
        if (string.IsNullOrEmpty(blog.BlogContent))
        {
            return BadRequest("Blog content is required");
        }

        reqModel.BlogTitle = reqModel.BlogTitle;
        reqModel.BlogAuthor = reqModel.BlogAuthor;
        reqModel.BlogContent = reqModel.BlogContent;

        int result = await _appDbContext.SaveChangesAsync();
        string message = result > 0 ? "Saving successful" : "Saving failed";
        return Ok(message);
    }
    [HttpPatch("{id}")]
    public IActionResult PathchBLog(int id, BlogDataModel blog)
    {
        var item = _appDbContext.Blogs.FirstOrDefault(x => x.BlogId == id);
        if (item == null)
        {
            return NotFound("No data is not found!");
        }
        if (!string.IsNullOrEmpty(blog.BlogTitle))
        {
            item.BlogTitle = blog.BlogTitle;
        }
        if (!string.IsNullOrEmpty(blog.BlogAuthor))
        {
            item.BlogAuthor = blog.BlogAuthor;
        }
        if (!string.IsNullOrEmpty(blog.BlogContent))
        {
            item.BlogContent = blog.BlogContent;
        }
        int result = _appDbContext.SaveChanges();
        string message = result > 0 ? "Saving successful" : "Saving failed";
        return Ok(message);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBLog(int id)
    {
        var item = _appDbContext.Blogs.FirstOrDefault(x => x.BlogId == id);
        if (item == null)
        {
            return BadRequest("No data is't found");
        }
        _appDbContext.Blogs.Remove(item);
        int result = _appDbContext.SaveChanges();
        string message = result > 0 ? "Delete successful" : "Delete failed";
        return Ok(message);
    }
}