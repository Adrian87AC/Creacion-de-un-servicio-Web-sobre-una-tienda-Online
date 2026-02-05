# 🧪 Guía de Prueba - Servicio Web TiendaDB

## ⚠️ Nota Importante
Este es un proyecto **ASP.NET ASMX**, NO es un proyecto Node.js. Por eso `npm install` no funciona. No necesitas npm para este proyecto.

## 📋 Requisitos Previos

- ✅ XAMPP instalado
- ✅ Visual Studio (con soporte ASP.NET)
- ✅ MySQL Connector/NET

## 🚀 Pasos para Probar el Proyecto

### Paso 1: Iniciar Servicios de XAMPP

1. Abre **XAMPP Control Panel** (ya lo abrí automáticamente)
2. Haz clic en **"Start"** en:
   - ✅ **Apache**
   - ✅ **MySQL**

### Paso 2: Crear la Base de Datos

**Opción A - phpMyAdmin (Recomendado):**

1. Abre tu navegador y ve a: `http://localhost/phpmyadmin`
2. Haz clic en "Nueva" (New) en el panel izquierdo
3. Nombre de la base de datos: `TiendaDB`
4. Haz clic en "Crear"
5. Selecciona la base de datos `TiendaDB`
6. Ve a la pestaña "Importar" (Import)
7. Haz clic en "Seleccionar archivo" y busca:
   ```
   C:\xampp\htdocs\Creacion-de-un-servicio-Web-sobre-una-tienda-Online\App_data\database.sql
   ```
8. Haz clic en "Continuar" (Go)

**Opción B - Línea de comandos:**

```powershell
# Conectar a MySQL
C:\xampp\mysql\bin\mysql.exe -u root

# Dentro de MySQL, ejecutar:
CREATE DATABASE IF NOT EXISTS TiendaDB;
USE TiendaDB;
SOURCE C:\xampp\htdocs\Creacion-de-un-servicio-Web-sobre-una-tienda-Online\App_data\database.sql;
exit;
```

### Paso 3: Verificar la Base de Datos

En phpMyAdmin, verifica que se crearon estas 6 tablas:
- ✅ Usuarios
- ✅ Productos
- ✅ Categorias
- ✅ Pedidos
- ✅ DetallePedidos
- ✅ Logs

### Paso 4: Abrir el Proyecto en Visual Studio

**Opción A - Si tienes archivo .sln:**
```powershell
# Buscar archivo .sln en el directorio
Get-ChildItem -Path . -Filter *.sln
```

**Opción B - Crear nuevo proyecto:**

1. Abre **Visual Studio**
2. File → New → Project
3. Busca "ASP.NET Web Application (.NET Framework)"
4. Nombre: `TiendaWebService`
5. Location: `C:\xampp\htdocs\Creacion-de-un-servicio-Web-sobre-una-tienda-Online`
6. Framework: **.NET Framework 4.7.2**
7. Template: **Empty** (luego agregaremos los archivos existentes)

### Paso 5: Instalar MySQL Connector/NET

En Visual Studio:

1. Tools → NuGet Package Manager → **Package Manager Console**
2. Ejecutar:
   ```powershell
   Install-Package MySql.Data
   ```

### Paso 6: Ejecutar el Proyecto

1. En Visual Studio, presiona **F5** o haz clic en el botón ▶️ **Start**
2. Se abrirá el navegador automáticamente

### Paso 7: Probar el Servicio Web

**Opción 1 - Interfaz SOAP (Verificar que funciona):**

El navegador debería abrir algo como:
```
http://localhost:XXXXX/TiendaService.asmx
```

Deberías ver una página con la lista de 23 métodos disponibles:
- ValidarUsuario
- RegistrarUsuario
- ObtenerProductos
- CrearProducto
- etc.

**Opción 2 - Cliente Web (Interfaz visual):**

Navega a:
```
http://localhost:XXXXX/index.html
```

1. **Login** con las credenciales por defecto:
   - Usuario: `admin`
   - Contraseña: `admin123`

2. **Prueba las funcionalidades:**
   - ✅ Listar Productos
   - ✅ Crear un nuevo producto
   - ✅ Buscar productos
   - ✅ Ver usuarios
   - ✅ Generar reportes

## 🔧 Alternativa: Probar sin Visual Studio (Solo Apache)

Si no quieres usar Visual Studio, puedes configurar IIS o usar Apache con mod_mono, pero es más complicado. **Visual Studio es la forma más fácil**.

## 📊 Verificar que Todo Funciona

### Test 1: Base de Datos
```sql
-- En phpMyAdmin o MySQL CLI
USE TiendaDB;
SELECT * FROM Usuarios;
```

Deberías ver 2 usuarios:
- admin
- usuario1

### Test 2: Servicio Web
Navega a: `http://localhost:XXXXX/TiendaService.asmx`

Deberías ver la página de descripción del servicio.

### Test 3: Cliente Web
Navega a: `http://localhost:XXXXX/index.html`

Deberías ver la interfaz morada con el formulario de login.

## ❌ Solución de Problemas

### Error: "Could not load file or assembly 'MySql.Data'"
**Solución:** Instalar MySQL Connector/NET via NuGet (Paso 5)

### Error: "Unable to connect to any of the specified MySQL hosts"
**Solución:** 
1. Verificar que MySQL está ejecutándose en XAMPP
2. Verificar la cadena de conexión en `Web.config`

### Error: "Table 'TiendaDB.Logs' doesn't exist"
**Solución:** Ejecutar nuevamente el script SQL (Paso 2)

### El navegador no abre nada
**Solución:**
1. Verificar que el proyecto se compiló sin errores
2. Revisar la consola de errores de Visual Studio

## 📝 Archivos del Proyecto

```
├── index.html          ← Cliente web (HTML)
├── styles.css          ← Estilos
├── app.js              ← JavaScript (SOAP client)
├── TiendaService.asmx  ← Descriptor del servicio
├── TiendaService.asmx.cs ← Código del servicio (C#)
├── Web.config          ← Configuración
├── Models/             ← Modelos de datos
└── App_data/
    └── database.sql    ← Script de BD
```

## ✅ Checklist de Verificación

- [ ] XAMPP Apache iniciado
- [ ] XAMPP MySQL iniciado
- [ ] Base de datos TiendaDB creada
- [ ] 6 tablas creadas correctamente
- [ ] Proyecto abierto en Visual Studio
- [ ] MySQL Connector/NET instalado
- [ ] Proyecto compilado sin errores
- [ ] Navegador abre TiendaService.asmx
- [ ] Se ve la lista de métodos
- [ ] index.html carga correctamente
- [ ] Login funciona con admin/admin123

## 🎯 Resultado Esperado

Cuando todo funcione correctamente:

1. **TiendaService.asmx** mostrará 23 métodos web
2. **index.html** mostrará una interfaz morada moderna
3. Podrás hacer login y usar todas las funcionalidades
4. Los datos se guardarán en MySQL

---

**¿Necesitas ayuda?** Revisa el README.md para más detalles.
