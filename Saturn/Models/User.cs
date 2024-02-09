namespace Saturn.Models;

public class User
{
    [Key]
    public ulong UserId { get; set; }
    public string UserName { get; set; }
    public string ProfileImageSource { get; set; }
}
