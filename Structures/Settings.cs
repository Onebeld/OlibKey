namespace OlibKey.Structures
{
    public class Settings
    {
	    public string PathDatabase { get; set; }
		public string Theme { get; set; } = "Dark";
		public string Language { get; set; }
		public bool FirstRun { get; set; } = true;

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
