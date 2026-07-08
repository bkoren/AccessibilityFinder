using WebApp.Models.Admin.SendModels;

namespace WebApp.Models.Admin
{
    public class AdminVM
    {
        public List<ActivityAdminVM> ActivityAdminVMs { get; set; } = new List<ActivityAdminVM>();
        public List<ReviewAdminVM> ReviewAdminVMs { get; set; } = new List<ReviewAdminVM>();
        public List<LogsAdminVM > LogsAdminVMs { get; set; } = new List<LogsAdminVM>();
        public List<UsersAdminVM> UsersAdminVMs { get; set; } = new List<UsersAdminVM>();
        public List<TypeAdminVM> TypeAdminVMs { get; set; } = new List<TypeAdminVM>();
        public List<AccessibilityAdminVM> AccessibilityAdminVMs { get; set; } = new List<AccessibilityAdminVM>();

        public CreateActivityVM Activity { get; set; } = null!;
        public CreateTypeVM Type { get; set; } = null!;
        public CreateAccessibilityVM Accessibility { get; set; } = null!;

        public UpdateActivityVM Update { get; set; } = null!;
    }
}
