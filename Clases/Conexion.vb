Imports System.Configuration
Imports System.Data.SqlClient

Public Class Conexion

    Public Shared Function ObtenerConexion() As SqlConnection
        Dim cadena As String = ConfigurationManager.ConnectionStrings("GestionFunMicro").ConnectionString
        Return New SqlConnection(cadena)
    End Function

End Class