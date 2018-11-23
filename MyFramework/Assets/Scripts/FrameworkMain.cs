using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework
{
    /// <summary>
    /// 启动入口
    /// </summary>
    public class FrameworkMain : MonoBehaviour
    {
        #region private
        private static FrameworkMain _instance;
        private static Vector2 _realScreenSize = new Vector2(1280, 720);
        private readonly StateMachine<FrameworkMain> _gameStateFsm = new StateMachine<FrameworkMain>();
        private bool _doFsmUpdate = false;
        #endregion

        #region public
        public Camera UiCamera;
        #endregion

        #region attribute
        public static FrameworkMain Instance
        {
            get { return _instance; }
        }

        public static Vector2 RealScreenSize
        {
            get { return _realScreenSize; }
            set { _realScreenSize = value; }
        }

        public StateMachine<FrameworkMain> FSM
        {
            get { return _gameStateFsm; }
        }

        public bool Run
        {
            set { _doFsmUpdate = value; }
        }
        #endregion

        #region function

        private void Awake()
        {
            _instance = this;
            Init();
            InitGlobalManager();
            InitFsm();
            Loom.Initialize();
        }

        void Start()
        {
            DoGameStart();
        }

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            SDRootPath.Instance.Init();
            LTDebugOutput.Instance.Init();
        }
        
        //启动游戏
        public void InitGlobalManager()
        {
            AppFacade.Instance.StartUp();
        }

        //初始状态机
        private void InitFsm()
        {
            _gameStateFsm.Add(new CheckStage(this));
            _gameStateFsm.Add(new GameUIState(this));
        }

        //初始状态
        private void DoGameStart()
        {
            _gameStateFsm.ChangeState((int) MyStage.check);
        }

        //状态Running
        private void Update()
        {
            if (_doFsmUpdate)
                _gameStateFsm.currState.OnRunning(null);
        }

        //获取Manager
        public T GetManager<T>(string name) where T : Component
        {
            return AppFacade.Instance.GetManager<T>(name);
        }

        #endregion
    }
}


