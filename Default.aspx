<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Default.aspx.cs" Inherits="ADPASSWD3._Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <title>Reset Password</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.5/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-SgOJa3DmI69IUzQ2PVdRZhwQ+dy64/BUtbMJw1MZ8t5HZApcHrRKUc4W0kG879m7" crossorigin="anonymous">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />  
</head>
<body>
    <div class="row">
        <div class="col-sm-4"></div>
            <div class="col-sm-4">
                <form id="form1" runat="server" class="container mt-5">
                    <div class="card">
                        <div class="card-header">
                            <h3>Reset Password <i class="bi bi-building"> </i>XYZ</h3>
                        </div>
                        <div class="card-body">
                            <div class="form-group">
                                <i class="bi bi-person-vcard"></i>
                                <label for="txtUsername">Username:</label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <i class="bi bi-fingerprint"></i>
                                <label for="txtOldPassword">Old Password:</label>
                                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <i class="bi bi-fingerprint"></i>
                                <label for="txtNewPassword">New Password:</label>
                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control" />
                            </div>
                            <br />
                            <div class="form-group">
                                <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="btn btn-primary" OnClick="btnResetPassword_Click" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" />
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        <div class="col-sm-4"></div>
    </div>

    <div class="row">
        <div class="col-sm-4"></div>
            <div class="col-sm-4 container mt-5">
               <div class="card">
                    <div class="card-header">
                        <h3><i class="bi bi-exclamation-circle"> </i>Requirements</h3>
                    </div>
                    <div class="card-body">
                        <p title="อย่างน้อย 10 ตัวอักษร">- Minimum 10 characters</p>
                        <p title="ต้องมีตัวอักษรพิมพ์เล็ก 1 ตัว, ตัวอักษรพิมพ์ใหญ่ 1 ตัว, ตัวเลข 1 ตัว, และอักขระพิเศษ 1 ตัว">- At least 1 lowercase, 1 uppercase, 1 number, 1 special character</p>
                        <p title="ห้ามมีตัวอักษรซ้ำกัน (เช่น aa, 11)">- No repeated characters (e.g. aa, 11)</p>
                        <p title="รหัสผ่านต้องไม่มีชื่อผู้ใช้">- Password must not contain the username</p>
                        <p title="ต้องหลีกเลี่ยงรูปแบบที่อ่อนแอหรือทั่วไป (เช่น password, admin, 123456, qwerty)">- Must avoid weak or common patterns (e.g. password, admin, 123456, qwerty)</p>
                        <p title="ห้ามใช้รูปแบบเช่น abcd, 1234">- No patterns like abcd, 1234</p>
                    </div>
               </div>
           </div>
        <div class="col-sm-4"></div>
    </div>

    <br />
    <div class="text-sm text-center text-secondary mb-3">
        <div>Copyright &copy;2025 TECHISSOLUTION.COM</div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.5/dist/js/bootstrap.bundle.min.js" integrity="sha384-k6d4wzSIapyDyv1kpU366/PK5hCdSbCRGRCMv+eplOQJWyd1fbcAu9OCUj5zNLiq" crossorigin="anonymous"></script>
</body>
</html>
