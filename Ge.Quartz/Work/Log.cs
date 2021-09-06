using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Quartz.Work
{
    public class Log
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">文件路径</param>
        /// <param name="content">日志内容</param>
        /// <param name="isCover">是否覆盖</param>
        public static void WriteToFile(string name, string content, bool isCover)
        {
            FileStream fs = null;
            try
            {
                if (!isCover && File.Exists(name))
                {
                    fs = new FileStream(name, FileMode.Append, FileAccess.Write);
                    var sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(DateTime.Now + "【日志】" + content);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    File.WriteAllText(name, DateTime.Now + @"【日志】" + content, Encoding.UTF8);
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

        }
    }
}
