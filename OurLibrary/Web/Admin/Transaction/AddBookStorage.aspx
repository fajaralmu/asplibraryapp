<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="AddBookStorage.aspx.cs" Inherits="OurLibrary.Web.Admin.Transaction.AddBookStorage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelAddBookStorage" runat="server">
                <h2>Form Add New Storage </h2>
                <table class="nav-justified">
                    <tr>
                        <td style="width: 280px">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 280px">Master Book<br />
                            <asp:Label ID="Label1" runat="server" Text="Current Book: "></asp:Label>
                            <asp:Label ID="LabelCurrentBook" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="InputMasterBook" runat="server" />
                            <asp:Button CausesValidation="false" runat="server" ID="ButtonSearchMasterBook"
                                Text="Search Book" OnClick="ButtonSearchMasterBook_Click" CssClass="btn btn-primary" />
                            <!--   <input onkeyup="cek()" type="text" runat="server" class="form-control" id="inputMasterBookId" />

                             -->
                            <asp:Panel runat="server" ID="PanelBookList">
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 280px">Book Code</td>
                        <td>
                            <input type="text" runat="server" class="form-control" id="inputBookCode" /></td>
                    </tr>
                    <tr>
                        <td style="width: 280px">Additional Info</td>
                        <td>
                            <textarea id="inputAdditionalInfo" runat="server" class="form-control">

                    </textarea></td>
                    </tr>
                    <tr>
                        <td style="width: 280px">
                            <asp:Button ID="ButtonSave" runat="server" CssClass="btn btn-primary" OnClick="ButtonSave_Click" Text="Save" />
                        </td>
                        <td>
                            <asp:Label ID="LabelMessage" runat="server" /></td>
                    </tr>
                </table>

            </asp:Panel>
            <asp:Panel runat="server" ID="ListPanel">
                <p style="width: 200px">
                    Limit:
            <asp:TextBox ID="TextBoxLimit" runat="server" CausesValidation="false" TextMode="Number" AutoPostBack="True">5</asp:TextBox>

                </p>
                <asp:Panel ID="PanelNavigation" runat="server" CssClass="btn-group btn-group-sm">
                </asp:Panel>

                <asp:Table ID="TableList" CssClass="table table-striped" runat="server" Width="800px">
                    <asp:TableRow runat="server" AccessKey="1" TableSection="TableHeader" BackColor="#66CCFF" Font-Overline="False" Font-Strikeout="False" ForeColor="#0000CC">
                        <asp:TableCell runat="server" Font-Bold="True">No</asp:TableCell>
                        <asp:TableCell runat="server" Font-Bold="True">Record Id</asp:TableCell>
                        <asp:TableCell runat="server" Font-Bold="True">Book Code</asp:TableCell>
                        <asp:TableCell runat="server" Font-Bold="True">Book Title</asp:TableCell>
                        <asp:TableCell runat="server" Font-Bold="True">Info</asp:TableCell>
                        <asp:TableCell runat="server" Font-Bold="True">Option</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function cek() {
            alert("HELLO");
        }

    </script>
</asp:Content>
