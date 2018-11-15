using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyFramework
{
    /// <summary>
    /// 传统单利类
    /// </summary>
    public class ClassicalSingleton<T> where T : class,new()
    {
        protected static T mInstace;

        public static T Instance
        {
            get
            {
                if (mInstace == null)
                    mInstace = new T();

                return mInstace;
            }
        }

    }
}

