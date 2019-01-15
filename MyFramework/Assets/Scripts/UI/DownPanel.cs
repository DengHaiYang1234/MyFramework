using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Res;

namespace MyFramework
{
    public class DownPanel : BasePanel
    {
        static Text text_1;
        static Text text_2;
        static Text text_Title;
        private static Image sprite;
        private static Image BG;
        private static MyAsset asset = null;
        private void Awake()
        {
            text_1 = transform.Find("Test1").gameObject.GetComponent<Text>();
            text_2 = transform.Find("Test2").gameObject.GetComponent<Text>();
            text_Title = transform.Find("Title").gameObject.GetComponent<Text>();
            sprite = transform.Find("sprite").gameObject.GetComponent<Image>();
            BG = transform.Find("BG").gameObject.GetComponent<Image>();
            Owner = this;
            TestLoad();
        }



        public static void SetProgressValue(string str)
        {
            text_1.text = str;

        }

        public static void SetFileValue(string str)
        {
            if (text_2 != null)
            {
                text_2.text = str;
            }
            else
            {
                MyDebug.LogError("不存在 fileName  fileName  fileName！！！！！！");
            }
        }

        public static void GetTitle()
        {
            MyDebug.Log("=======  =======  =======  =======text_Title:" + text_Title.text);
        }

        public static void SetSprite(string name)
        {
            MyDebug.LogError("设置图片：" + name);
            //asset = FrameworkMain.Instance.ResMgr.LoadAsset<Sprite>(name);
            //sprite.sprite = asset.asset as Sprite;
            MyDebug.LogError("背景图片名称：" + BG.sprite);
        }

        public void TestLoad()
        {
            //transform.Find("BG (2)").gameObject.GetComponent<RawImage>().texture =  FrameworkMain.Instance.ResMgr.LoadAsset<Texture>("futianjizhao").asset as Texture;
            transform.Find("BG (2)").gameObject.GetComponent<RawImage>().texture = OnLoadAssets<Texture>("futianjizhao");
            OnLoadAssets<Texture>("anxijiaolian");
            OnLoadAssets<Texture>("chishanglianger");
            OnLoadAssets<Texture>("gaojinhong");
            OnLoadAssets<Texture>("hehemali");
            OnLoadAssets<Texture>("jiaotianwu");

            OnLoadAssets<Sprite>("ScoiatyExperienceCard");
            OnLoadAssets<Sprite>("ScoiatyExperienceCard");
            OnLoadAssets<Sprite>("ScoiatyExperienceCard");
            OnLoadAssetsSync<Sprite>("ScoiatyExperienceCard",null);
            OnLoadAssetsSync<Sprite>("ScoiatyExperienceCard", null);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
