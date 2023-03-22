using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data
{
	public class UserData : IUserData
	{
		private readonly ISqlDataAccess _dbAccess;
		public UserData(ISqlDataAccess dbAccess)
		{
			_dbAccess = dbAccess ?? throw new ArgumentNullException(nameof(dbAccess));
		}

		public async Task<User?> GetUser(string userName, CancellationToken cancellationToken)
		{
			var result = await _dbAccess.LoadData<User, dynamic>("dbo.[spUsers_Get]", new { UserName = userName }, cancellationToken);
			return result.FirstOrDefault();
		}

		public Task InsertUser(User user, CancellationToken cancellationToken) => _dbAccess.SaveData("dbo.[spUsers_Insert]", user, cancellationToken);
	}
}
