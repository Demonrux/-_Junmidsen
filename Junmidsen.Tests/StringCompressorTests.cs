using Junmidsen.Core;

namespace Junmidsen.Tests
{
    public class StringCompressorTests
    {
        [Fact]
        public void Compress_WithSingleChars_ReturnsSameString()
        {
            Assert.Equal("abc", StringCompressor.Compress("abc"));
        }

        [Fact]
        public void Compress_WithRepeatedChars_ReturnsCompressed()
        {
            Assert.Equal("a3b2c3d2e", StringCompressor.Compress("aaabbcccdde"));
        }

        [Fact]
        public void Decompress_WithCompressedString_ReturnsOriginal()
        {
            string original = "aaabbcccdde";
            string compressed = StringCompressor.Compress(original);
            Assert.Equal(original, StringCompressor.Decompress(compressed));
        }

        [Fact]
        public void Decompress_WithInvalidFormat_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => StringCompressor.Decompress("2a")); 
        }
    }
}