using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.DrawingCore;
using System.Linq;

namespace Common
{
    public class ImageHelper
    {
        /// <summary>
        /// 生成缩略图到MemoryStream
        /// </summary>
        /// <param name="SourceFile"></param>
        /// <param name="intThumbWidth"></param>
        /// <param name="intThumbHeight"></param>
        /// <param name="gap">是否补足空白</param>
        public static MemoryStream ThumbImageToStream(string SourceFile, int intThumbWidth, int intThumbHeight,bool gap=true)
        {
            MemoryStream ms = new MemoryStream();
            //原图加载 
            using (System.DrawingCore.Image sourceImage = System.DrawingCore.Image.FromFile(SourceFile))
            {
                //是否显示原图
                if (intThumbWidth == 0 && intThumbHeight == 0)
                {
                    sourceImage.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);
                    return ms;
                }
                //原图宽度和高度 
                int width = sourceImage.Width;
                int height = sourceImage.Height;
                int smallWidth;
                int smallHeight;
                //如果原图长宽均比缩略图的要小则返回原stream
                if (width <= intThumbWidth && height <= intThumbHeight)
                {
                    sourceImage.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);
                    return ms;
                }
                //获取第一张绘制图的大小,(比较 原图的宽/缩略图的宽  和 原图的高/缩略图的高) 
                if (((decimal)width) / height <= ((decimal)intThumbWidth) / intThumbHeight)
                {
                    smallWidth = intThumbHeight * width / height;
                    smallHeight = intThumbHeight;
                }
                else
                {
                    smallWidth = intThumbWidth;
                    smallHeight = intThumbWidth * height / width;
                }
                Image newimg = sourceImage.GetThumbnailImage(smallWidth, smallHeight, null, IntPtr.Zero);

                //使用原宽高比输出，不补足空白
                if (!gap)
                {
                    newimg.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);
                    return ms;
                }


                //新建一个图板,以最小等比例压缩大小绘制原图 
                using (System.DrawingCore.Image bitmap = new System.DrawingCore.Bitmap(smallWidth, smallHeight))
                {
                    //绘制中间图 
                    using (System.DrawingCore.Graphics g = System.DrawingCore.Graphics.FromImage(bitmap))
                    {

                        //高清,平滑 
                        g.InterpolationMode = System.DrawingCore.Drawing2D.InterpolationMode.High;
                        g.SmoothingMode = System.DrawingCore.Drawing2D.SmoothingMode.HighQuality;
                        g.Clear(Color.Transparent);
                        g.DrawImage(
                        sourceImage,
                        new System.DrawingCore.Rectangle(0, 0, smallWidth, smallHeight),
                        new System.DrawingCore.Rectangle(0, 0, width, height),
                        System.DrawingCore.GraphicsUnit.Pixel
                        );

                    }
                    //新建一个图板,以缩略图大小绘制中间图 
                    using (System.DrawingCore.Image bitmap1 = new System.DrawingCore.Bitmap(intThumbWidth, intThumbHeight))
                    {
                        //绘制缩略图 
                        using (System.DrawingCore.Graphics g = System.DrawingCore.Graphics.FromImage(bitmap1))
                        {
                            //高清,平滑 
                            g.InterpolationMode = System.DrawingCore.Drawing2D.InterpolationMode.High;
                            g.SmoothingMode = System.DrawingCore.Drawing2D.SmoothingMode.HighQuality;
                            g.Clear(Color.Transparent);
                            int lwidth = (smallWidth - intThumbWidth) / 2;
                            int bheight = (smallHeight - intThumbHeight) / 2;
                            g.DrawImage(bitmap, new Rectangle(0, 0, intThumbWidth, intThumbHeight), lwidth, bheight, intThumbWidth, intThumbHeight, GraphicsUnit.Pixel);
                            g.Dispose();
                            bitmap1.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);
                            return ms;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 文件是否是图片
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsImg(string filePath)
        {
            var imgExts = new string[] { ".png", ".jpg", ".gif", ".jpeg" };
            var ext = System.IO.Path.GetExtension(filePath);

            return imgExts.Contains(ext.ToLower());
        }
    }
}
