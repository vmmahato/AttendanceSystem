using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Database.Configuration
{
    public interface IConnectionSetting : ISettings
    {
        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <returns>Connection string to SQL</returns>
        string Get();
    }

    public class ConnectionSetting : IConnectionSetting
    {
        protected string _connectionString;
        public ConnectionSetting(string connectionString)
        {
            _connectionString = connectionString;
            // For fix on Testing Module not getting Connection String
            if (string.IsNullOrEmpty(connectionString))
            {
                _connectionString = "Data Source = WIN - NUJK5PDA864; Initial Catalog = BPRS; User Id = BPRS; Password = Dizzy - clam45!; Trusted_Connection = False;";
            }
        }
        public string Get()
        {
            return _connectionString;
        }
    }
}
