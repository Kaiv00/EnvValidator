# EnvValidator 🛡️

A lightweight C# utility designed to parse, validate, and check configuration/environment files for required keys and syntax errors.

## Features
- **Robust Parsing:** Strips comments, parses key-value pairs, and detects duplicate keys.
- **Fault-Tolerant:** Safely reports malformed lines or missing files without crashing.
- **Custom Rules:** Enforces required application configuration keys.

## Getting Started
- **Build:** `dotnet build`
- **Run:** `dotnet run --project src/EnvValidator/EnvValidator.csproj`
- **Test:** `dotnet test`