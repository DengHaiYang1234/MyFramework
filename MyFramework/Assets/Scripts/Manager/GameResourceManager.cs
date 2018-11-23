using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;
using System;

namespace Res
{
    public class GameResourceManager : IUpdate
    {
        public delegate void LoadResCallBack(bool result, string assetName, object prarmeter);




        public void LoadBaseAssetAsyn(string n, LoadResCallBack callback, object prarmeter)
        {
            
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}

