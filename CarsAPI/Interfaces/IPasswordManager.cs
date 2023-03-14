namespace CarsAPI.Interfaces
{
	public interface IPasswordManager
	{
		(byte[] passwordHash, byte[] passwordSalt) GeneratePasswordHashAndSalt(string password);

		bool VerifyPasswordHashAndSalt(string password, byte[] passwordHash, byte[] passwordSalt);

	}
}