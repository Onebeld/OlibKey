using Microsoft.AspNetCore.Http;
using OlibKey.Core;

namespace OlibKey.RemoteRequestHandler.Controllers;

public static class StorageController
{
	public static IResult StorageIsUnlocked()
	{
		string output;
		
		if (OlibKeyApp.ViewModel.Session.Storage is not null)
			output ="Unlocked";
		else
			output = "Locked";

		return Results.Content(output, "text/plain;charset=utf-8");
	}
}