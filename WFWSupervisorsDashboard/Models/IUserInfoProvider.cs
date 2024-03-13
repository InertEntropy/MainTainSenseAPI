using System; // Or any other necessary namespaces

namespace WFWSupervisorsDashboard.Models // Replace with your project's namespace
{
    public interface IUserInfoProvider
    {
        int UserID { get; }
        string DisplayName { get; }
        string Role { get; }

        void LoadUserInfo(int userId);
    }
}