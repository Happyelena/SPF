<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLibInfo.aspx.cs" Inherits="Website.AddLibInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Lib Info</title>
    <script type="text/javascript" src="static/js/jquery-1.6.1.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#btnAddLib").click(function () {
                var $libName = $("#txtLibName").val();
                var $libAddress = $("#txtLibAddress").val();
                var $libPhone = $("#txtLibPhone").val();

                $.ajax({ url: "poll.aspx", type: "post", data: ("txtLibName=" +
                $libName + "&txtLibAddress=" +
                $libAddress + "&txtLibPhone=" +
                $libPhone), success: function (msg) {
                    alert(msg);
                }
                });
            });
        });
    </script>
</head>
<body>
    <h2>
        This is the test page to test add Lib Info</h2>
    <table>
        <thead>
        </thead>
        <tbody>
            <tr>
                <td>
                    LibName:
                </td>
                <td>
                    <input type="text" name="txtLibName" id="txtLibName" />
                </td>
            </tr>
            <tr>
                <td>
                    libAddress:
                </td>
                <td>
                    <input type="text" name="txtLibAddress" id="txtLibAddress" />
                </td>
            </tr>
            <tr>
                <td>
                    libPhone:
                </td>
                <td>
                    <input type="text" name="txtLibPhone" id="txtLibPhone" />
                </td>
            </tr>
            <tr>
                <td>
                    <input type="button" id="btnAddLib" value="Add" />
                </td>
            </tr>
        </tbody>
    </table>
</body>
</html>
