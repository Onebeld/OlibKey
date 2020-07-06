namespace OlibKey.Structures
{
    public class Settings
    {
	    public string Theme { get; set; }
	    public string GenerationCount { get; set; }
	    public string PathStorage { get; set; }
	    public string ApplyTheme { get; set; }
		public string Language { get; set; }
		public bool FirstRun { get; set; } = true;

			public bool GeneratorAllowLowercase { get; set; }
	    public bool GeneratorAllowNumber { get; set; }
	    public bool GeneratorAllowOther { get; set; }
	    public bool GeneratorAllowSpecial { get; set; }
	    public bool GeneratorAllowUnderscore { get; set; }
	    public bool GeneratorAllowUppercase { get; set; }
	    public string GeneratorTextOther { get; set; }
	}
}
