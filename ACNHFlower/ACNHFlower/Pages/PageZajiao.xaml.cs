using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ACNHFlower.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PageZajiao : Page
    {
        public PageZajiao()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GlobalTool.ChangeZajiaoCheckBox(sender);

            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked == false) return;
            string senderName = cb.Name;
            switch (senderName)
            {
                case "CheckBoxColorL":
                    CheckBoxGeneL.IsChecked = false;
                    CheckBoxSeedL.IsChecked = false;
                    ComboBoxA1L.SelectedIndex = 0;
                    ComboBoxA2L.SelectedIndex = 0;
                    ComboBoxA3L.SelectedIndex = 0;
                    ComboBoxA4L.SelectedIndex = 0;
                    ComboBoxSeedL.SelectedIndex = 0;
                    break;
                case "CheckBoxColorR":
                    CheckBoxGeneR.IsChecked = false;
                    CheckBoxSeedR.IsChecked = false;
                    ComboBoxA1R.SelectedIndex = 0;
                    ComboBoxA2R.SelectedIndex = 0;
                    ComboBoxA3R.SelectedIndex = 0;
                    ComboBoxA4R.SelectedIndex = 0;
                    ComboBoxSeedR.SelectedIndex = 0;
                    break;
                case "CheckBoxGeneL":
                    CheckBoxColorL.IsChecked = false;
                    CheckBoxSeedL.IsChecked = false;
                    ComboBoxColorL.SelectedIndex = 0;
                    ComboBoxSeedL.SelectedIndex = 0;
                    break;
                case "CheckBoxGeneR":
                    CheckBoxColorR.IsChecked = false;
                    CheckBoxSeedR.IsChecked = false;
                    ComboBoxColorR.SelectedIndex = 0;
                    ComboBoxSeedR.SelectedIndex = 0;
                    break;
                case "CheckBoxSeedL":
                    CheckBoxGeneL.IsChecked = false;
                    CheckBoxColorL.IsChecked = false;
                    ComboBoxColorL.SelectedIndex = 0;
                    ComboBoxA1L.SelectedIndex = 0;
                    ComboBoxA2L.SelectedIndex = 0;
                    ComboBoxA3L.SelectedIndex = 0;
                    ComboBoxA4L.SelectedIndex = 0;
                    break;
                case "CheckBoxSeedR":
                    CheckBoxGeneR.IsChecked = false;
                    CheckBoxColorR.IsChecked = false;
                    ComboBoxColorR.SelectedIndex = 0;
                    ComboBoxA1R.SelectedIndex = 0;
                    ComboBoxA2R.SelectedIndex = 0;
                    ComboBoxA3R.SelectedIndex = 0;
                    ComboBoxA4R.SelectedIndex = 0;
                    break;
            }
        }
    }
}
