using AutoMapper;
using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace GrossAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly ApplicationDBContext _db;
        public ServiceController(ApplicationDBContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpGet("services")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAll()
        {
            var services = _db.Services.ToList();

            if (services == null)
                return NotFound();

            List<ServicesDTO> servicesList = new List<ServicesDTO>();
            foreach (var service in services)
            {
                ServicesDTO servicesDTO = new ServicesDTO
                {
                    Title = service.Title,
                    Price = service.Price,
                    CategoryId = service.CategoryID
                };
                servicesList.Add(servicesDTO);
            }
            return Ok(servicesList);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SearchServices(string searchString)
        {
            var service = await _db.Services.Where(search => search.Title.StartsWith(searchString.ToLower())).ToListAsync();
            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> FilterServices(string criteria, bool ascending)
        {
            if (criteria == null || criteria == "" || criteria.Length == 0)
                return BadRequest();

            string value = criteria.ToLower();
            if (value == "category")
            {
                var filteredByCategory = ascending ? _db.Services.Include(u => u.Categories).OrderBy(filter => filter.Title)
                                                   : _db.Services.Include(u => u.Categories).OrderByDescending(filter => filter.Title);
                return Ok(filteredByCategory);
            }

            List<Services> servicesList = new List<Services>();
            switch (value)
            {
                case "title":
                    if (!ascending)
                    {
                        servicesList = _db.Services.OrderByDescending(filter => filter.Title).ToList();
                    }
                    else
                    {
                        servicesList = _db.Services.OrderBy(filter => filter.Title).ToList();
                    }
                    break;

                case "price":
                    if (!ascending)
                    {
                        servicesList = _db.Services.OrderByDescending(filter => filter.Price).ToList();
                    }
                    else
                    {
                        servicesList = _db.Services.OrderBy(filter => filter.Price).ToList();
                    }
                    break;

                default:
                    return NotFound();
            }

            return Ok(servicesList);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ServicesDTO>> CreateService(ServicesDTO obj)
        {
            if (obj == null)
                return BadRequest();

            Services objToDB = new Services
            {
                Id = Guid.NewGuid(),
                Title = obj.Title,
                Price = obj.Price,
                CategoryID = obj.CategoryId
            };

            try
            {
                _db.Services.Add(objToDB);
                await _db.SaveChangesAsync();
                return Ok(obj);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}", Name = "UpdateService")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ServicesDTO>> UpdateService(Guid id,ServicesDTO obj)
        {
            if(obj == null) return BadRequest();

            var service = await _db.Services.FirstOrDefaultAsync(u => u.Id == id);
            
            if(service != null)
            {
                service.Price = obj.Price;
                service.Title = obj.Title;
                service.CategoryID = obj.CategoryId;

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

        [HttpDelete("{id}", Name = "DeleteService")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ServicesDTO>> DeleteService(Guid id)
        {
            var service = await _db.Services.FirstOrDefaultAsync(u => u.Id== id);
            if(service == null) return NotFound();

            try
            {
                _db.Services.Remove(service);
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
