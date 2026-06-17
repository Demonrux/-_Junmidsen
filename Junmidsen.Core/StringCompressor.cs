using System.Text;

namespace  Junmidsen.Core
{
    /// <summary>
    /// Provides methods for run-length encoding compression and decompression
    /// </summary>
    public static class StringCompressor
    {
        /// <summary>
        /// Compresses the input string by replacing consecutive identical characters
        /// with the character followed by the count. If count is 1, the number is omitted
        /// </summary>
        public static string Compress(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new StringBuilder();
            int i = 0;
            while (i < input.Length)
            {
                char current = input[i];
                int count = 1;
                while (i + count < input.Length && input[i + count] == current)
                    count++;

                result.Append(current);
                if (count > 1)
                    result.Append(count);

                i += count;
            }
            return result.ToString();
        }

        /// <summary>
        /// Decompresses a compressed string back to the original
        /// Expects a valid compressed format: letters followed by optional digit(s)
        /// </summary>
        public static string Decompress(string compressed)
        {
            if (string.IsNullOrEmpty(compressed))
                return compressed;

            var result = new StringBuilder();
            int i = 0;
            while (i < compressed.Length)
            {
                char ch = compressed[i];
                if (!char.IsLetter(ch))
                    throw new FormatException($"Expected a letter, but found '{ch}' at position {i}");

                i++;
                int count = 1;
                if (i < compressed.Length && char.IsDigit(compressed[i]))
                {
                    count = 0;
                    while (i < compressed.Length && char.IsDigit(compressed[i]))
                    {
                        count = count * 10 + (compressed[i] - '0');
                        i++;
                    }
                }
                result.Append(ch, count);
            }
            return result.ToString();
        }
    }
}