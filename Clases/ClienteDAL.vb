Imports System.Data
Imports System.Data.SqlClient

Public Class ClienteDAL

    ' @TODOS LOS CLIENTES
    Public Shared Function Listar() As List(Of Cliente)
        Dim lista As New List(Of Cliente)()

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("SELECT IdCliente, Nombre, Apellido, Telefono, Direccion FROM Clientes ORDER BY IdCliente ASC", cn)
                cn.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        lista.Add(MapearCliente(dr))
                    End While
                End Using
            End Using
        End Using

        Return lista
    End Function

    ' @UN SOLO CLIENTE POR ID
    Public Shared Function ObtenerPorId(idCliente As Integer) As Cliente
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("SELECT IdCliente, Nombre, Apellido, Telefono, Direccion FROM Clientes WHERE IdCliente = @id", cn)
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCliente
                cn.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then Return MapearCliente(dr)
                End Using
            End Using
        End Using

        Return Nothing
    End Function

    ' @ADD NUEVO CLIENTE
    Public Shared Sub Agregar(c As Cliente, usuario As String)
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Dim idNuevo As Integer

                    Dim sql As String = "INSERT INTO Clientes (Nombre, Apellido, Telefono, Direccion) " &
                                        "VALUES (@nombre, @apellido, @telefono, @direccion); " &
                                        "SELECT SCOPE_IDENTITY();"
                    Using cmd As New SqlCommand(sql, cn, tx)
                        AgregarParametros(cmd, c)
                        idNuevo = Convert.ToInt32(cmd.ExecuteScalar())
                    End Using

                    RegistrarBitacora("Agregar", idNuevo, usuario, cn, tx)
                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    ' @EDIT UN CLIENTE
    Public Shared Sub Editar(c As Cliente, usuario As String)
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Dim filas As Integer

                    Dim sql As String = "UPDATE Clientes SET Nombre = @nombre, Apellido = @apellido, " &
                                        "Telefono = @telefono, Direccion = @direccion WHERE IdCliente = @id"
                    Using cmd As New SqlCommand(sql, cn, tx)
                        AgregarParametros(cmd, c)
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = c.IdCliente
                        filas = cmd.ExecuteNonQuery()
                    End Using

                    If filas = 0 Then Throw New InvalidOperationException("El cliente no existe.")

                    RegistrarBitacora("Editar", c.IdCliente, usuario, cn, tx)
                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    ' @ELIMINAR CLIENTE
    Public Shared Sub Eliminar(idCliente As Integer, usuario As String)
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Dim filas As Integer

                    Using cmd As New SqlCommand("DELETE FROM Clientes WHERE IdCliente = @id", cn, tx)
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCliente
                        filas = cmd.ExecuteNonQuery()
                    End Using

                    If filas = 0 Then Throw New InvalidOperationException("El cliente no existe.")

                    RegistrarBitacora("Eliminar", idCliente, usuario, cn, tx)
                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    ' @REGISTRAR AUDITORIA EN BITACORA
    Private Shared Sub RegistrarBitacora(accion As String, idCliente As Integer, usuario As String,
                                         cn As SqlConnection, tx As SqlTransaction)
        Dim sql As String = "INSERT INTO Bitacora (Accion, IdClienteAfectado, NombreUsuario) " &
                            "VALUES (@accion, @idCliente, @usuario)"
        Using cmd As New SqlCommand(sql, cn, tx)
            cmd.Parameters.Add("@accion", SqlDbType.VarChar, 20).Value = accion
            cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = idCliente
            cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 50).Value = usuario
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ' ## ESTO ES PARA UTILIZAR PARAMETRIA 
    Private Shared Sub AgregarParametros(cmd As SqlCommand, c As Cliente)
        cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = c.Nombre
        cmd.Parameters.Add("@apellido", SqlDbType.VarChar, 100).Value = c.Apellido
        cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value = ValorONulo(c.Telefono)
        cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 200).Value = ValorONulo(c.Direccion)
    End Sub

    Private Shared Function ValorONulo(texto As String) As Object
        If String.IsNullOrWhiteSpace(texto) Then Return DBNull.Value
        Return texto
    End Function

    ' @MAPEO DE VARIABLES DE PARAMETRIA
    Private Shared Function MapearCliente(dr As SqlDataReader) As Cliente
        Dim c As New Cliente()
        c.IdCliente = Convert.ToInt32(dr("IdCliente"))
        c.Nombre = Convert.ToString(dr("Nombre"))
        c.Apellido = Convert.ToString(dr("Apellido"))
        c.Telefono = Convert.ToString(dr("Telefono"))
        c.Direccion = Convert.ToString(dr("Direccion"))
        Return c
    End Function

End Class