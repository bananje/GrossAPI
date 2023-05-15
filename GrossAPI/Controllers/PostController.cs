using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Models.RequestModel;
using GrossAPI.Models.ViewModel;
using GrossAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrossAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
           _db= db;
           _webHostEnvironment= webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<PostVM>> GetPosts()
        {
            var posts = await _db.Posts.ToListAsync();
            var postsImages = await _db.Images.Where(u => u.PostId != null).Select(u => u.IndexImg + u.Extension).ToListAsync();

            PostVM postVM = new PostVM
            {
                Post = posts,
                Image = postsImages
            };

            return Ok(postVM);
            //if(posts == null)
            //    return NotFound();

            //List<PostVM> list = new List<PostVM>();
            //foreach (var post in postsImages)
            //{

            //}



        }

        [HttpPost]
        public async Task<ActionResult> AddPost([FromForm]PostRM postRM)
        {
            if (postRM.Post == null)
                return BadRequest();

            Posts post = new Posts
            {
                Id = Guid.NewGuid(),
                ReleaseDate = DateTime.Now,
                Description = postRM.Post.Description,
                ShortDescription = postRM.Post.ShortDescription,
                Header = postRM.Post.Header,
                CreatedByUserId = "1",
            };

            try
            {
                _db.Posts.Add(post);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (postRM.Image != null)
            {
                for (int i = 0; i != postRM.Image.Images.Count; i++)
                {
                    string path = _webHostEnvironment.WebRootPath + WC.PathPostImage;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(postRM.Image.Images[i].FileName);

                    using (var fileStream = new FileStream(Path.Combine(path, fileName + extension), FileMode.Create))
                    {
                        postRM.Image.Images[i].CopyTo(fileStream);
                    }
                    Images image = new Images
                    {
                        Id = Guid.NewGuid(),
                        IndexImg = fileName,
                        Extension = extension,
                        PostId = _db.Posts.OrderByDescending(u => u.Id).FirstOrDefault().Id,
                        ReportId = null
                    };
                    _db.Images.Add(image);
                }

                try
                {
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return Ok(postRM);
        }
    }
}
