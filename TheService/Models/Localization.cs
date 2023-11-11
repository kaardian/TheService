using System.Text.Json;
using System;
using TheService.Properties;
using System.IO;

namespace TheService.Models
{
    internal static class Localization
    {
        internal static Messages ReadLocalization()
        {
            // Get the language.
            string language = Settings.Default.Localization;

            // Construct the path, and add file extension.
            string filePath = $@"C:\Git\TheService\TheService\bin\Debug\Localization\{language}.json";

            // Check that the file exists
            if (File.Exists(filePath))
            {
                // Read file.
                string jsonText = File.ReadAllText(filePath);

                // Convert into Messages object
                Messages localizedMessages = JsonSerializer.Deserialize<Messages>(jsonText);

                // Return the localized messages.
                return localizedMessages;
            }
            else
            {
                throw new Exception($"Failed to read the localization file, file '{filePath}' doesn't exist.");
            }
        }
    }
}
