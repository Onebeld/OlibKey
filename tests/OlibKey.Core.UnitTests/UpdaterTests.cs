using OlibKey.Core.Models;

namespace OlibKey.Core.UnitTests;

public class UpdaterTests
{
	[Test]
	public async Task Get_Update()
	{
		Update? update = await Update.Get();

		if (update is null)
		{
			Assert.Fail("Update is null");
			return;
		}

		TestContext.WriteLine(update.Version);
		TestContext.WriteLine(update.Link);
		TestContext.WriteLine(update.PublishedAt);
		TestContext.WriteLine(update.Body);
		
		Assert.Pass();
	}
}