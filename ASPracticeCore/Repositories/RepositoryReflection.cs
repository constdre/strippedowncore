using ASPracticeCore.Models;
using ASPracticeCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using ASPracticeCore.Areas.Accounts.Models;

namespace ASPracticeCore.Repositories
{
    public class RepositoryReflection : IRepositoryA
    {
        

        /// <summary>
        ///  Method-level Generic Repository - depending on arg type.
        ///  --------------------------------------------------------
        ///  Vanilla C# Generic CRUD functionality
        ///  -classname should be equal to tablename
        ///  -object property names and table column names should be identical
        ///  
        ///  *few non-generic methods with Dapper*
        /// </summary>

        string status = Constants.FAILED;
        string tableName;
        IDbConnection conn;
        
        public RepositoryReflection()
        {
            //list of table names with PRIMARY KEY IDENTITY constraints
            Util.DbUtil.InitializeIdentityTableList();
        }


        /// <summary>
        /// Generic sql insert method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns>procedure status</returns>
        public string Add<T>(T entity) where T: EntityBase
        {
            StringBuilder sb = new StringBuilder();
            tableName = Util.GetClassName(entity.GetType()).ToLower();
            Util.Log("Entity-based tableName =", tableName);
            PropertyInfo[] props = entity.GetType().GetProperties();

            //query building dynamic
            sb.Append("INSERT INTO ").Append(tableName);
            string propNamesQuery = Util.DbUtil.WritePropertyNamesOnQuery(props, tableName);
            string valueParamsQuery = Util.DbUtil.WriteQueryParams(props, tableName);

            sb.Append(propNamesQuery).Append(" VALUES ").Append(valueParamsQuery);// -> "(prop1,prop2) VALUES (@prop1,@prop2)"
            string dynamicQuery = sb.ToString();
            
            //query execution
            try
            {
                using SqlConnection conn = Util.DbUtil.GetConnection();
                conn.Open();
                using SqlCommand sqlCommand = new SqlCommand(dynamicQuery, conn);
                SqlParameterCollection commParams = sqlCommand.Parameters;
                //substitute query params with values from entity arg:
                Util.DbUtil.FillSqlCommandParams(ref commParams, entity);
                int rows = sqlCommand.ExecuteNonQuery();
                Util.Log("Add: Rows Affected =", rows);

                if (rows > 0)
                {
                    status = Constants.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                Util.Log("Add() EXCEPTION\n", ex.ToString());
                status =  Constants.INTERNAL_ERROR;
            }


            Util.Log("Add() final status \n", status, "\nEnd Add()");
            return status;
        }        
        public string AddEntity<T>(T entity) where T: IEntity
        {
            StringBuilder sb = new StringBuilder();
            tableName = Util.GetClassName(entity.GetType()).ToLower();
            Util.Log("Entity-based tableName =", tableName);
            PropertyInfo[] props = entity.GetType().GetProperties();


            //query building dynamic
            sb.Append("INSERT INTO ").Append(tableName);
            string propNamesQuery = Util.DbUtil.WritePropertyNamesOnQuery(props, tableName);
            string valueParamsQuery = Util.DbUtil.WriteQueryParams(props, tableName);

            sb.Append(propNamesQuery).Append(" VALUES ").Append(valueParamsQuery);// -> "(prop1,prop2) VALUES (@prop1,@prop2)"
            string dynamicQuery = sb.ToString();
            
            //query execution
            try
            {
                using (SqlConnection conn = Util.DbUtil.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(dynamicQuery, conn))
                    {
                        SqlParameterCollection commParams = sqlCommand.Parameters;

                        //substitute query params with values from entity arg:
                        Util.DbUtil.FillSqlCommandParams(ref commParams, entity);
                        int rows = sqlCommand.ExecuteNonQuery();
                        Util.Log("Add: Rows Affected =", rows);

                        if (rows > 0)
                        {
                            status = Constants.SUCCESS;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Util.Log("Add() EXCEPTION\n", ex.ToString());
                status =  Constants.INTERNAL_ERROR;
            }


            Util.Log("Add() final status \n", status, "\nEnd Add()");
            return status;
        }

        public string Delete()
        {
            throw new NotImplementedException();
        }

        public string Update<T>(T entity) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generic method that retrieves all table entries given className = tableName
        /// </summary>
        /// <typeparam name="T">The user-defined reference type</typeparam>
        /// <returns>List of table entries</returns>
        public List<T> GetAll<T>() 
        {

            Util.Log("Inside GetAll()");
            tableName = Util.GetClassName(typeof(T));
            List<T> entities = new List<T>();

            try
            {
                using (SqlConnection conn = Util.DbUtil.GetConnection())
                {
                    SqlCommand query = new SqlCommand("SELECT * FROM " + tableName, conn);
                    conn.Open();
                    SqlDataReader dr = query.ExecuteReader();
                    Util.Log("SELECT STATEMENT SUCCESSFUL = ", dr.HasRows);
                    DataTable schemaTable = dr.GetSchemaTable(); //contains column information - name, capacity, etc.
                    DataColumn column_key = schemaTable.Columns[0];//key for column name, think it's "COLUMN_NAME"

                    while (dr.Read())
                    {
                        
                        dynamic entity = Activator.CreateInstance(typeof(T));
                        
                        //iterate on the table's columns (rows of SchemaTable)
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            //get column name through key
                            string columnName = row[column_key].ToString();

                            PropertyInfo prop = typeof(T).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            

                            //get column value from datareader through column name 
                            var columnValue = dr.GetValue(dr.GetOrdinal(columnName));//or dr[columnName]
                            Util.Log($"{columnName} : {columnValue}");
                            //set entity's property value
                            prop.SetValue(entity, columnValue);     

                        }
                        entities.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log("GetAll() Exception\n", ex.ToString());
                return null;
            }

            return entities;
        }
        /// <summary>
        /// Retrieves table data according to given parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters">an anonymous object containing the search constraints</param>
        /// <returns></returns>
        public List<T> GetFiltered<T>(dynamic filters)
        {
            List<T> entities = new List<T>();
            tableName = Util.GetClassName(typeof(T));
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM " + tableName).Append(" WHERE ");
            PropertyInfo[] props = filters.GetType().GetProperties();

            //build the WHERE clause from the anonymous object's properties
            for(int i=0; i<props.Length; i++)
            {
                var name = props[i].Name;
                var val = "@" + name;
                sb.Append(name+" = "+val);

                if (i!=props.Length-1)
                {
                    sb.Append(" AND ");
                }
            }

            string finalQuery = sb.ToString();
            Util.Log(finalQuery);

            //Query execution, object building : reflection, add to List:
            try
            {

                using SqlConnection conn = Util.DbUtil.GetConnection();
                conn.Open();
                var query = new SqlCommand(finalQuery, conn);
                SqlParameterCollection commandParams = query.Parameters;
                Util.DbUtil.FillSqlCommandParams(ref commandParams, filters);

                SqlDataReader dr = query.ExecuteReader();
                //DataTable columnInfoTable = dr.GetSchemaTable();

                while (dr.Read())
                {
                    //the dr.GetName route
                    dynamic entity = Util.DbUtil.ProduceInstance(typeof(T), dr);
                    entities.Add(entity);
                }

            }
            catch(Exception e)
            {
                Util.Log(e.ToString());
            }


            return entities;

        }
        public T GetById<T>(int id) where T : EntityBase
        {
            return GetFiltered<T>(new {id}).FirstOrDefault();
        }

        //aims to consider properties of type EntityBase
        private bool WriteDynamicAddQuery(PropertyInfo[] props, string tableName, ref string query)
        {
            
            string colSpec, valueParams;
            bool isSkipId = Util.DbUtil.IsIdAutoIncrement(tableName);
            int factorIndex = isSkipId ? props.Length - 2 : props.Length - 1;
            bool isWithEntityProp = false;
            //start the query building:
            colSpec = "(";
            valueParams = "(";
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType == typeof(EntityBase))
                {
                    //Add(tableName,props[i]);
                    isWithEntityProp = true;
                }
                if (props[i].Name.ToLower().Equals("id") && isSkipId)
                {
                    continue;
                }
                colSpec += props[i].Name; //(Address, PhoneNo)
                string vParam = "@" + props[i].Name; //(@Address, @PhoneNo)
                valueParams += vParam;
                //
                if (i == factorIndex)
                {
                    break;
                }
                colSpec += ",";
                valueParams += ",";

            }
            colSpec += ")";
            valueParams += ")";


            query = colSpec + " VALUES " + valueParams; //(Address, PhoneNo) VALUES (@Address, @PhoneNo);
            return isWithEntityProp;
        }


        /// <summary>
        /// async - gets the child data of a parent entity e.g. dish : ingredients
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ownerClassName">class name of the parent, assumes schema table is equal to class name</param>
        /// <param name="itemClassName">class name of the child, assumes schema table is equal to class name</param>
        /// <param name="ownerId">id of the parent</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetItemsOfOwner<T>(string ownerClassName, string itemClassName, int ownerId)
        {
            string sql = "usp_GetItemsOfOwner";
            string fkName = ownerClassName + "id";
            using (conn = Util.DbUtil.GetConnection())
            {
                return await conn.QueryAsync<T>(sql, new { items_table = itemClassName.ToLower(), fk_column_name = fkName.ToLower(), fk_column_value = ownerId.ToString() }, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<Shareable> GetShareablesOfUser(int id)
        {
            string sql = "usp_GetItemsOfOwner";
            IEnumerable<Shareable> userShareables;
            using (conn = Util.DbUtil.GetConnection())
            {
                userShareables = conn.Query<Shareable>(sql, new { items_table = "shareable", fk_column_name = "userid", fk_column_value = id.ToString() }, commandType: CommandType.StoredProcedure);
            }
            return userShareables;
        }
        public UserAccount GetUserByEmail(string email)
        {
            string sql = "SELECT * FROM UserAccount WHERE email = @email";
            IEnumerable<UserAccount> result;
            try
            {
                using (conn = Util.DbUtil.GetConnection())
                {
                    result = conn.Query<UserAccount>(sql, new { email });
                }
            }
            catch (SqlException ex)
            {
                Util.Log("GetUserByEmail EXCEPTION!\n", ex.ToString());
                return null;
            }
            return result.FirstOrDefault();

        }

    }
}
