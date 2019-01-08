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
            sprite.sprite = FrameworkMain.Instance.ResMgr.Load<Sprite>(name);
        }

    }
}

