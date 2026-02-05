# 🛒 Servicio Web TiendaDB - ASP.NET ASMX

Proyecto de servicio web SOAP para gestión de una tienda online desarrollado con ASP.NET ASMX y MySQL.

## 📋 Descripción del Proyecto

Este proyecto implementa un servicio web completo para la gestión de una tienda online, incluyendo:

- **Gestión de Usuarios**: Registro, autenticación, actualización y eliminación
- **Gestión de Productos**: CRUD completo con búsqueda y filtrado por categorías
- **Gestión de Pedidos**: Creación de pedidos con detalles y seguimiento de estados
- **Gestión de Inventario**: Control de stock con alertas de stock bajo
- **Reportes**: Generación de reportes de ventas y estadísticas
- **Sistema de Logs**: Registro de eventos y errores del sistema

## 🎯 Requisitos del Sistema

### Software Necesario

- **Visual Studio 2019 o superior** con soporte para ASP.NET
- **MySQL Server 5.7 o superior** (incluido en XAMPP)
- **MySQL Connector/NET** (versión 8.0 o superior)
- **IIS Express** (incluido con Visual Studio)
- **.NET Framework 4.7.2** o superior

### Verificar Instalaciones

```powershell
# Verificar MySQL
mysql --version

# Verificar que MySQL esté ejecutándose
Get-Service -Name MySQL*
```

## 🚀 Instalación y Configuración

### 1. Configurar la Base de Datos

#### Opción A: Usando MySQL desde línea de comandos

```powershell
# Iniciar MySQL (si usas XAMPP)
cd C:\xampp
.\mysql_start.bat

# Conectar a MySQL
mysql -u root -p

# Ejecutar el script de creación
source C:\xampp\htdocs\Creacion-de-un-servicio-Web-sobre-una-tienda-Online\App_data\database.sql
```

#### Opción B: Usando phpMyAdmin (XAMPP)

1. Abrir http://localhost/phpmyadmin
2. Crear nueva base de datos llamada `TiendaDB`
3. Importar el archivo `App_data/database.sql`

### 2. Verificar la Cadena de Conexión

Editar `Web.config` y verificar que la cadena de conexión sea correcta:

```xml
<connectionStrings>
  <add name="TiendaDB" 
       connectionString="Server=localhost;Database=TiendaDB;Uid=root;Pwd=;" 
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

**Nota**: Si tu MySQL tiene contraseña, agrégala en `Pwd=tu_contraseña`

### 3. Instalar MySQL Connector/NET

#### Opción A: NuGet Package Manager (Recomendado)

En Visual Studio:
1. Clic derecho en el proyecto → "Manage NuGet Packages"
2. Buscar "MySql.Data"
3. Instalar la versión más reciente

#### Opción B: Package Manager Console

```powershell
Install-Package MySql.Data
```

### 4. Compilar y Ejecutar el Proyecto

1. Abrir el proyecto en Visual Studio
2. Presionar `F5` o clic en "Start"
3. El navegador se abrirá automáticamente

## 📚 Estructura del Proyecto

```
Creacion-de-un-servicio-Web-sobre-una-tienda-Online/
├── App_data/
│   └── database.sql          # Script de creación de BD
├── Models/
│   ├── Usuario.cs            # Modelo de Usuario
│   ├── Producto.cs           # Modelo de Producto
│   ├── Pedido.cs             # Modelo de Pedido y DetallePedido
│   ├── Categoria.cs          # Modelo de Categoría
│   └── ReporteVentas.cs      # Modelos para reportes
├── TiendaService.asmx        # Descriptor del servicio web
├── TiendaService.asmx.cs     # Implementación del servicio
├── Web.config                # Configuración del proyecto
├── index.html                # Cliente de prueba
└── README.md                 # Este archivo
```

## 🔧 Métodos del Servicio Web

### Gestión de Usuarios (5 métodos)

| Método | Descripción | Parámetros |
|--------|-------------|------------|
| `ValidarUsuario` | Valida credenciales de usuario | nombreUsuario, contraseña |
| `RegistrarUsuario` | Registra un nuevo usuario | nombreUsuario, contraseña, nombre, apellido, email |
| `ActualizarUsuario` | Actualiza información de usuario | usuarioID, nombre, apellido, email |
| `EliminarUsuario` | Elimina un usuario | usuarioID |
| `ObtenerUsuarios` | Obtiene lista de todos los usuarios | - |

### Gestión de Productos (5 métodos)

| Método | Descripción | Parámetros |
|--------|-------------|------------|
| `CrearProducto` | Crea un nuevo producto | nombre, descripcion, precio, stock, categoriaID |
| `ActualizarProducto` | Actualiza un producto | productoID, nombre, descripcion, precio, stock |
| `EliminarProducto` | Elimina un producto | productoID |
| `ObtenerProductos` | Obtiene todos los productos | - |
| `BuscarProductos` | Busca productos por nombre | termino |

### Gestión de Pedidos (4 métodos)

| Método | Descripción | Parámetros |
|--------|-------------|------------|
| `CrearPedido` | Crea un pedido con detalles | usuarioID, detalles (List) |
| `ObtenerPedidosPorUsuario` | Obtiene pedidos de un usuario | usuarioID |
| `ActualizarEstadoPedido` | Actualiza estado de pedido | pedidoID, nuevoEstado |
| `HistorialCompras` | Obtiene historial de compras | usuarioID |

### Gestión de Categorías (2 métodos)

| Método | Descripción | Parámetros |
|--------|-------------|------------|
| `ObtenerCategorias` | Obtiene todas las categorías | - |
| `BuscarProductosPorCategoria` | Busca productos por categoría | categoriaID |

### Gestión de Inventario (2 métodos)

| Método | Descripción | Parámetros |
|--------|-------------|------------|
| `GestionarInventario` | Actualiza stock (entrada/salida) | productoID, cantidad, tipoMovimiento |
| `ObtenerProductosStockBajo` | Productos con stock bajo | umbral |

### Reportes (2 métodos)

| Método | Descripción | Parámetros |
|--------|-------------|------------|
| `ReporteVentas` | Genera reporte de ventas | fechaInicio, fechaFin |
| `VentasPorCategoria` | Estadísticas por categoría | fechaInicio, fechaFin |

### Sistema de Logs (3 métodos)

| Método | Descripción | Parámetros |
|--------|-------------|------------|
| `RegistrarLogError` | Registra un evento/error | tipoLog, mensaje, usuario |
| `ObtenerLogs` | Obtiene logs del sistema | limite |
| `LimpiarLogsAntiguos` | Limpia logs antiguos | diasAntiguedad |

**Total: 23 métodos web implementados**

## 🧪 Probar el Servicio Web

### Opción 1: Cliente Web Incluido

1. Ejecutar el proyecto en Visual Studio
2. Navegar a `http://localhost:[puerto]/index.html`
3. Usar las credenciales por defecto:
   - **Usuario**: `admin`
   - **Contraseña**: `admin123`

### Opción 2: Navegador (Interfaz SOAP)

1. Navegar a `http://localhost:[puerto]/TiendaService.asmx`
2. Verás la lista de todos los métodos disponibles
3. Hacer clic en cualquier método para probarlo

### Opción 3: SoapUI o Postman

Importar el WSDL desde: `http://localhost:[puerto]/TiendaService.asmx?WSDL`

## 📊 Base de Datos

### Tablas Creadas

- **Usuarios**: Almacena información de usuarios del sistema
- **Productos**: Catálogo de productos
- **Categorias**: Categorías de productos
- **Pedidos**: Pedidos realizados
- **DetallePedidos**: Detalles de cada pedido
- **Logs**: Registro de eventos del sistema

### Datos de Prueba

El script incluye datos iniciales:

**Usuarios:**
- admin / admin123
- usuario1 / pass123

**Categorías:**
- Electrónica
- Ropa
- Hogar

**Productos:**
- Smartphone X (€699.99)
- T-Shirt Blue (€19.99)

## 🔍 Ejemplos de Uso

### Ejemplo 1: Validar Usuario (SOAP Request)

```xml
POST /TiendaService.asmx HTTP/1.1
Content-Type: text/xml; charset=utf-8
SOAPAction: "http://tempuri.org/ValidarUsuario"

<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <ValidarUsuario xmlns="http://tempuri.org/">
      <nombreUsuario>admin</nombreUsuario>
      <contraseña>admin123</contraseña>
    </ValidarUsuario>
  </soap:Body>
</soap:Envelope>
```

### Ejemplo 2: Crear Producto (JavaScript)

```javascript
const soapEnvelope = `<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <CrearProducto xmlns="http://tempuri.org/">
      <nombre>Laptop Pro</nombre>
      <descripcion>Laptop de alto rendimiento</descripcion>
      <precio>1299.99</precio>
      <stock>25</stock>
      <categoriaID>1</categoriaID>
    </CrearProducto>
  </soap:Body>
</soap:Envelope>`;

fetch('TiendaService.asmx', {
  method: 'POST',
  headers: {
    'Content-Type': 'text/xml; charset=utf-8',
    'SOAPAction': 'http://tempuri.org/CrearProducto'
  },
  body: soapEnvelope
});
```

## 🐛 Solución de Problemas

### Error: "Could not load file or assembly 'MySql.Data'"

**Solución**: Instalar MySQL Connector/NET via NuGet

```powershell
Install-Package MySql.Data
```

### Error: "Unable to connect to any of the specified MySQL hosts"

**Solución**: 
1. Verificar que MySQL esté ejecutándose
2. Verificar la cadena de conexión en `Web.config`
3. Verificar usuario y contraseña de MySQL

### Error: "Table 'TiendaDB.Logs' doesn't exist"

**Solución**: Ejecutar nuevamente el script `database.sql` actualizado

### El servicio no se muestra en el navegador

**Solución**:
1. Verificar que el archivo `TiendaService.asmx` existe
2. Verificar que el proyecto se compiló correctamente
3. Revisar la consola de errores de Visual Studio

## 📝 Notas Importantes

- **Seguridad**: Este es un proyecto educativo. En producción, implementar:
  - Hash de contraseñas (bcrypt, SHA256)
  - Autenticación con tokens (JWT)
  - Validación de entrada
  - Protección contra SQL Injection (usar parámetros, como ya se hace)

- **Transacciones**: El método `CrearPedido` usa transacciones para garantizar integridad

- **Logs**: El sistema registra automáticamente operaciones de inventario y reportes

## 👨‍💻 Autor

Proyecto desarrollado como práctica del módulo DWEC (Desarrollo Web en Entorno Cliente)

## 📄 Licencia

Este proyecto es de uso educativo.

---

**¿Necesitas ayuda?** Revisa la documentación de cada método en `http://localhost:[puerto]/TiendaService.asmx`
