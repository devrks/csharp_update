using Models.Options;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Models.Loggers
{
    public class DbLogger : ILogger
    {
        void ILogger.Log(Exception exception, DateTime? dateTime)
        {
            ((ILogger)this).Log(exception.Message, dateTime);
        }

        void ILogger.Log(string message, DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.Now;
            }
            using (var conn = new SqlConnection(new SettingsManager().GetSection<DbConnectionOptions>().ConnectionString))
            using (var command = new SqlCommand("LogMessage", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(new SqlParameter("Message", message));
                command.Parameters.Add(new SqlParameter("DateTime", dateTime));
                command.ExecuteNonQuery();
            }
        }
    }
}