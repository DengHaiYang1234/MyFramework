using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;



namespace MyFramework
{
    public class Util
    {
        #region  辅助方法
        /// <summary>
        /// 编写文件的MD5校验码
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string MD5File(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for(int i = 0;i < retVal.Length;i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch(Exception ex)
            {
                throw new System.Exception("md5File{} fail,err:" + ex.Message);
            }
        }

        /// <summary>
        /// 访问lua方法
        /// </summary>
        /// <param name="module"></param>  文件名
        /// <param name="func"></param> 方法名
        /// <param name="args"></param>
        /// <returns></returns>
        public static object[] CallMethod(string module, string func, params object[] args)
        {
            LuaManager luaMgr = GameFacade.Instance.GetManager<LuaManager>(ManagersName.lua);
            if (luaMgr == null)
                return null;
            return luaMgr.CallFunction(TrimPath(module) + "." + func);
        }

        /// <summary>
        /// 截取最后一个‘/’之后的路径.  例：Main.main
        /// </summary>
        /// <param name="origName"></param>
        /// <returns></returns>
        public static string TrimPath(string origName)
        {
            string fileName = origName;
            if (fileName.IndexOf('/') != -1)
            {
                fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
            }

            if (fileName.IndexOf('.') != -1)
                return fileName.Substring(0, fileName.LastIndexOf('.'));

            return fileName;
        }


        #endregion
    }
}

