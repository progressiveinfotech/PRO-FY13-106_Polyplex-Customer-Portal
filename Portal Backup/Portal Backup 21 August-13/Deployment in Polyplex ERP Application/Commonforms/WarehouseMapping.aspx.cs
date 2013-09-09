/* 
 Developer Name: Satish Pal
 Date Creation: 18-July-13
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;

public partial class Sales_MasterFinalDestination : System.Web.UI.Page
{
    #region***************************************Variables***************************************

    Common_Message objcommonmessage = new Common_Message();
    MasterWithGrid cs = new MasterWithGrid();
    Common com = new Common();
    string ErrorStatus;
    Connection objConnectionClass = new Connection();

    #endregion

    #region***************************************Events***************************************

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Label lbl = (Label)Master.FindControl("lbl_PageHeader");
                lbl.Text = "Warehouse Mapping";

                if (Session["UserID"] == null)
                {
                    Server.Transfer("../SessionExpired.aspx");
                }
                else
                {                    
                    makeGrid();
                    BindPlant();
                    txtCustomerCode.Focus();
                }

                #region Change Color and Readonly Fields

                txtWarehouseName.Attributes.Add("readonly", "true");
                txtWarehouseName.Attributes.Add("style", "background:lightgray");
                txtCustomerName.Attributes.Add("readonly", "true");
                txtCustomerName.Attributes.Add("style", "background:lightgray");

                #endregion
            }
        }
        catch { }

    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        /// <summary>
        /// This event is used for search the record in saved mapping in grid.
        /// </summary>
        makeGrid();
    }

    protected void gvMapping_SelectedIndexChanged(object sender, EventArgs e)
    {
        /// <summary>
        /// This event is used for select the existing record from all saved mapping in grid.
        /// </summary> 
        try
        {
            foreach (GridViewRow oldrow in gvMapping.Rows)
            {
                ImageButton imgbutton = (ImageButton)oldrow.FindControl("ImageButton1");
                imgbutton.ImageUrl = "~/Images/chkbxuncheck.png";
                oldrow.BackColor = Color.White;
            }
            ImageButton img = (ImageButton)gvMapping.Rows[gvMapping.SelectedIndex].FindControl("ImageButton1");
            img.ImageUrl = "~/Images/chkbxcheck.png";

            int DataKey = int.Parse(gvMapping.SelectedDataKey.Value.ToString());
            HidAutoId.Value = DataKey.ToString();
            BindHeaderRecords(HidAutoId.Value);
        }
        catch { }
    }

    protected void gvMapping_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        /// <summary>
        /// This event is used for indexing in mapping grid.
        /// </summary> 
        try
        {
            gvMapping.PageIndex = e.NewPageIndex;
            makeGrid();
        }
        catch { }
    }

    protected void gvMapping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /// <summary>
        /// This event is used to hide the id column in grid.
        /// </summary>
        try
        {
            if (e.Row.RowType != DataControlRowType.EmptyDataRow)
            {
                e.Row.Cells[7].Style.Add("display", "none");
                e.Row.Cells[8].Style.Add("display", "none");
                e.Row.Cells[9].Style.Add("display", "none");
            }
        }
        catch { }
    }

    protected void imgBtnWarehouseCode_Click(object sender, ImageClickEventArgs e)
    {
        /// <summary>
        /// This event is used to populate all the users in popup.
        /// </summary> 
        try
        {
            txtSearchFromPopup.Text = "";
            HidPopUpType.Value = "Warehouse";
            lPopUpHeader.Text = "Warehouse Master";
            lSearch.Text = "Search By Warehouse Code/Name: ";
            FillAllWarehouse("");
            ModalPopupExtender2.Show();
        }
        catch { }
    }

    protected void txtWarehouseCode_TextChanged(object sender, EventArgs e)
    {
        /// <summary>
        /// This event is used to validate entered WarehouseCode and populate user popup while entered WarehouseCode is not exist.
        /// </summary>
        try
        {
            string query = @"select A.autoid,StorageLocCode,Location,B.PlantCode,B.PlantName  from Prod_StorageLocation_Mst as A inner
                             join Com_Plant_Mst as B on A.PlantId =B.autoid where Status =1 and PlantId ='"+ddlPlant.SelectedValue +"'";
                   query +=" and StorageLocCode='" + txtWarehouseCode.Text.Trim() + "'";

            DataTable dt = new DataTable();
            dt = com.executeSqlQry(query);

            HidWarehouseId.Value = "";
            txtWarehouseCode.Text = "";
            txtWarehouseName.Text = "";

            if (dt.Rows.Count > 0)
            {
                HidWarehouseId.Value = dt.Rows[0]["autoid"].ToString();
                txtWarehouseCode.Text = dt.Rows[0]["StorageLocCode"].ToString();
                txtWarehouseName.Text = dt.Rows[0]["Location"].ToString();
            }
            else
            {
                try
                {
                    txtSearchFromPopup.Text = "";
                    HidPopUpType.Value = "Warehouse";
                    lPopUpHeader.Text = "Warehouse Master";
                    lSearch.Text = "Search By Warehouse Code/Name: ";
                    FillAllWarehouse("");
                    ModalPopupExtender2.Show();
                }
                catch { }
            }
        }
        catch { }
    }

    protected void imgBtnCustomerCode_Click(object sender, ImageClickEventArgs e)
    {
        /// <summary>
        /// This event is used to populate all the customers in popup.
        /// </summary> 
        try
        {
            txtSearchFromPopup.Text = "";
            HidPopUpType.Value = "Customer";
            lPopUpHeader.Text = "Customer Master";
            lSearch.Text = "Search By Customer Code/Name: ";
            FillAllCustomer("");
            ModalPopupExtender2.Show();
        }
        catch { }
    }

    protected void txtCustomerCode_TextChanged(object sender, EventArgs e)
    {
        /// <summary>
        /// This event is used to validate entered customercode and populate user popup while entered customercode is not exist.
        /// </summary>
        try
        {
            string query = @"select CustomerID,CustomerCode,Name from Sal_Glb_Customer_Mst where ActiveStatus =1 and
                             CustomerCode= '" + txtCustomerCode.Text.Trim() + "'";

            DataTable dt = new DataTable();
            dt = com.executeSqlQry(query);

            HidCustomerId.Value = "";
            txtCustomerCode.Text = "";
            txtCustomerName.Text = "";

            if (dt.Rows.Count > 0)
            {
                HidCustomerId.Value = dt.Rows[0]["CustomerID"].ToString();
                txtCustomerCode.Text = dt.Rows[0]["CustomerCode"].ToString();
                txtCustomerName.Text = dt.Rows[0]["Name"].ToString();
            }
            else
            {
                try
                {
                    txtSearchFromPopup.Text = "";
                    HidPopUpType.Value = "Customer";
                    lPopUpHeader.Text = "Customer Master";
                    lSearch.Text = "Search By Customer Code/Name: ";
                    FillAllCustomer("");
                    ModalPopupExtender2.Show();
                }
                catch { }
            }
        }
        catch { }
    }

    protected void gvPopUpGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        /// <summary>
        /// This event is used to select the selected record and fill its respective control(s) in all the popups.
        /// </summary>
        try
        {
            GridView gvPopUpGrid = (GridView)sender;
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            gvPopUpGrid.SelectedIndex = row.RowIndex;

            if (e.CommandName == "select")
            {
                foreach (GridViewRow oldrow in gvPopUpGrid.Rows)
                {
                    ImageButton imgbutton = (ImageButton)oldrow.FindControl("ImageButton1");
                    imgbutton.ImageUrl = "~/Images/chkbxuncheck.png";
                }
                ImageButton img = (ImageButton)row.FindControl("ImageButton1");
                img.ImageUrl = "~/Images/chkbxcheck.png";

                if (HidPopUpType.Value == "Customer")
                {
                    HidCustomerId.Value = gvPopUpGrid.Rows[gvPopUpGrid.SelectedIndex].Cells[1].Text;
                    txtCustomerCode.Text = gvPopUpGrid.Rows[gvPopUpGrid.SelectedIndex].Cells[2].Text;
                    txtCustomerName.Text = gvPopUpGrid.Rows[gvPopUpGrid.SelectedIndex].Cells[3].Text;
                }
                else if (HidPopUpType.Value == "Warehouse")
                {
                    HidWarehouseId.Value = gvPopUpGrid.Rows[gvPopUpGrid.SelectedIndex].Cells[1].Text;
                    txtWarehouseCode.Text = gvPopUpGrid.Rows[gvPopUpGrid.SelectedIndex].Cells[2].Text;
                    txtWarehouseName.Text = gvPopUpGrid.Rows[gvPopUpGrid.SelectedIndex].Cells[3].Text;
                }
            }
        }
        catch { }
    }

    protected void gvPopUpGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /// <summary>
        /// This event is used to hide the id field in popup grid for all the popups.
        /// </summary>
        try
        {
            if (e.Row.RowType != DataControlRowType.EmptyDataRow)
            {
                e.Row.Cells[1].Style.Add("display", "none");
            }
        }
        catch { }
    }

    protected void btnSearchInPopUp_Click(object sender, EventArgs e)
    {
        /// <summary>
        /// This event is used to search the entered record in popup grid for all the popups.
        /// </summary>
        try
        {
            if (HidPopUpType.Value == "Customer")
            {
                FillAllCustomer(txtSearchFromPopup.Text.Trim());
            }
            else if (HidPopUpType.Value == "Warehouse")
            {
                FillAllWarehouse(txtSearchFromPopup.Text.Trim());
            }
            
            txtSearchFromPopup.Focus();
            ModalPopupExtender2.Show();
        }
        catch { }
    }

    protected void gvPopUpGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        /// <summary>
        /// This event is used for indexing in popup grid for all the popups.
        /// </summary>
        try
        {
            gvPopUpGrid.PageIndex = e.NewPageIndex;
            if (HidPopUpType.Value == "Customer")
            {
                FillAllCustomer(txtSearchFromPopup.Text.Trim());
            }
            if (HidPopUpType.Value == "Warehouse")
            {
                FillAllWarehouse(txtSearchFromPopup.Text.Trim());
            }
            
            txtSearchFromPopup.Focus();
            ModalPopupExtender2.Show();
        }
        catch { }
    }

    protected void ImgBtnSave_Click(object sender, ImageClickEventArgs e)
    {
        /// <summary>
        /// This event is used to save all records in database.
        /// </summary>
        try
        {
            #region Check if mapping exist in table***************************************

            if (HidAutoId.Value == "0")
            {
                DataTable dt = new DataTable();
                string query = @"select AutoId from tblWarehouseMapping where WareHouseId ='" + HidWarehouseId.Value + "' and CustomerId ='" + HidCustomerId.Value + "'";
                dt = com.executeSqlQry(query);
                if (dt.Rows.Count > 0)
                {
                    MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info,"Mapping is already exist. Please check it.", 125, 300);
                    return;
                }
                dt = null;
            }

            #endregion********************************************************************

            objConnectionClass.OpenConnection();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Connection = objConnectionClass.PolypexSqlConnection;
            cmd.CommandTimeout = 60;
            cmd.CommandType = CommandType.StoredProcedure;

            #region All Parameters

            if (HidAutoId.Value == "0")
            {
                cmd.Parameters.Add("@AutoId", SqlDbType.Int).Value = 0;
            }
            else
            {
                cmd.Parameters.Add("@AutoId", SqlDbType.Int).Value = com.STRToInt(HidAutoId.Value);
            }
            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = com.STRToInt(HidCustomerId.Value);
            cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = com.STRToInt(ddlPlant.SelectedValue);
            cmd.Parameters.Add("@WareHouseId", SqlDbType.Int).Value = com.STRToInt(HidWarehouseId.Value);           

            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Bit).Value = chkActive.Checked;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = com.STRToInt(Session["UserId"].ToString());
            cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = com.STRToInt(Session["UserId"].ToString());

            cmd.Parameters.Add(new SqlParameter("@ErrorStatus", SqlDbType.VarChar, 10));
            cmd.Parameters["@ErrorStatus"].Direction = ParameterDirection.Output;

            cmd.CommandText = "SP_InsertUpdate_In_tblWarehouseMapping";
            cmd.ExecuteNonQuery();

            ErrorStatus = cmd.Parameters["@ErrorStatus"].Value.ToString();

            if (ErrorStatus == "0")
            {
                MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, objcommonmessage.RecordSaved, 125, 300);

                #region Clear All records after save

                ClearFields();
                makeGrid();

                #endregion
            }
            else
            {
                MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, objcommonmessage.RecordNotSaved, 125, 300);
            }
            ErrorStatus = "";

            #endregion
        }
        catch
        {
            MyMessageBoxInfo.Show(MyMessageBox.MessageType.Info, objcommonmessage.RecordNotSaved, 125, 300);
        }
        finally
        {
            objConnectionClass.CloseConnection();
        }
    }

    #endregion

    #region***************************************Functions***************************************    

    protected void makeGrid()
    {
        /// <summary>
        /// This method is used to get all the detail records in from the database in grid.
        /// </summary>
        try
        {
            string query = @"SELECT A.[AutoId]
                            ,A.[CustomerId]
                            ,B.CustomerCode
                            ,B.Name
                            ,A.[PlantId]
                            ,C.PlantName
                            ,[WareHouseId]
                            ,D.StorageLocCode
                            ,D.Location
                            ,A.[ActiveStatus]
                        FROM [tblWarehouseMapping] as A inner join Sal_Glb_Customer_Mst as B on B.CustomerID =A.CustomerId inner join
                        Com_Plant_Mst as C on A.[PlantId] = C.autoid inner join Prod_StorageLocation_Mst as D on A.[WareHouseId] = D.autoid 
                        where (B.CustomerCode like '%" + txtSearch.Text.Trim() + "%' or B.Name like '%" + txtSearch.Text.Trim() + "%' or";
            query += " C.PlantName like '%" + txtSearch.Text.Trim() + "%' or D.StorageLocCode like '%" + txtSearch.Text.Trim() + "%' or";
            query += " D.Location like '%" + txtSearch.Text.Trim() + "%')  order by A.AutoId desc";

            DataTable dt = cs.getGrid(query);
            gvMapping.DataSource = dt;
            gvMapping.DataBind();
            lblTotalRecords.Text = objcommonmessage.TotalRecord + dt.Rows.Count.ToString();
        }
        catch { }
    }

    private void ClearFields()
    {
        /// <summary>
        /// This method is used to clear all the header records.
        /// </summary>
        try
        {
            HidAutoId.Value = "0";            
            HidWarehouseId.Value = "";
            HidCustomerId.Value = "";
            txtWarehouseCode.Text = "";
            txtWarehouseName.Text = "";
            txtCustomerCode.Text = "";
            txtCustomerName.Text = "";
            ddlPlant.SelectedIndex = 0;
            chkActive.Checked = true;
            txtCustomerCode.Focus();
            txtSearch.Text = "";
        }
        catch { }
    }

    private void BindHeaderRecords(string AutoId)
    {
        /// <summary>
        /// This method is used to bind all the controls depend upon the selected record in mapping list grid.
        /// </summary>
        try
        {
            DataTable dt = new DataTable();
            string query = @"SELECT A.[AutoId]
                              ,A.[CustomerId]
                              ,B.CustomerCode
                              ,B.Name
                              ,A.[PlantId]
                              ,C.PlantName
                              ,[WareHouseId]
                              ,D.StorageLocCode
                              ,D.Location
                              ,A.[ActiveStatus]
                          FROM [tblWarehouseMapping] as A inner join Sal_Glb_Customer_Mst as B on B.CustomerID =A.CustomerId inner join Com_Plant_Mst as C
                          on A.[PlantId] = C.autoid inner join Prod_StorageLocation_Mst as D on A.[WareHouseId] = D.autoid 
                           where A.AutoId = '" + AutoId + "'";

            dt = com.executeSqlQry(query);
            if (dt.Rows.Count > 0)
            {                
                HidCustomerId.Value = dt.Rows[0]["CustomerId"].ToString();
                txtCustomerCode.Text = dt.Rows[0]["CustomerCode"].ToString();
                txtCustomerName.Text = dt.Rows[0]["Name"].ToString();
                ddlPlant.SelectedValue = dt.Rows[0]["PlantId"].ToString();
                HidWarehouseId.Value = dt.Rows[0]["WarehouseId"].ToString();
                txtWarehouseCode.Text = dt.Rows[0]["StorageLocCode"].ToString();
                txtWarehouseName.Text = dt.Rows[0]["Location"].ToString();

                if (dt.Rows[0]["ActiveStatus"].ToString() == "True")
                {
                    chkActive.Checked = true;
                }
                else
                {
                    chkActive.Checked = false;
                }
            }
            dt = null;
        }
        catch { }
    }

    protected void FillAllWarehouse(string Searchtext)
    {
        /// <summary>
        /// This method is used to get all warehouse according to the plantid and fill in popup grid.
        /// </summary>
        try
        {
            DataTable dt = new DataTable();

            string sql = @"select A.autoid,StorageLocCode,Location,B.PlantCode,B.PlantName  from Prod_StorageLocation_Mst as A inner join";
                   sql+=" Com_Plant_Mst as B on A.PlantId =B.autoid where Status =1 and PlantId ='"+ddlPlant.SelectedValue +"' and";
                   sql+=" (StorageLocCode like '%" + Searchtext + "%' or Location like '%" + Searchtext + "%' or B.PlantCode like '%" + Searchtext + "%'";
                   sql += " or B.PlantName like '%" + Searchtext + "%')";

            dt = com.executeSqlQry(sql);

            if (dt.Rows.Count > 0)
            {
                lblTotalRecordsPopUp.Text = objcommonmessage.TotalRecord + dt.Rows.Count.ToString();

                gvPopUpGrid.AutoGenerateColumns = true;
                gvPopUpGrid.AllowPaging = true;
                gvPopUpGrid.DataSource = dt;

                if (gvPopUpGrid.PageIndex > (dt.Rows.Count / gvPopUpGrid.PageSize))
                {
                    gvPopUpGrid.SetPageIndex(0);
                }
                gvPopUpGrid.DataBind();
            }
            else
            {
                lblTotalRecordsPopUp.Text = objcommonmessage.NoRecordFound;
                gvPopUpGrid.AllowPaging = false;
                gvPopUpGrid.DataSource = "";
                gvPopUpGrid.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblTotalRecordsPopUp.Text = objcommonmessage.NoRecordFound + ex.Message;
            gvPopUpGrid.AllowPaging = false;
            gvPopUpGrid.DataSource = "";
            gvPopUpGrid.DataBind();
        }
    }

    protected void FillAllCustomer(string Searchtext)
    {
        /// <summary>
        /// This method is used to get all customers and fill in popup grid.
        /// </summary>
        try
        {
            DataTable dt = new DataTable();

            string sql = @"select CustomerID,CustomerCode,Name from Sal_Glb_Customer_Mst where ActiveStatus =1 and (CustomerCode
                           like '%" + Searchtext + "%' or Name like '%" + Searchtext + "%')";

            dt = com.executeSqlQry(sql);

            if (dt.Rows.Count > 0)
            {
                lblTotalRecordsPopUp.Text = objcommonmessage.TotalRecord + dt.Rows.Count.ToString();

                gvPopUpGrid.AutoGenerateColumns = true;
                gvPopUpGrid.AllowPaging = true;
                gvPopUpGrid.DataSource = dt;

                if (gvPopUpGrid.PageIndex > (dt.Rows.Count / gvPopUpGrid.PageSize))
                {
                    gvPopUpGrid.SetPageIndex(0);
                }
                gvPopUpGrid.DataBind();
            }
            else
            {
                lblTotalRecordsPopUp.Text = objcommonmessage.NoRecordFound;
                gvPopUpGrid.AllowPaging = false;
                gvPopUpGrid.DataSource = "";
                gvPopUpGrid.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblTotalRecordsPopUp.Text = objcommonmessage.NoRecordFound + ex.Message;
            gvPopUpGrid.AllowPaging = false;
            gvPopUpGrid.DataSource = "";
            gvPopUpGrid.DataBind();
        }
    }

    protected void BindPlant()
    {
        /// <summary>
        /// This method is used to bind the plant.
        /// </summary>
        try
        {
            DataTable dt = new DataTable();
            string query = @"select autoid,(PlantCode +' - '+PlantName) as CodeName from Com_Plant_Mst where ActiveStatus =1";
            dt = com.executeSqlQry(query);
            if (dt.Rows.Count > 0)
            {
                ddlPlant.DataTextField = "CodeName";
                ddlPlant.DataValueField = "autoid";
                ddlPlant.DataSource = dt;
                if (dt.Rows.Count > 0)
                {
                    ddlPlant.DataBind();
                }
            }
        }
        catch { }
    }

    #endregion

}