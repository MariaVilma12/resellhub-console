using ResellHubConsole.Data;
using ResellHubConsole.Services.Users;
using ResellHubConsole.Models;
using ResellHubConsole.Services.ItemList;
using ResellHubConsole.Services.Menus;

class Program
{
    static void Main()
    {
        DatabaseInitializer.Initialize();

        var registerService = new RegisterService();
        var loginService = new LoginService();

        while (true)
        {
            Console.WriteLine("\n=== RESSELL HUB ===");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        RegisterUser(registerService);
                        break;

                    case "2":
                        LoginUser(loginService);
                        break;

                    case "3":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    static void RegisterUser(RegisterService registerService)
    {
        Console.WriteLine("\n=== USER REGISTRATION ===");

        Console.Write("Username: ");
        string username = Console.ReadLine()!;

        Console.Write("Password: ");
        string password = Console.ReadLine()!;

        registerService.Register(username, password);

        Console.WriteLine("Registration successful!");
    }

    static void LoginUser(LoginService loginService)
    {
        Console.WriteLine("\n=== LOGIN ===");

        Console.Write("Username: ");
        string username = Console.ReadLine()!;

        Console.Write("Password: ");
        string password = Console.ReadLine()!;

        User? user = loginService.Login(username, password);

        if (user != null)
        {
            Console.WriteLine($"Welcome back, {user.Username}!");

            var menu = new MainMenu();
            menu.Show(user);
        }
        else
        {
            Console.WriteLine("Invalid credentials.");
        }
    }
}