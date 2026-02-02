using System;

namespace TiendaWebService.Models
{
    public class ReporteVentas
    {
        public decimal TotalVendido { get; set; }
        public int TotalPedidos { get; set; }
        public int TotalProductosVendidos { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public ProductoMasVendido[] ProductosPopulares { get; set; }
    }

    public class ProductoMasVendido
    {
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadVendida { get; set; }
        public decimal TotalGenerado { get; set; }
    }
}