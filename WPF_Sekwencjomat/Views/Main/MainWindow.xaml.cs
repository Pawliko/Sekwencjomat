﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace WPF_Sekwencjomat
{

    public partial class MainWindow : Window
    {
        public FilesControl FilesControlObject;
        public PlayerControl PlayerControlObject;
        public SettingsControl SettingsControlObject;

        #region Metody Użytkownika
        private void MakeButtonPressedOnLeft(object sender)
        {
            if (!(sender is Button)) { return; }
            else
            {
                foreach (object item in StackPanel_ButtonsLeft.Children)
                {
                    if (item is Button)
                    {
                        ((Button)item).Background = new SolidColorBrush(Color.FromArgb(0,0,0,0));
                        ((Button)item).BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    }
                }
                Button btn = sender as Button;
                btn.Background = Helper.GetBrushFromHex("#FFE1F4FF");
                btn.BorderBrush = Helper.GetBrushFromHex("#FFBEE6FD");
            }
        }

        public void ChangeNavigationVisibility(bool makeVisible)
        {
            bool condition = 
                (StackPanel_ButtonsLeft.Visibility == Visibility.Visible) 
                && 
                (Border_BottomStatusBar.Visibility == Visibility.Visible);

            if (condition && !makeVisible)
            {
                SV_MainDisplay.Content = PlayerControlObject;
                MakeButtonPressedOnLeft(Button_PlayerObjectNavigator);
                Background = new SolidColorBrush(Colors.Black);
                StackPanel_ButtonsLeft.Visibility = Visibility.Collapsed;
                Border_BottomStatusBar.Visibility = Visibility.Collapsed;
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
            else
            {
                Background = new SolidColorBrush(Colors.White);
                StackPanel_ButtonsLeft.Visibility = Visibility.Visible;
                Border_BottomStatusBar.Visibility = Visibility.Visible;
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        public async void LoadUserSettings()
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                return;

            try
            {
                IsEnabled = false;

                Properties.Settings settings = Properties.Settings.Default;

                SettingsControlObject.CheckVLCFolderDLLs(settings.VLC_DLL_PATH);

                if (settings.WINDOW_LOCATION.Top < SystemParameters.WorkArea.Height && settings.WINDOW_LOCATION.Left < SystemParameters.WorkArea.Width)
                {
                    Top = settings.WINDOW_LOCATION.Top;
                    Left = settings.WINDOW_LOCATION.Left;
                    Width = settings.WINDOW_LOCATION.Width;
                    Height = settings.WINDOW_LOCATION.Height;
                }

                if (settings.WINDOWS_MAXIMIZED) { WindowState = WindowState.Maximized; }

                Helper.SetPlaybackOrder(settings.SETTINGS_ORDER_TAG);

                if (File.Exists(settings.REFVIDEO_PATH))
                {
                    FilesControlObject.TextBox_RefPath.Text = settings.REFVIDEO_PATH;
                }

                if (File.Exists(settings.COUNTEREVIDEO_PATH))
                {
                    FilesControlObject.TextBox_PauserPath.Text = settings.COUNTEREVIDEO_PATH;
                }

                await FilesControlObject.FileDataToGrid(settings.LIST_OF_FILES.ToArray());
            }
            catch { }
            finally
            {
                IsEnabled = true;
            }
        }

        public void SaveUserSettings()
        {
            try
            {
                Properties.Settings s = Properties.Settings.Default;

                if (WindowState == WindowState.Maximized)
                    s.WINDOWS_MAXIMIZED = true;
                else 
                    s.WINDOWS_MAXIMIZED = false;

                s.VLC_DLL_PATH = SettingsControlObject.TBX_VLCPath.Text;
                s.WINDOW_LOCATION = new Rect(Left, Top, Width, Height);
                s.SETTINGS_ORDER_TAG = Helper.GetPlaybackOrder();
                s.REFVIDEO_PATH = FilesControlObject.TextBox_RefPath.Text;
                s.COUNTEREVIDEO_PATH = FilesControlObject.TextBox_PauserPath.Text;
                s.LIST_OF_FILES = FilesControlObject.GetCurrentFilesInDataGrid();
                s.Save();
            }
            catch { }
        }

        #endregion



        public MainWindow()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;

            FilesControlObject = new FilesControl();
            SettingsControlObject = new SettingsControl();
            PlayerControlObject = new PlayerControl();

            SV_MainDisplay.Content = FilesControlObject;
            MakeButtonPressedOnLeft(Button_FileControl);
            //LoadUserSettings();

            new SearchingVLCDLLsWindow().ShowDialog();

            Visibility = Visibility.Visible;
            Topmost = true;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MakeButtonPressedOnLeft(sender);
            SV_MainDisplay.Content = FilesControlObject;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MakeButtonPressedOnLeft(sender);
            SV_MainDisplay.Content = SettingsControlObject;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MakeButtonPressedOnLeft(sender);
            SV_MainDisplay.Content = PlayerControlObject;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveUserSettings();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F12:
                    ChangeNavigationVisibility(false);
                    break;

                case Key.Escape:
                    ChangeNavigationVisibility(true);
                    break;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
