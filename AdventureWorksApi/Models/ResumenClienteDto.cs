namespace AdventureWorksApi.Models
{
    public class ResumenClienteDto
    {
        public int CantidadDeOrdenes { get; set; }
        public decimal TotalGastado { get; set; }
        public DateTime? PrimeraOrden { get; set ; }
        public DateTime? UltimaOrden { get; set; }
    }
}
