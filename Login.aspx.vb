Imports System.Web.Security

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                ' Crea un usuario por defecto: Usuario "admin", Contraseña "admin123"
                UsuarioDAL.CrearUsuarioDefecto("admin", "admin123")
            Catch ex As Exception
                Registro.RegistrarError(ex)
                lblMensaje.Text = "Error al inicializar el usuario base."
            End Try
        End If
    End Sub

    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        If Not Page.IsValid Then Return

        Try
            Dim usuario As String = txtUsuario.Text.Trim()

            If UsuarioDAL.ValidarUsuario(usuario, txtPassword.Text) Then
                FormsAuthentication.RedirectFromLoginPage(usuario, False)
            Else
                lblMensaje.Text = "Usuario o contraseña incorrectos."
            End If
        Catch ex As Exception
            Registro.RegistrarError(ex)
            lblMensaje.Text = "Ocurrió un error al iniciar sesión. Intente nuevamente."
        End Try
    End Sub
End Class