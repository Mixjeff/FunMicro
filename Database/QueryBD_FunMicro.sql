-- ============================================================
-- Base de datos: GestionFunMicro
-- Motor: SQL Server (probado con SQL Server Express)
-- ============================================================

-- 1. Crear la base de datos
CREATE DATABASE GestionFunMicro;
GO

USE GestionFunMicro;
GO

-- 2. Tabla de Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL
);
GO

-- 3. Tabla de Clientes
CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Telefono VARCHAR(15) NULL,
    Direccion VARCHAR(200) NULL
);
GO

-- 4. Tabla de Bitácora
CREATE TABLE Bitacora (
    IdBitacora INT IDENTITY(1,1) PRIMARY KEY,
    Accion VARCHAR(20) NOT NULL,          -- Agregar, Editar, Eliminar
    IdClienteAfectado INT NOT NULL,
    FechaHora DATETIME DEFAULT GETDATE(),
    NombreUsuario VARCHAR(50) NOT NULL    -- Usuario que hizo la modificación
);
GO

-- 5. Usuario inicial para poder ingresar a la aplicación
--    Usuario: admin   |   Contraseña: admin123
--    Solo para pruebas: cambie la contraseña desde la pantalla de Usuarios.
