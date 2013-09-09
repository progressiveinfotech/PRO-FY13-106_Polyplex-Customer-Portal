<%@ Page Title="" Language="C#" MasterPageFile="~/PolyplexMasterMain.master" AutoEventWireup="true"
    CodeFile="UserCreation.aspx.cs" Inherits="ApprovalForm" %>

<%@ Register Src="~/controls/MyMessageBox.ascx" TagName="MyMessageBox" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="CSS/PolyplexMaster.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="CSS/popupstyle.css" type="text/css" />
    <link href="CSS/grid.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
    </asp:ToolkitScriptManager>
    <div style="height: auto; position: relative;">
        <table style="width: 100%;">
            <tr valign="bottom" style="height: 20px">
                <td style="width: 10%">
                </td>
                <td style="width: 25%">
                </td>
                <td style="width: 35%">
                </td>
                <td style="width: 30%">
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td align="right">
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" Text="Employee Code: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeeCode" runat="server" TabIndex="1" Width="80%"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td align="right">
                    <asp:Label ID="Label52" runat="server" Text="Employee Name: "></asp:Label>
                    <span style="color: Red; font-weight: bold">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeeName" runat="server" TabIndex="2" Width="80%"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                        FilterType="Custom,LowercaseLetters,UppercaseLetters" TargetControlID="txtEmployeeName"
                        ValidChars=" ">
                    </asp:FilteredTextBoxExtender>
                    <br />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td align="right">
                    <asp:Label ID="Label5" runat="server" Text="Employee Location: "></asp:Label>
                    <span style="color: Red; font-weight: bold">*</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEmployeeLocation" TabIndex="3" CssClass="txtbx" Width="82%"
                        runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right">
                    &nbsp;
                </td>
                <td align="right">
                    Employee Type:
                </td>
                <td>
                    <asp:DropDownList ID="DdlUserType" TabIndex="3" CssClass="txtbx" Width="82%" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td align="right">
                    <asp:Label ID="Label1" runat="server" Text="Email ID: "></asp:Label>
                    <span style="color: Red; font-weight: bold">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtEmailID" runat="server" TabIndex="4" Width="80%"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" Text="Login ID: "></asp:Label>
                    <span style="color: Red; font-weight: bold">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtLoginId" runat="server" TabIndex="5" Width="80%"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td align="right">
                    <asp:Label ID="Label4" runat="server" Text="User Group: "></asp:Label>
                    <span style="color: Red; font-weight: bold">*</span>
                </td>
                <td>
                    <%--<asp:DropDownList ID="ddlUserGroup" TabIndex="6" CssClass="txtbx" Width="82%" runat="server">
                    </asp:DropDownList>--%>
                    <asp:ListBox ID="listBoxUserGroup" TabIndex="6" DataTextField="Group_Name" DataValueField="GroupID"
                        CssClass="txtbx" Width="82%" SelectionMode="Multiple" runat="server"></asp:ListBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right">
                    &nbsp;
                </td>
                <td align="right">
                    Active:
                </td>
                <td>
                    <asp:CheckBox ID="ChkStatus" Checked="true" runat="server" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr style="height: 10px">
                <td colspan="4">
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td align="right">
                </td>
                <td align="center" colspan="2">
                    <asp:ImageButton ID="btnSave" TabIndex="7" runat="server" Text="Save" ValidationGroup="1"
                        ImageUrl="~/Images/btnSave.png" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="ImageBtnCancel" TabIndex="8" runat="server" Text="Cancel" CausesValidation="false"
                        ImageUrl="~/Images/btnCancel.png" OnClientClick="window.close();" />
                    <uc1:MyMessageBox ID="MyMessageBoxInfo" runat="server" ShowCloseButton="true" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeName"
            Display="None" ErrorMessage="Employee Name is mandatory." ValidationGroup="1"
            ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
            TargetControlID="RequiredFieldValidator1">
        </asp:ValidatorCalloutExtender>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEmailID"
            Display="None" ErrorMessage="Email Id is mandatory." ValidationGroup="1" ForeColor="Red"
            SetFocusOnError="true"></asp:RequiredFieldValidator>
        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
            TargetControlID="RequiredFieldValidator3">
        </asp:ValidatorCalloutExtender>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Incorrect Email Id."
            Display="None" ValidationGroup="1" ControlToValidate="txtEmailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
            TargetControlID="RegularExpressionValidator1">
        </asp:ValidatorCalloutExtender>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLoginId"
            Display="None" ErrorMessage="Login ID is mandatory." ValidationGroup="1" ForeColor="Red"
            SetFocusOnError="true"></asp:RequiredFieldValidator>
        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
            TargetControlID="RequiredFieldValidator2">
        </asp:ValidatorCalloutExtender>
    </div>
    <asp:Panel ID="PnlShowSerach" runat="server" Height="400px" Width="650px" CssClass="modalPopup"
        ScrollBars="Auto" Style="display: none">
        <asp:Panel ID="Panel3" runat="server" Style="cursor: pointer">
            <table width="100%">
                <tr>
                    <td>
                        <div style="margin: 10px 0px 10px 20px">
                            <img src="Images/Polyplex Logo.png" height="40px" width="160px" alt="Polyplex" style="border: 1px solid black" /></div>
                    </td>
                    <td valign="top">
                        <div style="float: right; padding: 10px 10px 0 0">
                            <asp:ImageButton ID="ImgBtnCancel" runat="server" AlternateText="Cancel" ImageUrl="~/Images/delete.gif" /></div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div style="margin-left: 20px; margin-right: 20px">
            <table width="100%" cellpadding="0px" cellspacing="0px">
                <tr>
                    <td>
                        <div class="in_menu_head">
                            User List</div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="0px" cellspacing="0px">
                            <tr>
                                <td style="width: 20%">
                                    <asp:Label runat="server" Text="Search:" ID="lSearchList"></asp:Label>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtSearchList" runat="server" TabIndex="27" Width="80%"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearchlist" runat="server" TabIndex="10" Text="Submit" CausesValidation="false"
                                        OnClick="btnSearchlist_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvSearchList" runat="server" AutoGenerateColumns="false" Width="100%"
                            CssClass="GridViewStyle" ShowHeaderWhenEmpty="True" EmptyDataText="No Record Found."
                            AllowPaging="true" PageSize="6" OnPageIndexChanging="gvSearchList_PageIndexChanging"
                            OnRowCommand="gvSearchList_RowCommand">
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument='<%#Eval("UserId") %>'
                                                CommandName="select" ImageUrl="~/Images/chkbxuncheck.png" Text="Select" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserCode" HeaderText="User Code">
                                    <HeaderStyle HorizontalAlign="Center" Width="300px" />
                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserName" HeaderText="UserName">
                                    <HeaderStyle HorizontalAlign="Center" Width="300px" />
                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LocationCodeName" HeaderText="Location">
                                    <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LoginId" HeaderText="LoginId">
                                    <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EmailID" HeaderText="Email ID">
                                    <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="RowStyle" />
                            <SelectedRowStyle CssClass="SelectedRowStyle" />
                            <AlternatingRowStyle CssClass="AltRowStyle" />
                            <HeaderStyle CssClass="HeaderStyle" />
                            <PagerStyle CssClass="PagerStyle" HorizontalAlign="Left" BackColor="#C6C3C6" ForeColor="Black" />
                        </asp:GridView>
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Label"></asp:Label><br />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Label6"
        PopupControlID="PnlShowSerach" BackgroundCssClass="modalBackground" DropShadow="true"
        PopupDragHandleControlID="Panel3" CancelControlID="ImgBtnCancel" />
    <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
    <div>
        <asp:HiddenField ID="HidAutoId" runat="server" />
    </div>
</asp:Content>
