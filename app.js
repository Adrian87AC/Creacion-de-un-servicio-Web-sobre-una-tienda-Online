// TiendaDB - Cliente de Prueba del Servicio Web
// Script principal para comunicación SOAP y gestión de la interfaz

const SERVICE_URL = 'TiendaService.asmx';
let currentUser = null;

// ==================== FUNCIONES SOAP ====================

/**
 * Crea un sobre SOAP para llamar a un método del servicio web
 * @param {string} method - Nombre del método a llamar
 * @param {object} params - Parámetros del método
 * @returns {string} XML del sobre SOAP
 */
function createSOAPEnvelope(method, params) {
    let paramsXML = '';
    for (let key in params) {
        paramsXML += `<${key}>${params[key]}</${key}>`;
    }

    return `<?xml version="1.0" encoding="utf-8"?>
        <soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
                       xmlns:xsd="http://www.w3.org/2001/XMLSchema" 
                       xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
            <soap:Body>
                <${method} xmlns="http://tempuri.org/">
                    ${paramsXML}
                </${method}>
            </soap:Body>
        </soap:Envelope>`;
}

/**
 * Llama a un método del servicio web SOAP
 * @param {string} method - Nombre del método
 * @param {object} params - Parámetros del método
 * @returns {Promise<Document>} Documento XML con la respuesta
 */
async function callSOAPService(method, params) {
    try {
        const response = await fetch(SERVICE_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'text/xml; charset=utf-8',
                'SOAPAction': `http://tempuri.org/${method}`
            },
            body: createSOAPEnvelope(method, params)
        });

        const text = await response.text();
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(text, 'text/xml');

        return xmlDoc;
    } catch (error) {
        console.error('Error calling SOAP service:', error);
        throw error;
    }
}

// ==================== AUTENTICACIÓN ====================

/**
 * Inicia sesión con las credenciales proporcionadas
 */
async function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        const result = await callSOAPService('ValidarUsuario', {
            nombreUsuario: username,
            contraseña: password
        });

        const usuario = result.getElementsByTagName('Usuario')[0];

        if (usuario) {
            currentUser = {
                id: usuario.getElementsByTagName('UsuarioID')[0].textContent,
                nombre: usuario.getElementsByTagName('Nombre')[0].textContent,
                apellido: usuario.getElementsByTagName('Apellido')[0].textContent,
                email: usuario.getElementsByTagName('Email')[0].textContent
            };

            document.getElementById('authSection').classList.add('hidden');
            document.getElementById('userInfoSection').classList.remove('hidden');
            document.getElementById('mainContent').classList.remove('hidden');

            document.getElementById('welcomeMessage').textContent =
                `Bienvenido, ${currentUser.nombre} ${currentUser.apellido}`;
            document.getElementById('userEmail').textContent = currentUser.email;
        } else {
            showAlert('authResult', 'error', 'Credenciales incorrectas');
        }
    } catch (error) {
        showAlert('authResult', 'error', 'Error al conectar con el servicio: ' + error.message);
    }
}

/**
 * Cierra la sesión actual
 */
function logout() {
    currentUser = null;
    document.getElementById('authSection').classList.remove('hidden');
    document.getElementById('userInfoSection').classList.add('hidden');
    document.getElementById('mainContent').classList.add('hidden');
}

// ==================== GESTIÓN DE PRODUCTOS ====================

/**
 * Obtiene y muestra todos los productos
 */
async function obtenerProductos() {
    try {
        const result = await callSOAPService('ObtenerProductos', {});
        const productos = result.getElementsByTagName('Producto');

        let html = '<div class="results"><table class="table"><thead><tr>' +
            '<th>ID</th><th>Nombre</th><th>Precio</th><th>Stock</th><th>Categoría</th></tr></thead><tbody>';

        for (let prod of productos) {
            html += `<tr>
                <td>${prod.getElementsByTagName('ProductoID')[0].textContent}</td>
                <td>${prod.getElementsByTagName('Nombre')[0].textContent}</td>
                <td>€${parseFloat(prod.getElementsByTagName('Precio')[0].textContent).toFixed(2)}</td>
                <td>${prod.getElementsByTagName('Stock')[0].textContent}</td>
                <td>${prod.getElementsByTagName('CategoriaID')[0].textContent}</td>
            </tr>`;
        }

        html += '</tbody></table></div>';
        document.getElementById('productosResult').innerHTML = html;
    } catch (error) {
        showAlert('productosResult', 'error', 'Error al obtener productos: ' + error.message);
    }
}

function mostrarFormularioProducto() {
    document.getElementById('formProducto').classList.remove('hidden');
}

function ocultarFormularioProducto() {
    document.getElementById('formProducto').classList.add('hidden');
}

/**
 * Crea un nuevo producto
 */
async function crearProducto() {
    const nombre = document.getElementById('prodNombre').value;
    const descripcion = document.getElementById('prodDescripcion').value;
    const precio = document.getElementById('prodPrecio').value;
    const stock = document.getElementById('prodStock').value;
    const categoriaID = document.getElementById('prodCategoria').value;

    try {
        const result = await callSOAPService('CrearProducto', {
            nombre, descripcion, precio, stock, categoriaID
        });

        const success = result.getElementsByTagName('CrearProductoResult')[0].textContent === 'true';

        if (success) {
            showAlert('productosResult', 'success', 'Producto creado exitosamente');
            ocultarFormularioProducto();
            obtenerProductos();
        } else {
            showAlert('productosResult', 'error', 'Error al crear producto');
        }
    } catch (error) {
        showAlert('productosResult', 'error', 'Error: ' + error.message);
    }
}

/**
 * Busca productos por nombre
 */
async function buscarProductos() {
    const termino = document.getElementById('buscarProducto').value;

    try {
        const result = await callSOAPService('BuscarProductos', { termino });
        const productos = result.getElementsByTagName('Producto');

        let html = '<div class="results"><table class="table"><thead><tr>' +
            '<th>ID</th><th>Nombre</th><th>Precio</th><th>Stock</th></tr></thead><tbody>';

        for (let prod of productos) {
            html += `<tr>
                <td>${prod.getElementsByTagName('ProductoID')[0].textContent}</td>
                <td>${prod.getElementsByTagName('Nombre')[0].textContent}</td>
                <td>€${parseFloat(prod.getElementsByTagName('Precio')[0].textContent).toFixed(2)}</td>
                <td>${prod.getElementsByTagName('Stock')[0].textContent}</td>
            </tr>`;
        }

        html += '</tbody></table></div>';
        document.getElementById('productosResult').innerHTML = html;
    } catch (error) {
        showAlert('productosResult', 'error', 'Error al buscar productos: ' + error.message);
    }
}

// ==================== GESTIÓN DE USUARIOS ====================

function mostrarFormularioUsuario() {
    document.getElementById('formUsuario').classList.remove('hidden');
}

function ocultarFormularioUsuario() {
    document.getElementById('formUsuario').classList.add('hidden');
}

/**
 * Obtiene y muestra todos los usuarios
 */
async function obtenerUsuarios() {
    try {
        const result = await callSOAPService('ObtenerUsuarios', {});
        const usuarios = result.getElementsByTagName('Usuario');

        let html = '<div class="results"><table class="table"><thead><tr>' +
            '<th>ID</th><th>Usuario</th><th>Nombre</th><th>Email</th><th>Fecha Registro</th></tr></thead><tbody>';

        for (let user of usuarios) {
            html += `<tr>
                <td>${user.getElementsByTagName('UsuarioID')[0].textContent}</td>
                <td>${user.getElementsByTagName('NombreUsuario')[0].textContent}</td>
                <td>${user.getElementsByTagName('Nombre')[0].textContent} ${user.getElementsByTagName('Apellido')[0].textContent}</td>
                <td>${user.getElementsByTagName('Email')[0].textContent}</td>
                <td>${new Date(user.getElementsByTagName('FechaRegistro')[0].textContent).toLocaleDateString()}</td>
            </tr>`;
        }

        html += '</tbody></table></div>';
        document.getElementById('usuariosResult').innerHTML = html;
    } catch (error) {
        showAlert('usuariosResult', 'error', 'Error al obtener usuarios: ' + error.message);
    }
}

/**
 * Registra un nuevo usuario
 */
async function registrarUsuario() {
    const nombreUsuario = document.getElementById('userNombre').value;
    const contraseña = document.getElementById('userPassword').value;
    const nombre = document.getElementById('userNombreReal').value;
    const apellido = document.getElementById('userApellido').value;
    const email = document.getElementById('userEmail').value;

    try {
        const result = await callSOAPService('RegistrarUsuario', {
            nombreUsuario, contraseña, nombre, apellido, email
        });

        const success = result.getElementsByTagName('RegistrarUsuarioResult')[0].textContent === 'true';

        if (success) {
            showAlert('usuariosResult', 'success', 'Usuario registrado exitosamente');
            ocultarFormularioUsuario();
            obtenerUsuarios();
        } else {
            showAlert('usuariosResult', 'error', 'Error al registrar usuario');
        }
    } catch (error) {
        showAlert('usuariosResult', 'error', 'Error: ' + error.message);
    }
}

// ==================== GESTIÓN DE PEDIDOS ====================

/**
 * Obtiene los pedidos de un usuario
 */
async function obtenerPedidosUsuario() {
    const usuarioID = document.getElementById('pedidoUsuarioID').value;

    try {
        const result = await callSOAPService('ObtenerPedidosPorUsuario', { usuarioID });
        const pedidos = result.getElementsByTagName('Pedido');

        let html = '<div class="results"><table class="table"><thead><tr>' +
            '<th>ID Pedido</th><th>Fecha</th><th>Estado</th></tr></thead><tbody>';

        for (let pedido of pedidos) {
            const estado = pedido.getElementsByTagName('Estado')[0].textContent;
            const badgeClass = estado === 'Pendiente' ? 'badge-warning' :
                estado === 'Enviado' ? 'badge-success' : 'badge-danger';

            html += `<tr>
                <td>${pedido.getElementsByTagName('PedidoID')[0].textContent}</td>
                <td>${new Date(pedido.getElementsByTagName('FechaPedido')[0].textContent).toLocaleString()}</td>
                <td><span class="badge ${badgeClass}">${estado}</span></td>
            </tr>`;
        }

        html += '</tbody></table></div>';
        document.getElementById('pedidosResult').innerHTML = html;
    } catch (error) {
        showAlert('pedidosResult', 'error', 'Error al obtener pedidos: ' + error.message);
    }
}

// ==================== REPORTES ====================

/**
 * Genera un reporte de ventas
 */
async function generarReporteVentas() {
    const fechaInicio = document.getElementById('reporteFechaInicio').value;
    const fechaFin = document.getElementById('reporteFechaFin').value;

    try {
        const result = await callSOAPService('ReporteVentas', { fechaInicio, fechaFin });
        const reporte = result.getElementsByTagName('ReporteVentas')[0];

        let html = '<div class="results">';
        html += `<h3>Reporte de Ventas</h3>`;
        html += `<p><strong>Total Vendido:</strong> €${parseFloat(reporte.getElementsByTagName('TotalVendido')[0].textContent).toFixed(2)}</p>`;
        html += `<p><strong>Total Pedidos:</strong> ${reporte.getElementsByTagName('TotalPedidos')[0].textContent}</p>`;
        html += `<p><strong>Productos Vendidos:</strong> ${reporte.getElementsByTagName('TotalProductosVendidos')[0].textContent}</p>`;
        html += '</div>';

        document.getElementById('reporteResult').innerHTML = html;
    } catch (error) {
        showAlert('reporteResult', 'error', 'Error al generar reporte: ' + error.message);
    }
}

/**
 * Obtiene ventas por categoría
 */
async function obtenerVentasPorCategoria() {
    const fechaInicio = document.getElementById('reporteFechaInicio').value;
    const fechaFin = document.getElementById('reporteFechaFin').value;

    try {
        const result = await callSOAPService('VentasPorCategoria', { fechaInicio, fechaFin });

        let html = '<div class="results"><h3>Ventas por Categoría</h3><pre>' +
            new XMLSerializer().serializeToString(result) + '</pre></div>';

        document.getElementById('reporteResult').innerHTML = html;
    } catch (error) {
        showAlert('reporteResult', 'error', 'Error: ' + error.message);
    }
}

// ==================== INVENTARIO ====================

/**
 * Gestiona el inventario (entrada/salida de stock)
 */
async function gestionarInventario() {
    const productoID = document.getElementById('invProductoID').value;
    const cantidad = document.getElementById('invCantidad').value;
    const tipoMovimiento = document.getElementById('invTipo').value;

    try {
        const result = await callSOAPService('GestionarInventario', {
            productoID, cantidad, tipoMovimiento
        });

        const success = result.getElementsByTagName('GestionarInventarioResult')[0].textContent === 'true';

        if (success) {
            showAlert('inventarioResult', 'success', 'Inventario actualizado exitosamente');
        } else {
            showAlert('inventarioResult', 'error', 'Error al actualizar inventario');
        }
    } catch (error) {
        showAlert('inventarioResult', 'error', 'Error: ' + error.message);
    }
}

/**
 * Obtiene productos con stock bajo
 */
async function obtenerStockBajo() {
    const umbral = document.getElementById('stockUmbral').value;

    try {
        const result = await callSOAPService('ObtenerProductosStockBajo', { umbral });
        const productos = result.getElementsByTagName('Producto');

        let html = '<div class="results"><table class="table"><thead><tr>' +
            '<th>ID</th><th>Nombre</th><th>Stock</th></tr></thead><tbody>';

        for (let prod of productos) {
            html += `<tr>
                <td>${prod.getElementsByTagName('ProductoID')[0].textContent}</td>
                <td>${prod.getElementsByTagName('Nombre')[0].textContent}</td>
                <td><span class="badge badge-danger">${prod.getElementsByTagName('Stock')[0].textContent}</span></td>
            </tr>`;
        }

        html += '</tbody></table></div>';
        document.getElementById('inventarioResult').innerHTML = html;
    } catch (error) {
        showAlert('inventarioResult', 'error', 'Error: ' + error.message);
    }
}

// ==================== LOGS ====================

/**
 * Obtiene los logs del sistema
 */
async function obtenerLogs() {
    const limite = document.getElementById('logsLimite').value;

    try {
        const result = await callSOAPService('ObtenerLogs', { limite });

        let html = '<div class="results"><pre>' +
            new XMLSerializer().serializeToString(result) + '</pre></div>';

        document.getElementById('logsResult').innerHTML = html;
    } catch (error) {
        showAlert('logsResult', 'error', 'Error al obtener logs: ' + error.message);
    }
}

// ==================== UTILIDADES ====================

/**
 * Muestra una alerta en el elemento especificado
 * @param {string} elementId - ID del elemento donde mostrar la alerta
 * @param {string} type - Tipo de alerta (success, error, info)
 * @param {string} message - Mensaje a mostrar
 */
function showAlert(elementId, type, message) {
    const alertClass = type === 'success' ? 'alert-success' :
        type === 'error' ? 'alert-error' : 'alert-info';

    const html = `<div class="alert ${alertClass}">${message}</div>`;
    document.getElementById(elementId).innerHTML = html;

    // Auto-ocultar después de 5 segundos
    setTimeout(() => {
        document.getElementById(elementId).innerHTML = '';
    }, 5000);
}
