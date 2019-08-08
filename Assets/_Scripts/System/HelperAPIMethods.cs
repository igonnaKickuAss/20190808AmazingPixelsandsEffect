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
    }
}

