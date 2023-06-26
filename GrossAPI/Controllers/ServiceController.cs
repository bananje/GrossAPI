using AutoMapper;
using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Models.ViewModel;
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

        [HttpGet("services")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAll()
        {
            var services = _db.Services.Include(u => u.Categories).ToList();

            if (services == null)
                return NotFound();

            List<ServiceVM> servicesList = new List<ServiceVM>();
            foreach (var service in services)
            {
                ServiceVM servicesDTO = new ServiceVM
                {
                    Id = service.Id.ToString(),
                    Title = service.Title,
                    Price = service.Price,
                    Category = service.Categories.Title
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
            var services = await _db.Services.Where(search => search.Title.StartsWith(searchString.ToLower())).Include(u => u.Categories).ToListAsync();
            if (services == null)
                return NotFound();

            List<ServiceVM> servicesList = new List<ServiceVM>();
            foreach (var service in services)
            {
                ServiceVM servicesDTO = new ServiceVM
                {
                    Title = service.Title,
                    Price = service.Price,
                    Category = service.Categories.Title
                };
                servicesList.Add(servicesDTO);
            }

            return Ok(servicesList);
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
            List<Services> servicesList = new List<Services>();
            List<ServiceVM> listView = new List<ServiceVM>();

            if (value == "category")
            {
                var filteredByCategory = ascending ? _db.Services.Include(u => u.Categories).OrderBy(filter => filter.Title)
                                                   : _db.Services.Include(u => u.Categories).OrderByDescending(filter => filter.Title);
                foreach (var item in filteredByCategory)
                {
                    ServiceVM services = new ServiceVM
                    {
                        Title = item.Title,
                        Category = item.Categories.Title,
                        Price = item.Price,
                    };
                    listView.Add(services);
                }
                return Ok(listView);
            }

            switch (value)
            {
                case "title":
                    if (!ascending)
                    {
                        servicesList = _db.Services.OrderByDescending(filter => filter.Title).Include(u => u.Categories).ToList();
                    }
                    else
                    {
                        servicesList = _db.Services.OrderBy(filter => filter.Title).Include(u => u.Categories).ToList();
                    }
                    break;

                case "price":
                    if (!ascending)
                    {
                        servicesList = _db.Services.OrderByDescending(filter => filter.Price).Include(u => u.Categories).ToList();
                    }
                    else
                    {
                        servicesList = _db.Services.OrderBy(filter => filter.Price).Include(u => u.Categories).ToList();
                    }
                    break;

                default:
                    return NotFound();
            }

            foreach (var item in servicesList)
            {
                ServiceVM service = new ServiceVM
                {
                    Title = item.Title,
                    Price = item.Price,
                    Category = item.Categories.Title
                };
                listView.Add(service);
            }

            return Ok(listView);
        }

        [Authorize(Roles = WC.AdminRoleId)]
        [HttpPost("addservice")]
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

        [Authorize(Roles = WC.AdminRoleId)]
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


        [Authorize(Roles = WC.AdminRoleId)]
        [HttpDelete("deleteservice/{id}", Name = "DeleteService")]
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
