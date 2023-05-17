using GrossAPI.DataAccess;
using GrossAPI.Models.RequestModel;
using GrossAPI.Models;
using GrossAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using GrossAPI.Models.DTOModel;
using GrossAPI.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace GrossAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("reports", Name = "GetReports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReportRM>> GetReports()
        {
            var reports = await _db.Reports.ToListAsync();
            var postImages = await _db.Images.ToListAsync();

            if (reports == null)
                return BadRequest();

            List<ReportVM> reportVMList = new List<ReportVM>();
            foreach (var report in reports)
            {
                ReportDTO reportDTO = new ReportDTO
                {
                    Description = report.Description,
                    Header = report.Header,
                    Position = report.Position,
                };
                var images = postImages.Where(u => u.ReportId == report.Id).ToList();
                ReportVM reportVM = new ReportVM();
                List<string> imagesList = new List<string>();

                if (images != null)
                {
                    images.ForEach(img => { imagesList.Add(img.IndexImg + img.Extension); reportVM.Image = imagesList; });
                }

                reportVM.Report = reportDTO;
                reportVMList.Add(reportVM);
            }
            return Ok(reportVMList);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddReport([FromForm] ReportRM reportRM)
        {
            if (reportRM.Report == null)
                return BadRequest();           

            var reportDTO = reportRM.Report;
            Reports report = new Reports
            {
                Description = reportDTO.Description,
                Position =  reportDTO.Position,
                Header = reportDTO.Position,
                CreatedByUserId = "1"
            };
            try
            {
                _db.Reports.Add(report);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (reportRM.Image != null)
            {
                string path = _webHostEnvironment.WebRootPath + WC.PathReportImage;
                var reportId = _db.Reports.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                ImageHelper imageHelper = new ImageHelper(_db);
                imageHelper.AddImages(reportRM.Image, path, null, reportId);               
            }
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(reportRM);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteReport(int id)
        {
            var report = await _db.Reports.FindAsync(id);
            var postImages = await _db.Images.ToListAsync();

            if (report == null || id <= 0) return BadRequest();

            try
            {
                var images = await _db.Images.Where(u => u.ReportId == report.Id).ToListAsync();
                if (images != null)
                {
                    string upload = _webHostEnvironment.WebRootPath + WC.PathReportImage;
                    ImageHelper imageHelper = new ImageHelper(_db);
                    imageHelper.DeleteImages(upload, images);                    
                }

                _db.Reports.Remove(report);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateReport(int id, [FromForm] ReportRM obj)
        {
            if (obj.Report == null || id <= 0) return BadRequest();

            var report = await _db.Reports.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            if (obj.Image != null)
            {
                var postImages = await _db.Images.AsNoTracking().Where(u => u.ReportId == report.Id).ToListAsync();
                string path = _webHostEnvironment.WebRootPath + WC.PathReportImage;
                ImageHelper imageHelper = new ImageHelper(_db);

                if (postImages.Count > 0)
                {
                    imageHelper.DeleteImages(path, postImages);
                }
                imageHelper.AddImages(obj.Image, path, null, report.Id);
                
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            if (report == null) return NotFound();

            if (obj.Report.Description == null || obj.Report.Header == null || obj.Report.Position == null)
            {
                Reflection.ReloadProperties(report, obj.Report);              
            }
            Reports reportToDb = new Reports
            {
                Description = obj.Report.Description,
                Header = obj.Report.Header,
                Position = obj.Report.Position,
                CreatedByUserId = report.CreatedByUserId,
                Id = report.Id
            };
            try
            {
                _db.Reports.Update(reportToDb);
                await _db.SaveChangesAsync();
            }
            catch (Exception EX)
            {
                return BadRequest(EX.Message);
            }
            return NoContent();
        }
    }
}

