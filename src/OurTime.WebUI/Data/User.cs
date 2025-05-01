namespace OurTime.WebUI.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;  // OBS: i produktion hash
    }
}

