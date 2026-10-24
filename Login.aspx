<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="PruebasFunMic.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar sesión</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container d-flex align-items-center justify-content-center min-vh-100">
            <div class="card shadow-sm w-100" style="max-width: 400px;">
                <div class="card-body p-4">

                    <!-- Imagen centrada -->
                    <div class="text-center mb-4">
                        <img src="Img/FundaMicro.png" alt="Logo Empresa" class="img-fluid" style="max-height: 120px;" />
                    </div>

                    <h1 class="h4 text-center mb-4">Iniciar sesión</h1>

                    <div class="mb-3">
                        <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario"
                            CssClass="form-label" Text="Usuario" />
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"
                            MaxLength="50" autofocus="autofocus" autocomplete="username" />
                        <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario"
                            ErrorMessage="Ingrese el usuario." CssClass="text-danger small" Display="Dynamic" />
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword"
                            CssClass="form-label" Text="Contraseña" />
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                            TextMode="Password" MaxLength="100" autocomplete="current-password" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                            ErrorMessage="Ingrese la contraseña." CssClass="text-danger small" Display="Dynamic" />
                    </div>

                    <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-primary w-100 mt-2" />

                    <asp:Label ID="lblMensaje" runat="server" CssClass="d-block text-danger small text-center mt-3" />

                </div>
            </div>
        </div>
    </form>
</body>
</html>