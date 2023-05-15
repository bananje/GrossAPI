using GrossAPI.Models.DTOModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.RequestModel
{
    public class PostRM
    {
        [Required]
        public PostDTO Post { get; set; }
        [ValidateNever]
        public ImageDTO Image { get; set; }
    }
}
