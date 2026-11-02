using System;
using System.Collections.Generic;
using System.IO;

namespace EnvValidator;

public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; set; } = new();
    public Dictionary<string, string> Config { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public class ConfigValidator
{
    public static ValidationResult ValidateFile(string filePath, string[] requiredKeys)
    {
        var result = new ValidationResult();

        if (!File.Exists(filePath))
        {
            result.Errors.Add($"Configuration file not found: {filePath}");
            return result;
        }

        var rawConfig = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var line in File.ReadLines(filePath))
        {
            var trimmed = line.Trim();

            // Ignore comments and empty lines safely
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                continue;

            var equalIndex = trimmed.IndexOf('=');
            if (equalIndex <= 0)
            {
                result.Errors.Add($"Malformed line (missing '='): '{line}'");
                continue;
            }

            var key = trimmed.Substring(0, equalIndex).Trim();
            var value = trimmed.Substring(equalIndex + 1).Trim();

            if (rawConfig.ContainsKey(key))
            {
                result.Errors.Add($"Duplicate configuration key found: '{key}'");
            }
            else
            {
                rawConfig[key] = value;
            }
        }

        result.Config = rawConfig;

        // Verify required keys exist and aren't empty
        foreach (var reqKey in requiredKeys)
        {
            if (!rawConfig.ContainsKey(reqKey) || string.IsNullOrWhiteSpace(rawConfig[reqKey]))
            {
                result.Errors.Add($"Missing required configuration key: '{reqKey}'");
            }
        }

        return result;
    }
}