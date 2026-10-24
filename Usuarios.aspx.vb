Imports System.Text.RegularExpressions
Imports System.Web.Security

Public Class Usuarios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblUsuario.Text = "Usuario: " & User.Identity.Name
            CargarUsuarios()
        Else
            LimpiarMensaje()
        End If
    End Sub

    Private Sub CargarUsuarios()
        Try
            gvUsuarios.DataSource = UsuarioDAL.Listar()
            gvUsuarios.DataBind()
        Catch ex As Exception
            Registro.RegistrarError(ex)
            MostrarMensaje("No se pudo cargar la lista de usuarios.", True)
        End Try
    End Sub

    ' Formulario en modo "crear usuario"
    Private Sub ModoCrear()
        hfIdUsuario.Value = ""
        txtUsuario.Text = ""
        txtUsuario.ReadOnly = False
        rfvUsuario.Enabled = True
        revUsuario.Enabled = True
        lblTitulo.Text = "Nuevo usuario"
        btnGuardar.Text = "Crear usuario"
        btnGuardar.CssClass = "btn btn-success me-2"
    End Sub

    ' Formulario en modo "cambiar contraseña" (el nombre no se edita)
    Private Sub ModoCambiarPassword(u As Usuario)
        hfIdUsuario.Value = u.IdUsuario.ToString()
        txtUsuario.Text = u.NombreUsuario
        txtUsuario.ReadOnly = True
        rfvUsuario.Enabled = False
        revUsuario.Enabled = False
        lblTitulo.Text = "Cambiar contraseña"
        btnGuardar.Text = "Cambiar contraseña"
        btnGuardar.CssClass = "btn btn-primary me-2"
    End Sub

    Private Sub MostrarMensaje(texto As String, esError As Boolean)
        lblMensaje.Text = texto
        lblMensaje.CssClass = If(esError, "alert alert-danger d-block", "alert alert-success d-block")

        Dim script As String = "
        setTimeout(function() {
            var lbl = document.getElementById('" & lblMensaje.ClientID & "');
            if (lbl) { lbl.classList.remove('d-block'); lbl.classList.add('d-none'); }
        }, 3000);

        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }"

        ClientScript.RegisterStartupScript(Me.GetType(), "MensajeUsuarios", script, True)
    End Sub

    Private Sub LimpiarMensaje()
        lblMensaje.Text = ""
        lblMensaje.CssClass = ""
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not Page.IsValid Then Return

        ' Validaciones del servidor (las del navegador se pueden saltar)
        Dim password As String = txtPassword.Text
        If password.Length < 8 Then
            MostrarMensaje("La contraseña debe tener al menos 8 caracteres.", True)
            Return
        End If
        If password <> txtConfirmar.Text Then
            MostrarMensaje("Las contraseñas no coinciden.", True)
            Return
        End If

        Try
            If String.IsNullOrEmpty(hfIdUsuario.Value) Then
                Dim nombre As String = txtUsuario.Text.Trim()

                If Not Regex.IsMatch(nombre, "^[A-Za-z0-9._-]{3,50}$") Then
                    MostrarMensaje("El usuario debe tener de 3 a 50 caracteres: letras, números, punto, guion o guion bajo.", True)
                    Return
                End If

                If UsuarioDAL.Existe(nombre) Then
                    MostrarMensaje("Ya existe un usuario con ese nombre.", True)
                    Return
                End If

                UsuarioDAL.CrearUsuario(nombre, password)
                MostrarMensaje("Usuario creado correctamente.", False)
            Else
                UsuarioDAL.CambiarPassword(Convert.ToInt32(hfIdUsuario.Value), password)
                MostrarMensaje("Contraseña actualizada correctamente.", False)
            End If

            ModoCrear()
            CargarUsuarios()
        Catch ex As Exception
            Registro.RegistrarError(ex)
            MostrarMensaje("No se pudo guardar el usuario. Intente nuevamente.", True)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        ModoCrear()
    End Sub

    Protected Sub gvUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvUsuarios.RowCommand
        Dim id As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), id) Then Return

        Try
            Dim u As Usuario = UsuarioDAL.ObtenerPorId(id)
            If u Is Nothing Then
                MostrarMensaje("El usuario ya no existe.", True)
                CargarUsuarios()
                Return
            End If

            If e.CommandName = "Cambiar" Then
                ModoCambiarPassword(u)

            ElseIf e.CommandName = "Eliminar" Then
                If String.Equals(u.NombreUsuario, User.Identity.Name, StringComparison.OrdinalIgnoreCase) Then
                    MostrarMensaje("No puede eliminar su propio usuario.", True)
                    Return
                End If

                UsuarioDAL.Eliminar(id)
                ModoCrear()
                MostrarMensaje("Usuario eliminado correctamente.", False)
                CargarUsuarios()
            End If
        Catch ex As Exception
            Registro.RegistrarError(ex)
            MostrarMensaje("No se pudo completar la operación. Intente nuevamente.", True)
        End Try
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        FormsAuthentication.SignOut()
        Response.Redirect("~/Login.aspx")
    End Sub

End Class