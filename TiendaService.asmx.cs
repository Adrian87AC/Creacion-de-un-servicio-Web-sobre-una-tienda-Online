using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Services;
using MySql.Data.MySqlClient;
using TiendaWebService.Models;

namespace TiendaWebService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class TiendaService : System.Web.Services.WebService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["TiendaDB"].ConnectionString;

        #region Métodos de Usuario

        [WebMethod(Description = "Valida las credenciales de un usuario")]
        public Usuario ValidarUsuario(string nombreUsuario, string contraseña)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @usuario AND Contraseña = @pass";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                        cmd.Parameters.AddWithValue("@pass", contraseña);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Usuario
                                {
                                    UsuarioID = reader.GetInt32("UsuarioID"),
                                    NombreUsuario = reader.GetString("NombreUsuario"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),
                                    Email = reader.GetString("Email"),
                                    FechaRegistro = reader.GetDateTime("FechaRegistro")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar usuario: " + ex.Message);
            }
            
            return null;
        }

        [WebMethod(Description = "Registra un nuevo usuario en el sistema")]
        public bool RegistrarUsuario(string nombreUsuario, string contraseña, string nombre, string apellido, string email)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Usuarios (NombreUsuario, Contraseña, Nombre, Apellido, Email) 
                                   VALUES (@usuario, @pass, @nombre, @apellido, @email)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                        cmd.Parameters.AddWithValue("@pass", contraseña);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellido", apellido);
                        cmd.Parameters.AddWithValue("@email", email);
                        
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar usuario: " + ex.Message);
            }
        }

        [WebMethod(Description = "Actualiza la información de un usuario existente")]
        public bool ActualizarUsuario(int usuarioID, string nombre, string apellido, string email)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Usuarios 
                                   SET Nombre = @nombre, Apellido = @apellido, Email = @email 
                                   WHERE UsuarioID = @id";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", usuarioID);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellido", apellido);
                        cmd.Parameters.AddWithValue("@email", email);
                        
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar usuario: " + ex.Message);
            }
        }

        [WebMethod(Description = "Elimina un usuario del sistema")]
        public bool EliminarUsuario(int usuarioID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Usuarios WHERE UsuarioID = @id";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", usuarioID);
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario: " + ex.Message);
            }
        }

        [WebMethod(Description = "Obtiene la lista de todos los usuarios")]
        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Usuarios";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                UsuarioID = reader.GetInt32("UsuarioID"),
                                NombreUsuario = reader.GetString("NombreUsuario"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Email = reader.GetString("Email"),
                                FechaRegistro = reader.GetDateTime("FechaRegistro")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener usuarios: " + ex.Message);
            }
            
            return usuarios;
        }

        #endregion

        #region Métodos de Producto

        [WebMethod(Description = "Crea un nuevo producto")]
        public bool CrearProducto(string nombre, string descripcion, decimal precio, int stock, int categoriaID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaID) 
                                   VALUES (@nombre, @desc, @precio, @stock, @catID)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@desc", descripcion);
                        cmd.Parameters.AddWithValue("@precio", precio);
                        cmd.Parameters.AddWithValue("@stock", stock);
                        cmd.Parameters.AddWithValue("@catID", categoriaID);
                        
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear producto: " + ex.Message);
            }
        }

        [WebMethod(Description = "Actualiza la información de un producto")]
        public bool ActualizarProducto(int productoID, string nombre, string descripcion, decimal precio, int stock)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Productos 
                                   SET Nombre = @nombre, Descripcion = @desc, 
                                       Precio = @precio, Stock = @stock 
                                   WHERE ProductoID = @id";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productoID);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@desc", descripcion);
                        cmd.Parameters.AddWithValue("@precio", precio);
                        cmd.Parameters.AddWithValue("@stock", stock);
                        
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar producto: " + ex.Message);
            }
        }

        [WebMethod(Description = "Elimina un producto")]
        public bool EliminarProducto(int productoID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Productos WHERE ProductoID = @id";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productoID);
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar producto: " + ex.Message);
            }
        }

        [WebMethod(Description = "Obtiene todos los productos")]
        public List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Productos";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto
                            {
                                ProductoID = reader.GetInt32("ProductoID"),
                                Nombre = reader.GetString("Nombre"),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString("Descripcion"),
                                Precio = reader.GetDecimal("Precio"),
                                Stock = reader.GetInt32("Stock"),
                                CategoriaID = reader.GetInt32("CategoriaID")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener productos: " + ex.Message);
            }
            
            return productos;
        }

        [WebMethod(Description = "Busca productos por nombre")]
        public List<Producto> BuscarProductos(string termino)
        {
            List<Producto> productos = new List<Producto>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Productos WHERE Nombre LIKE @termino";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@termino", "%" + termino + "%");
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    ProductoID = reader.GetInt32("ProductoID"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString("Descripcion"),
                                    Precio = reader.GetDecimal("Precio"),
                                    Stock = reader.GetInt32("Stock"),
                                    CategoriaID = reader.GetInt32("CategoriaID")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar productos: " + ex.Message);
            }
            
            return productos;
        }

        #endregion

        #region Métodos de Pedido

        [WebMethod(Description = "Crea un nuevo pedido con sus detalles")]
        public int CrearPedido(int usuarioID, List<DetallePedido> detalles)
        {
            MySqlConnection conn = null;
            MySqlTransaction transaction = null;
            
            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
                transaction = conn.BeginTransaction();
                
                // Insertar pedido
                string queryPedido = "INSERT INTO Pedidos (UsuarioID, Estado) VALUES (@usuarioID, 'Pendiente'); SELECT LAST_INSERT_ID();";
                int pedidoID;
                
                using (MySqlCommand cmd = new MySqlCommand(queryPedido, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@usuarioID", usuarioID);
                    pedidoID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                
                // Insertar detalles
                string queryDetalle = @"INSERT INTO DetallePedidos (PedidoID, ProductoID, Cantidad, PrecioUnitario) 
                                      VALUES (@pedidoID, @productoID, @cantidad, @precio)";
                
                foreach (var detalle in detalles)
                {
                    using (MySqlCommand cmd = new MySqlCommand(queryDetalle, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@pedidoID", pedidoID);
                        cmd.Parameters.AddWithValue("@productoID", detalle.ProductoID);
                        cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@precio", detalle.PrecioUnitario);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Actualizar stock
                    string queryStock = "UPDATE Productos SET Stock = Stock - @cantidad WHERE ProductoID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(queryStock, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@id", detalle.ProductoID);
                        cmd.ExecuteNonQuery();
                    }
                }
                
                transaction.Commit();
                return pedidoID;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception("Error al crear pedido: " + ex.Message);
            }
            finally
            {
                conn?.Close();
            }
        }

        [WebMethod(Description = "Obtiene los pedidos de un usuario")]
        public List<Pedido> ObtenerPedidosPorUsuario(int usuarioID)
        {
            List<Pedido> pedidos = new List<Pedido>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Pedidos WHERE UsuarioID = @usuarioID";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuarioID", usuarioID);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pedidos.Add(new Pedido
                                {
                                    PedidoID = reader.GetInt32("PedidoID"),
                                    UsuarioID = reader.GetInt32("UsuarioID"),
                                    FechaPedido = reader.GetDateTime("FechaPedido"),
                                    Estado = reader.GetString("Estado")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener pedidos: " + ex.Message);
            }
            
            return pedidos;
        }

        [WebMethod(Description = "Actualiza el estado de un pedido")]
        public bool ActualizarEstadoPedido(int pedidoID, string nuevoEstado)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Pedidos SET Estado = @estado WHERE PedidoID = @id";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                        cmd.Parameters.AddWithValue("@id", pedidoID);
                        
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar estado: " + ex.Message);
            }
        }

        [WebMethod(Description = "Obtiene el historial de compras de un usuario")]
        public List<Pedido> HistorialCompras(int usuarioID)
        {
            return ObtenerPedidosPorUsuario(usuarioID);
        }

        #region Métodos de Categoría

        [WebMethod(Description = "Obtiene todas las categorías")]
        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Categorias";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorias.Add(new Categoria
                            {
                                CategoriaID = reader.GetInt32("CategoriaID"),
                                NombreCategoria = reader.GetString("NombreCategoria")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarLogError("Error", "Error al obtener categorías: " + ex.Message, "Sistema");
                throw new Exception("Error al obtener categorías: " + ex.Message);
            }
            
            return categorias;
        }

        [WebMethod(Description = "Busca productos por categoría")]
        public List<Producto> BuscarProductosPorCategoria(int categoriaID)
        {
            List<Producto> productos = new List<Producto>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Productos WHERE CategoriaID = @catID";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@catID", categoriaID);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    ProductoID = reader.GetInt32("ProductoID"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString("Descripcion"),
                                    Precio = reader.GetDecimal("Precio"),
                                    Stock = reader.GetInt32("Stock"),
                                    CategoriaID = reader.GetInt32("CategoriaID")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarLogError("Error", "Error al buscar productos por categoría: " + ex.Message, "Sistema");
                throw new Exception("Error al buscar productos por categoría: " + ex.Message);
            }
            
            return productos;
        }

        #endregion

        #region Métodos de Inventario

        [WebMethod(Description = "Gestiona el inventario de un producto (suma o resta stock)")]
        public bool GestionarInventario(int productoID, int cantidad, string tipoMovimiento)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "";
                    
                    if (tipoMovimiento.ToUpper() == "ENTRADA")
                    {
                        query = "UPDATE Productos SET Stock = Stock + @cantidad WHERE ProductoID = @id";
                    }
                    else if (tipoMovimiento.ToUpper() == "SALIDA")
                    {
                        query = "UPDATE Productos SET Stock = Stock - @cantidad WHERE ProductoID = @id";
                    }
                    else
                    {
                        throw new Exception("Tipo de movimiento inválido. Use 'ENTRADA' o 'SALIDA'");
                    }
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cantidad", cantidad);
                        cmd.Parameters.AddWithValue("@id", productoID);
                        
                        int result = cmd.ExecuteNonQuery();
                        
                        if (result > 0)
                        {
                            RegistrarLogError("Inventario", 
                                $"Movimiento de inventario: ProductoID={productoID}, Cantidad={cantidad}, Tipo={tipoMovimiento}", 
                                "Sistema");
                        }
                        
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarLogError("Error", "Error al gestionar inventario: " + ex.Message, "Sistema");
                throw new Exception("Error al gestionar inventario: " + ex.Message);
            }
        }

        [WebMethod(Description = "Obtiene productos con stock bajo (menos de un umbral)")]
        public List<Producto> ObtenerProductosStockBajo(int umbral)
        {
            List<Producto> productos = new List<Producto>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Productos WHERE Stock < @umbral";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@umbral", umbral);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    ProductoID = reader.GetInt32("ProductoID"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString("Descripcion"),
                                    Precio = reader.GetDecimal("Precio"),
                                    Stock = reader.GetInt32("Stock"),
                                    CategoriaID = reader.GetInt32("CategoriaID")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarLogError("Error", "Error al obtener productos con stock bajo: " + ex.Message, "Sistema");
                throw new Exception("Error al obtener productos con stock bajo: " + ex.Message);
            }
            
            return productos;
        }

        #endregion

        #region Métodos de Reportes

        [WebMethod(Description = "Genera un reporte de ventas en un período de tiempo")]
        public ReporteVentas ReporteVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            ReporteVentas reporte = new ReporteVentas
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Total vendido y cantidad de pedidos
                    string queryTotales = @"
                        SELECT 
                            COUNT(DISTINCT p.PedidoID) as TotalPedidos,
                            SUM(dp.Cantidad) as TotalProductos,
                            SUM(dp.Cantidad * dp.PrecioUnitario) as TotalVendido
                        FROM Pedidos p
                        INNER JOIN DetallePedidos dp ON p.PedidoID = dp.PedidoID
                        WHERE p.FechaPedido BETWEEN @fechaInicio AND @fechaFin";
                    
                    using (MySqlCommand cmd = new MySqlCommand(queryTotales, conn))
                    {
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                reporte.TotalPedidos = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                reporte.TotalProductosVendidos = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                                reporte.TotalVendido = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                            }
                        }
                    }
                    
                    // Productos más vendidos
                    string queryPopulares = @"
                        SELECT 
                            prod.ProductoID,
                            prod.Nombre,
                            SUM(dp.Cantidad) as CantidadVendida,
                            SUM(dp.Cantidad * dp.PrecioUnitario) as TotalGenerado
                        FROM DetallePedidos dp
                        INNER JOIN Productos prod ON dp.ProductoID = prod.ProductoID
                        INNER JOIN Pedidos p ON dp.PedidoID = p.PedidoID
                        WHERE p.FechaPedido BETWEEN @fechaInicio AND @fechaFin
                        GROUP BY prod.ProductoID, prod.Nombre
                        ORDER BY CantidadVendida DESC
                        LIMIT 10";
                    
                    List<ProductoMasVendido> populares = new List<ProductoMasVendido>();
                    
                    using (MySqlCommand cmd = new MySqlCommand(queryPopulares, conn))
                    {
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                populares.Add(new ProductoMasVendido
                                {
                                    ProductoID = reader.GetInt32("ProductoID"),
                                    NombreProducto = reader.GetString("Nombre"),
                                    CantidadVendida = reader.GetInt32("CantidadVendida"),
                                    TotalGenerado = reader.GetDecimal("TotalGenerado")
                                });
                            }
                        }
                    }
                    
                    reporte.ProductosPopulares = populares.ToArray();
                }
                
                RegistrarLogError("Reporte", 
                    $"Reporte de ventas generado: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}", 
                    "Sistema");
            }
            catch (Exception ex)
            {
                RegistrarLogError("Error", "Error al generar reporte de ventas: " + ex.Message, "Sistema");
                throw new Exception("Error al generar reporte de ventas: " + ex.Message);
            }
            
            return reporte;
        }

        [WebMethod(Description = "Obtiene estadísticas de ventas por categoría")]
        public List<Dictionary<string, object>> VentasPorCategoria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Dictionary<string, object>> resultado = new List<Dictionary<string, object>>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            c.NombreCategoria,
                            COUNT(DISTINCT p.PedidoID) as TotalPedidos,
                            SUM(dp.Cantidad) as ProductosVendidos,
                            SUM(dp.Cantidad * dp.PrecioUnitario) as TotalVentas
                        FROM Categorias c
                        INNER JOIN Productos prod ON c.CategoriaID = prod.CategoriaID
                        INNER JOIN DetallePedidos dp ON prod.ProductoID = dp.ProductoID
                        INNER JOIN Pedidos p ON dp.PedidoID = p.PedidoID
                        WHERE p.FechaPedido BETWEEN @fechaInicio AND @fechaFin
                        GROUP BY c.CategoriaID, c.NombreCategoria
                        ORDER BY TotalVentas DESC";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var dict = new Dictionary<string, object>
                                {
                                    { "Categoria", reader.GetString("NombreCategoria") },
                                    { "TotalPedidos", reader.GetInt32("TotalPedidos") },
                                    { "ProductosVendidos", reader.GetInt64("ProductosVendidos") },
                                    { "TotalVentas", reader.GetDecimal("TotalVentas") }
                                };
                                resultado.Add(dict);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarLogError("Error", "Error al obtener ventas por categoría: " + ex.Message, "Sistema");
                throw new Exception("Error al obtener ventas por categoría: " + ex.Message);
            }
            
            return resultado;
        }

        #endregion

        #region Métodos de Logs

        [WebMethod(Description = "Registra un error o evento en el sistema")]
        public bool RegistrarLogError(string tipoLog, string mensaje, string usuario)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Logs (TipoLog, Mensaje, Usuario) 
                                   VALUES (@tipo, @mensaje, @usuario)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tipo", tipoLog);
                        cmd.Parameters.AddWithValue("@mensaje", mensaje);
                        cmd.Parameters.AddWithValue("@usuario", usuario ?? "Sistema");
                        
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception)
            {
                // No lanzar excepción para evitar bucles infinitos
                return false;
            }
        }

        [WebMethod(Description = "Obtiene los logs registrados en el sistema")]
        public List<Dictionary<string, object>> ObtenerLogs(int limite)
        {
            List<Dictionary<string, object>> logs = new List<Dictionary<string, object>>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Logs ORDER BY FechaHora DESC LIMIT @limite";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@limite", limite);
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var dict = new Dictionary<string, object>
                                {
                                    { "LogID", reader.GetInt32("LogID") },
                                    { "TipoLog", reader.GetString("TipoLog") },
                                    { "Mensaje", reader.GetString("Mensaje") },
                                    { "FechaHora", reader.GetDateTime("FechaHora") },
                                    { "Usuario", reader.IsDBNull(reader.GetOrdinal("Usuario")) ? "" : reader.GetString("Usuario") }
                                };
                                logs.Add(dict);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener logs: " + ex.Message);
            }
            
            return logs;
        }

        [WebMethod(Description = "Limpia logs antiguos del sistema")]
        public bool LimpiarLogsAntiguos(int diasAntiguedad)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Logs WHERE FechaHora < DATE_SUB(NOW(), INTERVAL @dias DAY)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dias", diasAntiguedad);
                        int result = cmd.ExecuteNonQuery();
                        
                        RegistrarLogError("Mantenimiento", 
                            $"Se eliminaron {result} logs con más de {diasAntiguedad} días", 
                            "Sistema");
                        
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al limpiar logs: " + ex.Message);
            }
        }

        #endregion
    }
}