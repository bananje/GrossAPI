using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Models.RequestModel;
using GrossAPI.Models.ViewModel;
using GrossAPI.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GrossAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly ApplicationDBContext _db;
        public OrderController(ApplicationDBContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddOrder([FromBody] OrderDTO orderDTO)
        {           
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var statuses = _db.Statuses.AsNoTracking();

            Orders order = new Orders()
            {
                Id = Guid.NewGuid(),
                StatusId = statuses.AsNoTracking().FirstOrDefault(u => u.Id == WC.ActiveStatusId).Id,
                CreatedByUserId = claimsIdentity.Name,
                OrderDate = DateTime.Now,
            };
            await _db.AddAsync(order);
            await _db.SaveChangesAsync();

            foreach (var obj in orderDTO.ResponseDTO.ServiceId)
            {
                Responses responses = new Responses()
                {
                    OrderId = order.Id,
                    ServiceId = obj
                };
                await _db.Responses.AddRangeAsync(responses);
            }         
            _db.SaveChanges();  
            return Ok();
        }
    }
}
