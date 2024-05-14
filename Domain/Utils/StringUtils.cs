using System.Text.RegularExpressions;

namespace dogsitting_backend.Domain.Utils
{
    public static class StringUtils
    {
        public static string ToKebabCase(string input)
        {
            // Remove special characters using regex
            string withoutSpecialCharacters = Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");

            // Replace spaces with dashes
            string result = Regex.Replace(withoutSpecialCharacters, @"\s+", "-");

            // Convert to lowercase
            result = result.ToLower();

            return result;
        }
    }
}
