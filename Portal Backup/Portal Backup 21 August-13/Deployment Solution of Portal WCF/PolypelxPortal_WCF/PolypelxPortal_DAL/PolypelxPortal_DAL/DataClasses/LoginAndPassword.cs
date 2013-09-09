using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Specialized;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using PolypelxPortal_BAL.BasicClasses;
using PolypelxPortal_BAL.PortalClasses;

namespace PolypelxPortal_DAL.DataClasses
{
    public class LoginAndPassword
    {
        #region *****************************Variables**************************************

        SqlCommand cmd;
        Common com = new Common();
        PolypelxPortal_DAL.DataClasses.DataCommon objdatacommon = new PolypelxPortal_DAL.DataClasses.DataCommon();

        #endregion

        #region *****************************Functions**************************************

        /// <summary>
        /// Created By: Satish
        /// Created Date: 17 July 2013
        /// Function for UserLogin which will get creadentials from login page and send user details for login.
        /// </summary>
        public string UserLogin(Stream Parameterdetails)
        {
            LoginDetails objlogindetails = new LoginDetails();
            StreamReader objStreamReader = new StreamReader(Parameterdetails);
            string JsonStringForDeSerialized = objStreamReader.ReadToEnd();
            objStreamReader.Dispose();

            objlogindetails = JsonHelper.JsonDeserialize<LoginDetails>(JsonStringForDeSerialized);

            string LoginId = objlogindetails.LoginId;
            string Password = objlogindetails.Password;

            DataTable dt = new DataTable();
            cmd = new SqlCommand();
            cmd.CommandText = @"SELECT U.UserID,U.UserCode,U.UserName,U.EmailID,U.LocationID,Loc.LocationName,Utype.UserTypeId,Utype.Type,
                                (case when U.ActiveStatus =1 then '1' when U.ActiveStatus =0 then '2' end) as ActiveStatus
                                FROM Com_Login_Mst AS L INNER JOIN Com_User_Mst AS U ON L.UserID = U.UserID Inner join
                                Com_UserType_Mst Utype On U.UserTypeId=Utype.UserTypeId inner join Com_Location_Mst as Loc on
                                U.LocationID =Loc.LocationID WHERE L.LoginID ='" + LoginId + "' AND L.Password ='" + Password + "' AND";
            cmd.CommandText += " Utype.Type in ('Customer','C_Owner','AR_Owner')";

            dt = objdatacommon.GetDataTableWithQuery(cmd);
            UserDetails objUserDetails = new UserDetails();
            if (dt.Rows.Count > 0)
            {
                objUserDetails.UserId = dt.Rows[0]["UserId"].ToString();
                objUserDetails.UserCode = dt.Rows[0]["UserCode"].ToString();
                objUserDetails.UserName = dt.Rows[0]["UserName"].ToString();
                objUserDetails.EmailId = dt.Rows[0]["EmailID"].ToString();
                objUserDetails.LocationId = dt.Rows[0]["LocationID"].ToString();
                objUserDetails.LocationName = dt.Rows[0]["LocationName"].ToString();
                objUserDetails.UserTypeId = dt.Rows[0]["UserTypeId"].ToString();
                objUserDetails.UserType = dt.Rows[0]["Type"].ToString();
                objUserDetails.ActiveStatus = dt.Rows[0]["ActiveStatus"].ToString();
            }
            else
            {
                objUserDetails.ActiveStatus = "0";
            }
            string JsonStringForSerialized = JsonHelper.JsonSerializer<UserDetails>(objUserDetails);
            return JsonStringForSerialized;
        }

        /// <summary>
        /// Created By: Satish
        /// Created Date: 17 July 2013
        /// Function for ForgotPassword which will get creadentials from forget password page and send user current status.
        /// </summary>
        public string ForgotPassword(Stream Parameterdetails)
        {
            ForgotPassword objForgotPassword = new ForgotPassword();

            StreamReader objStreamReader = new StreamReader(Parameterdetails);
            string JsonStringForDeSerialized = objStreamReader.ReadToEnd();
            objStreamReader.Dispose();

            objForgotPassword = JsonHelper.JsonDeserialize<ForgotPassword>(JsonStringForDeSerialized);

            string LoginId = objForgotPassword.LoginId;
            string EmailId = objForgotPassword.EmailId;                        

            DataTable dt = new DataTable();
            cmd = new SqlCommand();
            cmd.CommandText = @"SELECT L.LoginID,U.UserName,L.Password,(case when U.ActiveStatus =1 then '1' when U.ActiveStatus =0 then '2' end)
                                as ActiveStatus,U.EmailID FROM Com_Login_Mst AS L INNER JOIN Com_User_Mst AS U ON L.UserID = U.UserID Inner join
                                Com_UserType_Mst Utype On U.UserTypeId=Utype.UserTypeId inner join Com_Location_Mst as Loc on
                                U.LocationID =Loc.LocationID WHERE L.LoginID ='" + LoginId + "' AND U.EmailID ='" + EmailId + "' AND";
            cmd.CommandText += " Utype.Type in ('Customer','C_Owner','AR_Owner')";

            dt = objdatacommon.GetDataTableWithQuery(cmd);
            objForgotPassword = new ForgotPassword();
            if (dt.Rows.Count > 0)
            {
                objForgotPassword.LoginId = dt.Rows[0]["LoginID"].ToString();
                objForgotPassword.UserName = dt.Rows[0]["UserName"].ToString();
                objForgotPassword.Password = dt.Rows[0]["Password"].ToString();
                objForgotPassword.EmailId = dt.Rows[0]["EmailID"].ToString();
                objForgotPassword.ActiveStatus = dt.Rows[0]["ActiveStatus"].ToString();
            }
            else
            {
                objForgotPassword.ActiveStatus = "0";
            }
            string JsonStringForSerialized = JsonHelper.JsonSerializer<ForgotPassword>(objForgotPassword);
            return JsonStringForSerialized;
        }

        /// <summary>
        /// Created By: Satish
        /// Created Date: 18 July 2013
        /// Function for change password which will get credentials from change password page and send user current status.
        /// </summary>
        public string ChangePassword(Stream Parameterdetails)
        {
            ChangePassword objChangePassword = new ChangePassword();

            StreamReader objStreamReader = new StreamReader(Parameterdetails);
            string JsonStringForDeSerialized = objStreamReader.ReadToEnd();
            objStreamReader.Dispose();

            objChangePassword = JsonHelper.JsonDeserialize<ChangePassword>(JsonStringForDeSerialized);

            int UserId = objChangePassword.UserId;
            string OldPassowrd = objChangePassword.OldPassowrd;
            string NewPassword = objChangePassword.NewPassword;

            DataTable dt = new DataTable();
            cmd = new SqlCommand();
            cmd.CommandText = @"SELECT COUNT(*) as TotalCount FROM Com_Login_Mst WHERE UserID ='" + UserId + "' AND Password ='" + OldPassowrd + "'";

            dt = objdatacommon.GetDataTableWithQuery(cmd);
            objChangePassword = new ChangePassword();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["TotalCount"].ToString() == "1" && UserId > 0)
                {
                    cmd = new SqlCommand();
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = NewPassword;
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.Add(new SqlParameter("@ErrorStatus", SqlDbType.VarChar, 10));
                    cmd.Parameters["@ErrorStatus"].Direction = ParameterDirection.Output;

                    cmd.CommandText = "SP_Update_In_Com_Login_Mst_ForPassword";
                    cmd = objdatacommon.ExecuteSqlProcedure(cmd);

                    string ErrorStatus = cmd.Parameters["@ErrorStatus"].Value.ToString();
                    if (ErrorStatus == "0")
                    {
                        objChangePassword.ActiveStatus = "1";
                        objChangePassword.SaveMessage = "Password has been updated successfully.";
                        objChangePassword.Type = "status";
                    }
                    else
                    {
                        objChangePassword.ActiveStatus = "0";
                        objChangePassword.SaveMessage = "Password has not been updated.";
                        objChangePassword.Type = "warning";
                    }
                }
                else
                {
                    objChangePassword.ActiveStatus = "0";
                    objChangePassword.SaveMessage = "Old password is wrong.";
                    objChangePassword.Type = "error";
                }
            }

            string JsonStringForSerialized = JsonHelper.JsonSerializer<ChangePassword>(objChangePassword);
            return JsonStringForSerialized;
        }

        #endregion
    }
}
