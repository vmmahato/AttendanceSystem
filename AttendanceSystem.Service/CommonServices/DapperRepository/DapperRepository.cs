using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AttendanceSystem.DatabaseConnectionFactory;
using AttendanceSystem.PageExtension;
using Dapper;

namespace AttendanceSystem.DapperServices
{
    public class DapperRepository : IDapperRepository
    {
        protected IDatabaseFactory _databaseFactory;
        public DapperRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public void ExecuteStoredProc(string storedProcName, object parameters, int? timeout = null)
        {
            _databaseFactory.Connection.Execute(storedProcName, param: parameters, transaction: _databaseFactory.Transaction, commandType: CommandType.StoredProcedure);

        }
        public IEnumerable<T> ExecuteStoredProc<T>(string storedProcName, object _parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.Query<T>(storedProcName, _parameters, commandTimeout: timeout, commandType: CommandType.StoredProcedure, transaction: _databaseFactory.Transaction);
        }

        public int Execute(string query, object _parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.Execute(query, _parameters, transaction: _databaseFactory.Transaction);
        }

        public IEnumerable<T> ExecuteQuery<T>(string query, object _parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.Query<T>(query, _parameters, transaction: _databaseFactory.Transaction, commandTimeout: timeout);
        }

        public async Task<IPagedList<T>> ExecuteQueryWithPagedListAsync<T>(string query, object _parameters, int pageSize, int pageIndex, string orderBy, int? timeout = null)
        {
            int totalCount = 0;
            DynamicParameters parameters = (DynamicParameters)_parameters;
            var sqlQuery = (string.Format(@"SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY {2}) AS RowNum FROM ({3}) T) AS Paged  WHERE RowNum BETWEEN ( ( {1} - 1 ) * {0} ) + 1 AND ({0} * {1}) ORDER BY {2}  select @TotalCount=count(*) from ({3}) aa", pageSize, pageIndex, orderBy, query));
            parameters.Add("TotalCount", DbType.Int32, direction: ParameterDirection.Output);
            IEnumerable<T> results;

            results = await _databaseFactory.Connection.QueryAsync<T>(Convert.ToString(sqlQuery), parameters, transaction: _databaseFactory.Transaction, commandTimeout: timeout);
            totalCount = parameters.Get<int>("TotalCount");
            return results.ToPagedList(pageIndex, pageSize, totalCount);
        }

        public Task ExecuteStoreProcAsync(string storedProcName, object parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.ExecuteAsync(storedProcName, param: parameters, transaction: _databaseFactory.Transaction, commandTimeout: timeout, commandType: CommandType.StoredProcedure);
        }  
        public Task<IEnumerable<T>> ExecuteStoredProcAsync<T>(string storedProcName, object _parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.QueryAsync<T>(storedProcName, _parameters, commandType: CommandType.StoredProcedure, transaction: _databaseFactory.Transaction, commandTimeout: timeout);
        }
        public Task<int> ExecuteAsync(string query, object _parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.ExecuteAsync(query, _parameters, transaction: _databaseFactory.Transaction, commandTimeout: timeout);
        }
        public Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object _parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.QueryAsync<T>(query, _parameters, transaction: _databaseFactory.Transaction, commandTimeout: timeout);
        }
      
        public T ExecuteQueryFirstOrDefault<T>(string query, object _parameters)
        {
            return _databaseFactory.Connection.QueryFirstOrDefault<T>(query, _parameters, transaction: _databaseFactory.Transaction);
        }
        public Task<T> ExecuteQueryFirstOrDefaultAsync<T>(string query, object _parameters)
        {
            return _databaseFactory.Connection.QueryFirstOrDefaultAsync<T>(query, _parameters, transaction: _databaseFactory.Transaction);

        }

        public T ExecuteStoredFirstOrDefault<T>(string storedProcName, object _parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.QueryFirstOrDefault<T>(storedProcName, _parameters, commandType: CommandType.StoredProcedure, transaction: _databaseFactory.Transaction, commandTimeout: timeout);
        }

        public Task<T> ExecuteStoredFirstOrDefaultAsync<T>(string storedProcName, object _parameters, int? timeout = null)
        {
            return _databaseFactory.Connection.QueryFirstOrDefaultAsync<T>(storedProcName, _parameters, commandType: CommandType.StoredProcedure, transaction: _databaseFactory.Transaction, commandTimeout: timeout);
        }
    }
    public interface IDapperRepository
        {
            Task ExecuteStoreProcAsync(string storedProcName, object parameters, int? timeout = null);
            void ExecuteStoredProc(string storedProcName, object parameters, int? timeout = null);
            IEnumerable<T> ExecuteStoredProc<T>(string storedProcName, object _parameters, int? timeout = null);
            Task<IEnumerable<T>> ExecuteStoredProcAsync<T>(string storedProcName, object _parameters, int? timeout = null);
            T ExecuteStoredFirstOrDefault<T>(string storedProcName, object _parameters, int? timeout = null);
            Task<T> ExecuteStoredFirstOrDefaultAsync<T>(string storedProcName, object _parameters, int? timeout = null);
            int Execute(string query, object _parameters, int? timeout = null);
            Task<int> ExecuteAsync(string query, object _parameters, int? timeout = null);
            IEnumerable<T> ExecuteQuery<T>(string query, object _parameters, int? timeout = null);
            T ExecuteQueryFirstOrDefault<T>(string query, object _parameters);
            Task<T> ExecuteQueryFirstOrDefaultAsync<T>(string query, object _parameters);
            Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object _parameters, int? timeout = null);
            Task<IPagedList<T>> ExecuteQueryWithPagedListAsync<T>(string query, object _parameters, int pageSize, int pageIndex, string orderBy, int? timeout = null);
    }
}
