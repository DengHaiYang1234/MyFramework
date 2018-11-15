﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class MyFramework_UtilWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(MyFramework.Util), typeof(System.Object));
		L.RegFunction("Log", Log);
		L.RegFunction("LogErr", LogErr);
		L.RegFunction("LogWarn", LogWarn);
		L.RegFunction("MD5File", MD5File);
		L.RegFunction("AppContentPath", AppContentPath);
		L.RegFunction("CallMethod", CallMethod);
		L.RegFunction("TrimPath", TrimPath);
		L.RegFunction("LoadAsset", LoadAsset);
		L.RegFunction("GetPlatfromFoldername", GetPlatfromFoldername);
		L.RegFunction("New", _CreateMyFramework_Util);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("DataPath", get_DataPath, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateMyFramework_Util(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				MyFramework.Util obj = new MyFramework.Util();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: MyFramework.Util.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Log(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			object arg0 = ToLua.ToVarObject(L, 1);
			MyFramework.Util.Log(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogErr(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			object arg0 = ToLua.ToVarObject(L, 1);
			MyFramework.Util.LogErr(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarn(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			object arg0 = ToLua.ToVarObject(L, 1);
			MyFramework.Util.LogWarn(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MD5File(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = MyFramework.Util.MD5File(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AppContentPath(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			string o = MyFramework.Util.AppContentPath();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallMethod(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			string arg0 = ToLua.CheckString(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
			object[] o = MyFramework.Util.CallMethod(arg0, arg1, arg2);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TrimPath(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = MyFramework.Util.TrimPath(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAsset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.AssetBundle arg0 = (UnityEngine.AssetBundle)ToLua.CheckObject(L, 1, typeof(UnityEngine.AssetBundle));
			string arg1 = ToLua.CheckString(L, 2);
			UnityEngine.GameObject o = MyFramework.Util.LoadAsset(arg0, arg1);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPlatfromFoldername(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			string o = MyFramework.Util.GetPlatfromFoldername();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DataPath(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, MyFramework.Util.DataPath);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

