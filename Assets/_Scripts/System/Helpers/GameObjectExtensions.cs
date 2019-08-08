using System.Collections;
using UnityEngine;

namespace OLiOYouxi.OSystem.Helpers
{
    static public class GameObjectExtensions
    {
        /// <summary>
        /// 是否激活该GameObject
        /// </summary>
        /// <param name="go">对象</param>
        /// <param name="active">true&not</param>
        static public void TrySetActive(this GameObject go, bool active)
        {
            
            if(go.activeInHierarchy != active)
            {
                go.SetActive(active);
            }
        }

        /// <summary>
        /// 是否激活该GameObject
        /// </summary>
        /// <param name="trans">对象</param>
        /// <param name="active">true&not</param>
        static public void TrySetActive(this Transform trans, bool active)
        {
            trans.gameObject.TrySetActive(active);
        }


        static public Vector2 Get01Position(this RectTransform rt, Vector2 pos, Camera camera)
        {
            Vector2 p;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, pos, camera, out p);
            p = new Vector2(
                (p.x + rt.rect.width * rt.pivot.x) / rt.rect.width,
                (p.y + rt.rect.height * rt.pivot.y) / rt.rect.height
            );
            return p;
        }


        /// <summary>
        /// 销毁子GameObject,推荐在Game里使用
        /// </summary>
        /// <param name="tf">父对象</param>
        static public void ClearChildren(this Transform tf)
        {
            int len = tf.childCount;
            for (int i = 0; i < len; i++)
            {
                Object.Destroy(tf.GetChild(i).gameObject);
            }
        }


        /// <summary>
        /// 销毁子GameObject,推荐在Game里使用
        /// </summary>
        /// <param name="tf">父对象</param>
        /// <param name="action">无返回值委托,参数是子GameObject</param>
        static public void ClearChildren(this Transform tf, System.Action<Transform> action = null)
        {
            int len = tf.childCount;
            for (int i = 0; i < len; i++)
            {
                Transform t = tf.GetChild(i);
                if (action != null)
                {
                    action.Invoke(t);
                }
                Object.Destroy(t.gameObject);
            }
        }

        /// <summary>
        /// 在子GameObject执行了一段耗时间的协程方法后依序销毁子GameObject,推荐在Game里使用
        /// 该协程有可能是位移等等
        /// </summary>
        /// <param name="tf">父对象</param>
        /// <param name="func">带返回值(Coroutine)委托,参数是子GameObject</param>
        /// <returns>Coroutine:Func委托返回的是协程</returns>
        static public IEnumerator ClearChildren(this Transform tf, System.Func<Transform, Coroutine> func = null)
        {
            for (int i = 0; i <= tf.childCount; i++)
            {
                //TODO..因为yield return之后tf.childCount会变化,如果不-1,就会出现Transform child out of bounds错误
                if (i == 2)
                {
                    i--;
                }
                Transform t = tf.GetChild(i);
                if (func != null)
                {
                    //TODO..等待协程完成
                    yield return func.Invoke(t);
                }
                //TODO..销毁该GameObject
                Object.Destroy(t.gameObject);

            }
        }

        /// <summary>
        /// 在父GameObject执行了一段耗时间的协程方法后依序销毁子GameObject,推荐在Game里使用
        /// 该协程有可能是位移等等
        /// </summary>
        /// <param name="tf">父对象</param>
        /// <param name="action">无返回值委托,参数是子GameObject</param>
        /// <param name="wasted">耗时间的协程</param>
        /// <returns>Coroutine:参数wasted是协程</returns>
        static public IEnumerator ClearChildren(this Transform tf, System.Action<Transform> action = null, Coroutine wasted = null)
        {
            int len = tf.childCount;
            for (int i = 0; i < len; i++)
            {
                yield return wasted;
                Transform t = tf.GetChild(i);
                if (action != null)
                {
                    action.Invoke(t);
                }
                Object.Destroy(t.gameObject);
            }
        }


        /// <summary>
        /// 立即销毁子GameObject,推荐在Editor里使用
        /// </summary>
        /// <param name="tf">对象</param>
        public static void ClearChildrenImmediately(this Transform tf)
        {
            int len = tf.childCount;
            for (int i = 0; i < len; i++)
            {
                Object.DestroyImmediate(tf.GetChild(0).gameObject, false);
            }
        }


        static public bool ApproxEqual(this Color c1, Color c2, float epsilon = .05f)
        {
            return (c1[0] - c2[0]) * (c1[0] - c2[0])
                + (c1[1] - c2[1]) * (c1[1] - c2[1])
                    + (c1[2] - c2[2]) * (c1[2] - c2[2])
                    + (c1[3] - c2[3]) * (c1[3] - c2[3]) < epsilon * epsilon;
        }

        static public bool IsGrayscale(this Color c)
        {
            return c.r == c.g && c.r == c.b;
        }

        static public bool IsBluescale(this Color c)
        {
            return (c.r * 255 == 154 && c.g * 255 == 190 && c.b * 255 == 255) || c.r * 255 == 0 && c.g * 255 == 100 && c.b * 255 == 255;
        }

    }
}