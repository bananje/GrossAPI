namespace GrossAPI.Models.DTOModel
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public ResponseDTO ResponseDTO { get; set; }
    }
}
