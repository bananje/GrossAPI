using GrossAPI.Models.DTOModel;

namespace GrossAPI.Models.ViewModel
{
    public class ReportVM
    {
        public ReportDTO Report { get; set; }
        public List<string> Image { get; set; }
    }
}
