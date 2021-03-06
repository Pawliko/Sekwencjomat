﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Vlc.DotNet.Wpf;

namespace Sekwencjomat.Controls
{
    public partial class SettingsControl : UserControl
    {
        #region Metody Użytkownika
        private void SetHelperPlaybackScale(RadioButton rb)
        {
            if (rb.Name == "RadioButton_ACR")
            {
                Helper.CurrentPlaybackScale = Helper.PlaybackScaleEnum.ACR;
            }
            else if (rb.Name == "RadioButton_CCR")
            {
                Helper.CurrentPlaybackScale = Helper.PlaybackScaleEnum.CCR;
            }
            else if (rb.Name == "RadioButton_DCR")
            {
                Helper.CurrentPlaybackScale = Helper.PlaybackScaleEnum.DCR;
            }
            else if (rb.Name == "RadioButton_DCRmod")
            {
                Helper.CurrentPlaybackScale = Helper.PlaybackScaleEnum.DCRmod;
            }
        }

        public void SetHelperPlaybackMode(ComboBoxItem cbi)
        {
            if (cbi.Content.ToString().Contains("Mal"))
            {
                Helper.CurrentPlaybackMode = Helper.PlaybackModeEnum.Descending;
            }
            else if (cbi.Content.ToString().Contains("Ros"))
            {
                Helper.CurrentPlaybackMode = Helper.PlaybackModeEnum.Ascending;
            }
            else if (cbi.Content.ToString().Contains("Los"))
            {
                Helper.CurrentPlaybackMode = Helper.PlaybackModeEnum.Random;
            }
            else if (cbi.Content.ToString().Contains("Wyp"))
            {
                Helper.CurrentPlaybackMode = Helper.PlaybackModeEnum.Convex;
            }
            else if (cbi.Content.ToString().Contains("Wkl"))
            {
                Helper.CurrentPlaybackMode = Helper.PlaybackModeEnum.Concave;
            }
        }

        public void HelperPlaybackPropetiesToControls()
        {
            switch (Helper.CurrentPlaybackScale)
            {
                case Helper.PlaybackScaleEnum.ACR:
                    switch (Helper.CurrentPlaybackMode)
                    {
                        case Helper.PlaybackModeEnum.Descending:
                            ComboBoxItem_ACR_Descending.IsSelected = true;
                            break;
                        case Helper.PlaybackModeEnum.Ascending:
                            ComboBoxItem_ACR_Ascending.IsSelected = true;
                            break;
                        case Helper.PlaybackModeEnum.Concave:
                            ComboBoxItem_ACR_Concave.IsSelected = true;
                            break;
                        case Helper.PlaybackModeEnum.Convex:
                            ComboBoxItem_ACR_Convex.IsSelected = true;
                            break;
                        case Helper.PlaybackModeEnum.Random:
                            ComboBoxItem_ACR_Random.IsSelected = true;
                            break;
                    }
                    RadioButton_ACR.IsChecked = true;
                    break;

                case Helper.PlaybackScaleEnum.DCR:
                    switch (Helper.CurrentPlaybackMode)
                    {
                        case Helper.PlaybackModeEnum.Descending:
                            ComboBoxItem_DCR_Descending.IsSelected = true;
                            break;
                        case Helper.PlaybackModeEnum.Ascending:
                            ComboBoxItem_DCR_Ascending.IsSelected = true;
                            break;
                        case Helper.PlaybackModeEnum.Concave:
                            ComboBoxItem_DCR_Concave.IsSelected = true;
                            break;
                        case Helper.PlaybackModeEnum.Convex:
                            ComboBoxItem_DCR_Convex.IsSelected = true;
                            break;
                        case Helper.PlaybackModeEnum.Random:
                            ComboBoxItem_DCR_Random.IsSelected = true;
                            break;
                    }
                    RadioButton_DCR.IsChecked = true;
                    break;
            }
        }

        public bool CheckVLCFolderDLLs(string path)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }

            try
            {
                Dispatcher.Invoke(() =>
                {
                    new VlcControl().SourceProvider.CreatePlayer(new DirectoryInfo(path));
                    Image_VLCPathStatus.Source = new BitmapImage(new Uri(@"/Sekwencjomat;component/Resources/UI/Checkmark_16x16.png", UriKind.Relative));
                    TextBox_VLCPath.Text = path;
                });
                return true;
            }
            catch { }

            return false;
        }
        #endregion



        public SettingsControl()
        {
            InitializeComponent();
        }



        #region Metody Kontrolek
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HelperPlaybackPropetiesToControls();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!CheckVLCFolderDLLs(dialog.SelectedPath))
                {
                    MessageBox.Show($"Niewłaściwy folder plików programu VLC");
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                SetHelperPlaybackScale(sender as RadioButton);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((ComboBoxItem)((ComboBox)sender).SelectedItem != null)
            {
                SetHelperPlaybackMode((ComboBoxItem)((ComboBox)sender).SelectedItem);
            }
        }

        private void ComboBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((ComboBoxItem)((ComboBox)sender).SelectedItem != null)
            {
                SetHelperPlaybackMode((ComboBoxItem)((ComboBox)sender).SelectedItem);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, "[^0-9.-]+"))
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}