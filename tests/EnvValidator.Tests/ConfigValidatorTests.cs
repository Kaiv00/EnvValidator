using System.IO;
using Xunit;
using EnvValidator;

namespace EnvValidator.Tests;

public class ConfigValidatorTests
{
    [Fact]
    public void ValidateFile_ValidConfig_ReturnsValidResult()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "PORT=8080\nAPI_KEY=secret123\nDATABASE_URL=sql://localhost");

        var result = ConfigValidator.ValidateFile(tempFile, new[] { "PORT", "API_KEY" });

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.Equal("8080", result.Config["PORT"]);

        File.Delete(tempFile);
    }

    [Fact]
    public void ValidateFile_MissingRequiredKey_ReportsError()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "PORT=8080");

        var result = ConfigValidator.ValidateFile(tempFile, new[] { "PORT", "MISSING_KEY" });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("MISSING_KEY"));

        File.Delete(tempFile);
    }

    [Fact]
    public void ValidateFile_MalformedLine_CapturesErrorWithoutCrashing()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "INVALID_LINE_WITHOUT_EQUALS_SIGN");

        var result = ConfigValidator.ValidateFile(tempFile, new[] { "PORT" });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Malformed line"));

        File.Delete(tempFile);
    }

    [Fact]
    public void ValidateFile_NonExistentFile_ReturnsError()
    {
        var result = ConfigValidator.ValidateFile("non_existent_env.env", new[] { "PORT" });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("not found"));
    }
}