namespace AdventureWorksApi.Models
{
    public class ProductoDto
    {
        public int ProductID { get; set; }
        public String Name { get; set; }
        public Decimal ListPrice { get; set; }
        public String? Color { get; set; }
    }
}
