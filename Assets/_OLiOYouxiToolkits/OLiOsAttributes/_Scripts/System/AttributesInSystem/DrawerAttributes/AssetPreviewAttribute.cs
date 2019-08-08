using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 绘制一个资源的预览图
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AssetPreviewAttribute : ADrawerAttribute
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// 绘制一个资源的预览图，三滴项目为透视预览图，二滴项目为正交预览图
        /// </summary>
        /// <param name="width">默认64</param>
        /// <param name="height">默认64</param>
        public AssetPreviewAttribute(int width = 64, int height = 64)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
