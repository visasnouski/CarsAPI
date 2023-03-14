namespace DataAccess.Options
{
	public class DbConnectionSettings
	{
		public string DefaultConnection { get; set; } =
			"Server=(localdb)\\MSSQLLocalDB; Database=CarsDB; Trusted_Connection=true;";
	}
}
