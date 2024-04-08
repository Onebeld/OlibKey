using OlibKey.Core.Models;
using OlibKey.Core.Other;

namespace OlibKey.Core.UnitTests;

public class UpdaterTests
{
	[Test]
	public async Task Get_Update()
	{
		Update? update = await Updater.GetUpdate();

		if (update is null)
		{
			Assert.Fail("Update is null");
			return;
		}

		Update updateValue = update.Value;
		
		TestContext.WriteLine(updateValue.Version);
		TestContext.WriteLine(updateValue.Link);
		TestContext.WriteLine(updateValue.PublishedAt);
		TestContext.WriteLine(updateValue.Body);
		
		Assert.Pass();
	}
}