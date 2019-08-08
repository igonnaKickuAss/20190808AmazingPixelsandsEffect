using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    [DisallowMultipleComponent]
    public class TimeController : MonoBehaviour
    {
        #region -- 单例 --
        static private TimeController timeController = null;
        static public TimeController instance
        {
            get
            {
                if (!timeController)
                {
                    timeController = FindObjectOfType<TimeController>() as TimeController;
                    if (!timeController)
                    {
                        DebuggerFather.instance.ToDebugLogErr("你得在Scene中激活一个携带TimeController组件的GameObject。", EnumCentre.ColorName.red);
                    }
                    else
                    {
                        timeController.InitData();
                    }
                }
                return timeController;
            }
        }

        private void InitData()
        {
            if (m_Coroutines == null)
                m_Coroutines = new Dictionary<int, Coroutine>();

        }

        #endregion


        #region -- Private Data --
        private Dictionary<int, Coroutine> m_Coroutines = null;

        #endregion

        #region -- Public APIMethods --
        public int AddTimeController(Action action, float delay, int repeat)
        {
            int randomKey;
            do
            {
                randomKey = UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue);
            }
            while (m_Coroutines.ContainsKey(randomKey));
            Coroutine coroutine = StartCoroutine(DelayFunc(action, delay, repeat, randomKey));
            m_Coroutines.Add(randomKey, coroutine);

            //返回令牌(靠这个结束该协程)
            return randomKey;
        }

        public void StopTimeController(int key)
        {
            if (m_Coroutines.ContainsKey(key))
                StopCoroutine(m_Coroutines[key]);
        }

        #endregion

        #region -- Private APIMethods --
        private IEnumerator DelayFunc(Action action, float delay, int repeat, int key)
        {
            while (repeat-- != 0)
            {
                yield return YieReturns.instance.GetWaitForOneSeccond();
                action();
            }

            m_Coroutines.Remove(key);
        }

        #endregion
        
    }
}

