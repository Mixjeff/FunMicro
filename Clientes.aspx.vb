Imports System.Web.Security
Imports System.Text.RegularExpressions

Public Class Clientes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblUsuario.Text = "Usuario: " & User.Identity.Name
            CargarClientes()
        Else
            LimpiarMensaje()
        End If
    End Sub

    Private Sub CargarClientes()
        Try
            gvClientes.DataSource = ClienteDAL.Listar()
            gvClientes.DataBind()
        Catch ex As Exception
            Registro.RegistrarError(ex)
            MostrarMensaje("No se pudo cargar la lista de clientes.", True)
        End Try
    End Sub

    Private Sub LimpiarFormulario()
        hfIdCliente.Value = ""
        txtNombre.Text = ""
        txtApellido.Text = ""
        txtTelefono.Text = ""
        txtDireccion.Text = ""
        btnGuardar.Text = "Agregar"
        btnGuardar.CssClass = "btn btn-success me-2"
    End Sub

    Private Sub MostrarMensaje(texto As String, esError As Boolean)
        lblMensaje.Text = texto
        lblMensaje.CssClass = If(esError, "alert alert-danger d-block", "alert alert-success d-block")

        ' Oculta el mensaje a los 3 segundos y limpia el historial del POST
        Dim script As String = "
    setTimeout(function() {
        var lbl = document.getElementById('" & lblMensaje.ClientID & "');
        if (lbl) { lbl.classList.remove('d-block'); lbl.classList.add('d-none'); }
    }, 3000);

    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }"

        ClientScript.RegisterStartupScript(Me.GetType(), "MensajeF5", script, True)
    End Sub

    Private Sub LimpiarMensaje()
        lblMensaje.Text = ""
        lblMensaje.CssClass = ""
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not Page.IsValid Then Return

        ' Validación del formato telefónico
        Dim telefonoFormulario As String = txtTelefono.Text.Trim()
        If Not Regex.IsMatch(telefonoFormulario, "^\d{4}-?\d{4}$") Then
            MostrarMensaje("El teléfono debe tener el formato 1234-5678 o 12345678.", True)
            Return
        End If

        ' Prevención de doble clic
        Dim ultimoClic As DateTime = If(Session("UltimoGuardado") IsNot Nothing, Convert.ToDateTime(Session("UltimoGuardado")), DateTime.MinValue)
        If (DateTime.Now - ultimoClic).TotalSeconds < 2 Then
            Return
        End If
        Session("UltimoGuardado") = DateTime.Now

        Try
            Dim c As New Cliente()
            c.Nombre = txtNombre.Text.Trim()
            c.Apellido = txtApellido.Text.Trim()
            c.Telefono = telefonoFormulario
            c.Direccion = txtDireccion.Text.Trim()

            If String.IsNullOrEmpty(hfIdCliente.Value) Then
                ClienteDAL.Agregar(c, User.Identity.Name)
                MostrarMensaje("Cliente agregado correctamente.", False)
            Else
                c.IdCliente = Convert.ToInt32(hfIdCliente.Value)
                ClienteDAL.Editar(c, User.Identity.Name)
                MostrarMensaje("Cliente actualizado correctamente.", False)
            End If

            LimpiarFormulario()
            CargarClientes()
        Catch ex As Exception
            Registro.RegistrarError(ex)
            MostrarMensaje("No se pudo guardar el cliente. Intente nuevamente.", True)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarFormulario()
        LimpiarMensaje()
    End Sub

    Protected Sub gvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        Dim id As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), id) Then Return

        Try
            If e.CommandName = "Editar" Then
                Dim c As Cliente = ClienteDAL.ObtenerPorId(id)
                If c Is Nothing Then
                    MostrarMensaje("El cliente ya no existe.", True)
                    CargarClientes()
                    Return
                End If

                hfIdCliente.Value = c.IdCliente.ToString()
                txtNombre.Text = c.Nombre
                txtApellido.Text = c.Apellido
                txtTelefono.Text = c.Telefono
                txtDireccion.Text = c.Direccion
                btnGuardar.Text = "Guardar cambios"
                btnGuardar.CssClass = "btn btn-primary me-2"

            ElseIf e.CommandName = "Eliminar" Then
                ClienteDAL.Eliminar(id, User.Identity.Name)
                LimpiarFormulario()
                MostrarMensaje("Cliente eliminado correctamente.", False)
                CargarClientes()
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