using OlibKey.Core.Models.DatabaseModels;
using OlibKey.Core.Models.DatabaseModels.StorageTypes;
using OlibKey.Core.Settings;

namespace OlibKey.Core.UnitTests;

public class DatabaseTests
{
	[Test]
	public void Convert_Database_To_Json()
	{
		Database database = new();
		database.Data.Add(new Login
		{
			Name = "Test",
			Tags =
			[
				"work"
			],
			Username = "TestUsername",
			Password = "TestPassword"
		});
		
		TestContext.WriteLine(database.ToJson());
		
		Assert.Pass();
	}

	[Test]
	public void Lock_And_Unlock_Database()
	{
		const string masterPassword = "udfjg02345";
		
		Database database = new();
		database.Data.Add(new Login
		{
			Name = "Test",
			Tags =
			[
				"work"
			],
			Username = "TestUsername",
			Password = "TestPassword"
		});
		database.Settings = new DatabaseSettings
		{
			Iterations = 10_000,
			Name = "TestDatabase",
		};

		string file = database.Lock(masterPassword);
		
		Database.Unlock(file, masterPassword);

		Assert.Pass();
	}
}