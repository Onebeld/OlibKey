using OlibKey.Core.Helpers;

namespace OlibKey.Core.UnitTests;

public class CompressorTests
{
    [Test]
    public void Checking_For_Identical_Data()
    {
        string defaultText = "This text is an ordinary text that doesn't stand out in any particular way.";

        string compressedText = Compressor.Compress(defaultText);

        string decompressedText = Compressor.Decompress(compressedText);

        Assert.That(defaultText, Is.EqualTo(decompressedText));
    }
}