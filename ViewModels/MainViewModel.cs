using CommunityToolkit.Mvvm.ComponentModel;

namespace capy_desktop_pet.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}
