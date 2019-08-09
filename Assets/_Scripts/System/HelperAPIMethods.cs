using System;
using UnityEngine;

namespace OLiOYouxi.OSystem
{
    static public class HelperAPIMethods
    {
        /// <summary>
        /// 任意方向的速度
        /// </summary>
        /// <param name="v"></param>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        static public void RandomVelocity(ref Vector2 v, float xMin, float xMax, float yMin, float yMax)
        {
            v.Set(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax));
        }

        static public void EnumToString<T>(T e, out string es)
        {
            es = string.Empty;
            if (typeof(T).IsEnum)
                es = e.ToString();

        }

        static public void StringToEnum<T>(string es, out T e)
        {
            e = default(T);
            if (typeof(T).IsEnum)
                e = (T)Enum.Parse(typeof(T), es);

        }

        /// <summary>
        /// 近似值比较
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        static public bool ApproxEqual(this Color c1, Color c2, float epsilon = .05f)
        {
            return (c1[0] - c2[0]) * (c1[0] - c2[0])
                + (c1[1] - c2[1]) * (c1[1] - c2[1])
                    + (c1[2] - c2[2]) * (c1[2] - c2[2])
                    + (c1[3] - c2[3]) * (c1[3] - c2[3]) < epsilon * epsilon;
        }

        /// <summary>
        /// 是灰色妈
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static public bool IsGrayscale(this Color c)
        {
            return c.r == c.g && c.r == c.b;
        }

        /// <summary>
        /// 是蓝色吗
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static public bool IsBluescale(this Color c)
        {
            return (c.r * 255 == 154 && c.g * 255 == 190 && c.b * 255 == 255) || c.r * 255 == 0 && c.g * 255 == 100 && c.b * 255 == 255;
        }
    }
}

