namespace DataAccess.DbAccess
{
	public interface ISqlDataAccess
	{
		public Task<IEnumerable<T>> LoadData<T, TU>(string storedProcedure, TU parameters, CancellationToken cancellationToken);

		public Task SaveData<T>(string storedProcedure, T parameters, CancellationToken cancellationToken);
	}
}