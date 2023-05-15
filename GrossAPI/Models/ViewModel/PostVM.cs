using GrossAPI.Models.DTOModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GrossAPI.Models.ViewModel
{
    public class PostVM
    {
        public List<Posts> Post { get; set; }
        public List<string> Image { get; set; }
    }
}
