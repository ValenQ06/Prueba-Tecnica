-- =============================================
-- PRUEBA TECNICA SQL SERVER
-- Sistema de Comerciantes
-- =============================================

-- Script de creación completo
-- Ejecutar el archivo completo para:
-- 1 Crear la base de datos
-- 2 Crear las tablas
-- 3 Crear triggers
-- 4 Crear índices
-- 5 Insertar datos semilla
-- 6 Crear funciones
-- 7 Crear procedimiento almacenado
-------------------------------------------------------------------------------------------

-- 1 Crear base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'pruebasql_comerciantes_BD')
BEGIN
    CREATE DATABASE pruebasql_comerciantes_BD;
END
GO

USE pruebasql_comerciantes_BD;
GO
---------------------------------------------------------------------------------------------

-- 2 Creacion de tablas
CREATE TABLE Rol (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    CorreoElectronico VARCHAR(150) NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    IdRol INT NOT NULL,
    FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);
GO

CREATE TABLE Municipio (
    IdMunicipio INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Comerciante (
    IdComerciante INT IDENTITY(1,1) PRIMARY KEY,
    NombreRazonSocial VARCHAR(150) NOT NULL,
    IdMunicipio INT NOT NULL,
    Telefono VARCHAR(20) NULL,
    CorreoElectronico VARCHAR(150) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Estado VARCHAR(10) NOT NULL,
    FechaActualizacion DATETIME NULL,
    UsuarioActualizacion INT NULL,

    FOREIGN KEY (IdMunicipio) REFERENCES Municipio(IdMunicipio),
    FOREIGN KEY (UsuarioActualizacion) REFERENCES Usuario(IdUsuario)
);
GO

CREATE TABLE Establecimiento (
    IdEstablecimiento INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    Ingresos DECIMAL(18,2) NOT NULL,
    NumeroEmpleados INT NOT NULL,
    IdComerciante INT NOT NULL,
    FechaActualizacion DATETIME NULL,
    UsuarioActualizacion INT NULL,

    FOREIGN KEY (IdComerciante) REFERENCES Comerciante(IdComerciante),
    FOREIGN KEY (UsuarioActualizacion) REFERENCES Usuario(IdUsuario)
);
GO
---------------------------------------------------------------------------------------------

-- 3 Triggers
CREATE TRIGGER TR_Comerciante_Auditoria
ON Comerciante
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE C
    SET 
        FechaActualizacion = GETDATE(),
        UsuarioActualizacion = I.UsuarioActualizacion
    FROM Comerciante C
    INNER JOIN inserted I ON C.IdComerciante = I.IdComerciante;
END;
GO

CREATE TRIGGER TR_Establecimiento_Auditoria
ON Establecimiento
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE E
    SET 
        FechaActualizacion = GETDATE(),
        UsuarioActualizacion = I.UsuarioActualizacion
    FROM Establecimiento E
    INNER JOIN inserted I ON E.IdEstablecimiento = I.IdEstablecimiento;
END;
GO
---------------------------------------------------------------------------------------------

-- 4 Indices
CREATE NONCLUSTERED INDEX IX_Comerciante_Estado
ON Comerciante (Estado);
GO

CREATE NONCLUSTERED INDEX IX_Establecimiento_IdComerciante
ON Establecimiento (IdComerciante);
GO

CREATE NONCLUSTERED INDEX IX_Comerciante_IdMunicipio
ON Comerciante (IdMunicipio);
GO

CREATE NONCLUSTERED INDEX IX_Establecimiento_Comerciante_Ingresos_Empleados
ON Establecimiento (IdComerciante)
INCLUDE (Ingresos, NumeroEmpleados);
GO
---------------------------------------------------------------------------------------------

-- 5 Datos semilla
INSERT INTO Rol (Nombre)
VALUES 
('Administrador'),
('Auxiliar de Registro');
GO

INSERT INTO Usuario (Nombre, CorreoElectronico, Contrasena, IdRol)
VALUES
('Carlos Ramirez','carlos.ramirez@olsoftware.com','Admin123',1),
('Laura Gomez','laura.gomez@olsoftware.com','Aux123',2);
GO

INSERT INTO Municipio (Nombre)
VALUES
('Cali'),
('Bogotá'),
('Medellín'),
('Barranquilla'),
('Cartagena');
GO

INSERT INTO Comerciante
(
NombreRazonSocial,
IdMunicipio,
Telefono,
CorreoElectronico,
FechaRegistro,
Estado,
UsuarioActualizacion
)
VALUES
('Distribuidora El Sol',1,'3104567890','contacto@elsol.com',GETDATE(),'Activo',1),
('Comercializadora Andina',2,'3209876543','ventas@andina.com',GETDATE(),'Activo',1),
('Inversiones La 14',3,NULL,'info@la14.com',GETDATE(),'Activo',2),
('Mercados del Caribe',4,'3152223344',NULL,GETDATE(),'Inactivo',2),
('Grupo Empresarial Nova',5,'3189997766','contacto@nova.com',GETDATE(),'Activo',1);
GO

INSERT INTO Establecimiento
(
Nombre,
Ingresos,
NumeroEmpleados,
IdComerciante,
UsuarioActualizacion
)
VALUES
('Tienda El Sol Centro',15230.50,5,1,1),
('MiniMarket El Sol Norte',9800.75,3,1,1),
('Super Andina',20340.90,10,2,2),
('Andina Express',7540.30,4,2,2),
('La 14 Plaza',45000.00,20,3,1),
('La 14 Express',12000.45,6,3,1),
('Caribe Market',18900.60,8,4,2),
('Nova Comercial',25000.80,12,5,1),
('Nova Centro',17650.10,7,5,1),
('Nova Express',8900.95,3,5,2);
GO
---------------------------------------------------------------------------------------------

-- 6 Funciones
CREATE FUNCTION fn_TotalIngresosComerciante (@IdComerciante INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

    DECLARE @TotalIngresos DECIMAL(18,2)

    SELECT 
        @TotalIngresos = ISNULL(SUM(Ingresos),0)
    FROM Establecimiento
    WHERE IdComerciante = @IdComerciante

    RETURN @TotalIngresos

END
GO

CREATE FUNCTION fn_TotalEmpleadosComerciante (@IdComerciante INT)
RETURNS INT
AS
BEGIN

    DECLARE @TotalEmpleados INT

    SELECT 
        @TotalEmpleados = ISNULL(SUM(NumeroEmpleados),0)
    FROM Establecimiento
    WHERE IdComerciante = @IdComerciante

    RETURN @TotalEmpleados

END
GO

CREATE FUNCTION fn_CantidadEstablecimientos (@IdComerciante INT)
RETURNS INT
AS
BEGIN

    DECLARE @Cantidad INT

    SELECT 
        @Cantidad = COUNT(*)
    FROM Establecimiento
    WHERE IdComerciante = @IdComerciante

    RETURN @Cantidad

END
GO
---------------------------------------------------------------------------------------------

-- 7 Procedimiento almacenado
CREATE PROCEDURE sp_ReporteComerciantesActivos
AS
BEGIN

    SET NOCOUNT ON;

    SELECT 
        C.NombreRazonSocial,
        M.Nombre AS Municipio,
        C.Telefono,
        C.CorreoElectronico,
        C.FechaRegistro,
        C.Estado,

        dbo.fn_CantidadEstablecimientos(C.IdComerciante) AS CantidadEstablecimientos,
        dbo.fn_TotalIngresosComerciante(C.IdComerciante) AS TotalIngresos,
        dbo.fn_TotalEmpleadosComerciante(C.IdComerciante) AS CantidadEmpleados

    FROM Comerciante C

    INNER JOIN Municipio M
        ON C.IdMunicipio = M.IdMunicipio

    WHERE C.Estado = 'Activo'

    ORDER BY CantidadEstablecimientos DESC

END
GO