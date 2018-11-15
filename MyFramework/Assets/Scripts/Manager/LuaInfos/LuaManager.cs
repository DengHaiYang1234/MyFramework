using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

 
namespace MyFramework
{
    public class LuaManager : BaseClass
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
            loader = new LuaLoader();
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);
            LuaBinder.Bind(lua);
            LuaCoroutine.Register(lua, this);
        }

        /// <summary>
        /// 手动初始化
        /// </summary>
        public void InitStart()
        {
            InitLuaPath();
            InitLuaBunlde();
            this.lua.Start();
            this.StartMain();
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
            if (AppConst.DebugMode)
            {
                string rootPath = AppConst.HotFixRoot;
                lua.AddSearchPath(rootPath + "/LuaScripts/");
                lua.AddSearchPath(rootPath + "/ToLua/Lua/");
            }
            else
                lua.AddSearchPath(Util.DataPath + "lua");
        }

        /// <summary>
        /// 初始化  添加AssetBundle文件
        /// </summary>
        void InitLuaBunlde()
        {
            if (loader.beZip)
            {
                loader.AddBundle("lua/lua");
                loader.AddBundle("lua/lua_cjson");
                loader.AddBundle("lua/lua_misc");
                loader.AddBundle("lua/lua_system");
                loader.AddBundle("lua/lua_system_reflection");
                loader.AddBundle("lua/lua_unityengine");
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
