using System.Data;
using System.Data.SqlClient;
using Dapper;
using DataAccess.Options;
using Microsoft.Extensions.Options;

namespace DataAccess.DbAccess
{
	public class SqlDataAccess : ISqlDataAccess
	{
		private readonly DbConnectionSettings _dbConnectionSettings;

		public SqlDataAccess(IOptions<DbConnectionSettings> dbSettings)
		{
			_dbConnectionSettings = dbSettings.Value ?? throw new ArgumentNullException(nameof(dbSettings));
		}

		public async Task<IEnumerable<T>> LoadData<T, TU>(string storedProcedure, TU parameters, CancellationToken cancellationToken)
		{
			using IDbConnection connection = new SqlConnection(_dbConnectionSettings.DefaultConnection);
			var commandDefinition = new CommandDefinition(storedProcedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
			return await connection.QueryAsync<T>(commandDefinition);
		}

		public async Task SaveData<T>(string storedProcedure, T parameters, CancellationToken cancellationToken)
		{
			using IDbConnection connection = new SqlConnection(_dbConnectionSettings.DefaultConnection);
			var commandDefinition = new CommandDefinition(storedProcedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
			await connection.ExecuteAsync(commandDefinition);
		}
	}
}

