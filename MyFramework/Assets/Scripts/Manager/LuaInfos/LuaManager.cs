using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using Res;

 
namespace MyFramework
{
    public class LuaManager : MonoBehaviour
    {
        # region Lua管理类.主要负责通过lua虚拟机，加载Lua文件或方法
        private LuaState lua;
        private LuaLoader loader;
        private LuaLooper loop = null;

        /// <summary>
        /// LuaManager启动初始化
        /// </summary>
        private void Awake()
        {
            loader = new LuaLoader();//Lua  AssetBundle加载
            lua = new LuaState();//创建tolua提供的LuaState对象。
            this.OpenLibs();//启动第三方库
            lua.LuaSetTop(0);
            LuaBinder.Bind(lua);//向lua注册C#的代码
            LuaCoroutine.Register(lua, this);//注册协程
        }

        /// <summary>
        /// 手动初始化
        /// </summary>
        public void InitStart()
        {
            InitLuaPath();
            InitLuaBunlde();
            this.lua.Start(); //启动lua虚拟机
            //this.StartMain();
            this.StartLooper();
        }

        /// <summary>
        /// 初始第三方库
        /// </summary>
        void OpenLibs()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }



        /// <summary>
        /// 初始化lua文件的加载路径
        /// </summary>
        void InitLuaPath()
        {
            if (FrameworkDefaultSetting.DebugMode)
            {
                lua.AddSearchPath(RuntimeResPath.GetLuaDataPath );
                lua.AddSearchPath(RuntimeResPath.GetToLuaDataPath);
            }
            else
                lua.AddSearchPath(RuntimeResPath.GetLuaAssetsDataPath);
        }

        /// <summary>
        /// 初始化  添加AssetBundle文件
        /// </summary>
        void InitLuaBunlde()
        {
            if (loader.beZip)
            {
                loader.AddBundle("lua");
                loader.AddBundle("lua_cjson");
                loader.AddBundle("lua_misc");
                loader.AddBundle("lua_system");
                loader.AddBundle("lua_system_reflection");
                loader.AddBundle("lua_unityengine");
            }
        }

        /// <summary>
        /// 入口
        /// </summary>
        void StartMain()
        {
            lua.DoFile("Main.lua"); //其实就是包装了Loadfile，根据loadfile的返回函数运行一遍。  dofile每次加载,loadfile只加载文件而不执行。 
            LuaFunction main = lua.GetFunction("Main.main");
            main.Call();
            main.Dispose();
            main = null;
        }

        void StartLooper()
        {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        /// <summary>
        /// 加载Lua文件
        /// </summary>
        /// <param name="fileName"></param>
        public void DoFile(string fileName)
        {
            lua.DoFile(fileName);
        }

        /// <summary>
        /// 加载Lua文件中的lua方法
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object[] CallFunction(string funcName, params object[] args)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
                return func.Call(args);

            return null;
        }

        #endregion
    }
}
