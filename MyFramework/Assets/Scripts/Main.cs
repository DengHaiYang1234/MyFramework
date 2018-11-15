using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{
    /// <summary>
    /// 启动入口
    /// </summary>
    public class Main : MonoBehaviour
    {
        private static Main _instance;

        private void Awake()
        {
            _instance = this;
            Init();
        }

        void Start()
        {
            AppFacade.Instance.StartUp();
        }

        public Camera UiCamera;

        public static Main Instance
        {
            get { return _instance; }
        }

        private static Vector2 _realScreenSize = new Vector2(1280, 720);

        public static Vector2 RealScreenSize
        {
            get { return _realScreenSize; }
            set { _realScreenSize = value; }
        }

        public void Init()
        {
            SDRootPath.Instance.Init();
        }

    }
}


