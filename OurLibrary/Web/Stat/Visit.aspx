<%@ Page Title="Visitor Page" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="Visit.aspx.cs" Inherits="OurLibrary.Web.Stat.Visit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelInput" runat="server">
                <h2>Visitor Page Recording</h2>
                <table style="text-align: center" class="table">
                    <tr>
                        <td>
                            <p>
                                Input Student ID
                            </p>
                            <asp:TextBox ID="TextBoxStudentId" runat="server" />
                            <h3 class="clock"></h3>
                            <asp:Button CssClass="btn btn-default" ID="ButtonSearch" runat="server" Text="Submit" CausesValidation="true" OnClick="ButtonSearch_Click" />

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxStudentId"
                                ErrorMessage="Student Id Required" ForeColor="Red" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <p>Student Info</p>
                            <asp:Panel ID="PanelStudentInfo" runat="server">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript">

        function serverClock() {
           
            timeReq("/Web/Api/Info", document.querySelectorAll('.clock')[0]);
        }

        function timeReq(url, clock_label) {
            var request = new XMLHttpRequest();
            request.open("POST", url, true);
            request.onreadystatechange = function () {
                if (this.readyState == this.DONE && this.status == 200) {
                    if (this.responseText != null) {
                        let response_time = this.responseText;
                        response_time = JSON.parse(response_time);
                        clock_label.innerHTML = "Server Time:"+ response_time.data;
                    } else {
                        clock_label.innerHTML = "Server Error";
                    }

                } else {
                    clock_label.innerHTML = "Server Error";
                }
            }
            request.send();
        }

        setInterval(serverClock, 1000);
    </script>
</asp:Content>
