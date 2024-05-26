namespace NexumTech.Infra.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Base64Logo { get; set; }
        public byte[]? Logo { get; set; }
    }
}
