<%@ Page Title="Catalog-Home" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OurLibrary.Web.Catalog.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h2>Catalog Page</h2>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="SearchPanelUniversal" runat="server">
                <asp:Button CausesValidation="false" Text="Advanced Search" class="btn btn-primary" ID="ShowAdvanced" runat="server" CssClass="btn btn-success" OnClick="ShowAdvanced_Click" />

                <div role="form">
                    <asp:Label ID="LabelMessage" runat="server" />
                    <div class="form-group">
                        <label>Search Keyword</label>
                        <input type="text" class="form-control" id="SearchUniversalInput" runat="server" />
                    </div>

                </div>

            </asp:Panel>
            <asp:Panel ID="SearchPanelAdvanced" runat="server" Visible="false">
                <asp:Button CausesValidation="false" Text="Hide Advanced" class="btn btn-primary" ID="HideAdvanced" runat="server" CssClass="btn btn-warning" OnClick="HideAdvanced_Click" />

                <div role="form">
                    <asp:Label ID="Label1" runat="server" />
                    <div class="form-group">
                        <label>Title</label>
                        <input type="text" class="form-control" id="SearchTitle" runat="server" />
                    </div>
                    <div class="form-group">
                        <label>Author</label>
                        <input type="text" class="form-control" id="SearchAuthor" runat="server" />
                    </div>
                    <div class="form-group">
                        <label>Publisher</label>
                        <input type="text" class="form-control" id="SearchPublisher" runat="server" />
                    </div>
                    <div class="form-group">
                        <label>ISBN</label>
                        <input type="text" class="form-control" id="SearchISBN" runat="server" />
                    </div>
                    <div class="form-group">
                        <label>Category</label>
                        <input type="text" class="form-control" id="SearchCategory" runat="server" />
                    </div>

                </div>

            </asp:Panel>
            
            <div class="form-group">
                <asp:Button CausesValidation="false"  Text="Search" class="btn btn-primary" ID="SearchButton" runat="server" OnClick="SearchButton_Click" />
            </div>
            <asp:Panel runat="server" ID="ListPanel">
                <p class="form-control-static" style="width: 326px">
                    Item to show:
            <asp:TextBox ID="TextBoxLimit" CssClass="form-control" OnTextChanged="TextBoxLimit_TextChanged" runat="server" CausesValidation="false" TextMode="Number" AutoPostBack="True">5</asp:TextBox>

                </p>
                <p id="nav_info" runat="server" style="width: 200px">
                    &nbsp;
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TextBoxLimit" ErrorMessage="Minimum value is 1" ForeColor="Red" MaximumValue="1000" MinimumValue="1"></asp:RangeValidator>
                </p>
                <asp:Panel ID="PanelNavigation" runat="server" CssClass="btn-group btn-group-sm">
                </asp:Panel>
               <p></p>
                <div id="CatalogPanel" runat="server" class="row">

                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
