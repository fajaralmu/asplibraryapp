<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="CheckStudentIssue.aspx.cs" Inherits="OurLibrary.Web.Admin.Transaction.CheckStudentIssue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3>Check Student Issue</h3>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelInput" runat="server">
                <table>
                    <tr valign="top">
                        <td>
                            <p>Input Student ID:</p>
                            <asp:TextBox ID="TextBoxStudentID" runat="server" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxStudentID"
                                ForeColor="Red" ErrorMessage="ID required" />
                            <p>Input Duration:</p>
                            <asp:TextBox Text="14" ID="TextBoxDuration" TextMode="Number" runat="server" />
                        </td>
                        <td>
                            <p>Input Issue Record To Return:</p>
                            <asp:TextBox ID="TextBoxIssueRecordId" runat="server" />
                        </td>
                        <td rowspan="3" >
                             <asp:Panel ID="PanelBookIssues" runat="server"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="ButtonSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="ButtonSearch_Click" />
                        </td>
                        <td>
                            <asp:Button ID="ButtonReturn" runat="server" Text="Add" CssClass="btn btn-default" OnClick="ButtonReturn_Click" />
                            <asp:Button ID="ButtonSubmitReturn" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="ButtonSubmitReturn_Click" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <h4>Issue History</h4>
                            <hr />
                            <asp:Panel ID="PanelIssueLis" runat="server">
                            </asp:Panel>
                        </td>
                        <td>
                            <h4>Return History</h4>
                            <hr />
                            <asp:Panel ID="PanelIssueReturn" runat="server">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
