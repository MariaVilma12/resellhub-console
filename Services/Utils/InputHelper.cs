namespace ResellHubConsole.Utils;

public static class InputHelper
{
    public static string ReadRequiredString(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine("Input cannot be empty.");
        }
    }

    public static double ReadPositiveDouble(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (double.TryParse(input, out double value) && value > 0)
                return value;

            Console.WriteLine("Please enter a valid positive number.");
        }
    }

    public static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (int.TryParse(input, out int value))
                return value;

            Console.WriteLine("Please enter a valid number.");
        }
    }
    
    public static T ReadEnum<T>(string message) where T : struct, Enum
    {
        while (true)
        {
            Console.WriteLine($"\n{message}");

            var values = Enum.GetValues<T>().ToList();

            for (int i = 0; i < values.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {values[i]}");
            }

            Console.Write("Select option: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice) &&
                choice >= 1 &&
                choice <= values.Count)
            {
                return values[choice - 1];
            }

            Console.WriteLine("Invalid selection. Try again.");
        }
    }
}