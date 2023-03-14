namespace DataAccess.Models
{
	public class User
	{
		public string UserName { get; set; } = null!;

		public byte[] PasswordHash { get; set; } = null!;

		public byte[] PasswordSalt { get; set; } = null!;
	}
}
