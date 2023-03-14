using DataAccess.Models;

namespace DataAccess.Data
{
	public interface IUserData
	{
		public Task<User?> GetUser(string userName);

		public Task InsertUser(User user);
	}
}