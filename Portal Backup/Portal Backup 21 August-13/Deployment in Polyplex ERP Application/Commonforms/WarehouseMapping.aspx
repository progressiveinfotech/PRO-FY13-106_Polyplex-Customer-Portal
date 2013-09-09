<%@ Page Title="Final Destination" Language="C#" MasterPageFile="~/Masters/EmptyMaster.master"
    AutoEventWireup="true" CodeFile="WarehouseMapping.aspx.cs" Inherits="Sales_MasterFinalDestination" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/controls/MyMessageBox.ascx" TagName="MyMessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/grid.css" type="text/css" rel="Stylesheet" />
    <style type="text/css">
        .customCalloutStyle div, .customCalloutStyle td
        {
            border: solid 1px Black;
            background-color: #BDDEFF;
            color: Black;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .txtbox
        {
            width: 130px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScript" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <br />
                <div align="center">
                    <table>
                        <tr>
                            <td align="right">
                                <strong>Search:</strong>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                </div>
                </td>
                <td align="left">
                    &nbsp;&nbsp;
                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                        CausesValidation="false" OnClick="btnSearch_Click" />
                </td>
                </tr> </table>
                <br />
                <table width="100%" cellpadding="3px">
                    <tr>
                        <td colspan="2">
                            <center>
                                <asp:Panel ID="Panel2" runat="server">
                                    <asp:GridView ID="gvMapping" runat="server" AllowPaging="True" PageSize="5" EmptyDataText="No Records Found."
                                        ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" DataKeyNames="AutoId"
                                        CssClass="GridViewStyle" OnPageIndexChanging="gvMapping_PageIndexChanging" OnSelectedIndexChanged="gvMapping_SelectedIndexChanged"
                                        OnRowDataBound="gvMapping_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument='<%#Eval("AutoId") %>'
                                                            CommandName="select" ImageUrl="~/Images/chkbxuncheck.png" Text="Select" /></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode"></asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="CustomerName"></asp:BoundField>
                                            <asp:BoundField DataField="PlantName" HeaderText="Plant"></asp:BoundField>
                                            <asp:BoundField DataField="StorageLocCode" HeaderText="User Code"></asp:BoundField>
                                            <asp:BoundField DataField="Location" HeaderText="User Name"></asp:BoundField>
                                            <asp:BoundField DataField="ActiveStatus" HeaderText="Active Status"></asp:BoundField>
                                            <asp:BoundField DataField="CustomerId" HeaderText="Customer ID"></asp:BoundField>
                                            <asp:BoundField DataField="PlantId" HeaderText="Plant ID"></asp:BoundField>
                                            <asp:BoundField DataField="WareHouseId" HeaderText="WareHouse ID"></asp:BoundField>
                                        </Columns>
                                        <RowStyle CssClass="RowStyle" />
                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                        <HeaderStyle CssClass="HeaderStyle" />
                                    </asp:GridView>
                                </asp:Panel>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <center>
                                <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                            </center>
                        </td>
                    </tr>
                    <tr style="height: 5px">
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td width="35%">
                            <div align="right">
                                <asp:Label runat="server" Text="Customer Code:" ID="Label11"></asp:Label>
                                <span style="color: Red; font-weight: bold">*</span></div>
                        </td>
                        <td width="50%">
                            <div align="left">
                                <asp:TextBox runat="server" Width="200px" ID="txtCustomerCode" AutoPostBack="True"
                                    OnTextChanged="txtCustomerCode_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnCustomerCode" runat="server" Height="16px" ImageUrl="~/images/select.gif"
                                    CausesValidation="False" OnClick="imgBtnCustomerCode_Click" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div align="right">
                                <asp:Label runat="server" Text="Customer Name:" ID="Label12"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div align="left">
                                <asp:TextBox ID="txtCustomerName" runat="server" Width="200px" CssClass="txtbox"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div align="right">
                                <asp:Label runat="server" Text="Plant:" ID="Label4"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div align="left">
                                <asp:DropDownList ID="ddlPlant" Width="205px" runat="server" CssClass="txtbox">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="35%">
                            <div align="right">
                                <asp:Label runat="server" Text="Warehouse Code:" ID="Label1"></asp:Label>
                                <span style="color: Red; font-weight: bold">*</span></div>
                        </td>
                        <td width="50%">
                            <div align="left">
                                <asp:TextBox runat="server" Width="200px" ID="txtWarehouseCode" AutoPostBack="True"
                                    OnTextChanged="txtWarehouseCode_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnWarehouseCode" runat="server" Height="16px" ImageUrl="~/images/select.gif"
                                    CausesValidation="False" OnClick="imgBtnWarehouseCode_Click" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div align="right">
                                <asp:Label runat="server" Text="Warehouse Name:" ID="Label2"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div align="left">
                                <asp:TextBox ID="txtWarehouseName" runat="server" Width="200px" CssClass="txtbox"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div align="right">
                                Active Status:</div>
                        </td>
                        <td>
                            <div align="left">
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div align="center">
                                <asp:ImageButton ID="ImgBtnSave" runat="server" Text="Save" ValidationGroup="1" ImageUrl="~/Images/btnSave.png"
                                    OnClick="ImgBtnSave_Click" />
                                <asp:ImageButton ID="ImageBtnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                    ImageUrl="~/Images/btnCancel.png" OnClientClick="window.close();" />
                                <uc1:MyMessageBox ID="MyMessageBoxInfo" runat="server" ShowCloseButton="true" />
                            </div>
                        </td>
                        <%--<td align="left">
                            
                        </td>--%>
                    </tr>
                </table>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="1" runat="server"
                        ErrorMessage="Customer Code is mandatory." Display="None" ControlToValidate="txtCustomerCode"
                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                            ID="ValidatorCalloutExtender1" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                        </asp:ValidatorCalloutExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="1" runat="server"
                        ErrorMessage="Warehouse Code is mandatory." Display="None" ControlToValidate="txtWarehouseCode"
                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                            ID="ValidatorCalloutExtender2" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                        </asp:ValidatorCalloutExtender>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <asp:Panel ID="PanelShowPopUpGrid" runat="server" Height="400px" Width="650px" CssClass="modalPopup"
            Style="display: none">
            <asp:Panel ID="Panel3" runat="server" Style="cursor: pointer">
                <table width="100%">
                    <tr>
                        <td>
                            <div style="margin: 10px 0px 10px 20px">
                                <img src="../Images/Polyplex Logo.png" height="40px" width="160px" alt="Polyplex"
                                    style="border: 1px solid black" /></div>
                        </td>
                        <td valign="top">
                            <div style="float: right; padding: 10px 10px 0 0">
                                <asp:ImageButton ID="ImgBtnCancelPopUp" runat="server" AlternateText="Cancel" ImageUrl="~/Images/delete.gif" /></div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="margin-left: 20px; margin-right: 20px">
                <table width="100%" cellpadding="0px" cellspacing="0px">
                    <tr>
                        <td>
                            <div class="in_menu_head">
                                <asp:Label ID="lPopUpHeader" runat="server" Text="PopUp"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0px" cellspacing="0px">
                                <tr>
                                    <td style="width: 27%">
                                        <asp:Label runat="server" Text="Search:" ID="lSearch"></asp:Label>
                                    </td>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="txtSearchFromPopup" runat="server" TabIndex="27" Width="80%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSearchInPopUp" runat="server" TabIndex="10" Text="Submit" CausesValidation="false"
                                            OnClick="btnSearchInPopUp_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel4" Height="250px" Width="100%" runat="server" ScrollBars="Auto">
                                <asp:GridView ID="gvPopUpGrid" runat="server" AutoGenerateColumns="false" Width="98%"
                                    CssClass="GridViewStyle" ShowHeaderWhenEmpty="True" EmptyDataText="No Record Found."
                                    AllowPaging="true" PageSize="5" OnRowCommand="gvPopUpGrid_RowCommand" OnRowDataBound="gvPopUpGrid_RowDataBound"
                                    OnPageIndexChanging="gvPopUpGrid_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="select"
                                                        ImageUrl="~/Images/chkbxuncheck.png" Text="Select" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="RowStyle" />
                                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                                    <AlternatingRowStyle CssClass="AltRowStyle" />
                                    <HeaderStyle CssClass="HeaderStyle" />
                                    <PagerStyle CssClass="PagerStyle" HorizontalAlign="Left" BackColor="#C6C3C6" ForeColor="Black" />
                                </asp:GridView>
                            </asp:Panel>
                            <asp:Label ID="lblTotalRecordsPopUp" runat="server" Text="Label"></asp:Label><br />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="Label7"
            PopupControlID="PanelShowPopUpGrid" BackgroundCssClass="modalBackground" DropShadow="true"
            PopupDragHandleControlID="Panel2" CancelControlID="ImgBtnCancelPopUp" />
    </div>
    <div>
        <asp:HiddenField ID="HidAutoId" Value="0" runat="server" />
        <asp:HiddenField ID="HidWarehouseId" runat="server" />
        <asp:HiddenField ID="HidCustomerId" runat="server" />
        <asp:HiddenField ID="HidPopUpType" runat="server" />
    </div>
</asp:Content>
