using UnityEngine;

namespace OLiOYouxi.OSystem
{
    public class YieReturns
    {
        //单例
        static private YieReturns yieReturns = null;
        static public YieReturns instance
        {
            get
            {
                if (yieReturns != null)
                {
                    return yieReturns;
                }
                else
                {
                    yieReturns = new YieReturns();
                    yieReturns.Init();
                    return yieReturns;
                }

            }
        }


        #region -- Private Data --
        private WaitForEndOfFrame waitForEndOfFrame = null;
        private WaitForFixedUpdate waitForFixedUpdate = null;
        private WaitForSeconds waitForOne = null;
        private WaitForSeconds waitForTwo = null;
        private WaitForSecondsRealtime waitForOneRealtime = null;
        private WaitForSecondsRealtime waitForTwoRealtime = null;


        #endregion


        #region -- 初始化 --
        private void Init()
        {
            waitForEndOfFrame = new WaitForEndOfFrame();
            waitForFixedUpdate = new WaitForFixedUpdate();
            waitForOne = new WaitForSeconds(1.0f);
            waitForTwo = new WaitForSeconds(2.0f);
            waitForOneRealtime = new WaitForSecondsRealtime(1.0f);
            waitForTwoRealtime = new WaitForSecondsRealtime(2.0f);
        }


        #endregion


        #region -- MONO --
        public WaitForEndOfFrame GetWaitForEndOfFrame()
        {
            return waitForEndOfFrame;
        }

        public WaitForFixedUpdate GetWaitForFixedUpdate()
        {
            return waitForFixedUpdate;
        }   
        





        public WaitForSeconds GetWaitForOneSeccond()
        {
            return waitForOne;
        }

        public WaitForSeconds GetWaitForTwoSecconds()
        {
            return waitForTwo;
        }

        public WaitForSecondsRealtime GetWaitForOneSeccondRealtime()
        {
            return waitForOneRealtime;
        }

        public WaitForSecondsRealtime GetWaitForTwoSeccondsRealtime()
        {
            return waitForTwoRealtime;
        }

        #endregion

    }
}
