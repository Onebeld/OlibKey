using Avalonia.Threading;
using OtpNet;
using PleasantUI;

namespace OlibKey.Core.Models;

public class OlibTotp : ViewModelBase, IDisposable
{
	private DispatcherTimer _timer;

	private Totp? _totp;

	private string? _generatedCode;
	private int _remainingSeconds;
	private int _interval;

	public string? GeneratedCode
	{
		get => _generatedCode;
		private set => RaiseAndSet(ref _generatedCode, value);
	}

	public double RemainingSeconds
	{
		get => _remainingSeconds;
		private set => RaiseAndSet(ref _remainingSeconds, (int)value);
	}

	public double Interval
	{
		get => _interval;
		private set => RaiseAndSet(ref _interval, (int)value);
	}

	public OlibTotp(string secretKey, int interval = 30, OtpHashMode hashMode = OtpHashMode.Sha1)
	{
		Interval = interval;

		_totp = new Totp(Base32Encoding.ToBytes(secretKey.Replace(" ", "")),
			step: interval,
			timeCorrection: new TimeCorrection(DateTime.UtcNow),
			mode: hashMode);

		_timer = new DispatcherTimer();
		_timer.Tick += TimerOnTick;
		_timer.Start();
	}

	private void TimerOnTick(object? sender, EventArgs e)
	{
		if (_totp is null)
			return;

		RemainingSeconds = _totp.RemainingSeconds();
		GeneratedCode = _totp.ComputeTotp();

		RaisePropertyChanged(nameof(Interval));
	}

	public void Dispose()
	{
		_timer.Stop();

		_totp = null;
		_timer = null!;
	}
}