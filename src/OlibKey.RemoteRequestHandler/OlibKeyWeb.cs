using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using OlibKey.RemoteRequestHandler.Controllers;

namespace OlibKey.RemoteRequestHandler;

public static class OlibKeyWeb
{
	private static WebApplication _app = null!;
	
	public static void RunApp()
	{
		WebApplicationBuilder builder = WebApplication.CreateSlimBuilder();

		builder.WebHost.UseUrls("http://localhost:27419");
		
		_app = builder.Build();

		ConfigureStorageApi();
		
		_app.Run();
	}
	
	public static async Task StopApp()
	{
		await _app.StopAsync();
	}

	private static void ConfigureStorageApi()
	{
		RouteGroupBuilder storageApi = _app.MapGroup("/storage");
		
		storageApi.MapGet("/isUnlocked", StorageController.StorageIsUnlocked);
	}
}