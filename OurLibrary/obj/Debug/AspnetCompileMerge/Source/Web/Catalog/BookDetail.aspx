<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="BookDetail.aspx.cs" Inherits="OurLibrary.Web.Catalog.BookDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="BookDetailPanel" runat="server">
        <h2 id="BannerTitle" runat="server">Book Detail</h2>
        <table class="table">
            <tr>
                <td style="width: 180px">&nbsp;</td>
                <td style="width: 303px">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Label ID="Label1" runat="server" Text="Title"></asp:Label>
                </td>
                <td style="width: 303px">
                    <asp:Label ID="LabelTitle" runat="server" Text="Label"></asp:Label>
                </td>
                <td rowspan="6">
                    <a class="thumbnail" style="width: 170px; text-align:center"  id="ImageThumbnail" runat="server">
                        <asp:Image Width="150" Height="200" ID="BookCoverImage" runat="server" BorderColor="#999999" BorderStyle="Outset" />

                    </a>
                </td>
            </tr>
            <tr>
                <td style="height: 22px; width: 180px">
                    <asp:Label ID="Label2" runat="server" Text="Author"></asp:Label>
                </td>
                <td style="height: 22px; width: 303px;">
                    <asp:Label ID="LabelAuthor" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Label ID="Label3" runat="server" Text="Publisher"></asp:Label>
                </td>
                <td style="width: 303px">
                    <asp:Label ID="LabelPublisher" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Label ID="Label4" runat="server" Text="Category"></asp:Label>
                </td>
                <td style="width: 303px">
                    <asp:Label ID="LabelCategory" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Label ID="Label5" runat="server" Text="ISBN"></asp:Label>
                </td>
                <td style="width: 303px">
                    <asp:Label ID="LabelISBN" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Label ID="Label6" runat="server" Text="Page"></asp:Label>
                </td>
                <td style="width: 303px">
                    <asp:Label ID="LabelPage" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Label ID="Label7" runat="server" Text="Review"></asp:Label>
                </td>
                <td style="width: 303px" rowspan="2">
                    <asp:Label ID="LabelReview" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Availability"></asp:Label>
                    &nbsp;:
                    <asp:Label ID="LabelAvailabilityStatus" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:LinkButton runat="server" CssClass="btn btn-primary" PostBackUrl="~/Web/Catalog/">Back To Catalog</asp:LinkButton>
                </td>
                <td>
                    <asp:Panel ID="PanelAvailabilityDetail" runat="server">
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <p>
            &nbsp;
        </p>
    </asp:Panel>
</asp:Content>
