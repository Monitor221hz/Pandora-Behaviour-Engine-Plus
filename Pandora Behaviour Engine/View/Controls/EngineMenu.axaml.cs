using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Pandora.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace Pandora.Views;

public partial class EngineMenu : ReactiveUserControl<EngineViewModel>
{
    public EngineMenu()
    {
        InitializeComponent();

		this.WhenActivated(disposables =>
        {
            this.BindInteraction(
                ViewModel,
                vm => vm.ShowAboutDialog,
                async context =>
                {
                    var dialogViewModel = context.Input;
                    var aboutDialog = new TaskDialog
                    {
                        DataContext = dialogViewModel,
                        Header = AboutDialogViewModel.Header,
                        SubHeader = AboutDialogViewModel.SubHeader,
                        Content = AboutDialogViewModel.Content,
                        FooterVisibility = TaskDialogFooterVisibility.Never,
                        IsFooterExpanded = false,
                        IconSource = (IconSource)Application.Current.FindResource("IconPandora"),
                        ShowProgressBar = false
                    };
                    aboutDialog.Commands.Add(new TaskDialogCommand
                    {
                        Text = "Check Update",
                        Description = "Check for a new version",
                        IsEnabled = false,
                        ClosesOnInvoked = false,
                        IconSource = new SymbolIconSource { Symbol = Symbol.Refresh }
                    });
                    aboutDialog.Commands.Add(new TaskDialogCommand
                    {
                        Text = "Github",
                        Description = "Visit GitHub page",
                        IsEnabled = true,
                        Command = ViewModel!.OpenUrlCommand,
                        CommandParameter = "https://github.com/Monitor221hz/Pandora-Behaviour-Engine-Plus",
                        ClosesOnInvoked = false,
                        IconSource = (IconSource)Application.Current.FindResource("IconGithub")
                    });
                    aboutDialog.Commands.Add(new TaskDialogCommand
                    {
                        Text = "Discord",
                        Description = "Join the Discord group",
                        IsEnabled = true,
                        Command = ViewModel!.OpenUrlCommand,
                        CommandParameter = "https://discord.gg/8nUQCWMn3w",
                        ClosesOnInvoked = false,
                        IconSource = (IconSource)Application.Current.FindResource("IconDiscord")
                    });

                    aboutDialog.Buttons.Add(new TaskDialogButton("Close", TaskDialogStandardResult.Close));

                    aboutDialog.XamlRoot = (Visual)VisualRoot;

                    await aboutDialog.ShowAsync(true);
                    context.SetOutput(Unit.Default);
                });
        });
    }
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (VisualRoot is AppWindow aw)
        {
            TitleBarHost.ColumnDefinitions[4].Width = new GridLength(aw.TitleBar.RightInset, GridUnitType.Pixel);
        }
    }
}
