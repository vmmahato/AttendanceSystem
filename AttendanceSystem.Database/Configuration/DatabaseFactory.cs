using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System;
using AttendanceSystem.Database.Configuration;
using AttendanceSystem.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AttendanceSystem.DatabaseConnectionFactory
{
    /*
     * Returns Connection and DbContext
     * Inject this over DbContext injection.
     * */

    public class DatabaseFactory : IDatabaseFactory
    {
        private IDbConnection _connection;
        private AttendanceSystemDbContext _context;
        private IDbTransaction _dbTransaction;
        public DatabaseFactory(IConnectionSetting setting, AttendanceSystemDbContext context)
        {
            ConnectionString = setting.Get();
            _context = context;
        }
        public string ConnectionString { get; set; }

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(ConnectionString);
                }
                return _connection;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return _dbTransaction;
            }
        }


        public AttendanceSystemDbContext Context
        {
            get
            {
                return _context;
            }
        }

        public void ChangeConnection(IDbConnection toConnection)
        {
            _connection = toConnection;
        }
        public void ChangeContext(AttendanceSystemDbContext tocontext)
        {
            _context = tocontext;
        }

        public void ChangeTransaction(IDbTransaction toTransaction)
        {
            _dbTransaction = toTransaction;
        }
    }

    public interface IDatabaseFactory
    {
        string ConnectionString { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        AttendanceSystemDbContext Context { get; }
        void ChangeConnection(IDbConnection toConnection);
        void ChangeContext(AttendanceSystemDbContext tocontext);
        void ChangeTransaction(IDbTransaction toTransaction);
    }
}
