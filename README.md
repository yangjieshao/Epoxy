# Epoxy - A minimum MVVM assister library. 

![Epoxy bin](Images/Epoxy.160.png)

[![Project Status: WIP – Initial development is in progress, usable release suitable for the public.](https://www.repostatus.org/badges/latest/wip.svg)](https://www.repostatus.org/#wip)

|Package|Status|
|:--|:--|
|Epoxy.Wpf|[![NuGet Epoxy.Wpf](https://img.shields.io/nuget/v/Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Wpf)|
|Epoxy.Uwp|[![NuGet Epoxy.Uwp](https://img.shields.io/nuget/v/Epoxy.Uwp.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Uwp)|
|Epoxy.Xamarin.Forms|[![NuGet Epoxy.Xamarin.Forms](https://img.shields.io/nuget/v/Epoxy.Xamarin.Forms.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Xamarin.Forms)|

## What is this ?

* Epoxy is .NET XAML Model-View-ViewModel data-bindable infrastructure library, and very simple usage API sets.
* Supported platforms:
  * WPF: .NET Core 3.0/3.1, .NET Framework 4.5/4.8
  * UWP: Universal Windows 10
  * Xamarin Forms: .NET Standard 2.0
* Safe asynchronous operation (async-await) ready.
* C# 8.0 nullable reference types ready.
* Easy understandable.
* Supported simplest and minimalism Model-View-ViewModel design.
* Smallest footprint.
* Friction-free for combination other MVVM frameworks such as ReactiveProperty and etc.

## Sample code

You can refer full WPF application sample code in [EpoxyHello.Wpf](samples/EpoxyHello.Wpf).

Full asynchronous fetching and updating into ListBox when you click a button.

![EpoxyHello.Wpf](https://github.com/kekyo/Epoxy/raw/master/Images/sample.png)

## Getting started minimum MVVM application

Completed separately xaml based view declarations (Suppressed, refer full sample code instead):

```xml
<Window
    x:Class="EpoxyHello.Wpf.Views.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    Title="EpoxyHello.Wpf" Height="450" Width="800">

    <!-- Place view model instance, it'll verify types (by IDE)  -->
    <Window.DataContext>
        <viewmodels:MainWindowViewModel />
    </Window.DataContext>
    
    <DockPanel>
        <!-- Binding button click event. -->
        <Button DockPanel.Dock="Top" Height="30"
                Command="{Binding Fetch}">Asynchronous fetch r/aww from Reddit!</Button>
        <Grid>
            <!-- Binding an image collection. -->
            <ListBox ItemsSource="{Binding Items}"
                ScrollViewer.HorizontalScrollBarVisibility="Disabled"
                ScrollViewer.VerticalScrollBarVisibility="Auto"
                ScrollViewer.CanContentScroll="False">
                <ListBox.ItemTemplate>
                    <DataTemplate>
                        <!-- Binding an image. -->
                        <Image
                            Source="{Binding Image}"
                            Stretch="UniformToFill" />
                    </DataTemplate>
                </ListBox.ItemTemplate>
            </ListBox>
        </Grid>
    </DockPanel>
</Window>
```

Completed separately ViewModel implementation.

```csharp
// Step 1: Write view model class deriving from Epoxy.ViewModel.
public sealed class MainWindowViewModel : ViewModel
{
    // Step 2: Expose properties to view.
    //    Epoxy can handle with C# 8.0's nullable reference types.
    public Command? Fetch
    {
        get => this.GetValue();
        private set => this.SetValue(value);
    }

    public ObservableCollection<ImageSource>? Items
    {
        // Step 2-1: Can suppress the GetValue() generic argument
        //   if use basic types (primitives, string, Command and etc).
        get => this.GetValue<ObservableCollection<ImageSource>?>();
        private set => this.SetValue(value);
    }

    public MainWindowViewModel()
    {
        // Step 3: Property setter will raise PropertyChanged events if value is changed.
        this.Items = new ObservableCollection<ItemViewModel>();

        // Step 4: A handler for fetch button.
        //   Ofcourse, we can use async/await safely in lambda expressions!
        this.Fetch = Command.Create(async () =>
        {
            var reddits = await Reddit.FetchNewPostsAsync("r/aww");

            this.Items.Clear();

            foreach (var reddit in reddits)
            {
                this.Items.Add(await Reddit.FetchImageAsync(reddit.Url));
            }
        });
    }
}
```

## Minor but unique useful features

### ChildrenBinder

TODO:

[For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L71)

[For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L119)

### Anchor/Pile

Anchor/Pile pair is a loose connection between UIElement (Xamarin Forms Element) and view models.

Rare case in MVVM architecture, we have to access directly UIElement member,
but sometimes gointg to wait the pitfall of circular references (and couldn't unbind by GC).

The Pile pull in the UIElement's anchor, and we can rent temporary UIElement reference safely inside view model.

[For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L39)

[For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L74)

## License

Apache-v2
