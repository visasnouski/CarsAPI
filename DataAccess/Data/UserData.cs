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

		public async Task<User?> GetUser(string userName)
		{
			var result = await _dbAccess.LoadData<User, dynamic>("dbo.[spUsers_Get]", new { UserName = userName });
			return result.FirstOrDefault();
		}

		public Task InsertUser(User user) => _dbAccess.SaveData("dbo.[spUsers_Insert]", user);
	}
}
