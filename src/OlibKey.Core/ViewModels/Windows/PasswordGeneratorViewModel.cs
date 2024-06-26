using Avalonia.Input;
using OlibKey.Core.Helpers;
using OlibKey.Core.Helpers.Safety;
using PleasantUI;
using PleasantUI.Controls;

namespace OlibKey.Core.ViewModels.Windows;

public class PasswordGeneratorViewModel : ViewModelBase
{
	private readonly ContentDialog? _contentDialog;

	private string _password = string.Empty;
	private bool _returnRequired;

	#region Properties

	public string Password
	{
		get => _password;
		set => RaiseAndSet(ref _password, value);
	}

	public bool ReturnRequired
	{
		get => _returnRequired;
		set => RaiseAndSet(ref _returnRequired, value);
	}

	public bool ShowTitle { get; set; }

	#endregion

	public PasswordGeneratorViewModel(ContentDialog? contentDialog, bool returnRequired)
	{
		_contentDialog = contentDialog;
		ReturnRequired = returnRequired;

		_contentDialog?.KeyBindings.Add(new KeyBinding
		{
			Command = Command.Create(SavePassword),
			Gesture = KeyGesture.Parse("Enter")
		});

		GeneratePassword();
	}

	public async void CopyPassword()
	{
		await OlibKeyApp.TopLevel.Clipboard?.SetTextAsync(Password);
	}

	public void GeneratePassword()
	{
		Password = PasswordGenerator.Generate();
	}

	public void SavePassword()
	{
		if (string.IsNullOrEmpty(Password))
			Password = PasswordGenerator.Generate();

		_contentDialog?.Close(Password);
	}

	public void CloseWindow()
	{
		_contentDialog?.Close(null);
	}
}