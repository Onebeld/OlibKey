namespace OlibPasswordManager.Properties.Core
{
    public class Settings
    {
        public string GeneratorMinCount { get; set; }
        public string GeneratorMaxCount { get; set; }

        public string AppGlobalString { get; set; }

        public bool GeneratorAllowLowercase { get; set; }
        public bool GeneratorAllowUppercase { get; set; }
        public bool GeneratorAllowNumber { get; set; }
        public bool GeneratorAllowSpecial { get; set; }
        public bool GeneratorAllowUnderscore { get; set; }
        public bool GeneratorAllowSpace { get; set; }
        public bool GeneratorAllowOther { get; set; }
        public bool GeneratorRequireLowercase { get; set; }
        public bool GeneratorRequireUppercase { get; set; }
        public bool GeneratorRequireNumber { get; set; }
        public bool GeneratorRequireSpecial { get; set; }
        public bool GeneratorRequireUnderscore { get; set; }
        public bool GeneratorRequireSpace { get; set; }
        public bool GeneratorRequireOther { get; set; }
        public string GeneratorTextOther { get; set; }
    }
}
