using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFramework
{
    public class DownPanel : MonoBehaviour
    {
        static Text text_1;
        static Text text_2;
        static Text text_Title;
        private static Image sprite;

        private void Awake()
        {
            text_1 = transform.Find("Test1").gameObject.GetComponent<Text>();
            text_2 = transform.Find("Test2").gameObject.GetComponent<Text>();
            text_Title = transform.Find("Title").gameObject.GetComponent<Text>();
            sprite = transform.Find("sprite").gameObject.GetComponent<Image>();
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
                Util.LogErr("不存在 fileName  fileName  fileName！！！！！！");
            }
        }

        public static void GetTitle()
        {
            Util.Log("=======  =======  =======  =======text_Title:" + text_Title.text);
        }

        public static void SetSprite(string name)
        {
            //sprite.sprite = ResMain.Instance.GetSpriteByName(name);
        }

    }

        //public static void SetProgressValue(string str)
        //{
        //    if (progressValue == null)
        //    {
        //        GameObject panel = GetObj();
        //        progressValue = panel.transform.Find("Canvas/DownPro").gameObject.GetComponent<Text>();

        //        progressValue.text = str;
        //    }

        //}

        //public static void SetFileValue(string str)
        //{
        //    if (fileName == null)
        //    {
        //        GameObject panel = GetObj();

        //        if (panel.transform.Find("Canvas/DownName") != null)
        //        {
        //            fileName = panel.transform.Find("Canvas/DownName").gameObject.GetComponent<Text>();

        //            fileName.text = str;
        //        }
        //        else
        //        {
        //            Util.LogErr("不存在 fileName  fileName  fileName！！！！！！");
        //        }
        //    }
        //}

        //public static GameObject GetObj()
        //{
        //    GameObject panel = GameObject.Find("DownLoadPanel(Clone)") as GameObject;
        //    return panel;
        //}
    //}
}

