<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="BookIssue.aspx.cs" Inherits="OurLibrary.Web.Admin.Transaction.BookIssue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3>Book Issue Page</h3>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="info" runat="server">
            </div>
            <asp:Panel runat="server">

                <table class="nav-justified" style="height: 108px">
                    <tr>
                        <td style="width: 124px">Sudent ID</td>
                        <td style="width: 174px">
                            <asp:TextBox ID="TextBoxStudentID" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 275px">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxStudentID" ErrorMessage="Student Id Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 124px">Record ID</td>
                        <td style="width: 174px">
                            <asp:TextBox ID="TextBoxRecordId" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 275px">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxRecordId" ErrorMessage="Record Id Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 124px">or</td>
                        <td style="width: 174px">Rec ID:<asp:Label ID="LabelRecordList" runat="server">
                        </asp:Label>
                        </td>
                        <td style="width: 275px">
                            <asp:Label ID="LabelCurrentBook" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 124px; height: 38px;">Search By Title</td>
                        <td style="width: 174px; height: 38px;">
                            <asp:TextBox ID="InputMasterBook" runat="server" />
                            <asp:Button ID="ButtonSearchMasterBook" runat="server" CausesValidation="false" CssClass="btn btn-primary" OnClick="ButtonSearchMasterBook_Click" Text="Search Book" />

                        </td>
                        <td style="height: 38px; width: 275px"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="ButtonSave" CssClass="btn btn-primary" CausesValidation="false" runat="server" Text="Add" OnClick="ButtonSave_Click" />
                            <asp:Button ID="ButtonClearList" CausesValidation="false" runat="server" CssClass="btn btn-danger" OnClick="ButtonClearList_Click" Text="Clear" />

                        </td>
                        <td colspan="2">
                            <asp:Panel runat="server" ID="PanelBookList">
                            </asp:Panel>
                        </td>

                    </tr>
                </table>



                <asp:Panel ID="PanelBookIssues" runat="server">
                </asp:Panel>
                <asp:Button ID="ButtonSaveIssue" CssClass="btn btn-primary" runat="server" Text="Commit Issue" OnClick="ButtonSaveIssue_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function SetTextInput(input, textId) {
            let element = document.getElementById(textId);
            element.value = input;
        }
    </script>
</asp:Content>
