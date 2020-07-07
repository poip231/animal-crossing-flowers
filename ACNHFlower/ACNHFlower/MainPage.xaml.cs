using ACNHFlower.Dialogs;
using ACNHFlower.Helpers;
using ACNHFlower.Models;
using ACNHFlower.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static ACNHFlower.Models.MyFlower;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ACNHFlower
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
            SetTitleBar();
            BindGlobal();

            BindData();
            BindFlower();

            FrameMain.Navigate(typeof(PageStartup));
        }

        /// <summary>
        /// 设置自定义标题栏
        /// </summary>
        private void SetTitleBar()
        {
            //设置窗口最小值
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Windows.Foundation.Size(800, 800));

            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

            coreTitleBar.ExtendViewIntoTitleBar = true;
            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
            appTitleBar.ButtonForegroundColor = Colors.Black;
            //按钮非活动
            appTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            appTitleBar.ButtonInactiveForegroundColor = Colors.Black;

            Window.Current.SetTitleBar(RectTitle);
        }

        /// <summary>
        /// 绑定全局
        /// </summary>
        private void BindGlobal()
        {
            GlobalTool.FrameMain = FrameMain;
            GlobalTool.ComboBoxChoose = ComboBoxChoose;
            GlobalTool.ButtonSearch = ButtonSearch;
            GlobalTool.TipSearch = TeachingTipSearch;

            GlobalTool.NaviViewMain = NaviViewMain;
            GlobalTool.NaviItemZajiao = NaviViewItemZajiao;
            GlobalTool.NaviItemParent = NaviViewItemParent;
        }

        private async void BindData()
        {
            string json = await FlowerHelper.GetFlowerAll();
            List<MyFlower> list = JsonConvert.DeserializeObject<List<MyFlower>>(json);
            GlobalTool.FlowerAll = list; 
        }

        private void BindFlower()
        {
            List<string> Flowers = new List<string>();
            foreach (int i in Enum.GetValues(typeof(FlowerType)))
            {
                if (i == 0) continue;
                Flowers.Add(FlowerHelper.FlowerNameShow[(FlowerType)i]);
            }
            GlobalTool.ComboBoxChoose.ItemsSource = Flowers;
            GlobalTool.ComboBoxChoose.SelectionChanged += ComboBoxChoose_SelectionChanged;
            GlobalTool.ComboBoxChoose.SelectedIndex = 0;
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            string senderName = (sender as MenuFlyoutItem).Name;
            switch (senderName)
            {
                case "MenuExit":
                    GlobalTool.CloseApp();
                    break;
                case "MenuAbout":
                    await GlobalTool.ShowDialog("关于",
                        "动物森友会花卉杂交 v"
                        + GlobalTool.AppVersion
                        + "\n\n作者：FunJoo\n联系方式：huanzhuzai@outlook.com\n数据来源：Fandom");
                    break;
            }
        }

        private bool FirstClick = true;
        private NavigationViewItem CurrentItem;
        private void NaviViewMain_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (FirstClick)
            {
                FirstClick = false;
                FrameMain.Navigate(typeof(PageZajiao));
                FrameMain.Navigate(typeof(PageParent));
                if (NaviViewItemZajiao == sender.SelectedItem) FrameMain.GoBack();
            }
            else
            {
                if (sender.SelectedItem == CurrentItem) return;
                if (NaviViewItemZajiao == sender.SelectedItem) FrameMain.GoBack();
                if (NaviViewItemParent == sender.SelectedItem) FrameMain.GoForward();
            }
            CurrentItem = (NavigationViewItem)sender.SelectedItem;
        }

        private void ComboBoxChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            GlobalTool.SelectedFlower = (FlowerType)cb.SelectedIndex + 1;

            GlobalTool.ListColorDic = new List<MyFlower>();
            GlobalTool.ListColorDic = FlowerHelper.GetFlowerPart(GlobalTool.SelectedFlower);

            GlobalTool.ListColor = new List<string>();
            GlobalTool.ListColorName = new List<string>();
            GlobalTool.ListColor.Add("");
            GlobalTool.ListColorName.Add("无");
            foreach (var a in GlobalTool.ListColorDic)
            {
                if (!GlobalTool.ListColor.Contains(a.Color) && a.Color != "Unknown")
                {
                    GlobalTool.ListColor.Add(a.Color);
                    MyColor mc = (MyColor)Enum.Parse(typeof(MyColor), a.Color);
                    GlobalTool.ListColorName.Add(FlowerHelper.ColorNameShow[mc]);
                }
            }
        }
    }
}
