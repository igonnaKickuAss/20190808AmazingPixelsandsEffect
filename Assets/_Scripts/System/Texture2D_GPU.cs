using UnityEngine;

namespace OLiOYouxi.OSystem
{
    public class Texture2D_GPU
    {
        #region -- Private Data --
        int width, height;
        gColor[] gColorArr = null;
        ComputeShader shader = null;
        RenderTexture rTexture = null;
        #endregion

        #region -- Public Data --
        public Texture texture = null;
        public Texture2D texture2D = null;
        #endregion

        #region -- Structs --
        public struct gColor
        {
            public Vector4 color;
        }

        #endregion

        #region -- Public APIMethods --
        public Texture2D_GPU(int width, int height, ComputeShader shader)
        {
            this.width = width;
            this.height = height;
            this.shader = shader;
            this.gColorArr = new gColor[this.width * this.height];  //一维数组，vector4存放颜色
            this.rTexture = new RenderTexture(this.width, this.height, 24);
            this.inputbuffer = new ComputeBuffer(gColorArr.Length, 16);
        }

        public gColor[] GetPixels(Texture2D tex)
        {
            ComputeBuffer outputBuffer = new ComputeBuffer(gColorArr.Length, 16);
            int k = shader.FindKernel("GetPixels");

            outputBuffer.SetData(gColorArr);

            shader.SetBuffer(k, "ResultG", outputBuffer);
            shader.SetTexture(k, "tex", tex);
            shader.SetInt("w", width);

            shader.Dispatch(k, width / 8, height / 8, 1);

            outputBuffer.GetData(gColorArr);

            outputBuffer.Dispose();

            return gColorArr;
        }

        public void SetColorArr()
        {
            GetPixels(this.texture2D);
        }

        public Color GetPixel(int x, int y)
        {
            int i = (width * y) + x;    //唯一像素坐标位置数据
            return gColorArr[i].color;
        }

        public void SetPixel(int x, int y, Vector4 color)
        {
            int i = (width * y) + x;    //唯一像素坐标位置数据
            gColorArr[i].color = color;
        }

        public void SetPixels(Color[] color)
        {
            for (int i = 0; i < gColorArr.Length; i++)
            {
                gColorArr[i].color = color[i];
            }
        }

        ComputeBuffer inputbuffer;    //一个float值为多大

        public void Apply()
        {
            rTexture.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            rTexture.wrapMode = TextureWrapMode.Repeat;
            rTexture.filterMode = FilterMode.Point;
            rTexture.enableRandomWrite = true;
            rTexture.Create();
            texture = rTexture;

            //ComputeBuffer inputbuffer = new ComputeBuffer(gColorArr.Length, 16);    //一个float值为多大

            int k = shader.FindKernel("Apply");

            // 写入 GPU   //设置参数
            inputbuffer.SetData(gColorArr);
            shader.SetBuffer(k, "colors", inputbuffer);

            shader.SetInt("width", width);

            shader.SetTexture(k, "ResultA", texture);
            shader.Dispatch(k, width / 32, height / 32, 1);

            TextureToTexture2D(ref texture);

            //inputbuffer.Dispose();  //释放
        }

        #endregion

        #region -- Private APIMethods --
        private void TextureToTexture2D(ref Texture texture)
        {
            //Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
            //texture2D.wrapMode = TextureWrapMode.Repeat;
            //texture2D.filterMode = FilterMode.Point;

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
            Graphics.Blit(texture, renderTexture);

            RenderTexture.active = renderTexture;
            this.texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            this.texture2D.Apply();

            RenderTexture.active = currentRT;
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        #endregion


    }
}