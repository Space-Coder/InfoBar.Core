# InfoBar.Core - Simple info bar in Windows 11 style
[![NuGet version](https://badge.fury.io/nu/InfoBar.svg)](https://www.nuget.org/packages/InfoBar.Core/)
____
## How to use
- Install [NuGet](https://www.nuget.org/packages/InfoBar.Core/) package in your WPF Application.
- Add this code in **MainWindow.xaml**.

```XML
xmlns:InfoBar="clr-namespace:InfoBar;assembly=InfoBar"
```
- Add resource dictionary in **App.xaml**.

``` XML
<Application
    x:Class="InfoBar.Demo.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="clr-namespace:InfoBar.Demo"
    StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/InfoBar;component/Resources/Theme.Light.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

- Create InfoBar element and set x:Name property.

```XML
<InfoBar:InfoBarBox
            x:Name="InfoBarBox"
            Margin="0,-79,0,0"
            VerticalAlignment="Top" />
```

- For showing message call **ShowMessage()** method from InfoBar element.

```C#
 public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InfoBarBox.ShowMessage("Your message", InfoBarStatus.CRITICAL, InfoBarPosition.TOP);
        }
    }
}
```

## Example screenshots
![Image alt](https://github.com/Space-Coder/InfoBar/raw/master/picture1.png)
![Image alt](https://github.com/Space-Coder/InfoBar/raw/master/picture2.png)
![Image alt](https://github.com/Space-Coder/InfoBar/raw/master/picture3.png)
![Image alt](https://github.com/Space-Coder/InfoBar/raw/master/picture4.png)
