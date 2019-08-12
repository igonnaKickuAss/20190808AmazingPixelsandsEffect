using UnityEngine;
using UnityEngine.Events;

namespace OLiOYouxi.OSystem.Test
{
    using OLiOCEvents;
    using OLiOUEvents;
    using OLiOUEvents.UEventsBase;

    public class TestUCEventScript : MonoBehaviour
	{
        //程序在这里开始
        private void Start()
		{
            
        }

        #region -- OLiOCEvent --
        //Register
        [OLiOYouxiAttributes.Button("OLiOCEvent注册")]
        private void CEventRegisterTest()
        {
            //Register non param
            CEventsManager.Instance.PutCEventInVoidDic("cv0", () =>
            {
                DebuggerFather.instance.ToDebugLog("这是键cv0的方法1！", EnumCentre.ColorName.red);
            });

            CEventsManager.Instance.PutCEventInVoidDic("cv0", () =>
            {
                DebuggerFather.instance.ToDebugLog("这是键cv0的方法2！", EnumCentre.ColorName.red);
            });

            //Register one param..
            CEventsManager.Instance.PutCEventInVoidDic<TestMessage>("cv1", (o) =>
            {
                DebuggerFather.instance.ToDebugLog($"这是键cv1的方法1！T：age:{o.age} gender:{o.gender} age:{o.age}", EnumCentre.ColorName.red);
            });


            //RegisterObject non param..
            CEventsManager.Instance.PutCEventInObjectDic("co0", () =>
            {
                DebuggerFather.instance.ToDebugLog("这是键co0的方法1！", EnumCentre.ColorName.red);
                return new TestMessage
                {
                    age = "21",
                    gender = "男",
                    name = "奥利奥"
                };
            });

            //RegisterObject one param..
            CEventsManager.Instance.PutCEventInObjectDic<TestMessage>("co1", (o) =>
            {
                DebuggerFather.instance.ToDebugLog($"这是键co1的方法1！T：age:{o.age} gender:{o.gender} age:{o.age}", EnumCentre.ColorName.red);
                return new TestMessage
                {
                    age = "20",
                    gender = "男",
                    name = "迈克尔唐僧"
                };
            });
            
            CEventsManager.Instance.PutCEventInObjectDic<TestMessage>("co1", (o) =>
            {
                DebuggerFather.instance.ToDebugLog($"这是键co1的方法2！T：age:{o.age} gender:{o.gender} age:{o.age}", EnumCentre.ColorName.red);
                return new TestMessage
                {
                    age = "20",
                    gender = "女",
                    name = "徐泰勒"
                };
            });

            print("OLiOCEvent注册成功！！！");
        }


        //Invoke
        [OLiOYouxiAttributes.Button("CV0事件调用")]
        private void CEventCV0Test()
        {
            CEventsManager.Instance.LookUpCEventToVoidDic("cv0");
        }

        [OLiOYouxiAttributes.Button("CV1事件调用")]
        private void CEventCV1Test()
        {
            CEventsManager.Instance.LookUpCEventToVoidDic<TestMessage>("cv1", new TestMessage
            {
                age = "1",
                gender = "诺德人",
                name = "抓根宝"
            });
        }

        [OLiOYouxiAttributes.Button("CO0事件调用")]
        private void CEventCO0Test()
        {
            var o = CEventsManager.Instance.LookUpCEventToObjectDic("co0");

            if (o != null)
                print("co0有返回值！！");
        }

        [OLiOYouxiAttributes.Button("CO1事件调用")]
        private void CEventCO1Test()
        {
            var o = CEventsManager.Instance.LookUpCEventToObjectDic<TestMessage>("co1", new TestMessage
            {
                age = "12",
                gender = "梭莫",
                name = "大使"
            });

            if (o != null)
                print("co1有返回值！！");
        }

        #endregion

        #region -- OLiOUEvent --
        [OLiOYouxiAttributes.BoxGroup("OLiOEvent事件中心")] [SerializeField] private VoidEvent m_VoidEvent = null;
        [OLiOYouxiAttributes.BoxGroup("OLiOEvent事件中心")] [SerializeField] private StringEvent m_StringEvent = null;

        //Register
        [OLiOYouxiAttributes.Button("OLiOUEvent注册")]
        private void UEventRegisterTest()
        {
            //Register non param
            UEventsManager.Instance.PutUEventInVoidDic(m_VoidEvent, "uv0");
            m_VoidEvent.AddListener(new UnityAction(NonParamInScript));

            //Register one param
            UEventsManager.Instance.PutUEventInVoidDic<string>(m_StringEvent, "uv1");
            m_StringEvent.AddListener(new UnityAction<string>(OneParamInScript));

            print("OLiOUEvent注册成功！！！");
        }
        
        public void NonParamInScript()
        {
            DebuggerFather.instance.ToDebugLog("这是uv0的方法NonParamInScript！！", EnumCentre.ColorName.red);
        }

        public void NonParamInInspector()
        {
            DebuggerFather.instance.ToDebugLog("这是uv0的方法NonParamInInspector！！", EnumCentre.ColorName.red);
        }

        public void OneParamInScript(string s)
        {
            DebuggerFather.instance.ToDebugLog($"这是uv1的方法OneParamInScript！！ 参数：{s}", EnumCentre.ColorName.red);
        }

        public void OneParamInInspector(string s)
        {
            DebuggerFather.instance.ToDebugLog($"这是uv1的方法OneParamInInspector！！ 参数：{s}", EnumCentre.ColorName.red);
        }

        //Invoke
        [OLiOYouxiAttributes.Button("UV0事件调用")]
        private void UEventUV0Test()
        {
            UEventsManager.Instance.LookUpUEventToVoidDic("uv0");
        }

        [OLiOYouxiAttributes.Button("UV1事件调用")]
        private void UEventUV1Test()
        {
            UEventsManager.Instance.LookUpUEventToVoidDic<string>("uv1", "奥利奥");
        }

        #endregion

    }

    class TestMessage
    {
        public string name;
        public string gender;
        public string age;
    }
}