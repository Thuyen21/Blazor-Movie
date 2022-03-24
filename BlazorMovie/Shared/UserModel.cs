namespace BlazorMovie.Shared
{
    public class UserModel
    {
        public Guid? Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Role { get; set; }
        public double? Wallet { get; set; }
        public string? UserAgent { get; set; }
    }
}
