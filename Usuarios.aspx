<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Usuarios.aspx.vb" Inherits="PruebasFunMic.Usuarios" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Usuarios</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand navbar-dark bg-dark">
            <div class="container">
                <span class="navbar-brand">Gestión de clientes</span>
                <ul class="navbar-nav me-auto">
                    <li class="nav-item"><a class="nav-link" href="Clientes.aspx">Clientes</a></li>
                    <li class="nav-item"><a class="nav-link active" href="Usuarios.aspx">Usuarios</a></li>
                </ul>
                <div class="d-flex align-items-center">
                    <asp:Label ID="lblUsuario" runat="server" CssClass="text-white me-3" />
                    <asp:LinkButton ID="btnSalir" runat="server" CssClass="btn btn-outline-light btn-sm"
                        CausesValidation="False">Cerrar sesión</asp:LinkButton>
                </div>
            </div>
        </nav>

        <main class="container my-4">

            <asp:Label ID="lblMensaje" runat="server" />

            <div class="card shadow-sm mb-4">
                <div class="card-header bg-white fw-semibold">
                    <asp:Label ID="lblTitulo" runat="server" Text="Nuevo usuario" />
                </div>
                <div class="card-body">
                    <asp:HiddenField ID="hfIdUsuario" runat="server" />

                    <div class="row g-3">
                        <div class="col-md-4">
                            <asp:Label ID="lblNombreUsuario" runat="server" AssociatedControlID="txtUsuario"
                                CssClass="form-label" Text="Usuario" />
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"
                                MaxLength="50" autocomplete="off" />
                            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario"
                                ErrorMessage="El usuario es obligatorio." CssClass="text-danger small" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revUsuario" runat="server" ControlToValidate="txtUsuario"
                                ValidationExpression="^[A-Za-z0-9._-]{3,50}$"
                                ErrorMessage="3 a 50 caracteres: letras, números, punto, guion o guion bajo."
                                CssClass="text-danger small" Display="Dynamic" />
                        </div>

                        <div class="col-md-4">
                            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword"
                                CssClass="form-label" Text="Contraseña" />
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                                TextMode="Password" MaxLength="100" autocomplete="new-password" />
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="La contraseña es obligatoria." CssClass="text-danger small" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword"
                                ValidationExpression="^.{8,100}$"
                                ErrorMessage="Mínimo 8 caracteres." CssClass="text-danger small" Display="Dynamic" />
                        </div>

                        <div class="col-md-4">
                            <asp:Label ID="lblConfirmar" runat="server" AssociatedControlID="txtConfirmar"
                                CssClass="form-label" Text="Confirmar contraseña" />
                            <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-control"
                                TextMode="Password" MaxLength="100" autocomplete="new-password" />
                            <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server" ControlToValidate="txtConfirmar"
                                ErrorMessage="Confirme la contraseña." CssClass="text-danger small" Display="Dynamic" />
                            <asp:CompareValidator ID="cvConfirmar" runat="server" ControlToValidate="txtConfirmar"
                                ControlToCompare="txtPassword" Operator="Equal" Type="String"
                                ErrorMessage="Las contraseñas no coinciden." CssClass="text-danger small" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="mt-4">
                        <asp:Button ID="btnGuardar" runat="server" Text="Crear usuario" CssClass="btn btn-success me-2"
                            UseSubmitBehavior="false"
                            OnClientClick="if (typeof(Page_ClientValidate) == 'function') { if (!Page_ClientValidate()) { return false; } } this.disabled = true; this.value = 'Guardando...';" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary"
                            CausesValidation="False" />
                    </div>
                </div>
            </div>

            <div class="card shadow-sm">
                <div class="card-header bg-white fw-semibold">Listado de usuarios</div>
                <div class="table-responsive">
                    <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="IdUsuario" GridLines="None"
                        CssClass="table table-striped table-hover align-middle mb-0"
                        HeaderStyle-CssClass="table-light"
                        EmptyDataText="No hay usuarios registrados.">
                        <Columns>
                            <asp:BoundField DataField="IdUsuario" HeaderText="ID" />
                            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                            <asp:TemplateField HeaderText="Acciones"
                                HeaderStyle-CssClass="text-end" ItemStyle-CssClass="text-end text-nowrap">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCambiar" runat="server" CommandName="Cambiar"
                                        CommandArgument='<%# Eval("IdUsuario") %>' CausesValidation="False"
                                        CssClass="btn btn-sm btn-outline-primary">Cambiar contraseña</asp:LinkButton>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Eliminar"
                                        CommandArgument='<%# Eval("IdUsuario") %>' CausesValidation="False"
                                        CssClass="btn btn-sm btn-danger ms-1"
                                        OnClientClick="return confirm('¿Desea eliminar este usuario?');">Eliminar</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </main>
    </form>
</body>
</html>
