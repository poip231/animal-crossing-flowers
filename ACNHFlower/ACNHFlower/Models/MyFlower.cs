using ACNHFlower.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACNHFlower.Models
{
    class MyFlower
    {
        private int id;
        public string ID
        {
            get { return id.ToString(); }
            set { int.TryParse(value, out id); }
        }
        private int a1;
        public string A1
        {
            get { return a1.ToString(); }
            set { int.TryParse(value, out a1); }
        }
        private int a2;
        public string A2
        {
            get { return a2.ToString(); }
            set { int.TryParse(value, out a2); }
        }
        private int a3;
        public string A3
        {
            get { return a3.ToString(); }
            set { int.TryParse(value, out a3); }
        }
        private int a4;
        public string A4
        {
            get { return a4.ToString(); }
            set { int.TryParse(value, out a4); }
        }
        private int pairs;
        public string Pairs
        {
            get { return pairs.ToString(); }
            set { int.TryParse(value, out pairs); }
        }
        private MyColor color;
        public string Color
        {
            get { return color.ToString(); }
            set { color = (MyColor)Enum.Parse(typeof(MyColor), value); }
        }
        private bool isseed;
        public string IsSeed
        {
            get
            {
                if (isseed) return "True";
                return "False";
            }
            set
            {
                if ("1" == value) isseed = true;
                else isseed = false;
            }
        }
        private FlowerType type = FlowerType.Unknown;
        public string Type
        {
            get { return type.ToString(); }
            set
            {
                type = (FlowerType)Enum.Parse(typeof(FlowerType), value);
            }
        }
        public string Note { get; set; }

        public MyFlower() { }

        public MyFlower(FlowerType flower, Gene aa1, Gene aa2, Gene aa3, Gene aa4 = Gene.Unknown)
        {
            type = flower;
            a1 = (int)aa1;
            a2 = (int)aa2;
            a3 = (int)aa3;
            a4 = (int)aa4;
            pairs = flower == FlowerType.Roses ? 4 : 3;
        }

        public List<MyFlower> GetChildren()
        {
            List<MyFlower> mList = new List<MyFlower>();
            if ((Gene)a4 != Gene.Unknown)
            {
                if (a1 < 4 || a2 < 4 || a3 < 4 || a4 < 4) return mList;

                var aa = GetGeneAAName();
                string[,] child = new string[,] {
                { aa[0].ToString(),aa[1].ToString()},
                { aa[2].ToString(),aa[3].ToString()},
                { aa[4].ToString(),aa[5].ToString()},
                { aa[6].ToString(),aa[7].ToString()},
                };

                List<string> ls = new List<string>();
                Permutation(ref ls, child, "");
                foreach (var s in ls)
                {
                    MyFlower ll = new MyFlower(
                        type,
                        (Gene)Enum.Parse(typeof(Gene), s[0].ToString()),
                        (Gene)Enum.Parse(typeof(Gene), s[1].ToString()),
                        (Gene)Enum.Parse(typeof(Gene), s[2].ToString()),
                        (Gene)Enum.Parse(typeof(Gene), s[3].ToString())
                        );
                    mList.Add(ll);
                }
            }
            else
            {
                if (a1 < 4 || a2 < 4 || a3 < 4) return mList;

                var aa = GetGeneAAName();
                string[,] child = new string[,] {
                { aa[0].ToString(),aa[1].ToString()},
                { aa[2].ToString(),aa[3].ToString()},
                { aa[4].ToString(),aa[5].ToString()}
                };

                List<string> ls = new List<string>();
                Permutation(ref ls, child, "");
                foreach (var s in ls)
                {
                    MyFlower ll = new MyFlower(
                        type,
                        (Gene)Enum.Parse(typeof(Gene), s[0].ToString()),
                        (Gene)Enum.Parse(typeof(Gene), s[1].ToString()),
                        (Gene)Enum.Parse(typeof(Gene), s[2].ToString())
                        );
                    mList.Add(ll);
                }
            }
            return mList;
        }

        public MyColor GetColor()
        {
            var ml = FlowerHelper.GetFlowerPart(type);
            for (int i = 0; i < ml.Count; i++)
            {
                if ((Gene)a4 != Gene.Unknown)
                {
                    if (a1 == Convert.ToInt32(ml[i].A1) && a2 == Convert.ToInt32(ml[i].A2) && a3 == Convert.ToInt32(ml[i].A3) && a4 == Convert.ToInt32(ml[i].A4))
                    {
                        return (MyColor)Enum.Parse(typeof(MyColor), ml[i].Color);
                    }
                }
                else
                {
                    if (a1 == Convert.ToInt32(ml[i].A1) && a2 == Convert.ToInt32(ml[i].A2) && a3 == Convert.ToInt32(ml[i].A3))
                    {
                        return (MyColor)Enum.Parse(typeof(MyColor), ml[i].Color);
                    }
                }
            }
            return MyColor.Unknown;
        }

        public bool GetIsSeed()
        {
            return isseed;
        }

        public FlowerType GetFlowerType()
        {
            return type;
        }

        public int[] GetIntArray()
        {
            if (pairs > 3) return new int[] { a1, a2, a3, a4 };
            return new int[] { a1, a2, a3 };
        }

        public Gene[] GetGeneArray()
        {
            if (pairs > 3) return new Gene[] { (Gene)a1, (Gene)a2, (Gene)a3, (Gene)a4 };
            return new Gene[] { (Gene)a1, (Gene)a2, (Gene)a3 };
        }

        public string[] GetGeneNameArray()
        {
            string gt = FlowerHelper.GeneType[type];
            string[] re;
            if (pairs > 3)
            {
                re = new string[] { ((Gene)a1).ToString(), ((Gene)a2).ToString(), ((Gene)a3).ToString(), ((Gene)a4).ToString() };
            }
            else
            {
                re = new string[] { ((Gene)a1).ToString(), ((Gene)a2).ToString(), ((Gene)a3).ToString() };
            }
            for (int i = 0; i < re.Length; i++)
            {
                re[i] = re[i].Replace('a', char.ToLower(gt[i]));
                re[i] = re[i].Replace('A', char.ToUpper(gt[i]));
            }
            return re;
        }

        public string GetGeneName()
        {
            var a = GetGeneNameArray();
            if (pairs > 3) return a[0] + a[1] + a[2] + a[3];
            return a[0] + a[1] + a[2];
        }

        public string[] GetGeneAANameArray()
        {
            string[] re;
            if (pairs > 3)
            {
                re = new string[] { ((Gene)a1).ToString(), ((Gene)a2).ToString(), ((Gene)a3).ToString(), ((Gene)a4).ToString() };
            }
            else
            {
                re = new string[] { ((Gene)a1).ToString(), ((Gene)a2).ToString(), ((Gene)a3).ToString() };
            }
            return re;
        }

        public string GetGeneAAName()
        {
            var a = GetGeneAANameArray();
            if (pairs > 3) return a[0] + a[1] + a[2] + a[3];
            return a[0] + a[1] + a[2];
        }

        public string GetImagePath()
        {
            return
                Color != "Unknown"
                ?
                "ms-appx:///Assets/" + Type + Color + ".png"
                :
                "ms-appx:///Assets/" + Color + ".png";
        }

        public enum Gene
        {
            Unknown = 0,
            a = 2,
            A = 3,
            aa = 4,
            Aa = 5,
            AA = 6
        }

        public enum MyColor
        {
            Unknown,
            White,
            Yellow,
            Red,
            Pink,
            Dark,
            Orange,
            Blue,
            Purple,
            Green,
            YellowRed,
            Gold
        }

        public enum FlowerType
        {
            Unknown,
            Cosmos,
            Hyacinths,
            Lilies,
            Mums,
            Pansies,
            Roses,
            Tulips,
            Windflower
        }

        /// <summary>
        /// 获得子配型专用排列组合
        /// </summary>
        /// <param name="ls">存放配型的列表</param>
        /// <param name="arr">二维数组</param>
        /// <param name="rs">传递的字符</param>
        /// <param name="_m">处于第几行的指示</param>
        private void Permutation(ref List<string> ls, string[,] arr, string rs = "", int _m = 0)
        {
            string s = rs;
            if (_m < arr.GetLength(0))
            {
                rs += arr[_m, 0];
                Permutation(ref ls, arr, rs, _m + 1);
                s += arr[_m, 1];
                Permutation(ref ls, arr, s, _m + 1);
            }
            else
            {
                if (!ls.Contains(rs)) ls.Add(rs);
            }
        }
    }
}
