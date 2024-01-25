using System.Data;
using System.Reflection;

namespace Erp.Tools.CL
{
    public static class DbConnectionExtensions
    {
        public static int ExecuteNonQuery(this IDbConnection dbConnection, string query, bool isStoredProcedure = false, object? parameters = null)
        {
            using IDbCommand command = CreateCommand(dbConnection, query, isStoredProcedure, parameters);
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            return command.ExecuteNonQuery();
        }

        public static object? ExecuteScalar(this IDbConnection dbConnection, string query, bool isStoredProcedure = false, object? parameters = null)
        {
            using IDbCommand command = CreateCommand(dbConnection, query, isStoredProcedure, parameters);
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            object? result = command.ExecuteScalar();
            return result is DBNull ? null : result;
        }

        public static IEnumerable<TResult> ExecuteReader<TResult>(this IDbConnection dbConnection, string query, Func<IDataRecord, TResult> mapper, bool isStoredProcedure = false, object? parameters = null)
        {
            ArgumentNullException.ThrowIfNull(nameof(mapper));

            using IDbCommand command = CreateCommand(dbConnection, query, isStoredProcedure, parameters);
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            using IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return mapper(reader);
            }
        }

        private static IDbCommand CreateCommand(IDbConnection dbConnection, string query, bool isStoredProcedure, object? parameters)
        {
            IDbCommand command = dbConnection.CreateCommand();
            command.CommandText = query;

            if (isStoredProcedure)
            {
                command.CommandType = CommandType.StoredProcedure;
            }

            if (parameters is not null)
            {
                Type type = parameters.GetType();

                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    IDataParameter dataParameter = command.CreateParameter();
                    dataParameter.ParameterName = propertyInfo.Name;
                    dataParameter.Value = propertyInfo.GetValue(parameters, null) ?? DBNull.Value;
                    command.Parameters.Add(dataParameter);
                }
            }

            return command;
        }
    }
}
