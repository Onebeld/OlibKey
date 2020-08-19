namespace OlibKey.Structures
{
	public class Settings
	{
		public string PathDatabase { get; set; }
		public string Theme { get; set; } = "Gloomy";
		public string Language { get; set; }
		public bool FirstRun { get; set; } = true;
		public int AutosaveDuration { get; set; } = 2;
		public int BlockDuration { get; set; } = 5;
		public bool AutoblockEnabled { get; set; } = true;
		public int MessageDuration { get; set; } = 3;
		public bool UsingGPU { get; set; } = false;

		public string GenerationCount { get; set; } = "10";
		public bool GeneratorAllowLowercase { get; set; } = true;
		public bool GeneratorAllowNumber { get; set; } = true;
		public bool GeneratorAllowOther { get; set; }
		public bool GeneratorAllowSpecial { get; set; }
		public bool GeneratorAllowUnderscore { get; set; }
		public bool GeneratorAllowUppercase { get; set; } = true;
		public string GeneratorTextOther { get; set; }
	}
}
