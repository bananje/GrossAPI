namespace GrossAPI.Models.DTOModel
{
    public class OrderDTO
    {
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string CreatedByUser { get; set; }
        public string? FullName { get; set; }
        public string? TelNumber{ get; set; }
        public string? Email { get; set; }
    }
}
