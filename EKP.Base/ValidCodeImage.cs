using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace EKP.Base
{
    public class ValidCodeImage
    {
        private string checkCode;

        public string CheckCode
        {
            get { return checkCode; }
        }


        public ValidCodeImage(int num)
        {
            checkCode = GenCode(num);
        }

        /// <summary>
        /// '产生随机字符串
        /// </summary>
        /// <param name="num">随机出几个字符</param>
        /// <returns>随机出的字符串</returns>
        protected string GenCode(int num)
        {
            string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWSYZ";
            char[] chastr = str.ToCharArray();
            // string[] source ={ "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "#", "$", "%", "&", "@" };
            string code = "";
            Random rd = new Random();
            int i;
            for (i = 0; i < num; i++)
            {
                //code += source[rd.Next(0, source.Length)];
                code += str.Substring(rd.Next(0, str.Length), 1);
            }
            return code;

        }

        /// <summary>
        /// 生成图片（增加背景噪音线、前景噪音点）
        /// </summary>
        public Bitmap CreateCheckCodeImage()
        {
            if (checkCode.Trim() == "" || checkCode == null)
                return null;

            var image = new Bitmap((int)(checkCode.Length * 15), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                // 画图片的背景噪音线
                int i;
                for (i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    //g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                var font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold));
                //var brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);
                // 随机生成验证码的颜色
                var brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)), Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)), 1.2F, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                //g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                var ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            catch
            {
                g.Dispose();
                image.Dispose();
            }
            return image;
        }
    }
}