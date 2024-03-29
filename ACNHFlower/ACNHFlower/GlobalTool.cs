﻿using ACNHFlower.Dialogs;
using ACNHFlower.Helpers;
using ACNHFlower.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using static ACNHFlower.Models.MyFlower;

namespace ACNHFlower
{
    class GlobalTool
    {

        #region MainPage.xaml

        public static Frame FrameMain;
        public static ComboBox ComboBoxChoose;
        public static Button ButtonSearch;
        public static TeachingTip TipSearch;

        public static DialogHelper ProDialog = new DialogHelper();

        public static Windows.UI.Xaml.Controls.NavigationView NaviViewMain;
        public static Windows.UI.Xaml.Controls.NavigationViewItem NaviItemZajiao;
        public static Windows.UI.Xaml.Controls.NavigationViewItem NaviItemParent;

        public static List<MyFlower> FlowerAll;

        /// <summary>
        /// 设置了哪种花朵
        /// </summary>
        public static FlowerType SelectedFlower;
        /// <summary>
        /// 所设置花朵种类的所有花
        /// </summary>
        public static List<MyFlower> ListColorDic;

        public static List<string> ListColor;
        public static List<string> ListColorName;

        public static string AppVersion = string.Format("{0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Revision);

        public static int MainViewID = ApplicationView.GetForCurrentView().Id;

        /// <summary>
        /// 主Frame导航到页面
        /// </summary>
        /// <param name="page">typeof(Page)</param>
        /// <returns>是否成功</returns>
        public static bool FMNavigate(Type page)
        {
            try
            {
                FMGoBack();
                FrameMain.Navigate(page, null, new DrillInNavigationTransitionInfo());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 主FrameGoBack
        /// </summary>
        public static void FMGoBack()
        {
            if (FrameMain.CanGoBack)
            {
                do
                {
                    FrameMain.GoBack();
                } while (FrameMain.CanGoBack);
            }
        }

        /// <summary>
        /// 关闭App
        /// </summary>
        public static void CloseApp()
        {
            Windows.UI.Xaml.Application.Current.Exit();
        }

        /// <summary>
        /// 显示进度条
        /// </summary>
        public static void ShowProgress()
        {
            DialogHelper.CreateContentDialog(new ProgressDialog(), true);
        }

        /// <summary>
        /// 取消进度条
        /// </summary>
        public static void CloseProgress()
        {
            DialogHelper.ActiveDialog?.Hide();
        }

        #endregion

        #region ContentDialog

        /// <summary>
        /// 显示警告框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        public static async Task ShowDialog(string title, string msg)
        {
            ContentDialog noResultDialog = new ContentDialog
            {
                Title = title,
                Content = msg,
                CloseButtonText = "确定"
            };
            await noResultDialog.ShowAsync();
        }

        #endregion

        #region PageZajiao.xaml

        public static bool BoolColorL;
        public static bool BoolColorR;
        public static bool BoolGeneL;
        public static bool BoolGeneR;
        public static bool BoolSeedL;
        public static bool BoolSeedR;

        public static int IndexColorL;
        public static int IndexColorR;

        public static int IndexSeedL;
        public static int IndexSeedR;

        public static string ItemA1L = "Unknown";
        public static string ItemA2L = "Unknown";
        public static string ItemA3L = "Unknown";
        public static string ItemA4L = "Unknown";

        public static string ItemA1R = "Unknown";
        public static string ItemA2R = "Unknown";
        public static string ItemA3R = "Unknown";
        public static string ItemA4R = "Unknown";

        public static void ChangeZajiaoComboBox(object sender)
        {
            var s = sender as ComboBox;
            switch (s.Name)
            {
                case "ComboBoxColorL":
                    IndexColorL = s.SelectedIndex;
                    break;
                case "ComboBoxColorR":
                    IndexColorR = s.SelectedIndex;
                    break;
                case "ComboBoxA1L":
                    ItemA1L = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA2L":
                    ItemA2L = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA3L":
                    ItemA3L = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA4L":
                    ItemA4L = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA1R":
                    ItemA1R = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA2R":
                    ItemA2R = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA3R":
                    ItemA3R = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA4R":
                    ItemA4R = s.SelectedItem.ToString();
                    break;
                case "ComboBoxSeedL":
                    IndexSeedL = s.SelectedIndex;
                    break;
                case "ComboBoxSeedR":
                    IndexSeedR = s.SelectedIndex;
                    break;
            }
        }

        public static void ChangeZajiaoCheckBox(object sender)
        {
            var s = sender as CheckBox;
            switch (s.Name)
            {
                case "CheckBoxColorL":
                    BoolColorL = s.IsChecked.Value;
                    break;
                case "CheckBoxColorR":
                    BoolColorR = s.IsChecked.Value;
                    break;
                case "CheckBoxGeneL":
                    BoolGeneL = s.IsChecked.Value;
                    break;
                case "CheckBoxGeneR":
                    BoolGeneR = s.IsChecked.Value;
                    break;
                case "CheckBoxSeedL":
                    BoolSeedL = s.IsChecked.Value;
                    break;
                case "CheckBoxSeedR":
                    BoolSeedR = s.IsChecked.Value;
                    break;
            }
        }

        #endregion

        #region PageParent.xaml

        public static bool BoolColor;
        public static bool BoolGene;

        public static int IndexColor;

        public static string ItemA1 = "Unknown";
        public static string ItemA2 = "Unknown";
        public static string ItemA3 = "Unknown";
        public static string ItemA4 = "Unknown";

        public static void ChangeParentComboBox(object sender)
        {
            var s = sender as ComboBox;
            switch (s.Name)
            {
                case "ComboBoxColor":
                    IndexColor = s.SelectedIndex;
                    break;
                case "ComboBoxA1":
                    ItemA1 = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA2":
                    ItemA2 = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA3":
                    ItemA3 = s.SelectedItem.ToString();
                    break;
                case "ComboBoxA4":
                    ItemA4 = s.SelectedItem.ToString();
                    break;
            }
        }

        public static void ChangeParentCheckBox(object sender)
        {
            var s = sender as CheckBox;
            switch (s.Name)
            {
                case "CheckBoxColor":
                    BoolColor = s.IsChecked.Value;
                    break;
                case "CheckBoxGene":
                    BoolGene = s.IsChecked.Value;
                    break;
            }
        }

        #endregion

    }
}
