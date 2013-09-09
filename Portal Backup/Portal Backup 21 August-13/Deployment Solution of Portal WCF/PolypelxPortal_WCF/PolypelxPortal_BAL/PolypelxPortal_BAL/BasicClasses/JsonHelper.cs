using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Script.Services;

namespace PolypelxPortal_BAL.BasicClasses
{
    public class JsonHelper
    {
        #region *****************************Functions**************************************

        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        /// <summary>
        /// Function for Deserializing a strem and return an object of class.
        /// Created By: Lalit
        /// Created Date: 19july 2013
        /// </summary>
        public static T DeserializeObj<T>(Stream stream) where T : new()
        {
            T Obj = new T();
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                StreamReader objStreamReader = new StreamReader(stream);
                string JsonStringForDeSerialized = objStreamReader.ReadToEnd();
                objStreamReader.Dispose();
                Obj = JsonHelper.JsonDeserialize<T>(JsonStringForDeSerialized);
            }
            catch (Exception ex)
            { Log.LogMessage(ex.ToString()); }
            return Obj;
        }

        /// <summary>
        /// Function for Deserializing a strem and return an object of class.
        /// Created By: Satish
        /// Created Date: 30 July 2013
        /// </summary>
        public static T DeserializeJsonStream<T>(Stream stream) where T : new()
        {
            T Obj = new T();
            try
            {
                StreamReader objStreamReader = new StreamReader(stream);
                string JsonStringForDeSerialized = objStreamReader.ReadToEnd();
                objStreamReader.Dispose();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Obj = serializer.Deserialize<T>(JsonStringForDeSerialized);                
            }
            catch (Exception ex)
            { Log.LogMessage(ex.ToString()); }
            return Obj;
        }

        #endregion
    }
}
