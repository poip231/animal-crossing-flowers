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
    public sealed partial class PageZajiao : Page
    {
        public PageZajiao()
        {
            this.InitializeComponent();

            BindFlower();
            BindGene();
        }

        SynchronizationContext Sync = SynchronizationContext.Current;

        /// <summary>
        /// 设置了哪种花色左
        /// </summary>
        private MyColor SelectedColorL;
        /// <summary>
        /// 设置了哪种花色右
        /// </summary>
        private MyColor SelectedColorR;
        
        //选择了对应花色的所有基因型
        private List<MyFlower> SelectedColorDicL;
        private List<MyFlower> SelectedColorDicR;

        /// <summary>
        /// 种子列表
        /// </summary>
        private List<MyFlower> ListSeed;

        private void BindFlower()
        {
            int index = GlobalTool.ComboBoxChoose.SelectedIndex;
            GlobalTool.ComboBoxChoose.SelectionChanged -= GroupChanged;
            GlobalTool.ComboBoxChoose.SelectedIndex = -1;
            GlobalTool.ComboBoxChoose.SelectionChanged += GroupChanged;
            GlobalTool.ComboBoxChoose.SelectedIndex = index;

            GlobalTool.ButtonSearch.Click -= Button_Click;
            GlobalTool.ButtonSearch.Click += Button_Click;

            CheckBoxColorL.IsChecked = true;
            CheckBoxColorR.IsChecked = true;
        }

        private void BindSeed()
        {
            ListSeed = new List<MyFlower>();
            foreach (var a in GlobalTool.ListColorDic)
            {
                if (a.GetIsSeed()) ListSeed.Add(a);
            }
            List<string> ListSeedName = new List<string>();
            ListSeedName.Add("无");
            foreach (var a in ListSeed)
            {
                string s = FlowerHelper.FlowerNameShow[a.GetFlowerType()] + " " + FlowerHelper.ColorNameShow[a.GetColor()] + " " + a.GetGeneName();
                ListSeedName.Add(s);
            }
            ComboBoxSeedL.SelectionChanged -= GroupChanged;
            ComboBoxSeedR.SelectionChanged -= GroupChanged;
            ComboBoxSeedL.ItemsSource = ListSeedName;
            ComboBoxSeedR.ItemsSource = ListSeedName;
            ComboBoxSeedL.SelectedIndex = 0;
            ComboBoxSeedR.SelectedIndex = 0;
            ComboBoxSeedL.SelectionChanged += GroupChanged;
            ComboBoxSeedR.SelectionChanged += GroupChanged;
        }

        private void BindGene()
        {
            List<string> Genes = new List<string>();
            foreach (int i in Enum.GetValues(typeof(Gene)))
            {
                if (i == 0 || i > 3) Genes.Add(((Gene)i).ToString());
            }
            ComboBoxA1L.ItemsSource = Genes;
            ComboBoxA2L.ItemsSource = Genes;
            ComboBoxA3L.ItemsSource = Genes;
            ComboBoxA4L.ItemsSource = Genes;
            ComboBoxA1L.SelectedIndex = 0;
            ComboBoxA2L.SelectedIndex = 0;
            ComboBoxA3L.SelectedIndex = 0;
            ComboBoxA4L.SelectedIndex = 0;
            ComboBoxA1L.SelectionChanged += GroupChanged;
            ComboBoxA2L.SelectionChanged += GroupChanged;
            ComboBoxA3L.SelectionChanged += GroupChanged;
            ComboBoxA4L.SelectionChanged += GroupChanged;

            ComboBoxA1R.ItemsSource = Genes;
            ComboBoxA2R.ItemsSource = Genes;
            ComboBoxA3R.ItemsSource = Genes;
            ComboBoxA4R.ItemsSource = Genes;
            ComboBoxA1R.SelectedIndex = 0;
            ComboBoxA2R.SelectedIndex = 0;
            ComboBoxA3R.SelectedIndex = 0;
            ComboBoxA4R.SelectedIndex = 0;
            ComboBoxA1R.SelectionChanged += GroupChanged;
            ComboBoxA2R.SelectionChanged += GroupChanged;
            ComboBoxA3R.SelectionChanged += GroupChanged;
            ComboBoxA4R.SelectionChanged += GroupChanged;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalTool.NaviViewMain.SelectedItem != GlobalTool.NaviItemZajiao) return;

            List<MyFlower> ParentL = new List<MyFlower>();
            List<MyFlower> ParentR = new List<MyFlower>();

            if (GlobalTool.BoolColorL == true)
            {//按颜色L
                if (GlobalTool.IndexColorL == 0)
                {
                    GlobalTool.TipSearch.IsOpen = true;
                    return;
                }
                foreach (var everyflower in SelectedColorDicL) ParentL.Add(everyflower);
            }
            if (GlobalTool.BoolColorR == true)
            {//按颜色R
                if (GlobalTool.IndexColorR == 0)
                {
                    GlobalTool.TipSearch.IsOpen = true;
                    return;
                }
                foreach (var everyflower in SelectedColorDicR) ParentR.Add(everyflower);
            }
            if (GlobalTool.BoolGeneL == true)
            {//按基因型L
                Gene a1 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA1L);
                Gene a2 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA2L);
                Gene a3 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA3L);
                Gene a4 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA4L);
                MyFlower f = new MyFlower(GlobalTool.SelectedFlower, a1, a2, a3, a4);
                foreach (var i in f.GetIntArray())
                {
                    if (i == 0)
                    {
                        GlobalTool.TipSearch.IsOpen = true;
                        return;
                    }
                }
                foreach (var a in GlobalTool.ListColorDic)
                {
                    if (f.GetGeneName() == a.GetGeneName()) ParentL.Add(a);
                }
            }
            if (GlobalTool.BoolGeneR == true)
            {//按基因型R
                Gene a1 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA1R);
                Gene a2 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA2R);
                Gene a3 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA3R);
                Gene a4 = (Gene)Enum.Parse(typeof(Gene), GlobalTool.ItemA4R);
                MyFlower f = new MyFlower(GlobalTool.SelectedFlower, a1, a2, a3, a4);
                foreach (var i in f.GetIntArray())
                {
                    if (i == 0)
                    {
                        GlobalTool.TipSearch.IsOpen = true;
                        return;
                    }
                }
                foreach (var a in GlobalTool.ListColorDic)
                {
                    if (f.GetGeneName() == a.GetGeneName()) ParentR.Add(a);
                }
            }
            if (GlobalTool.BoolSeedL == true)
            {//按种子L
                int index = GlobalTool.IndexSeedL;
                if (index == 0)
                {
                    GlobalTool.TipSearch.IsOpen = true;
                    return;
                }
                ParentL.Add(ListSeed[index - 1]);
            }
            if (GlobalTool.BoolSeedR == true)
            {//按种子R
                int index = GlobalTool.IndexSeedR;
                if (index == 0)
                {
                    GlobalTool.TipSearch.IsOpen = true;
                    return;
                }
                ParentR.Add(ListSeed[index - 1]);
            }

            GlobalTool.ShowProgress();
            await Task.Factory.StartNew(() =>
            {
                ObservableCollection<ChildCard> result = new ObservableCollection<ChildCard>();
                foreach (var L in ParentL)
                {
                    foreach (var R in ParentR)
                    {
                        var children = FlowerHelper.GetOurChildren(L, R);
                        foreach (var child in children)
                        {
                            var childcard = new ChildCard(L, R, child, FlowerHelper.GetProbability(L, R, child));
                            bool isinList = false;
                            foreach (var a in result)
                            {
                                if (
                                    (
                                    childcard.Gene == a.Gene &&
                                    childcard.GeneP1 == a.GeneP1 &&
                                    childcard.GeneP2 == a.GeneP2
                                    )
                                    ||
                                    (
                                    childcard.Gene == a.Gene &&
                                    childcard.GeneP2 == a.GeneP1 &&
                                    childcard.GeneP1 == a.GeneP2
                                    )
                                    ) isinList = true;
                            }
                            if (!isinList) result.Add(childcard);
                        }
                    }
                }
                Sync.Post((o) =>
                {
                    GlobalTool.CloseProgress();
                    var r = o as ObservableCollection<ChildCard>;
                    ListViewChildren.ItemsSource = r;
                }, result);
            });
        }

        private void GroupChanged(object sender, SelectionChangedEventArgs e)
        {
            GlobalTool.ChangeZajiaoComboBox(sender);

            ComboBox cb = sender as ComboBox;

            string senderName = cb.Name;
            switch (senderName)
            {
                case "ComboBoxChoose":
                    {
                        if (cb.SelectedIndex < 0) return;
                        TextBlockTypeL.Text = FlowerHelper.FlowerNameShow[GlobalTool.SelectedFlower];
                        TextBlockTypeR.Text = TextBlockTypeL.Text;

                        ComboBoxColorL.SelectionChanged -= GroupChanged;
                        ComboBoxColorR.SelectionChanged -= GroupChanged;
                        ComboBoxColorL.ItemsSource = GlobalTool.ListColorName;
                        ComboBoxColorR.ItemsSource = GlobalTool.ListColorName;
                        ComboBoxColorL.SelectedIndex = 0;
                        ComboBoxColorR.SelectedIndex = 0;
                        ComboBoxColorL.SelectionChanged += GroupChanged;
                        ComboBoxColorR.SelectionChanged += GroupChanged;
                        ComboBoxColorL.SelectedIndex = 1;
                        ComboBoxColorR.SelectedIndex = 1;

                        CheckBoxColorL.IsChecked = true;
                        CheckBoxColorR.IsChecked = true;

                        //需要在选定花朵之后绑定种子
                        BindSeed();
                    }
                    break;
                case "ComboBoxColorL":
                    {
                        if (cb.SelectedIndex == 0) return;

                        SelectedColorL = (MyColor)Enum.Parse(typeof(MyColor), GlobalTool.ListColor[cb.SelectedIndex]);

                        TextBlockColorL.Text = FlowerHelper.ColorNameShow[SelectedColorL];
                        TextBlockColorL.Foreground = new SolidColorBrush(FlowerHelper.ColorShow[SelectedColorL]);
                        ImageFlowerL.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + GlobalTool.SelectedFlower.ToString() + SelectedColorL.ToString() + ".png"));

                        string s = "";
                        SelectedColorDicL = new List<MyFlower>();
                        foreach (var a in GlobalTool.ListColorDic)
                        {
                            if (a.Color == GlobalTool.ListColor[cb.SelectedIndex])
                            {
                                SelectedColorDicL.Add(a);
                                string n = a.GetGeneName();
                                if (!s.Contains(n)) s += n + " ";
                            }
                        }
                        TextBlockGeneL.Text = s;
                    }
                    break;
                case "ComboBoxColorR":
                    {
                        if (cb.SelectedIndex == 0) return;

                        SelectedColorR = (MyColor)Enum.Parse(typeof(MyColor), GlobalTool.ListColor[cb.SelectedIndex]);

                        TextBlockColorR.Text = FlowerHelper.ColorNameShow[SelectedColorR];
                        TextBlockColorR.Foreground = new SolidColorBrush(FlowerHelper.ColorShow[SelectedColorR]);
                        ImageFlowerR.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + GlobalTool.SelectedFlower.ToString() + SelectedColorR.ToString() + ".png"));

                        string s = "";
                        SelectedColorDicR = new List<MyFlower>();
                        foreach (var a in GlobalTool.ListColorDic)
                        {
                            if (a.Color == GlobalTool.ListColor[cb.SelectedIndex])
                            {
                                SelectedColorDicR.Add(a);
                                string n = a.GetGeneName();
                                if (!s.Contains(n)) s += n + " ";
                            }
                        }
                        TextBlockGeneR.Text = s;
                    }
                    break;
                case "ComboBoxA1L":
                case "ComboBoxA2L":
                case "ComboBoxA3L":
                case "ComboBoxA4L":
                    {
                        if (CheckBoxGeneL.IsChecked == false) return;

                        int a1 = ComboBoxA1L.SelectedIndex;
                        int a2 = ComboBoxA2L.SelectedIndex;
                        int a3 = ComboBoxA3L.SelectedIndex;
                        int a4 = ComboBoxA4L.SelectedIndex;
                        int aa1 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA1L.SelectedItem.ToString());
                        int aa2 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA2L.SelectedItem.ToString());
                        int aa3 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA3L.SelectedItem.ToString());
                        int aa4 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA4L.SelectedItem.ToString());

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
                            TextBlockGeneL.Text = cd.GetGeneName();
                            TextBlockColorL.Text = FlowerHelper.ColorNameShow[cd.GetColor()];
                            TextBlockColorL.Foreground = new SolidColorBrush(FlowerHelper.ColorShow[cd.GetColor()]);

                            ImageFlowerL.Source = new BitmapImage(new Uri(cd.GetImagePath()));
                        }
                    }
                    break;
                case "ComboBoxA1R":
                case "ComboBoxA2R":
                case "ComboBoxA3R":
                case "ComboBoxA4R":
                    {
                        if (CheckBoxGeneR.IsChecked == false) return;

                        int a1 = ComboBoxA1R.SelectedIndex;
                        int a2 = ComboBoxA2R.SelectedIndex;
                        int a3 = ComboBoxA3R.SelectedIndex;
                        int a4 = ComboBoxA4R.SelectedIndex;
                        int aa1 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA1R.SelectedItem.ToString());
                        int aa2 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA2R.SelectedItem.ToString());
                        int aa3 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA3R.SelectedItem.ToString());
                        int aa4 = (int)(Gene)Enum.Parse(typeof(Gene), ComboBoxA4R.SelectedItem.ToString());

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
                            TextBlockGeneR.Text = cd.GetGeneName();
                            TextBlockColorR.Text = FlowerHelper.ColorNameShow[cd.GetColor()];
                            TextBlockColorR.Foreground = new SolidColorBrush(FlowerHelper.ColorShow[cd.GetColor()]);

                            ImageFlowerR.Source = new BitmapImage(new Uri(cd.GetImagePath()));
                        }
                    }
                    break;
                case "ComboBoxSeedL":
                    {
                        if (cb.SelectedIndex == 0) return;
                        int index = cb.SelectedIndex - 1;
                        TextBlockGeneL.Text = ListSeed[index].GetGeneName();
                        TextBlockColorL.Text = FlowerHelper.ColorNameShow[ListSeed[index].GetColor()];
                        TextBlockColorL.Foreground = new SolidColorBrush(FlowerHelper.ColorShow[ListSeed[index].GetColor()]);
                        string path = ListSeed[index].GetImagePath();
                        ImageFlowerL.Source = new BitmapImage(new Uri(path));
                    }
                    break;
                case "ComboBoxSeedR":
                    {
                        if (cb.SelectedIndex == 0) return;
                        int index = cb.SelectedIndex - 1;
                        TextBlockGeneR.Text = ListSeed[index].GetGeneName();
                        TextBlockColorR.Text = FlowerHelper.ColorNameShow[ListSeed[index].GetColor()];
                        TextBlockColorR.Foreground = new SolidColorBrush(FlowerHelper.ColorShow[ListSeed[index].GetColor()]);
                        string path = ListSeed[index].GetImagePath();
                        ImageFlowerR.Source = new BitmapImage(new Uri(path));
                    }
                    break;
            }
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
