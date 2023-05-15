using GrossAPI.Models.DTOModel;
using GrossAPI.Models;
using Microsoft.AspNetCore.Mvc;
using GrossAPI.DataAccess;

namespace GrossAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _db;
        public CategoryController(ApplicationDBContext db)
        {
            _db= db;
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAll()
        {
            var categories = _db.Categories.ToList();

            if (categories == null)
                return NotFound();
           
            return Ok(categories);
        }

        [HttpPost("categorycreate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CategoryDTO obj)
        {
            if (obj == null)
                return BadRequest();

            try
            {
                Categories category = new Categories
                {
                    Id = Guid.NewGuid(),
                    Title = obj.Title
                };

                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(Guid id, CategoryDTO obj)
        {
            if (obj == null) return BadRequest();

            var categories = await _db.Categories.FindAsync(id);

            if (categories != null)
            {
                categories.Title = obj.Title;
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryDTO>> DeleteCategory(Guid id)
        {
            var categories = await _db.Categories.FindAsync(id);
            if (categories == null) return NotFound();

            try
            {
                _db.Categories.Remove(categories);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
