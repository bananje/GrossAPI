using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Models.ViewModel;
using GrossAPI.Utils;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GrossAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackOrdersController : Controller
    {
        private readonly ApplicationDBContext _db;
        public FeedbackOrdersController(ApplicationDBContext db)
        {
            _db= db;
        }

        [HttpPost("addfborder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddOrder([FromBody] FeedbackOrdersDTO order)
        {
            if (order == null)
                return BadRequest();

            string[] fullName = order.FullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (fullName.Length < 3)
                return BadRequest();

            FeedbackOrders objToDb = new FeedbackOrders
            {
                Id = Guid.NewGuid(),
                Surname = fullName[0],
                Name = fullName[1],
                Patronymic = fullName[2],
                Email = order.Email,
                TelNumber = order.TelNumber,
                Status = WC.ActiveStatusId
            };
           
            try
            {
                await _db.FeedbackOrders.AddAsync(objToDb);
                await _db.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        
        [HttpGet("fborders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPosts()
        {
            var fbOrders = await _db.FeedbackOrders.ToListAsync();
            if (fbOrders.Count == 0 || fbOrders == null)
                return NotFound();

            List<FeedBackOrdersVM> orders = new List<FeedBackOrdersVM>();
            foreach (var fbOrder in fbOrders)
            {
                FeedBackOrdersVM feedbackOrdersDTO = new FeedBackOrdersVM()
                {
                    Email = fbOrder.Email,
                    FullName = fbOrder.Surname + " " + fbOrder.Name + " " + fbOrder.Patronymic,
                    TelNumber = fbOrder.TelNumber,
                    Status = fbOrder.Status
                };
                orders.Add(feedbackOrdersDTO);
            }

            return Ok(orders);
        }
    }
}
