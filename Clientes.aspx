<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Clientes.aspx.vb" Inherits="PruebasFunMic.Clientes" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Clientes</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand navbar-dark bg-dark">
            <div class="container">
                <span class="navbar-brand">Gestión de clientes</span>
                <ul class="navbar-nav me-auto">
                    <li class="nav-item"><a class="nav-link active" href="Clientes.aspx">Clientes</a></li>
                    <li class="nav-item"><a class="nav-link" href="Usuarios.aspx">Usuarios</a></li>
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
                <div class="card-header bg-white fw-semibold">Datos del cliente</div>
                <div class="card-body">
                    <asp:HiddenField ID="hfIdCliente" runat="server" />

                    <div class="row g-3">
                        <div class="col-md-6">
                            <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre"
                                CssClass="form-label" Text="Nombre" />
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                                ErrorMessage="El nombre es obligatorio." CssClass="text-danger small" Display="Dynamic" />
                        </div>

                        <div class="col-md-6">
                            <asp:Label ID="lblApellido" runat="server" AssociatedControlID="txtApellido"
                                CssClass="form-label" Text="Apellido" />
                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="100" />
                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                                ErrorMessage="El apellido es obligatorio." CssClass="text-danger small" Display="Dynamic" />
                        </div>

                        <div class="col-md-4">
                            <asp:Label ID="lblTelefono" runat="server" AssociatedControlID="txtTelefono"
                                CssClass="form-label" Text="Teléfono" />
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"
                                TextMode="Phone" MaxLength="9" />
                            <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" ControlToValidate="txtTelefono"
                                ErrorMessage="El teléfono es obligatorio." CssClass="text-danger small" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revTelefono" runat="server" ControlToValidate="txtTelefono"
                                ValidationExpression="^\d{4}-?\d{4}$"
                                ErrorMessage="Formato: 1234-5678 o 12345678." CssClass="text-danger small" Display="Dynamic" />
                        </div>

                        <div class="col-md-8">
                            <asp:Label ID="lblDireccion" runat="server" AssociatedControlID="txtDireccion"
                                CssClass="form-label" Text="Dirección" />
                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" MaxLength="200" />
                            <asp:RequiredFieldValidator ID="rfvDireccion" runat="server" ControlToValidate="txtDireccion"
                                ErrorMessage="La dirección es obligatoria." CssClass="text-danger small" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="mt-4">
                        <asp:Button ID="btnGuardar" runat="server" Text="Agregar" CssClass="btn btn-success me-2"
                            UseSubmitBehavior="false"
                            OnClientClick="if (typeof(Page_ClientValidate) == 'function') { if (!Page_ClientValidate()) { return false; } } this.disabled = true; this.value = 'Guardando...';" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary"
                            CausesValidation="False" />
                    </div>
                </div>
            </div>

            <div class="card shadow-sm">
                <div class="card-header bg-white fw-semibold">Listado de clientes</div>
                <div class="table-responsive">
                    <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="IdCliente" GridLines="None"
                        CssClass="table table-striped table-hover align-middle mb-0"
                        HeaderStyle-CssClass="table-light"
                        EmptyDataText="No hay clientes registrados.">
                        <Columns>
                            <asp:BoundField DataField="IdCliente" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                            <asp:TemplateField HeaderText="Acciones"
                                HeaderStyle-CssClass="text-end" ItemStyle-CssClass="text-end text-nowrap">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditar" runat="server" CommandName="Editar"
                                        CommandArgument='<%# Eval("IdCliente") %>' CausesValidation="False"
                                        CssClass="btn btn-sm btn-outline-primary">Editar</asp:LinkButton>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Eliminar"
                                        CommandArgument='<%# Eval("IdCliente") %>' CausesValidation="False"
                                        CssClass="btn btn-sm btn-danger ms-1">Eliminar</asp:LinkButton>
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