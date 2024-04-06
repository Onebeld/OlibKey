using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Media;
using OlibKey.Core.Helpers;
using OlibKey.Core.Models;
using OlibKey.Core.Settings;
using OlibKey.Core.StaticMembers;
using PleasantUI;
using PleasantUI.Core;
using PleasantUI.Core.Enums;
using PleasantUI.Windows;

namespace OlibKey.Core.ViewModels;

public class SettingsViewModel : ViewModelBase
{
	public bool IsNotLoaded = true;

	public static List<FontFamily> FontFamilies { get; }

	public Language SelectedLanguage
	{
		get => Localization.Languages.First(lang => lang.Key == OlibKeySettings.Instance.Language);
		set
		{
			if (IsNotLoaded) return;

			OlibKeySettings.Instance.Language = value.Key;
			OlibKeyApp.UpdateLanguage();
		}
	}

	public FontFamily SelectedFontFamily
	{
		get => FontFamily.Parse(OlibKeySettings.Instance.FontName);
		set => OlibKeySettings.Instance.FontName = value.Name;
	}

	public Theme SelectedTheme
	{
		get => PleasantSettings.Instance.Theme;
		set
		{
			if (IsNotLoaded) return;

			PleasantSettings.Instance.Theme = value;
		}
	}

	public bool UseAccentColor
	{
		get => !PleasantSettings.Instance.PreferUserAccentColor;
		set
		{
			if (IsNotLoaded) return;

			PleasantSettings.Instance.PreferUserAccentColor = !value;

			if (PleasantSettings.Instance.PreferUserAccentColor)
				return;

			Color? accent = Application.Current?.PlatformSettings?.GetColorValues().AccentColor1;

			if (accent is null)
				return;

			PleasantSettings.Instance.NumericalAccentColor = accent.Value.ToUInt32();
		}
	}

	static SettingsViewModel()
	{
		FontFamilies = new List<FontFamily>(FontManager.Current.SystemFonts.OrderBy(x => x.Name));
	}

	public async void ChangeAccentColor()
	{
		Color? color =
			await ColorPickerWindow.SelectColor(OlibKeyApp.Main, PleasantSettings.Instance.NumericalAccentColor);

		if (color is null)
			return;

		PleasantSettings.Instance.NumericalAccentColor = color.Value.ToUInt32();
	}

	/// <summary>
	/// Asynchronously copies the accent color in hex format to the clipboard and shows a notification that reports the task has been completed.
	/// </summary>
	public async void CopyAccentColor()
	{
		await OlibKeyApp.TopLevel.Clipboard?.SetTextAsync(
			$"#{PleasantSettings.Instance.NumericalAccentColor.ToString("x8").ToUpper()}")!;

		OlibKeyApp.ShowNotification(OlibKeyApp.GetLocalizationString("Information"),
			OlibKeyApp.GetLocalizationString("ColorCopied"),
			NotificationType.Information,
			TimeSpan.FromSeconds(2));
	}

	public async void PasteAccentColor()
	{
		string? data = await OlibKeyApp.TopLevel.Clipboard?.GetTextAsync()!;

		if (uint.TryParse(data, out uint uintColor))
			PleasantSettings.Instance.NumericalAccentColor = uintColor;
		else if (Color.TryParse(data, out Color color))
			PleasantSettings.Instance.NumericalAccentColor = color.ToUInt32();
	}

	public async void ResetSettings()
	{
		string result = await MessageBox.Show(OlibKeyApp.Main, OlibKeyApp.GetLocalizationString(""), string.Empty,
			MessageBoxButtons.ReverseYesNo);

		if (result != "Yes") return;

		SelectedTheme = Theme.System;

		OlibKeyApp.UpdateLanguage();

		RaisePropertyChanged(nameof(SelectedLanguage));
		RaisePropertyChanged(nameof(SelectedFontFamily));
		RaisePropertyChanged(nameof(SelectedTheme));

		OlibKeySettings.Reset();

		// TODO: Notification after resetting
		OlibKeyApp.ShowNotification("", "", NotificationType.Success, TimeSpan.FromSeconds(2));
	}
}