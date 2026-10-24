# PruebasFunMic

Aplicación web para gestionar la información de clientes, desarrollada en ASP.NET Web Forms con VB.NET y SQL Server, como prueba técnica. Incluye autenticación de usuarios, mantenimiento de clientes (ver, agregar, editar y eliminar), administración de usuarios y una bitácora que registra cada cambio hecho sobre los clientes.

## Funcionalidades
-Login con usuario y contraseña, validado contra la tabla Usuarios de SQL Server.
-Mantenimiento de clientes: listar, agregar, editar y eliminar, con validaciones en el navegador y en el servidor.
-Bitácora de acciones: cada alta, edición o eliminación de un cliente se registra en la tabla Bitacora con la acción, el ID del cliente, la fecha y hora, y el usuario que la realizó.
-Mantenimiento de usuarios: crear usuarios nuevos, cambiar contraseñas y eliminar usuarios, para dar acceso a más personas al sistema.
-Cierre de sesión y bloqueo de todas las pantallas para usuarios no autenticados.

OJO: No se usan frameworks adicionales ni ORM: el acceso a datos está hecho con ADO.NET puro.

## Requisitos
-Visual Studio 2022 con la carga de trabajo ASP.NET y desarrollo web.
-.NET Framework 4.8 (Developer Pack).
-SQL Server o SQL Server Express, y SQL Server Management Studio (SSMS).
-Conexión a internet para cargar Bootstrap desde el CDN.

# Pasos de Instalación
1. Crear la base de datos
--Abrir el archivo Database/GestionFunMicro.sql y ejecútelo completo.
2. Configurar la cadena de conexión
--En Web.config, ajuste el valor de Server según su instancia
3. Ejecutar el programa estableciendo el login.aspx como página de inicio.
-Credenciales iniciales
Usuario: admin
Contraseña: admin123*

## Limitaciones conocidas
No hay roles: cualquier usuario autenticado puede administrar clientes y usuarios.
Si se elimina a un usuario con la sesión abierta, su sesión sigue activa hasta que expire (30 minutos).
Las acciones sobre usuarios no se registran en la bitácora, que es exclusiva de los cambios sobre clientes.
Bootstrap se carga desde un CDN; sin conexión a internet la interfaz se verá sin estilos.
