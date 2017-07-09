using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Bing_Wallpaper.Pages.Time_Line_Page;

namespace Bing_Wallpaper
{
    

    public partial class PageSwitcher : Window
    {
        private readonly Home_Page _mainPage;


        public PageSwitcher()
        {
            InitializeComponent();
            BingWallpaperManager man = new BingWallpaperManager();
            _mainPage = new Home_Page(man);
            var timeLinePage = new Time_Line_Page(man);
            var settingsPage = new Settings_Page();
            

            Switcher.PageSwitcher = this;
            Switcher.Switch(_mainPage);

            
            HomeButton.SelectionCommand =  new SwitchCommand(_ => Switcher.Switch(_mainPage));
            TimeLineButton.SelectionCommand = new SwitchCommand(_ => Switcher.Switch(timeLinePage));
            SettingsButton.SelectionCommand = new SwitchCommand(_ => Switcher.Switch(settingsPage));

            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow,
                this.OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow,
                this.OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow,
                this.OnCanResizeWindow));
            this.KeyDown += OnKeyDown;
        }

        public void Navigate(UserControl nextPage)
        {
            this.ContentControl.Content = nextPage;
        }

        public void Navigate(UserControl nextPage, object state)
        {
            this.Content = nextPage;
            ISwitchable s = nextPage as ISwitchable;

            if (s != null)
                s.UtilizeState(state);
            else
                throw new ArgumentException("NextPage is not ISwitchable! "
                                            + nextPage.Name.ToString());
        }

                private void OnKeyDown(object sender, KeyEventArgs e)
                {
                    if (e.Key == Key.Left)
                    {
                        _mainPage.manager.PreviousWallpaper();
                    }
        
                    if (e.Key == Key.Right)
                    {
                        _mainPage.manager.NextWallpaper();
                    }
                }

        public void SwitchHomepage()
        {
            Switcher.Switch(_mainPage);
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
    
    public class SwitchCommand : ICommand
    {
        private readonly Action<object> _action;
        public SwitchCommand(Action<object> action)
        {
            this._action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

    public static class Switcher
    {
        public static PageSwitcher PageSwitcher;
        public static void Switch(UserControl newPage)
        {
            PageSwitcher.Navigate(newPage);
        }

        public static void Switch(UserControl newPage, object state)
        {
            PageSwitcher.Navigate(newPage, state);
        }
    }

    public interface ISwitchable
    {
        void UtilizeState(object state);
    }
}
