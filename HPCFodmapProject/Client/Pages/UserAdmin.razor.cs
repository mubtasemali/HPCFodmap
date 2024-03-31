using HPCFodmapProject.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Navigations;
using HPCFodmapProject.Client.HttpRepository;
using Microsoft.AspNetCore.Components.Authorization;
using static System.Net.WebRequestMethods;


namespace HPCFodmapProject.Client.Pages;

public partial class UserAdmin
{
    [Inject]
    IUserHttpRepository UserRepo { get; set; }
    public List<UserEditDto> Users { get; set; } = new List<UserEditDto>();
    private bool IsUserModalVisible { get; set; } = false;
    private UserEditDto userEditDto = new UserEditDto();
    public SfToast ToastObj;
    private string? toastContent = string.Empty;
    private string? toastSuccess = "e-toast-success";
    //COMMENTED OUT state has changed methods because it was not recognizing the methods

    //removing override to fix error
    protected  async Task OnInitializedAsync()
    {
        await ReloadGrid();
    }

    public async Task ReloadGrid()
    {
        DataResponse<List<UserEditDto>> response = await UserRepo.GetAllUsersAsync();
        if (response.Succeeded)
        {
            Users = response.Data;
        }
        else
        {
            toastContent = "Error retrieving users";
            toastSuccess = "e-toast-danger";
            await ToastObj.ShowAsync();
        }
    }

    public async Task AddUserOnSubmit()
    {
        var response = await UserRepo.UpdateUser(userEditDto);
        if (response)
        {
            IsUserModalVisible = false;
            userEditDto = new UserEditDto();
            await ReloadGrid();
            toastContent = "User updated successfully";
            toastSuccess = "e-toast-success";
          //  StateHasChanged();
            await ToastObj.ShowAsync();
        }
        else
        {
            toastContent = "Error updating user";
            toastSuccess = "e-toast-danger";
            await ToastObj.ShowAsync();
        }
    }

    public async Task Reset()
    {
        try
        {
            userEditDto = new UserEditDto();
            IsUserModalVisible = false;
        }
        catch
        {
            // add error handling
        }
    }

    public async Task UserRowSelectedHandler(RowSelectEventArgs<UserEditDto> args)
    {
        try
        {
            userEditDto = args.Data;
        }
        catch
        {
            toastContent = "Error accessing user data in grid";
            toastSuccess = "e-toast-danger";
            await ToastObj.ShowAsync();
        }
    }

    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "GridUserDelete")
        {
            if (userEditDto.Id is not null)
            {

                var res = await UserRepo.DeleteUser(userEditDto.Id);
                if (res)
                {
                    await ReloadGrid();
                    toastContent = $"{userEditDto.Email} removed!";
                   // StateHasChanged();
                    await ToastObj.ShowAsync();
                }
                else
                {
                    toastContent = $"Failed to delete user {userEditDto.Email}";
                    toastSuccess = "e-toast-danger";
                    //StateHasChanged();
                    await ToastObj.ShowAsync();
                }

            }
            else
            {
                toastContent = $"Please select a user";
                toastSuccess = "e-toast-warning";
                //StateHasChanged();
                await ToastObj.ShowAsync();
            }
        }
    }

    public async Task UserDoubleClickHandler(RecordDoubleClickEventArgs<UserEditDto> args)
    {
        try
        {
            userEditDto = args.RowData;
            IsUserModalVisible = true;
        }
        catch
        {
            toastContent = "Error accessing user data in grid";
            toastSuccess = "e-toast-danger";
            await ToastObj.ShowAsync();
        }
    }

    public async Task ToggleAdminUserEventHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args, string UserId)
    {
        bool res = await UserRepo.ToggleAdminUser(UserId);
        if (!res)
        {
            toastContent = "Error update user admin";
            toastSuccess = "e-toast-danger";
            await ToastObj.ShowAsync();

        }
    }

    public async Task ToggleEmailConfirmedUserEventHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args, string UserId)
    {
        bool res = await UserRepo.ToggleEmailConfirmedUser(UserId);
        if (!res)
        {
            toastContent = "Error update user email confirmed";
            toastSuccess = "e-toast-danger";
            await ToastObj.ShowAsync();
        }
    }
}