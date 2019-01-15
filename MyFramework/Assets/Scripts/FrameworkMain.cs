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

        private LuaManager luaMgr;
        private ResourceManager resMgr;
        private HotManager hotMgr;
        private ThreadManager threadMgr;
        private UIManager uiMgr;

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

        public LuaManager LuaMgr
        {
            get { return luaMgr; }
        }

        public ResourceManager ResMgr
        {
            get { return resMgr; }
        }

        public HotManager HotMgr
        {
            get { return hotMgr; }
        }

        public ThreadManager ThreadMgr
        {
            get { return threadMgr; }
        }

        public UIManager UIMgr
        {
            get { return uiMgr; }
        }



        /// <summary>
        /// 状态机
        /// </summary>
        public StateMachine<FrameworkMain> FSM
        {
            get { return _gameStateFsm; }
        }
        /// <summary>
        /// 启动state Running
        /// </summary>
        public bool Run
        {
            set { _doFsmUpdate = value; }
        }
        #endregion

        #region function

        private void Awake()
        {
            ClearConsole();
            Debug.Log("Launcher 启动");
            DontDestroyOnLoad(gameObject);
            _instance = this;
            Init();
            InitFsm();
            Loom.Initialize();//初始化消息监听
        }

        void Start()
        {
            DoGameStart();
        }

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            DebugRootPath.Instance.Init(); //初始化UI Canvas
            DebugOutput.Instace.Init();//初始化 Debug管理
            GameFacade.Instance.StartUp(); //启动游戏
            InitManager();
        }
        
        //初始状态机
        private void InitFsm()
        {
            _gameStateFsm.Add(new CheckStage(this));
            _gameStateFsm.Add(new StartGameState(this));
            MyDebug.Log("状态机初始化完毕");
        }

        private void InitManager()
        {
            resMgr = GetManager<ResourceManager>(ManagersName.resource);
            luaMgr = GetManager<LuaManager>(ManagersName.lua);
            hotMgr = GetManager<HotManager>(ManagersName.hot);
            threadMgr =  GetManager<ThreadManager>(ManagersName.thread);
            uiMgr = GetManager<UIManager>(ManagersName.ui);
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
            return GameFacade.Instance.GetManager<T>(name);
        }


        private void ClearConsole()
        {
#if UNITY_EDITOR
            var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
#endif
        }

        #endregion
    }
}


