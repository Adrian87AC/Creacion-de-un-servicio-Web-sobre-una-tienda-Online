using System;
using System.Collections.Generic;

namespace TiendaWebService.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; }
        public List<DetallePedido> Detalles { get; set; }
    }

    public class DetallePedido
    {
        public int DetalleID { get; set; }
        public int PedidoID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}