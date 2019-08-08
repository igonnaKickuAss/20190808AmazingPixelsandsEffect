using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OLiOYouxiAttributes.Editor
{
    static public class EncodingUtility
    {
        //参考： https://blog.csdn.net/qq_39097425/article/details/83899708
        #region -- Private Data --
        static private UTF8Encoding utf8Encoding;
        static private UnicodeEncoding utf16Encoding;
        static private ASCIIEncoding asciiEncoding;
        #endregion

        #region -- ShotC --
        static public Encoding UTF8
        {
            get
            {
                if (utf8Encoding == null)
                {
                    utf8Encoding = new UTF8Encoding(true);
                }
                return utf8Encoding;
            }
        }

        static public Encoding UTF16
        {
            get
            {
                if (utf16Encoding == null)
                {
                    utf16Encoding = new UnicodeEncoding();
                }
                return utf16Encoding;
            }
        }

        static public Encoding ASCII
        {
            get
            {
                if (asciiEncoding == null)
                {
                    asciiEncoding = new ASCIIEncoding();
                }
                return asciiEncoding;
            }
        }
        #endregion

    }
}
