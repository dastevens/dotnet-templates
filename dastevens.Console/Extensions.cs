namespace dastevens.Console
{
    using System.CommandLine;

    internal static class Extensions
    {
        public static void WriteJson<T>(this IConsole console, T item)
        {
            console.Write(
                System.Text.Json.JsonSerializer.Serialize(
                    item,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true,
                    }));
        }
    }
}