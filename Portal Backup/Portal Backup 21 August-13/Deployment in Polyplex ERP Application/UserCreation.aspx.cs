using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class ApprovalForm : System.Web.UI.Page
{
    #region********************************************Variables ************************************************

    Common com = new Common();
    Connection objConnectionClass = new Connection();
    Common_Message objcommonmessage = new Common_Message(); 
    string ErrorStatus, RecordNo;

    #endregion

    #region********************************************Events ************************************************

    protected void Page_Load(object sender, EventArgs e)
    {       
        
        if (!IsPostBack)
        {
            Log.PageHeading(this, "User Creation");

            ViewState["LoginId"]=null;
            txtEmployeeCode.Text = AutogenerateNo();
            BindEmployeeLocation();
            BindUserGroup();
            BindSearchList();
            //Added by Lalit on 9July 2013
            BindUserType();
            //End
            txtEmployeeName.Focus();
            GetGroupIdOfUser("0");

            txtEmployeeCode.Attributes.Add("readonly", "true");
            txtEmployeeCode.Attributes.Add("style", "background:lightgray");
        }

        ImageButton btnAdd = (ImageButton)Master.FindControl("btnAdd");
        btnAdd.CausesValidation = false;
        btnAdd.Click += new ImageClickEventHandler(btnAdd_Click);

        ImageButton imgbtnSearch = (ImageButton)Master.FindControl("imgbtnSearch");
        imgbtnSearch.CausesValidation = false;
        imgbtnSearch.Click += new ImageClickEventHandler(imgbtnSearch_Click);
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        ClearAll();
        DropDownList ddlSearch = (DropDownList)Master.FindControl("ddlSearch");
        TextBox txtSearch = (TextBox)Master.FindControl("txtSearch");
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = "";
    }    

    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DropDownList ddlSearch = (DropDownList)Master.FindControl("ddlSearch");
            TextBox txtSearch = (TextBox)Master.FindControl("txtSearch");
            txtSearchList.Text = "";

            Get_AllUserList(ddlSearch.SelectedValue.ToString(), txtSearch.Text.Trim());
            lSearchList.Text = "Search By " + ddlSearch.SelectedItem.ToString() + ": ";
        }
        catch (Exception ex) { }

        ModalPopupExtender1.Show();
    }

    protected void gvSearchList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView gvSearchList = (GridView)sender;
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            gvSearchList.SelectedIndex = row.RowIndex;

            if (e.CommandName == "select")
            {
                foreach (GridViewRow oldrow in gvSearchList.Rows)
                {
                    ImageButton imgbutton = (ImageButton)oldrow.FindControl("ImageButton1");
                    imgbutton.ImageUrl = "~/Images/chkbxuncheck.png";
                }
                ImageButton img = (ImageButton)row.FindControl("ImageButton1");
                img.ImageUrl = "~/Images/chkbxcheck.png";

                HidAutoId.Value = Convert.ToString(e.CommandArgument);
                BindHeaderRecords(HidAutoId.Value);
            }
        }
        catch (Exception ex) { }
    }

    protected void gvSearchList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSearchList.PageIndex = e.NewPageIndex;
            DropDownList ddlSearch = (DropDownList)Master.FindControl("ddlSearch");
            TextBox txtSearch = (TextBox)Master.FindControl("txtSearch");

            Get_AllUserList(ddlSearch.SelectedValue.ToString(), txtSearch.Text.Trim());
            lSearchList.Text = "Search By " + ddlSearch.SelectedItem.ToString() + ": ";
            ModalPopupExtender1.Show();
        }
        catch (Exception ex) { }
    }

    protected void btnSearchlist_Click(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlSearch = (DropDownList)Master.FindControl("ddlSearch");
            Get_AllUserList(ddlSearch.SelectedValue.ToString(), txtSearchList.Text.Trim());
            txtSearchList.Focus();
        }
        catch (Exception ex) { }

        ModalPopupExtender1.Show();
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            if (HidAutoId.Value == "")
            {
                string sql = "select LoginID from Com_Login_Mst where LoginID ='" + txtLoginId.Text.Trim() + "'";
                dt = com.executeSqlQry(sql);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["LoginID"].ToString().ToLower() == txtLoginId.Text.Trim().ToLower())
                    {
                        MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, "Login ID is already created.", 125, 300);
                        return;
                    }
                }
                dt = null;
            }
            else
            {
                if (ViewState["LoginId"].ToString().ToLower () != txtLoginId.Text.Trim().ToLower ())
                {
                    string sql = "select LoginID from Com_Login_Mst where LoginID ='" + txtLoginId.Text.Trim() + "'";
                    dt = com.executeSqlQry(sql);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["LoginID"].ToString().ToLower() == txtLoginId.Text.Trim().ToLower())
                        {
                            MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, "Login ID is already created.", 125, 300);
                            return;
                        }
                    }
                    dt = null;
                }
            }

            #region Line Item Records

            DataTable dtlineitem = new DataTable();
            dtlineitem = (DataTable)ViewState["Group"];
            int TotalLineItem = dtlineitem.Rows.Count;

            if (dtlineitem.Rows.Count > 0)
            {
                for (int i = TotalLineItem; i > 0; i--)
                {
                    dtlineitem.Rows.RemoveAt(i-1);
                }
            }
            for (int i = 0; i < Convert.ToInt32(ViewState["TotalGroup"].ToString()); i++)
            {
                if (listBoxUserGroup.Items[i].Selected == true)
                {
                    DataRow objdrLineItem = dtlineitem.NewRow();
                    objdrLineItem["GroupID"] = com.STRToInt(listBoxUserGroup.Items[i].Value);
                    dtlineitem.Rows.Add(objdrLineItem);
                }
            }

            if (dtlineitem.Rows.Count == 0)
            {
                MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info,"Select atleast one user group.", 125, 300);
                return;
            }

            #endregion

            objConnectionClass.OpenConnection();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Connection = objConnectionClass.PolypexSqlConnection;
            cmd.CommandTimeout = 60;
            cmd.CommandType = CommandType.StoredProcedure;

            if (HidAutoId.Value == "")
            {
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = 0;
            }
            else
            {
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt32(HidAutoId.Value);
            }            
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = txtEmployeeName.Text.Trim();
            cmd.Parameters.Add("@LocationID", SqlDbType.Int).Value = Convert.ToInt32(ddlEmployeeLocation.SelectedValue);
            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar).Value = txtEmailID.Text.Trim();
            cmd.Parameters.Add("@LoginID", SqlDbType.VarChar).Value = txtLoginId.Text.Trim ();
            //Added by Lalit 9July 2013
            cmd.Parameters.Add("@UserTypeId", SqlDbType.VarChar).Value = DdlUserType.SelectedValue;
            //End
            #region Table Parameter

            cmd.Parameters.AddWithValue("@dtLineItemsOfGroup", dtlineitem);

            #endregion

            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Bit).Value = ChkStatus.Checked;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"].ToString());
            cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = Convert.ToInt32(Session["UserId"].ToString());

            cmd.Parameters.Add(new SqlParameter("@ErrorStatus", SqlDbType.VarChar, 10));
            cmd.Parameters["@ErrorStatus"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@NewEmployeeCode", SqlDbType.VarChar, 30);
            cmd.Parameters["@NewEmployeeCode"].Direction = ParameterDirection.Output;

            cmd.CommandText = "SP_InsertUpdate_In_Com_UserAndLogin_Mst";
            cmd.ExecuteNonQuery();

            ErrorStatus = cmd.Parameters["@ErrorStatus"].Value.ToString();
            RecordNo = cmd.Parameters["@NewEmployeeCode"].Value.ToString();

            if (ErrorStatus == "0")
            {
                if (RecordNo != "0")
                {
                    MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, objcommonmessage.RecordSaved + ". User Code is:" + RecordNo, 125, 300);
                }
                else
                {
                    MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, objcommonmessage.RecordSaved, 125, 300);
                }
                
                #region Clear All records after save
                ClearAll();
                #endregion
            }
            else
            {
                MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, objcommonmessage.RecordNotSaved, 125, 300);
                return;
            }            
            ErrorStatus = "";
            RecordNo = "";
        }
       catch (Exception ex) {
           MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, objcommonmessage.RecordNotSaved, 125, 300);
        }
    }

    #endregion

    #region********************************************Methods ************************************************

    protected void BindSearchList()
    {
        try
        {
            DropDownList ddlSearch = (DropDownList)Master.FindControl("ddlSearch");
            ddlSearch.Items.Add(new ListItem("User Code", "UserCode"));
            ddlSearch.Items.Add(new ListItem("User Name", "UserName"));
            ddlSearch.Items.Add(new ListItem("Login ID", "LoginID"));            
        }
        catch (Exception ex) { }
    }

    protected void ClearAll()
    {
        HidAutoId.Value = "";
        ViewState["LoginId"] = null;
        txtEmployeeCode.Text = AutogenerateNo();
        txtEmployeeName.Text = "";
        ddlEmployeeLocation.SelectedIndex = 0;
        txtEmailID.Text = "";
        DeselectUserGroup();
        txtLoginId.Text = "";
        txtEmployeeName.Focus();
        //Added by Lalit 9July 2013
        DdlUserType.Items.Clear();
        BindUserType();
        ChkStatus.Checked = true;
        //End
    }

    protected void BindEmployeeLocation()
    {
        try
        {
            DataTable dt = new DataTable();
            string Query = "select LocationID,(LocationCode+' - '+LocationName) as CodeName from Com_Location_Mst where ActiveStatus =1";
            dt = com.executeSqlQry(Query);

            ddlEmployeeLocation.DataSource = dt;
            ddlEmployeeLocation.DataTextField = "CodeName";
            ddlEmployeeLocation.DataValueField = "LocationID";
            ddlEmployeeLocation.DataBind();
        }
        catch (Exception ex) { }
    }

    protected void BindUserGroup()
    {
        try
        {
            DataTable dt = new DataTable();
            string Query = "select GroupID,Group_Name from Com_Group_Mst where Active =1";
            dt = com.executeSqlQry(Query);

            ViewState["TotalGroup"] = dt.Rows.Count;
            listBoxUserGroup.DataSource = dt;
            listBoxUserGroup.DataTextField = "Group_Name";
            listBoxUserGroup.DataValueField = "GroupID";
            listBoxUserGroup.DataBind();
        }
        catch (Exception ex) { }
    }

    protected string AutogenerateNo()
    {
        int inv_series;
        string inv_no = "";
        try
        {
            inv_series = getSeries();
            inv_no = "EMP" + inv_series.ToString().PadLeft(4, '0');
        }
        catch (Exception ex)
        {

        }
        return inv_no;
    }

    public int getSeries()
    {
        int piseries = 1;
        try
        {

            string sql = "select MAX(UserID) from Com_User_Mst";
            DataTable dt = com.executeSqlQry(sql);
            if (dt.Rows.Count > 0)
            {
                piseries = int.Parse(dt.Rows[0][0].ToString()) + 1;
            }
            else
            {
                piseries = 1;
            }
        }
        catch (Exception ex)
        {

        }

        return piseries;
    }

    public void Get_AllUserList(string SearchType, string SearchText)
    {
        try
        {            
            DataTable dt = new DataTable();

            //Modified by Lalit 9 July 2013. now deactive users will also come in search.
            string Query = @"select U.UserID,UserCode,UserName,EmailID,LoginID
                           ,(select LocationCode +' - '+LocationName from Com_Location_Mst where LocationID =U.LocationId) as LocationCodeName
                            from Com_User_Mst as U inner join Com_Login_Mst as L on U.UserID =L.UserID where (UserCode like '%"+SearchText+"%' or";
                   Query +=" UserName like '%"+SearchText+"%' or LoginID like '%"+SearchText+"%')";

            dt = com.executeSqlQry(Query);

            if (dt.Rows.Count > 0)
            {
                gvSearchList.DataSource = dt;
                gvSearchList.AllowPaging = true;
                gvSearchList.DataBind();
                lblTotalRecords.Text = objcommonmessage.TotalRecord + dt.Rows.Count.ToString();
            }
            else
            {
                lblTotalRecords.Text = objcommonmessage.NoRecordFound;
                gvSearchList.AllowPaging = false;
                gvSearchList.DataSource = "";
                gvSearchList.DataBind();
            }            
        }
        catch { }        
    }

    private void BindHeaderRecords(string UserId)
    {
        try
        {
            DataTable dt = new DataTable();
            string Query = @" select UserCode,UserName,EmailID,LoginID,LocationID,UserTypeId,ActiveStatus from Com_User_Mst as U inner join Com_Login_Mst as L on U.UserID
                             =L.UserID  where U.UserID ='"+UserId+"'";

            dt = com.executeSqlQry(Query);
            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["UserCode"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["UserName"].ToString();
                try
                {
                    ddlEmployeeLocation.SelectedValue = dt.Rows[0]["LocationID"].ToString();
                }
                catch { }
                txtEmailID.Text = dt.Rows[0]["EmailID"].ToString();
                txtLoginId.Text = dt.Rows[0]["LoginID"].ToString();


                //Added by Lalit 9 July 2013
                com.SetDropDownValues(DdlUserType, dt.Rows[0]["UserTypeId"].ToString());
                ChkStatus.Checked = Convert.ToBoolean(dt.Rows[0]["ActiveStatus"].ToString());
                //End
                BindUserGroup();

                DataTable dtgroup = new DataTable();
                dtgroup = GetGroupIdOfUser(UserId);

                if (dtgroup.Rows.Count > 0)
                {
                    for (int i = 0; i < Convert.ToInt32(ViewState["TotalGroup"].ToString()); i++)
                    {
                        for (int j = 0; j < dtgroup.Rows.Count; j++)
                        {
                            if (listBoxUserGroup.Items[i].Value == dtgroup.Rows[j]["GroupID"].ToString())
                            {
                                listBoxUserGroup.Items[i].Selected = true;
                            }
                        }
                    }
                }
                else
                {
                    DeselectUserGroup();
                }
                ViewState["LoginId"] = dt.Rows[0]["LoginID"].ToString();
                dt = null;
            }
        }
        catch (Exception ex) { }
    }

    private void DeselectUserGroup()
    {
        try
        {
            for (int i = 0; i < Convert.ToInt32(ViewState["TotalGroup"].ToString()); i++)
            {
                listBoxUserGroup.Items[i].Selected = false;
            }
        }
        catch { }
    }

    private DataTable GetGroupIdOfUser(string UserId)
    {
        DataTable dt = new DataTable();
        try
        {
            //Changed By Lalit 9 July 2013. Disabled user can be activated and also Group can be changed.
            string Query = "select GroupID from Com_UserGroupMapping_Mst WHERE UserID ='" + UserId + "'";
            dt = com.executeSqlQry(Query);
            ViewState["Group"] = dt;
        }
        catch { }
        return dt;
    }

    protected void BindUserType()
    { 
        try
        {
            DataTable dt = new DataTable();
            string Query = "select UserTypeId,Type from Com_UserType_Mst where ActiveStatus =1";
            dt = com.executeSqlQry(Query);
            if (dt != null && dt.Rows.Count > 0)
            {
                DdlUserType.DataSource = dt;
                DdlUserType.DataTextField = "Type";
                DdlUserType.DataValueField = "UserTypeId";
                DdlUserType.DataBind();              
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Modified by satish on 10-July-13
                    if (dt.Rows[i]["Type"].ToString () == "Normal")
                    {
                        DdlUserType.SelectedValue = dt.Rows[i]["UserTypeId"].ToString();
                        break;
                    }
                }

            }
        }
        catch { }
    }
    
    #endregion
}