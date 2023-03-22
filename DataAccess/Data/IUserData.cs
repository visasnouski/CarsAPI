using DataAccess.Models;

namespace DataAccess.Data
{
	public interface IUserData
	{
		public Task<User?> GetUser(string userName, CancellationToken cancellationToken);

		public Task InsertUser(User user, CancellationToken cancellationToken);
	}
}