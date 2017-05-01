using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Bing_Wallpaper
{
    public partial class MainWindow : Window
    {
        private BingWallpapWallpaperManager manager;


        public MainWindow()
        {
            InitializeComponent();
            manager = new BingWallpapWallpaperManager(this);
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow,
                this.OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow,
                this.OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow,
                this.OnCanResizeWindow));
            this.KeyDown += new KeyEventHandler(OnKeyDown);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                manager.PreviousWallpaper();
            }

            if (e.Key == Key.Right)
            {
                manager.NextWallpaper();
            }
        }

        private void Forwards_Button_Click(object sender, RoutedEventArgs e)
        {
            manager.NextWallpaper();
        }

        private void Backwards_Button_Click(object sender, RoutedEventArgs e)
        {
            manager.PreviousWallpaper();
        }

        private void Change_Wallpaper_Button_Click(object sender, RoutedEventArgs e)
        {
            Change_Wallpaper.ChangeWallpaper.SetBackground(manager.fullPath);
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
}