<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectConversion.ascx.cs"
    Inherits="CMSModules_WebAnalytics_FormControls_SelectConversion" %>
<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelector"
    TagPrefix="cms" %>
<asp:Panel runat="server" ID="pnlSelectButton" Visible="false">
    <cms:CMSTextBox runat="server" ID="txtConversion" CssClass="SelectorTextBox" MaxLength="100" />
    <cms:LocalizedButton runat="server" ID="btnSelect" ResourceString="general.select"
        CssClass="ContentButton" />
</asp:Panel>
<asp:Panel runat="server" ID="pnlSelectDropDown" Visible="false">
    <asp:DropDownList runat="server" ID="ddlConversions" CssClass="DropDownField" />
</asp:Panel>
<asp:Panel runat="server" ID="pnlSelectObject">
    <cms:CMSUpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <cms:UniSelector ID="usConversions" ShortID="ss" runat="server" ObjectType="analytics.conversion" AllowEditTextBox="true"   
                ReturnColumnName="ConversionName" SelectionMode="SingleTextBox" ResourcePrefix="conversionselect" AllowAll="true" AllowEmpty="false" />
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</asp:Panel>
