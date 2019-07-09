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
</asp:Content>
