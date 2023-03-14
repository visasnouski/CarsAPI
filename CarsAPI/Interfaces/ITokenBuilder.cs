namespace CarsAPI.Interfaces
{
    public interface ITokenBuilder
    {
	    string CreateToken(string userName, string secret);
    }
}