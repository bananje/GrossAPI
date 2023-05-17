using Azure;
using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Models.RequestModel;
using GrossAPI.Models.ViewModel;
using GrossAPI.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

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

        [HttpGet("posts", Name = "GetPosts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostVM>> GetPosts()
        {           
           var posts = await _db.Posts.ToListAsync();
           var postImages = await _db.Images.ToListAsync();

           if (posts == null)
               return BadRequest();        

           List<PostVM> postVMList = new List<PostVM>();
           foreach (var post in posts)
           {
                PostDTO postDTO = new PostDTO
                {
                    Description = post.Description,
                    Header = post.Header,
                    ShortDescription= post.ShortDescription,
                };
                var images = postImages.Where(u => u.PostId == post.Id).ToList();
                PostVM postVM = new PostVM();
                List<string> imagesList = new List<string>();

                if (images != null)
                {
                    images.ForEach(img => { imagesList.Add(img.IndexImg + img.Extension); postVM.Image = imagesList; });
                }       
              
              postVM.Post = postDTO;
              postVMList.Add(postVM);
           }                  
           return Ok(postVMList);          
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddPost([FromForm]PostRM postRM)
        {
            if (postRM.Post == null)
                return BadRequest();

            Posts post = new Posts
            {
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
                string path = _webHostEnvironment.WebRootPath + WC.PathPostImage;
                var postId = _db.Posts.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                ImageHelper imageHelper = new ImageHelper(_db);
                imageHelper.AddImages(postRM.Image, path, postId, null);

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return Ok(postRM);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeletePost(int id)
        {          
            var post = await _db.Posts.FindAsync(id);
            var postImages = await _db.Images.ToListAsync();

            if (post == null || id <= 0) return BadRequest();

            try
            {
                var images = await _db.Images.Where(u => u.PostId== post.Id).ToListAsync();
                if(images != null)
                {
                    string upload = _webHostEnvironment.WebRootPath + WC.PathPostImage;
                    ImageHelper imageHelper = new ImageHelper(_db);
                    imageHelper.DeleteImages(upload, images);
                }
               
                _db.Posts.Remove(post);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePost(int id, [FromForm] PostRM obj)
        {
            if(obj.Post == null|| id <= 0) return BadRequest();

            var post = await _db.Posts.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            if(obj.Image != null)
            {
                var postImages = await _db.Images.AsNoTracking().Where(u => u.PostId == post.Id).ToListAsync();
                string path = _webHostEnvironment.WebRootPath + WC.PathPostImage;
                ImageHelper imageHelper = new ImageHelper(_db);

                if (postImages.Count > 0)
                {
                    imageHelper.DeleteImages(path, postImages);
                }
                imageHelper.AddImages(obj.Image, path, post.Id, null);
                
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            if(post == null) return NotFound();

            if (obj.Post.Description == null || obj.Post.Header == null || obj.Post.ShortDescription == null)
            {
                Reflection.ReloadProperties(post, obj.Post);
            }
            Posts postToDb = new Posts
            {
                Description = obj.Post.Description,
                Header = obj.Post.Header,
                ShortDescription = obj.Post.ShortDescription,
                CreatedByUserId = post.CreatedByUserId,
                Id = post.Id,
                ReleaseDate = DateTime.Now
            };         
            try
            {
                _db.Posts.Update(postToDb);
                await _db.SaveChangesAsync();
            }
            catch(Exception EX)
            {
                return BadRequest(EX.Message);
            }           
            return NoContent();
        }
    }
}