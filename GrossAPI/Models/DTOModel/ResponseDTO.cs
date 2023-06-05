using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GrossAPI.Models.DTOModel
{
    public class ResponseDTO
    {
        [ValidateNever]
        public List<Guid> OrderId { get; set; }
        [ValidateNever]
        public List<Guid> ServiceId { get; set; }
    }
}
