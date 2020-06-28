using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using ASPracticeCore.Models;
using ASPracticeCore.Utils;
using Dapper;

namespace ASPracticeCore.Repositories
{
    public class RepositoryDapper : IRepositoryA
    {
        IDbConnection conn;
        string tableName;
        string sql;
        Type entityType;
        PropertyInfo[] props;

        public RepositoryDapper()
        {
            Util.DbUtil.InitializeIdentityTableList();
        }

        public string Add<T>(T entity) where T : EntityBase
        {
            StringBuilder sb = new StringBuilder();
            entityType = entity.GetType();
            tableName = Util.GetClassName(entityType);
            props = entityType.GetProperties();

            //dynamic query building:
            sb.Append("INSERT INTO " + tableName);
            sb.Append(Util.DbUtil.WritePropertyNamesOnQuery(props, tableName));
            sb.Append(Util.DbUtil.WriteQueryParams(props, tableName));
            int rowsAffected;
            sql = sb.ToString();
            Util.Log("Final built query: " + sql);

            using(conn = Util.DbUtil.GetConnection())
            {
                conn.Open();
                DynamicParameters parameters = Util.DbUtil.FillSqlDapperParams(entity);
                rowsAffected = conn.Execute(sql, parameters);
            }
            string returnString = (rowsAffected <= 0) ? Constants.SUCCESS : Constants.INTERNAL_ERROR;
            return returnString;
        }

        public string Delete()
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll<T>()
        {

            tableName = Util.GetClassName(typeof(T));
            using(conn = Util.DbUtil.GetConnection())
            {
                return conn.Query<T>("SELECT *  FROM " + tableName).ToList();
            }
        }

        public List<T> GetFiltered<T>(dynamic filters)
        {
            return null;
        }
        public IEnumerable<T> GetAllEntities<T>()
        {
            string tableName = Util.GetClassName(typeof(T));
            
            using (conn = Util.DbUtil.GetConnection())
            {
                conn.Open();
                return conn.Query<T>("SELECT *  FROM " + tableName);
            }
            
        }


        public T GetById<T>(int id) where T : EntityBase
        {
            return GetAll<T>().FirstOrDefault(t => t.Id == id);
        }

        public string Update<T>(T entity) where T : EntityBase
        {
            throw new NotImplementedException();
        }
    }
}
