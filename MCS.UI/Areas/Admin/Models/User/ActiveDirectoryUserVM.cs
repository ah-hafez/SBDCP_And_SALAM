namespace MCS.UI.Areas.Admin.Models.User
{
    public class ActiveDirectoryUserVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
    }
}