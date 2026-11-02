using System;
using System.IO;

namespace EnvValidator;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== EnvValidator v1.0 ===");

        string filePath = args.Length > 0 ? args[0] : ".env";

        // Generate a sample config file just for quick demo execution
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, """
                # Server Settings
                PORT=5000
                DATABASE_URL=postgres://localhost:5432/appdb
                # API_KEY is intentionally missing to demonstrate validation error handling
                """);
            Console.WriteLine($"[Info] Created a sample configuration file at '{filePath}'.\n");
        }

        // This down here define keys required by the application
        string[] requiredKeys = { "PORT", "DATABASE_URL", "API_KEY" };

        var result = ConfigValidator.ValidateFile(filePath, requiredKeys);

        if (result.IsValid)
        {
            Console.WriteLine("✅ Configuration is valid and ready!");
        }
        else
        {
            Console.WriteLine("❌ Configuration validation failed with the following errors:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  - {error}");
            }
            Environment.ExitCode = 1;
        }
    }
}