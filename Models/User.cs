namespace ResellHubConsole.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = "";

    // Password is not required in memory for most operations,
    // but we keep it here for simple login systems.
    public string Password { get; set; } = "";

    // Parameterless constructor (important for database mapping)
    public User()
    {
    }

    // Optional constructor for manual creation
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public bool CheckPassword(string password)
    {
        return Password == password;
    }
}