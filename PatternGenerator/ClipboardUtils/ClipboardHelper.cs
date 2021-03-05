using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PatternGenerator.ClipboardUtils
{
    public class ClipboardHelper
    {
        public static bool SetClipboard(ClipboardEntry entry)
        { 
            if (!ClipboardApi.OpenClipboard(IntPtr.Zero) || !ClipboardApi.EmptyClipboard())
                    return false;
            IntPtr handle = Marshal.AllocHGlobal(entry.data.Length);
            Marshal.Copy(entry.data, 0, handle, entry.data.Length);
            if (!ClipboardApi.SetClipboardData(entry.id, handle))
            {
                Marshal.FreeHGlobal(handle);
                return false;
            }
            ClipboardApi.CloseClipboard();
            return true;
        }
        public static bool SetClipboard(List<ClipboardEntry> entries)
        {
            if (!ClipboardApi.OpenClipboard(IntPtr.Zero) || !ClipboardApi.EmptyClipboard())
                return false;
            foreach (ClipboardEntry entry in entries)
            {
                IntPtr handle = Marshal.AllocHGlobal(entry.data.Length);
                Marshal.Copy(entry.data, 0, handle, entry.data.Length);
                if (!ClipboardApi.SetClipboardData(entry.id, handle))
                {
                    Marshal.FreeHGlobal(handle);
                    return false;
                }
            }
            ClipboardApi.CloseClipboard();
            return true;
        }
        public static void SetClipboardImage(Bitmap image)
        {
            using (MemoryStream pngMemStream = new MemoryStream())
            {
                image.Save(pngMemStream, ImageFormat.Png);

                uint id = ClipboardApi.RegisterClipboardFormatW("PNG");

                List<ClipboardEntry> entries = new List<ClipboardEntry>();
                entries.Add(CreateDIBV5Image(image));
                entries.Add(new ClipboardEntry(id, pngMemStream.ToArray()));
                entries.Add(CreateDIBImage(image));

                SetClipboard(entries);
            }
        }
        private static byte[] GetImageData(Bitmap sourceImage, out int stride)
        {
            BitmapData sourceData = sourceImage.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly, sourceImage.PixelFormat);
            stride = sourceData.Stride;
            byte[] data = new byte[stride * sourceImage.Height];
            Marshal.Copy(sourceData.Scan0, data, 0, data.Length);
            sourceImage.UnlockBits(sourceData);
            return data;
        }

        private static ClipboardEntry CreateDIBImage(Bitmap image)
        {
            byte[] imageData;

            using (Bitmap bm32b = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics gr = Graphics.FromImage(bm32b))
                {
                    gr.Clear(Color.White);
                    gr.DrawImage(image, new Rectangle(0, 0, bm32b.Width, bm32b.Height));
                }
                bm32b.RotateFlip(RotateFlipType.Rotate180FlipX);
                imageData = GetImageData(bm32b, out int stride);
            }

            MemoryStream stream = new MemoryStream();
            //BITMAPINFOHEADER
            stream.WriteUInt(0x28); //Size
            stream.WriteInt(image.Width);
            stream.WriteInt(image.Height);
            stream.WriteUShort(1); //bV5Planes
            stream.WriteUShort(32); //bits per pixel
            stream.WriteUInt(3); //Compression: bitfields, with color masks applied to int32 pixel to get specific channels
            stream.WriteUInt((uint)imageData.Length);
            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteUInt(0);
            stream.WriteUInt(0);
            stream.WriteUInt(0x00FF0000); //red bitfield mask
            stream.WriteUInt(0x0000FF00); //green bitfield mask
            stream.WriteUInt(0x000000FF); //blue bitfield mask
            
            stream.Write(imageData);

            return new ClipboardEntry(8, stream.ToArray());
        }

        private static ClipboardEntry CreateDIBV5Image(Bitmap image)
        {
            byte[] imageData;
            
            using (Bitmap bm32b = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics gr = Graphics.FromImage(bm32b))
                    gr.DrawImage(image, new Rectangle(0, 0, bm32b.Width, bm32b.Height));
                bm32b.RotateFlip(RotateFlipType.Rotate180FlipX);
                imageData = GetImageData(bm32b, out int stride);
            }

            MemoryStream stream = new MemoryStream();
            //BITMAPV5HEADER
            stream.WriteUInt(0x7C); //Size
            stream.WriteInt(image.Width);
            stream.WriteInt(image.Height);
            stream.WriteUShort(1); //bV5Planes
            stream.WriteUShort(32); //bits per pixel
            stream.WriteUInt(3); //Compression: bitfields, with color masks applied to int32 pixel to get specific channels
            stream.WriteUInt((uint)imageData.Length);
            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteUInt(0);
            stream.WriteUInt(0);
            stream.WriteUInt(0x00FF0000); //red bitfield mask
            stream.WriteUInt(0x0000FF00); //green bitfield mask
            stream.WriteUInt(0x000000FF); //blue bitfield mask
            stream.WriteUInt(0xFF000000); //alpha bitfield mask
            stream.WriteUInt(0x73524742); //color space = LCS_sRGB
            stream.Write(new byte[12 * 4]); //bytes useless for LCS_sRGB
            stream.WriteUInt(4); //intent = LCS_GM_ABS_COLORIMETRIC
            stream.WriteUInt(0);
            stream.WriteUInt(0);
            stream.WriteUInt(0);
            stream.WriteUInt(0x00FF0000); //apparently bitfield masks again for no fucking reason
            stream.WriteUInt(0x0000FF00);
            stream.WriteUInt(0x000000FF);

            stream.Write(imageData);

            return new ClipboardEntry(17, stream.ToArray());
        }
    }
}
