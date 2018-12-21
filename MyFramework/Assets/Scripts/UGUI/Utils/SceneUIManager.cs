using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFramework
{
    public class SceneUIManager : MonoBehaviour
    {
        public GameObject layer1;
        public GameObject layer2;
        public GameObject layer3;
        public GameObject layer4;
        public GameObject layer5;
        public GameObject mask;

        private void Awake()
        {
            LuaDataAgent.UILayers.Layer_1 = layer1;
            LuaDataAgent.UILayers.Layer_2 = layer2;
            LuaDataAgent.UILayers.Layer_3 = layer3;
            LuaDataAgent.UILayers.Layer_4 = layer4;
            LuaDataAgent.UILayers.Layer_5 = layer5;
            LuaDataAgent.UILayers.Layer_Mask = mask;
            //Canvas can = this.gameObject.GetComponent<Canvas>();
            //can.worldCamera = FrameworkMain.Instance.UiCamera;
        }

        

        void ChgReferenceResolution()
        {
            float fFlag = 16/(float) 9;
            float fCurr = Screen.width/(float) Screen.height;
            float fy = 0;
            if (fCurr >= fFlag)
                fy = 720;
            else
                fy = 960;

            CanvasScaler canvasScalerTemp = transform.GetComponent<CanvasScaler>();
            Vector2 vDest = Vector2.zero;
            vDest.x = canvasScalerTemp.referenceResolution.x;
            vDest.y = fy;
            canvasScalerTemp.referenceResolution = vDest;

            FrameworkMain.RealScreenSize = vDest;
        }
    }
}


