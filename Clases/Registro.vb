Imports System.IO

Public Class Registro

    Private Shared ReadOnly candado As New Object()

    Public Shared Sub RegistrarError(ex As Exception)
        Try
            Dim ruta As String = HttpContext.Current.Server.MapPath("~/App_Data/errores.log")
            SyncLock candado
                File.AppendAllText(ruta, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " " & ex.ToString() & Environment.NewLine)
            End SyncLock
        Catch
        End Try
    End Sub

End Class