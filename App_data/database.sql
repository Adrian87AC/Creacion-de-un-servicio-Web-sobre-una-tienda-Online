-- Create Database
CREATE DATABASE IF NOT EXISTS TiendaDB;
USE TiendaDB;

-- Table Categories
CREATE TABLE IF NOT EXISTS Categorias (
    CategoriaID INT AUTO_INCREMENT PRIMARY KEY,
    NombreCategoria VARCHAR(100) NOT NULL
);

-- Table Users
CREATE TABLE IF NOT EXISTS Usuarios (
    UsuarioID INT AUTO_INCREMENT PRIMARY KEY,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Contrasena VARCHAR(255) NOT NULL,
    Nombre VARCHAR(100),
    Apellido VARCHAR(100),
    Email VARCHAR(100) UNIQUE,
    FechaRegistro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Table Products
CREATE TABLE IF NOT EXISTS Productos (
    ProductoID INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    Descripcion TEXT,
    Precio DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    CategoriaID INT,
    FOREIGN KEY (CategoriaID) REFERENCES Categorias(CategoriaID)
);

-- Table Orders
CREATE TABLE IF NOT EXISTS Pedidos (
    PedidoID INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioID INT,
    FechaPedido DATETIME DEFAULT CURRENT_TIMESTAMP,
    Estado VARCHAR(50) DEFAULT 'Pendiente',
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

-- Table Order Details
CREATE TABLE IF NOT EXISTS DetallePedidos (
    DetalleID INT AUTO_INCREMENT PRIMARY KEY,
    PedidoID INT,
    ProductoID INT,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (PedidoID) REFERENCES Pedidos(PedidoID) ON DELETE CASCADE,
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);

-- Initial Data
INSERT INTO Categorias (NombreCategoria) VALUES ('Electrónica'), ('Ropa'), ('Hogar');
INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaID) VALUES 
('Smartphone X', 'A powerful smartphone', 699.99, 50, 1),
('T-Shirt Blue', 'Comfortable cotton t-shirt', 19.99, 100, 2);
INSERT INTO Usuarios (NombreUsuario, Contraseña, Nombre, Apellido, Email) VALUES 
('admin', 'admin123', 'Administrador', 'Sistema', 'admin@tienda.com'),
('usuario1', 'pass123', 'Juan', 'Pérez', 'juan@email.com');
