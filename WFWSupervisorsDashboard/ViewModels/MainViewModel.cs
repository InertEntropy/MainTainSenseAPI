using System.Windows;
using WFWSupervisorsDashboard.Models;

public class MainViewModel
{
    private readonly IUserInfoProvider _userInfo;

    public MainViewModel(IUserInfoProvider userInfo, int userId)
    {
        _userInfo = userInfo;

    }

    public void DisplayUserInfo(int userId)
    {
        _userInfo.LoadUserInfo(userId); // Assuming you have the user ID
        MessageBox.Show($"Name: {_userInfo.DisplayName}");
    }
    public void LoadUserInfo(int userId)
    {
        _userInfo.LoadUserInfo(userId);
    }
}