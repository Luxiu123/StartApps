using CommonUITools.Model;
using CommonUITools.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StartApp.Util;

public static class Utils {
    /// <summary>
    /// 获取可执行程序图标
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    [NoException]
    public static Stream? GetExeBitmap(string path) {
        return TaskUtils.Try(() => {
            uint _nIcons = PInvokeHelpers.PrivateExtractIcons(path, 0, 0, 0, null, null, 0, 0);
            if (_nIcons < 1) {
                return null;
            }
            IntPtr[] phicon = new IntPtr[_nIcons];
            uint[] piconid = new uint[_nIcons];
            _ = PInvokeHelpers.PrivateExtractIcons(path, 0, 128, 128, phicon, piconid, _nIcons, 0);
            Icon icon = Icon.FromHandle(phicon[0]);
            Bitmap bitmap = icon.ToBitmap();
            PInvokeHelpers.DestroyIcon(icon.Handle);
            Stream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        });
    }
}
