public class RegistrationDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsArtist { get; set; }
    public string ProfileImage { get; set; } = "https://picsum.photos/seed/default/300/300"; // Default image

    public string Address { get; set; } // Optional
}
