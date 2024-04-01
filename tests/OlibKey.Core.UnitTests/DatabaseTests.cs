using OlibKey.Core.Models.Database;
using OlibKey.Core.Models.Database.StorageTypes;

namespace OlibKey.Core.UnitTests;

public class DatabaseTests
{
	[Test]
	public void Convert_Database_To_Json()
	{
		Database database = new()
		{
			Data =
			[
				new Login
				{
					Name = "Test",
					Tags =
					[
						"work"
					],
					Username = "TestUsername",
					Password = "TestPassword"
				}
			]
		};
		
		TestContext.WriteLine(database.ToJson());
		
		Assert.Pass();
	}
}