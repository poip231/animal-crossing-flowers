using ACNHFlower.Helpers;
using ACNHFlower.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static ACNHFlower.Models.MyFlower;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ACNHFlower.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PageParent : Page
    {
        public PageParent()
        {
            this.InitializeComponent();
            BindFlower();
            BindGene();
        }

        SynchronizationContext Sync = SynchronizationContext.Current;

        /// <summary>
        /// 设置了哪种花色
        /// </summary>
        private MyColor SelectedColor;
        //选择了对应花色的所有基因型
        private List<MyFlower> SelectedColorDic;

        private void BindFlower()
        {
            int index = GlobalTool.ComboBoxChoose.SelectedIndex;
            GlobalTool.ComboBoxChoose.SelectionChanged -= GroupChanged;
            GlobalTool.ComboBoxChoose.SelectedIndex = -1;
            GlobalTool.ComboBoxChoose.SelectionChanged += GroupChanged;
            GlobalTool.ComboBoxChoose.SelectedIndex = index;

            GlobalTool.ButtonSearch.Click -= Button_Click;
            GlobalTool.ButtonSearch.Click += Button_Click;

            CheckBoxColor.IsChecked = true;
        }

        private void BindGene()
        {
            List<string> Genes = new List<string>();
            foreach (int i in Enum.GetValues(typeof(Gene)))
            {
                if (i == 0 || i > 3) Genes.Add(((Gene)i).ToString());
            }
            ComboBoxA1.ItemsSource = Genes;
            ComboBoxA2.ItemsSource = Genes;
            ComboBoxA3.ItemsSource = Genes;
            ComboBoxA4.ItemsSource = Genes;
            ComboBoxA1.SelectedIndex = 0;
            ComboBoxA2.SelectedIndex = 0;
            ComboBoxA3.SelectedIndex = 0;
            ComboBoxA4.SelectedIndex = 0;
            ComboBoxA1.SelectionChanged += GroupChanged;
            ComboBoxA2.SelectionChanged += GroupChanged;
            ComboBoxA3.SelectionChanged += GroupChanged;
            ComboBoxA4.SelectionChanged += GroupChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalTool.NaviViewMain.SelectedItem != GlobalTool.NaviItemParent) return;

            ObservableCollection<ParentCard> reCard = new ObservableCollection<ParentCard>();

            GlobalTool.ShowProgress();
            Task.Factory.StartNew(() =>
            {
                if (GlobalTool.BoolColor == true)
                {//按颜色查父本
                    if (GlobalTool.IndexColor == 0)
                    {
                        GlobalTool.TipSearch.IsOpen = true;
                        return;
                    }
                    if (SelectedColorDic == null || SelectedColorDic.Count == 0) return;

                    foreach (var everyflower in SelectedColorDic)
                    {
                        var parent = FlowerHelper.GetMyParent(everyflower);
                        foreach (var a in parent)
                        {
                            var aa = new ParentCard(a);
                            bool inResult = false;
                            foreach (var b in reCard)
                            {
                                if (b.TextGeneLeft == aa.TextGeneLeft && b.TextGeneRight == aa.TextGeneRight) inResult = true;
                            }
                            if (!inResult) reCard.Add(aa);
                        }
                    }
                }
                if (GlobalTool.BoolGene == true)
                {//按基因型查父本
                    Gene a1 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA1);
                    Gene a2 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA2);
                    Gene a3 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA3);
                    Gene a4 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA4);
                    MyFlower f = new MyFlower(GlobalTool.SelectedFlower, a1, a2, a3, a4);
                    var re = FlowerHelper.GetMyParent(f);

                    foreach (var a in re)
                    {
                        reCard.Add(new ParentCard(a));
                    }
                }

                Sync.Post((o) =>
                {
                    GlobalTool.CloseProgress();
                    var r = o as ObservableCollection<ParentCard>;
                    ListViewParent.ItemsSource = r;
                }, reCard);
            });
        }

        private void GroupChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            GlobalTool.ChangeParentComboBox(sender);

            string senderName = cb.Name;
            switch (senderName)
            {
                case "ComboBoxChoose":
                    {
                        if (cb.SelectedIndex < 0) return;
                        TextBlockType.Text = FlowerHelper.FlowerNameShow[GlobalTool.SelectedFlower];

                        ComboBoxColor.SelectionChanged -= GroupChanged;
                        ComboBoxColor.ItemsSource = GlobalTool.ListColorName;
                        ComboBoxColor.SelectedIndex = 0;
                        ComboBoxColor.SelectionChanged += GroupChanged;
                        ComboBoxColor.SelectedIndex = 1;

                        CheckBoxColor.IsChecked = true;
                    }
                    break;
                case "ComboBoxColor":
                    {
                        if (cb.SelectedIndex == 0) return;

                        SelectedColor = (MyColor)Enum.Parse(typeof(MyColor), GlobalTool.ListColor[cb.SelectedIndex]);

                        TextBlockColor.Text = FlowerHelper.ColorNameShow[SelectedColor];
                        TextBlockColor.Foreground = new SolidColorBrush(FlowerHelper.ColorShow[SelectedColor]);
                        ImageFlower.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + GlobalTool.SelectedFlower.ToString() + SelectedColor.ToString() + ".png"));

                        string s = "";
                        SelectedColorDic = new List<MyFlower>();
                        foreach (var a in GlobalTool.ListColorDic)
                        {
                            if (a.Color == GlobalTool.ListColor[cb.SelectedIndex])
                            {
                                SelectedColorDic.Add(a);
                                string n = a.GetGeneName();
                                if (!s.Contains(n)) s += n + " ";
                            }
                        }
                        TextBlockGene.Text = s;
                    }
                    break;
                case "ComboBoxA1":
                case "ComboBoxA2":
                case "ComboBoxA3":
                case "ComboBoxA4":
                    {
                        if (CheckBoxGene.IsChecked == false) return;

                        int a1 = ComboBoxA1.SelectedIndex;
                        int a2 = ComboBoxA2.SelectedIndex;
                        int a3 = ComboBoxA3.SelectedIndex;
                        int a4 = ComboBoxA4.SelectedIndex;
                        int aa1 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA1.SelectedItem.ToString());
                        int aa2 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA2.SelectedItem.ToString());
                        int aa3 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA3.SelectedItem.ToString());
                        int aa4 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA4.SelectedItem.ToString());

                        if (
                            (a1 > 0 && a2 > 0 && a3 > 0 && GlobalTool.SelectedFlower != FlowerType.Roses)
                            ||
                            (a1 > 0 && a2 > 0 && a3 > 0 && a4 > 0)
                            )
                        {
                            MyFlower cd = new MyFlower();
                            foreach (var a in GlobalTool.ListColorDic)
                            {
                                if (GlobalTool.SelectedFlower == FlowerType.Roses)
                                {
                                    if (a.A1 == aa1.ToString() && a.A2 == aa2.ToString() && a.A3 == aa3.ToString() && a.A4 == aa4.ToString())
                                    {
                                        cd = a;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (a.A1 == aa1.ToString() && a.A2 == aa2.ToString() && a.A3 == aa3.ToString())
                                    {
                                        cd = a;
                                        break;
                                    }
                                }

                            }
                            TextBlockGene.Text = cd.GetGeneName();
                            TextBlockColor.Text = FlowerHelper.ColorNameShow[cd.GetColor()];
                            TextBlockColor.Foreground = new SolidColorBrush(FlowerHelper.ColorShow[cd.GetColor()]);

                            ImageFlower.Source = new BitmapImage(new Uri(cd.GetImagePath()));
                        }
                    }
                    break;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GlobalTool.ChangeParentCheckBox(sender);

            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked == false) return;
            string senderName = cb.Name;
            switch (senderName)
            {
                case "CheckBoxColor":
                    CheckBoxGene.IsChecked = false;
                    ComboBoxA1.SelectedIndex = 0;
                    ComboBoxA2.SelectedIndex = 0;
                    ComboBoxA3.SelectedIndex = 0;
                    ComboBoxA4.SelectedIndex = 0;
                    break;
                case "CheckBoxGene":
                    CheckBoxColor.IsChecked = false;
                    ComboBoxColor.SelectedIndex = 0;
                    break;
            }
        }
    }
}
