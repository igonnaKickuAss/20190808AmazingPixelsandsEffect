using UnityEngine;
using System.Collections;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;
    using OLiOYouxiAttributes;

    [DisallowMultipleComponent]
	public class MainUpdateController : MonoBehaviour
	{
        #region -- Private Reference --
        [BoxGroup("引用类型监听：")] [Required] [Label("玩家一")] [SerializeField] private ACharacterHealth playerHealth0 = null;
        [BoxGroup("引用类型监听：")] [Required] [Label("玩家二")] [SerializeField] private ACharacterHealth playerHealth1 = null;

        #endregion

        #region -- Private Data --
        private Coroutine mainUpdateCoro = null;

        #endregion

        #region -- Invoke Data --



        #endregion

        #region -- MONO APIMethods --
        private void Awake()
        {
            mainUpdateCoro = StartCoroutine(MainUpdate());
        }

        #endregion

        #region -- CORO --
        private IEnumerator MainUpdate()
        {
            if (!(playerHealth0 && playerHealth1))
                yield break;

            while (Application.isPlaying) 
            {
                yield return YieReturns.instance.GetWaitForEndOfFrame();

                if (playerHealth0.isDead)
                    DebuggerFather.instance.ToDebugLog("游戏结束", EnumCentre.ColorName.red);
                else if (playerHealth1.isDead)
                    DebuggerFather.instance.ToDebugLog("游戏结束", EnumCentre.ColorName.red);

            }
        }

        #endregion

    }
}