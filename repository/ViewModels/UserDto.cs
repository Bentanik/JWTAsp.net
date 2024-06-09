namespace Repository.ViewModels
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password{ get; set; }

        public string RefreshToken { get; set; }

        public RoleCodeDto RoleCode { get; set; }
    }
}
