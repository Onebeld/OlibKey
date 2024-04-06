using OlibKey.Core.Models.StorageModels;
using OlibKey.Core.Models.StorageModels.StorageTypes;
using OlibKey.Core.Settings;

namespace OlibKey.Core.UnitTests;

public class StorageTests
{
	[Test]
	public void Convert_Storage_To_Json()
	{
		Storage storage = new();
		storage.Data.Add(new Login
		{
			Name = "Test",
			Tags =
			[
				"work"
			],
			Username = "TestUsername",
			Password = "TestPassword"
		});
		
		TestContext.WriteLine(storage.ToJson());
		
		Assert.Pass();
	}

	[Test]
	public void Lock_And_Unlock_Storage()
	{
		const string masterPassword = "udfjg02345";
		
		Storage storage = new();
		storage.Data.Add(new Login
		{
			Name = "Test",
			Tags =
			[
				"work"
			],
			Username = "TestUsername",
			Password = "TestPassword"
		});
		storage.Settings = new StorageSettings
		{
			Iterations = 10_000,
			Name = "TestStorage",
		};

		string file = storage.Lock(masterPassword);
		
		Storage.Unlock(file, masterPassword);

		Assert.Pass();
	}
}