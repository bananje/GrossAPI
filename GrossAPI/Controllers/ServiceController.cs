using AutoMapper;
using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Utils;
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

        //[HttpGet("services")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<List<ServicesDTO>>> GetServices(string? searchString, bool ascending, ServicesDTO servicesDTO)
        //{
        //    var servicesList = await _db.Services.ToListAsync();
        //    if (servicesList.Count <= 0 || servicesList == null)
        //        return NotFound();

        //    if (searchString != null || searchString != "")
        //    {
        //        var filteredServices = _db.Services.Where(val => val.));
        //    }
        //}

        [HttpGet("GetAll")]
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

        //public static object GetPropertyValue(object obj, string propertyName)
        //{
        //    return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        //}

        //[HttpGet("search")]
        //public async Task<ActionResult<ServicesDTO>> GetByTitle(bool ascending, string? sortValue, string? title)
        //{
        //    var filteredServices = _db.Services.Where(service =>
        //    (string.IsNullOrEmpty(title) || service.Title.StartsWith(title)));

        //    if (filteredServices == null)
        //        return NotFound();

        //    //ServicesDTO servicesDTO = new ServicesDTO();
        //    //Type type = servicesDTO.GetType();
        //    //PropertyInfo property = type.GetProperty(sortValue);


        //    ////property.SetValue(servicesDTO, sortValue);
        //    ////var propValue = property.GetValue(servicesDTO);
        //    var sortedServices = ascending ? filteredServices.OrderBy(value => value.Title) :
        //        filteredServices.OrderByDescending(value => value.Price);

        //    var services = sortedServices.Select(service => new ServicesDTO()
        //    {
        //        Id = service.Id,
        //        Title = service.Title,
        //        Price = service.Price
        //    });

        //    return Ok(services);
        //}
    }
}
