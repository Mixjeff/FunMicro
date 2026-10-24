Imports System.Data
Imports System.Data.SqlClient

Public Class UsuarioDAL

    ' Crear primer usuario
    Public Shared Sub CrearUsuarioDefecto(nombreUsuario As String, passwordPlano As String)
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            ' 1. Verificar si el usuario ya existe
            Dim cmdCheck As New SqlCommand("SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @usuario", cn)
            cmdCheck.Parameters.AddWithValue("@usuario", nombreUsuario)

            cn.Open()
            Dim existe As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

            ' 2. Si no existe, lo insertamos
            If existe = 0 Then
                Dim passwordHash As String = Seguridad.GenerarHash(passwordPlano)

                Dim cmdInsert As New SqlCommand("INSERT INTO Usuarios (NombreUsuario, PasswordHash) VALUES (@usuario, @hash)", cn)
                cmdInsert.Parameters.AddWithValue("@usuario", nombreUsuario)
                cmdInsert.Parameters.AddWithValue("@hash", passwordHash)

                cmdInsert.ExecuteNonQuery()
            End If
        End Using
    End Sub

    ' Validación que el usuario tenga credenciales correctas
    ' Utilizando parametro como buena practica de seguridad
    Public Shared Function ValidarUsuario(nombreUsuario As String, password As String) As Boolean
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("SELECT PasswordHash FROM Usuarios WHERE NombreUsuario = @usuario", cn)
                cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 50).Value = nombreUsuario
                cn.Open()

                Dim resultado As Object = cmd.ExecuteScalar()
                If resultado Is Nothing Then Return False

                Return Seguridad.VerificarPassword(password, resultado.ToString())
            End Using
        End Using
    End Function

    ' @CREAR USUARIO NUEVO
    Public Shared Sub CrearUsuario(nombreUsuario As String, password As String)
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("INSERT INTO Usuarios (NombreUsuario, PasswordHash) VALUES (@usuario, @hash)", cn)
                cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 50).Value = nombreUsuario
                cmd.Parameters.Add("@hash", SqlDbType.VarChar, 256).Value = Seguridad.GenerarHash(password)
                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' @MOSTRAR LOS USUARIOS EXISTENTES
    Public Shared Function Listar() As List(Of Usuario)
        Dim lista As New List(Of Usuario)()

        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("SELECT IdUsuario, NombreUsuario FROM Usuarios ORDER BY NombreUsuario", cn)
                cn.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim u As New Usuario()
                        u.IdUsuario = dr.GetInt32(0)
                        u.NombreUsuario = dr.GetString(1)
                        lista.Add(u)
                    End While
                End Using
            End Using
        End Using

        Return lista
    End Function

    ' @UN SOLO CLIENTE POR ID
    Public Shared Function ObtenerPorId(idUsuario As Integer) As Usuario
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("SELECT IdUsuario, NombreUsuario FROM Usuarios WHERE IdUsuario = @id", cn)
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUsuario
                cn.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        Dim u As New Usuario()
                        u.IdUsuario = dr.GetInt32(0)
                        u.NombreUsuario = dr.GetString(1)
                        Return u
                    End If
                End Using
            End Using
        End Using

        Return Nothing
    End Function

    ' @VALIDAR EXISTENCIA
    Public Shared Function Existe(nombreUsuario As String) As Boolean
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("SELECT COUNT(1) FROM Usuarios WHERE NombreUsuario = @usuario", cn)
                cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 50).Value = nombreUsuario
                cn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    ' @CAMBIAR CONTRASEÑA
    Public Shared Sub CambiarPassword(idUsuario As Integer, password As String)
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("UPDATE Usuarios SET PasswordHash = @hash WHERE IdUsuario = @id", cn)
                cmd.Parameters.Add("@hash", SqlDbType.VarChar, 256).Value = Seguridad.GenerarHash(password)
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUsuario
                cn.Open()
                If cmd.ExecuteNonQuery() = 0 Then Throw New InvalidOperationException("El usuario no existe.")
            End Using
        End Using
    End Sub

    ' @ELIMINAR USUARIO
    Public Shared Sub Eliminar(idUsuario As Integer)
        Using cn As SqlConnection = Conexion.ObtenerConexion()
            Using cmd As New SqlCommand("DELETE FROM Usuarios WHERE IdUsuario = @id", cn)
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUsuario
                cn.Open()
                If cmd.ExecuteNonQuery() = 0 Then Throw New InvalidOperationException("El usuario no existe.")
            End Using
        End Using
    End Sub

End Class