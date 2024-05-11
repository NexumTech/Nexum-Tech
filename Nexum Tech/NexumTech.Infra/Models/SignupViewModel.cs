namespace NexumTech.Infra.Models
{
    public class SignupViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhotoURL {  get; set; }
        public byte[]? Photo {  get; set; }
    }
}
