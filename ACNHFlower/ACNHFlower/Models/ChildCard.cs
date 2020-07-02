using ACNHFlower.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACNHFlower.Models
{
    class ChildCard
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="f">孩子花朵</param>
        /// <param name="probability">生成概率</param>
        public ChildCard(MyFlower p1, MyFlower p2, MyFlower f, float p)
        {
            Child = f;
            Parent1 = p1;
            Parent2 = p2;
            probability = p;
        }

        public MyFlower Child;
        public MyFlower Parent1;
        public MyFlower Parent2;

        private float probability;
        public string Probability => probability.ToString("#0.000");

        public string Name => FlowerHelper.FlowerNameShow[Child.GetFlowerType()] + " " + FlowerHelper.ColorNameShow[Child.GetColor()];
        public string Color => FlowerHelper.ColorNameShow[Child.GetColor()];
        public string Gene => Child.GetGeneName();
        public string ImagePath => Child.GetImagePath();

        public string GeneP1 => Parent1.GetGeneName();
        public string GeneP2 => Parent2.GetGeneName();
    }
}
