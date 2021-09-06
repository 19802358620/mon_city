using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Adm
{
    public class VideoHelper
    {
        /// <summary>
        /// 从视频画面中截取一帧画面为图片
        /// </summary>
        /// <param name="VideoName">视频文件pic/guiyu.mov</param>
        /// <param name="WidthAndHeight">图片的尺寸如:240*180</param>
        /// <param name="CutTimeFrame">开始截取的时间如:"1"</param>
        /// <returns></returns>
        public static string GetPicFromVideo(string VideoName, string WidthAndHeight, string CutTimeFrame)
        {
            string ffmpeg = @"D:\ffmpeg.exe"; // Server.MapPath("~");
            string PicName = @"D:\jietu.jpg";//Server.MapPath(Guid.NewGuid().ToString().Replace("-", "") + ".jpg");  //"D:/jietu.jpg";    //
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //startInfo.Arguments = " -i " + VideoName + " -y -f image2 -ss " + CutTimeFrame + " -t 0.001 -s " + WidthAndHeight + " " + PicName;  //設定程式執行參數
            startInfo.Arguments = " -i " + VideoName + " -y -f image2 -ss 2 -vframes 1  -s   " + WidthAndHeight + "   " + PicName;
            try
            {
                System.Diagnostics.Process.Start(startInfo);
                return PicName;
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }
       


    }
}
