using ACNHFlower.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI;
using static ACNHFlower.Models.MyFlower;

namespace ACNHFlower.Helpers
{
    class FlowerHelper
    {

        public const string FlowerJsonName = "flower_v02.json";

        /// <summary>
        /// 检查本地文件夹有没有flower_v2.json这个文件，没有就创建
        /// </summary>
        /// <param name="reset">是否重置本地词库，默认为非重置</param>
        public static async Task CheckLocalJson(bool reset = false)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            //StorageFile file = await folder.GetFileAsync(EmojiFileName);
            StorageFile file = (StorageFile) await folder.TryGetItemAsync(FlowerJsonName);

            //await file.DeleteAsync(); //调试删除文件用

            if (file != null && !reset) return;

            file = await folder.CreateFileAsync(FlowerJsonName, CreationCollisionOption.ReplaceExisting);
            Uri uri = new Uri("ms-appx:///Assets/" + FlowerJsonName);
            StorageFile jsonFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            string txt = await FileIO.ReadTextAsync(jsonFile);

            await FileIO.WriteTextAsync(file, txt);
        }

        /// <summary>
        /// 读取本地Emoji词库
        /// </summary>
        /// <returns>返回词库JSON</returns>
        public static async Task<string> GetFlowerAll()
        {
            GlobalTool.FlowerAll = new List<MyFlower>();
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.GetFileAsync(FlowerJsonName);
                return await FileIO.ReadTextAsync(file);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "";
            }
        }

        /// <summary>
        /// 获取部分花朵
        /// </summary>
        /// <param name="ft">花的种类</param>
        /// <returns>花朵列表</returns>
        public static List<MyFlower> GetFlowerPart(FlowerType ft)
        {
            string ftName = ft.ToString();
            List<MyFlower> rList = new List<MyFlower>();
            foreach (var cd in GlobalTool.FlowerAll)
            {
                if (cd.Type == ftName) rList.Add(cd);
            }
            return rList;
        }

        /// <summary>
        /// 获取父本对列表
        /// </summary>
        /// <param name="flower">要获取父本的花朵</param>
        /// <returns>父本对列表</returns>
        public static List<MyFlower[]> GetMyParent(MyFlower flower)
        {
            int[] ig = flower.GetIntArray();

            if (ig.Length > 3)
            {
                if (ig == new int[] { 0, 0, 0, 0 }) return new List<MyFlower[]>();
                if ((ig[0] > 0 && ig[0] < 4) || (ig[1] > 0 && ig[1] < 4) || (ig[2] > 0 && ig[2] < 4) || (ig[3] > 0 && ig[3] < 4)) return new List<MyFlower[]>();
            }
            else
            {
                if (ig == new int[] { 0, 0, 0 }) return new List<MyFlower[]>();
                if ((ig[0] > 0 && ig[0] < 4) || (ig[1] > 0 && ig[1] < 4) || (ig[2] > 0 && ig[2] < 4)) return new List<MyFlower[]>();
                ig = new int[] { ig[0], ig[1], ig[2] };
            }

            List<List<int[]>> lli = new List<List<int[]>>();
            for (int i = 0; i < ig.Length; i++)
            {
                List<int[]> li = new List<int[]>();
                switch (ig[i])
                {
                    case 4:
                        li.Add(new int[] { 4, 4 });
                        li.Add(new int[] { 4, 5 });
                        li.Add(new int[] { 5, 5 });
                        break;
                    case 5:
                        li.Add(new int[] { 5, 5 });
                        li.Add(new int[] { 4, 6 });
                        li.Add(new int[] { 5, 6 });
                        break;
                    case 6:
                        li.Add(new int[] { 5, 5 });
                        li.Add(new int[] { 5, 6 });
                        li.Add(new int[] { 6, 6 });
                        break;
                    default:
                        li.Add(new int[] { 4, 4 });
                        li.Add(new int[] { 4, 5 });
                        li.Add(new int[] { 5, 5 });
                        li.Add(new int[] { 5, 6 });
                        li.Add(new int[] { 6, 6 });
                        li.Add(new int[] { 4, 6 });
                        break;
                }
                lli.Add(li);
            }

            List<MyFlower[]> result = new List<MyFlower[]>();
            int[] left, right;

            for (int i = 0; i < lli[0].Count; i++)
            {
                left = new int[lli.Count];
                right = new int[lli.Count];
                left[0] = lli[0][i][0];
                right[0] = lli[0][i][1];
                for (int j = 0; j < lli[1].Count; j++)
                {
                    left[1] = lli[1][j][0];
                    right[1] = lli[1][j][1];
                    for (int k = 0; k < lli[2].Count; k++)
                    {
                        left[2] = lli[2][k][0];
                        right[2] = lli[2][k][1];
                        if (lli.Count == 4)
                        {
                            for (int h = 0; h < lli[3].Count; h++)
                            {
                                left[3] = lli[3][h][0];
                                right[3] = lli[3][h][1];
                                MyFlower mfLeft = new MyFlower(
                                flower.GetFlowerType(),
                                (Gene)left[0],
                                (Gene)left[1],
                                (Gene)left[2],
                                (Gene)left[3]);
                                MyFlower mfRight = new MyFlower(
                                    flower.GetFlowerType(),
                                    (Gene)right[0],
                                    (Gene)right[1],
                                    (Gene)right[2],
                                    (Gene)right[3]);
                                MyFlower[] ff = new MyFlower[] { mfLeft, mfRight };
                                if (!result.Contains(ff) || !result.Contains(ff.Reverse())) result.Add(ff);
                            }
                        }
                        else
                        {
                            MyFlower mfLeft = new MyFlower(
                                flower.GetFlowerType(),
                                (Gene)left[0],
                                (Gene)left[1],
                                (Gene)left[2]);
                            MyFlower mfRight = new MyFlower(
                                flower.GetFlowerType(),
                                (Gene)right[0],
                                (Gene)right[1],
                                (Gene)right[2]);
                            MyFlower[] ff = new MyFlower[] { mfLeft, mfRight };
                            if (!result.Contains(ff) || !result.Contains(ff.Reverse())) result.Add(ff);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取……孩子们
        /// </summary>
        /// <param name="f1">爸比</param>
        /// <param name="f2">妈咪</param>
        /// <returns>孩子们</returns>
        public static List<MyFlower> GetOurChildren(MyFlower f1, MyFlower f2)
        {
            List<MyFlower> result = new List<MyFlower>();
            if (f1.Type != f2.Type) return result;

            var AllFlowers = GetFlowerPart(f1.GetFlowerType());

            List<MyFlower> f1children = f1.GetChildren();
            List<MyFlower> f2children = f2.GetChildren();

            foreach (var f1child in f1children)
            {
                int[] aa = f1child.GetIntArray();
                foreach (var f2child in f2children)
                {
                    int[] bb = f2child.GetIntArray();
                    int[] cc = new int[aa.Length];
                    for (int i = 0; i < aa.Length; i++)
                    {
                        cc[i] = aa[i] + bb[i];
                    }
                    foreach (var f in AllFlowers)
                    {
                        int eq = 0;
                        var fi = f.GetIntArray();
                        for (int i = 0; i < fi.Length; i++)
                        {
                            if (cc[i] == fi[i]) eq++;
                        }
                        if (eq == fi.Length)
                        {
                            bool inresult = false;
                            foreach (var r in result)
                            {
                                if (r.GetGeneName() == f.GetGeneName()) inresult = true;
                            }
                            if (!inresult) result.Add(f);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取概率
        /// </summary>
        /// <param name="p1">父本1</param>
        /// <param name="p2">父本2</param>
        /// <param name="c">儿子</param>
        /// <returns>概率</returns>
        public static float GetProbability(MyFlower f1, MyFlower f2, MyFlower c)
        {
            if (f1.Type != f2.Type || f1.Type != c.Type) return 0;
            int[] f1ca = f1.GetIntArray();
            int[] f2ca = f2.GetIntArray();

            float zongshu = 1;
            List<int[]> f1c = new List<int[]>();
            List<int[]> f2c = new List<int[]>();
            int[] linshi = new int[f1ca.Length];
            Permutation(ref f1c, ref zongshu, linshi, f1ca, 0);
            Permutation(ref f2c, ref zongshu, linshi, f2ca, 0);

            int count = 0;
            for (int i = 0; i < f1c.Count; i++)
            {
                for (int j = 0; j < f2c.Count; j++)
                {
                    int[] ca = c.GetIntArray();
                    if (c.GetFlowerType() == FlowerType.Roses)
                    {
                        if (
                            ca[0] == f1c[i][0] + f2c[j][0] &&
                            ca[1] == f1c[i][1] + f2c[j][1] &&
                            ca[2] == f1c[i][2] + f2c[j][2] &&
                            ca[3] == f1c[i][3] + f2c[j][3]
                            )
                        {
                            count++;
                        }
                    }
                    else
                    {
                        if (ca[0] == f1c[i][0] + f2c[j][0] &&
                            ca[1] == f1c[i][1] + f2c[j][1] &&
                            ca[2] == f1c[i][2] + f2c[j][2]
                            )
                        {
                            count++;
                        }
                    }
                }
            }
            float r = count / zongshu;
            return r;
        }

        /// <summary>
        /// 获取所有子型，包含重复的
        /// </summary>
        /// <param name="children">存放儿子的列表</param>
        /// <param name="ls">临时结果</param>
        /// <param name="zongshu">概率总数</param>
        /// <param name="parent">父本</param>
        /// <param name="i">指示数</param>
        private static void Permutation(ref List<int[]> children, ref float zongshu, int[] linshi, int[] parent, int i)
        {
            if (i < parent.Length)
            {
                switch (parent[i])
                {
                    case 4:
                        linshi[i] = 2;
                        Permutation(ref children, ref zongshu, linshi, parent, i + 1);
                        break;
                    case 5:
                        zongshu *= 2;
                        linshi[i] = 2;
                        Permutation(ref children, ref zongshu, linshi, parent, i + 1);
                        linshi[i] = 3;
                        Permutation(ref children, ref zongshu, linshi, parent, i + 1);
                        break;
                    case 6:
                        linshi[i] = 3;
                        Permutation(ref children, ref zongshu, linshi, parent, i + 1);
                        break;
                }
            }
            else
            {
                if (linshi.Length == 4)
                {
                    children.Add(new int[] { linshi[0], linshi[1], linshi[2], linshi[3] });
                }
                else
                {
                    children.Add(new int[] { linshi[0], linshi[1], linshi[2] });
                }

            }
        }

        #region Flower Dictionary

        /// <summary>
        /// 花色对应颜色
        /// </summary>
        public static Dictionary<MyColor, Color> ColorShow = new Dictionary<MyColor, Color>()
        {
            { MyColor.Unknown , Colors.Black },
            { MyColor.Blue,Colors.Blue},
            { MyColor.Dark,Colors.Black},
            { MyColor.Green,Colors.Green},
            { MyColor.Orange,Colors.Orange},
            { MyColor.Pink,Colors.HotPink},
            { MyColor.Purple,Colors.Purple},
            { MyColor.Red,Colors.Red},
            { MyColor.White,Colors.Gray},
            { MyColor.Yellow,Colors.Goldenrod},
            { MyColor.YellowRed,Colors.DarkGoldenrod},
            { MyColor.Gold,Colors.Gold},
        };

        /// <summary>
        /// 花色对应中文
        /// </summary>
        public static Dictionary<MyColor, string> ColorNameShow = new Dictionary<MyColor, string>()
        {
            { MyColor.Unknown , "未知" },
            { MyColor.Blue,"蓝色"},
            { MyColor.Dark,"黑色"},
            { MyColor.Green,"绿色"},
            { MyColor.Orange,"橘色"},
            { MyColor.Pink,"粉红色"},
            { MyColor.Purple,"紫色"},
            { MyColor.Red,"红色"},
            { MyColor.White,"白色"},
            { MyColor.Yellow,"黄色"},
            { MyColor.YellowRed,"红黄"},
            { MyColor.Gold,"金色"},
        };

        /// <summary>
        /// 花名对应中文
        /// </summary>
        public static Dictionary<FlowerType, string> FlowerNameShow = new Dictionary<FlowerType, string>()
        {
            {FlowerType.Cosmos,"波斯菊" },
            {FlowerType.Hyacinths,"风信子" },
            {FlowerType.Lilies,"百合" },
            {FlowerType.Mums,"菊花" },
            {FlowerType.Pansies,"三色堇" },
            {FlowerType.Roses,"玫瑰" },
            {FlowerType.Tulips,"郁金香" },
            {FlowerType.Unknown,"未知" },
            {FlowerType.Windflower,"银莲花" },
        };

        /// <summary>
        /// 花种对应配型
        /// </summary>
        public static Dictionary<FlowerType, string> GeneType = new Dictionary<FlowerType, string>()
        {
            {FlowerType.Unknown,"" },
            {FlowerType.Cosmos,"RYS" },
            {FlowerType.Hyacinths,"RYW" },
            {FlowerType.Lilies,"RYS" },
            {FlowerType.Mums,"RYW" },
            {FlowerType.Pansies,"RYW" },
            {FlowerType.Roses,"RYWS" },
            {FlowerType.Tulips,"RYS" },
            {FlowerType.Windflower,"ROW"}
        };

        #endregion
    }
}
