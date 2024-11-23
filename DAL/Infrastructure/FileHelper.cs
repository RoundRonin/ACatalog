namespace DAL.Infrastructure;

public static class FileHelper
{
    public static async Task WriteLinesAsync(string filePath, IEnumerable<string> lines, bool append = true)
    {
        // Ensure the file exists
        EnsureFileExists(filePath);

        string tempFilePath = filePath + ".tmp";

        // Write changes to a temporary file
        await File.WriteAllLinesAsync(tempFilePath, lines);

        // Apply a lock to prevent concurrent writes
        lock (filePath)
        {
            // Merge changes
            var existingLines = File.Exists(filePath) ? File.ReadAllLines(filePath).ToList() : new List<string>();
            if (!append)
            {
                existingLines.Clear();
            }
            existingLines.AddRange(lines);

            // Write merged content back to the original file
            File.WriteAllLines(filePath, existingLines);
        }

        // Clean up temporary file
        File.Delete(tempFilePath);
    }

    public static async Task<IEnumerable<string>> ReadLinesAsync(string filePath)
    {
        // Ensure the file exists
        EnsureFileExists(filePath);

        if (File.Exists(filePath))
        {
            return await File.ReadAllLinesAsync(filePath);
        }

        return Enumerable.Empty<string>();
    }

    private static void EnsureFileExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            // Create an empty file
            using (File.Create(filePath)) { }
        }
    }
}
