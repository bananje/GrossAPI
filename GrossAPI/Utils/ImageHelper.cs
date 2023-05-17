using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace GrossAPI.Utils
{
    public class ImageHelper
    {
        private readonly ApplicationDBContext _db;
        public ImageHelper(ApplicationDBContext db)
        {
            _db = db;
        }     

        public void AddImages(ImageDTO imageDTO, string path, int? postId, int? reportId)
        {
            var images = imageDTO.Images;
            for (int i = 0; i != images.Count; i++)
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(images[i].FileName);                

                using (var fileStream = new FileStream(Path.Combine(path, fileName + extension), FileMode.Create))
                {
                    images[i].CopyTo(fileStream);
                }
                Images image = new Images
                {
                    Id = Guid.NewGuid(),
                    IndexImg = fileName,
                    Extension = extension,
                    PostId = postId,
                    ReportId = reportId
                };
                _db.Images.Add(image);
            }            
        }

        public void DeleteImages(string path, List<Images> images)
        {
            foreach (var img in images)
            {
                var file = Path.Combine(path, img.IndexImg + img.Extension);
                if (File.Exists(file)) File.Delete(file);
            }
            _db.Images.RemoveRange(images);
        }
    }
}
