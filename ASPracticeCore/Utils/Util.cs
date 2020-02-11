using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using ASPracticeCore.Models;
using Microsoft.AspNetCore.Http;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using ASPracticeCore.Controllers;

namespace ASPracticeCore.Utils
{
    public class Util
    {
        public class DbUtil
        {

            static readonly List<string> identityIdTables = new List<string>();
            
            /// <summary>
            /// Updates a SqlCommand's Query Params with values.
            /// </summary>
            /// <param name="cParams">The query SqlCommand.Params to be assigned values with</param>
            /// <param name="entity">The entity where the values comes from</param>
            public static void FillSqlCommandParams(ref SqlParameterCollection cParams, EntityBase entity)
            {
                //param format was generated, could just be passed.
                PropertyInfo[] props = entity.GetType().GetProperties();
                string tableName = GetClassName(entity);
                bool skipId = IsIdAutoIncrement(tableName);
                for (int i = 0; i < props.Length; i++)
                {
                    if (skipId && props[i].Name.ToLower() == "id")
                    {
                        //skip the ID since it's auto-increment by the DB
                        continue;
                    }
                    string key = "@" + props[i].Name.ToLower();//'@propName' is the param format I used


                    var propValue = props[i].GetValue(entity) ?? DBNull.Value;
                    cParams.AddWithValue(key, propValue);
                }
            }
            public static void FillSqlCommandParams(ref SqlParameterCollection cParams, dynamic entity)
            {
                PropertyInfo[] props = entity.GetType().GetProperties();
                string tableName = GetClassName(entity);
                bool skipId = IsIdAutoIncrement(tableName);
                for (int i = 0; i < props.Length; i++)
                {
                    if (skipId && props[i].Name.ToLower() == "id")
                    {
                        //skip the ID since it's auto-increment by the DB
                        continue;
                    }
                    string key = "@" + props[i].Name.ToLower();//'@propName' is the param format I used


                    var propValue = props[i].GetValue(entity) ?? DBNull.Value;
                    cParams.AddWithValue(key, propValue);
                }
            }
            public static DynamicParameters FillSqlDapperParams(EntityBase entity)
            {
                PropertyInfo[] props = entity.GetType().GetProperties();
                int length = props.Length;
                string tableName = GetClassName(entity);
                bool skipId = IsIdAutoIncrement(tableName);
                var parameters = new DynamicParameters();

                for (int i = 0; i < length; i++)
                {
                    if (skipId && props[i].Name.ToLower() == "id")
                    {
                        //skip the ID since it's auto-increment by the DB
                        continue;
                    }

                    string key = "@" + props[i].Name.ToLower();
                    Log(key, ":", props[i].GetValue(entity));
                    var propValue = props[i].GetValue(entity) ?? DBNull.Value;
                    parameters.Add(key, propValue);

                }
                return parameters;
            }
            public static string WritePropertyNamesOnQuery(PropertyInfo[] props, string tableName)
            {
                StringBuilder sb = new StringBuilder();
                int secondToLastIndex = props.Length - 2;
                int lastIndex = props.Length - 1;
                bool isSkipId = IsIdAutoIncrement(tableName);

                int factorIndex = isSkipId ? secondToLastIndex : lastIndex;

                sb.Append("(");
                for(int i=0; i<props.Length; i++){
                    
                    //skip if property is a custom reference type
                    if (!IsPrimitive(props[i].PropertyType))
                    {
                        //future: invoke Add<T>() again here to insert the custom type
                        continue;
                    }
                    //skip including the id on an id auto-increment table 
                    //prop name "id" is uniform since parent abstract class has it
                    if(props[i].Name.ToLower() == "id" && isSkipId == true)
                    {
                        continue;
                    }

                    sb.Append(props[i].Name.ToLower());

                    if (i == factorIndex)
                    {
                        //skip here already so there's no extra comma
                        continue;
                    }

                    sb.Append(",");
                }
                //end of specs, add closing parenthesis ")"
                sb.Append(")");
                return sb.ToString();//sample output - "(col1,col2,col3)"
            }
            public static string WriteQueryParams(PropertyInfo[] props, string tableName)
            {
                StringBuilder sb = new StringBuilder();
                int secondToLastIndex = props.Length - 2;
                int lastIndex = props.Length - 1;
                bool isSkipId = IsIdAutoIncrement(tableName);
                int factorIndex = isSkipId ? secondToLastIndex : lastIndex;
                string inputParam;
                sb.Append("(");
                //Prepare dynamic sql insert param acc to property count:
                for (int i = 0; i < props.Length; i++)
                {
                    //skip if property is a custom reference type
                    if (!IsPrimitive(props[i].PropertyType))
                    {
                        
                        continue;
                    }
                    if (isSkipId == true && props[i].Name.ToLower() == "id")
                    {
                        Log("skipping id...");
                        continue;
                    }

                    inputParam = "@"+props[i].Name.ToLower(); //format: @propName
                    sb.Append(inputParam);

                    if (i == factorIndex)
                    {
                        continue;
                    }

                    sb.Append(",");

                }
                sb.Append(")");
                return sb.ToString();//sample output - (@propName1, @propName2, @propName3)
            }

            public static dynamic ProduceInstance(Type t, SqlDataReader dr)
            {
                DataTable columnInfoTable = dr.GetSchemaTable();//table of schema's columns set (name, type, capacity)
                DataColumn column_key = columnInfoTable.Columns[0];//the key for the column name
                dynamic entity = Activator.CreateInstance(t);
                try
                {
                    Util.Log("Producing object of type", Util.GetClassName(t));
                    //fill-up the entity's properties with values from the SqlDataReader
                    foreach (DataRow row in columnInfoTable.Rows) //Rows are the columns itself of the table (name, phoneNo, email)
                    {
                        string columnName = row[column_key].ToString();//get column name of current row
                        var columnValue = dr[columnName];
                        Util.Log(columnName, ":", columnValue); 
                        //still following the premise of propName = columnName, assign columnVal to property
                        PropertyInfo prop = t.GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(entity, columnValue);
                    }
                }catch(Exception e)
                {
                    throw e; 
                }

                return entity;

            }


            
            public static bool IsIdAutoIncrement(string tableName)
            {
                return identityIdTables.Contains(tableName);
            }
            public static void InitializeIdentityTableList()
            {
                identityIdTables.Add("useraccount");
                identityIdTables.Add("statickv");
                identityIdTables.Add("session");
                identityIdTables.Add("sessiondata");
                
            }
            public static void InsertStaticData(string key, string value)
            {
                using (SqlConnection conn = GetConnection())
                {

                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO StaticKV VALUES(@key,@value);", conn);
                    command.Parameters.AddWithValue("@key", key);
                    command.Parameters.AddWithValue("@value", value);
                    if (command.ExecuteNonQuery() < 1)
                    {
                        Util.Log("Static data wasn't added!");
                        return;
                    }
                    Util.Log("Static Data", value, "was added!");
                }
            }
            public static IQueryable<StaticKV> GetStaticData()
            {
                List<StaticKV> staticData = new List<StaticKV>();
                using (SqlConnection conn = Util.DbUtil.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT*FROM StaticKV;";
                    SqlCommand command = new SqlCommand(query, conn);
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string key = dr.GetString(1);
                        string value = dr.GetString(2);

                        staticData.Add(new StaticKV() { Id = id, DataKey = key, DataValue = value });
                    }
                }
                return staticData.AsQueryable();
            }
            public static SqlConnection GetConnection()
            {
                string connString = @"Data Source=LAPTOP-DKG8Q1TH\SQLEXPRESS;Initial Catalog=ASPracticeCore; Integrated Security=True";
                return new SqlConnection(connString);
            }

        }


        public static bool IsPrimitive(Type type)
        {
            bool isPrimitive = type.IsPrimitive || type.Namespace == null || type.Namespace.Equals("System");
            Log(type + " isPrimitive?", isPrimitive);
            return isPrimitive;
        }


        public static void DisplayObjectProperties<T>(T obj) where T:class
        {
            Log("Expanding the properties:");
            Log("Type:", typeof(T));
            PropertyInfo[] props = typeof(T).GetProperties();
            Array.ForEach(props, p => {
                Log(p.Name, ":", p.GetValue(obj));
            });
        }

        public static string GetClassName(Object obj)
        {
            string[] names = obj.GetType().ToString().Split('.');
            return names[names.Length - 1].ToLower();
        }
        public static string GetClassName(Type type)
        {
            string[] names = type.ToString().Split('.');
            return names[names.Length - 1].ToLower();
        }
        public static string AttachStatusToMessage(string status, string message)
        {
            return status + "_" + message;
        }
        public static bool IsText(Object prop)
        {
            return prop.GetType() == typeof(string);
        }
        public static void Log(params object[] objs)
        {
            System.Diagnostics.Debug.WriteLine(string.Join(' ', objs));
        }

        public class ControllerUtil
        {
            public ControllerUtil()
            {
             
            }
            public static string[] GetFinalStatusMessages(string statusRef, params string[] messages)
            {
                //return success
                string[] finalStatusMessages = messages.Where(m => m != statusRef).ToArray();
                
                //If array is empty, everything's "success"-ful
                if (finalStatusMessages.Length == 0)
                {
                    finalStatusMessages = new string[1] {statusRef};
                }
                
                return finalStatusMessages;
            }

            public static bool IsAuthenticated()
            {

                var httpContextAccessor = new HttpContextAccessor();
                int activeUserId = httpContextAccessor.HttpContext.Session.Get<int>(Constants.KEY_USERID);
                bool isAuthenticated = (activeUserId != default) ? true : false;
                return isAuthenticated;
            }

           public static List<FilePath> GetFilesFromRequest(IFormFileCollection files, IConfiguration configuration)
            {
                List<FilePath> filePaths = new List<FilePath>();
                string savePath = configuration.GetValue<string>("FileSavePath");

                int i = 0;
                foreach (IFormFile file in files)
                {
                       
                    var filePath = new ImageFile() {
                        FileName = Guid.NewGuid().ToString() + "_" + file.FileName,
                        FileExtension = Path.GetExtension(savePath),
                        Path = savePath,
                        IsDisplayPic = false,
                        ImageSize = file.Length
                        
                    };
                    //for now the first image is display pic, display pic is another post request
                    if (i == 0)
                    {
                        filePath.IsDisplayPic = true;
                        i = 1;
                    }

                    filePaths.Add(filePath);
                }
                
                return filePaths;
            }

        }

    }
}
