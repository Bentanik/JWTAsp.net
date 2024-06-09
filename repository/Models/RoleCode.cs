namespace Repository.Models
{
    public class RoleCode
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
