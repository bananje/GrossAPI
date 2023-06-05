using Azure;
using GrossAPI.Models.DTOModel;

namespace GrossAPI.Models.ViewModel
{
    public class OrderVM
    {
        public OrderDTO Order { get; set; }
        public List<Services> Services { get; set; }
    }
}
