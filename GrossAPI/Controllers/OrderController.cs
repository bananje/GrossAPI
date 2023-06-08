using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Models.RequestModel;
using GrossAPI.Models.ViewModel;
using GrossAPI.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Drawing;
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

        [HttpGet("orders", Name = "GetOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetOrders()
        {
            var orders = await _db.Orders.Include(p=>p.ApplicationUser).ToListAsync();
            if (orders == null)
            {
                return NotFound();
            }
            List<OrderVM> result = new List<OrderVM>();
            foreach (var order in orders)
            {
                OrderDTO orderDTO = new OrderDTO
                {
                    OrderDate = order.OrderDate,
                    Status = WC.ActiveStatusId,
                    CreatedByUser = order.CreatedByUserId,
                    FullName = order.ApplicationUser.Name + " " + order.ApplicationUser.Surname,
                    TelNumber = order.ApplicationUser.TelNumber,
                    Email = order.ApplicationUser.Email,
                };
                List<ServicesDTO> servicesList = new List<ServicesDTO>();
                foreach (var service in order.Services)
                {
                    ServicesDTO servicesDTO = new ServicesDTO
                    {
                        Title = service.Title,
                        Price = service.Price,
                        CategoryId = service.CategoryID
                    };
                    servicesList.Add(servicesDTO);
                }
                OrderVM orderVM = new OrderVM
                {
                    Order = orderDTO,
                    Services = servicesList
                };
                result.Add(orderVM);
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddOrder(OrderVM orderVM)
        {
            if (orderVM == null)
            {
                return BadRequest();
            }

            List<Services> servicesToDb = new List<Services>();
            foreach (var service in orderVM.Services)
            {
                var serviceToDb = await _db.Services.FindAsync(service.Id);
                servicesToDb.Add(serviceToDb);
            }

            Orders orderToDb = new Orders
            {
                OrdersId = Guid.NewGuid(),
                OrderDate = orderVM.Order.OrderDate,
                //StatusId = orderVM.Order.Status,
                CreatedByUserId = orderVM.Order.CreatedByUser,
                Services = servicesToDb
            };

            _db.Add(orderToDb);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();

        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete (Guid Id)
        {
            if (Id == null)
            {
                return BadRequest();
            }

            var orderToDelete = await _db.Orders.FindAsync(Id);

            _db.Remove(orderToDelete);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();

        }

    }
}

//get
//var responses = await _db.Responses.ToListAsync();
//if (responses == null || responses.Count <= 0)
//    return NotFound();

//List<OrderVM> orders = new List<OrderVM>();
//foreach (var response in responses)
//{
//    var order = _db.Orders.FirstOrDefault(u => u.Id == response.OrderId);
//    var services = responses.Where(u => u.OrderId == order.Id).Select(u => u.Service).ToList();

//    OrderVM orderVM = new OrderVM
//    {
//        Order = new OrderDTO
//        {
//            OrderDate = order.OrderDate
//        },
//    };
//    List<Services> services1 = new List<Services>();
//    services.ForEach(o => { services1.Add(o); orderVM.Services = services1; });
//    orders.Add(orderVM);
//}
//return Ok(orders);

//post
//var claimsIdentity = (ClaimsIdentity)User.Identity;      

//Orders order = new Orders()
//{
//    Id = Guid.NewGuid(),

//    CreatedByUserId = claimsIdentity.Name,
//    OrderDate = DateTime.Now,
//};
//await _db.AddAsync(order);
//await _db.SaveChangesAsync();

//foreach (var obj in responseDTO.ServiceId)
//{
//    Responses responses = new Responses()
//    {
//        OrderId = order.Id,
//        ServiceId = obj
//    };
//    await _db.Responses.AddAsync(responses);
//}         
//_db.SaveChanges();  
//return Ok();
