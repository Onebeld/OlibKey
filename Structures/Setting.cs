namespace OlibKey.Structures
{
    public class Setting
    {
        public string Theme { get; set; }
        public string GenerationCount { get; set; }
        public string PathStorage { get; set; }
        public bool CollapseOnClose { get; set; }
        public bool AutorunApplication { get; set; }
        public bool CollapseWhenClosing { get; set; }

        public bool GeneratorAllowLowercase { get; set; }
        public bool GeneratorAllowNumber { get; set; }
        public bool GeneratorAllowOther { get; set; }
        public bool GeneratorAllowSpecial { get; set; }
        public bool GeneratorAllowUnderscore { get; set; }
        public bool GeneratorAllowUppercase { get; set; }
        public string GeneratorTextOther { get; set; }
    }
}
