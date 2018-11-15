using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LTDebugText : MonoBehaviour
{
    /// <summary>
    /// 点击消息自身
    /// </summary>
    public Text m_Text;
    public string condition;
    public string stackTrace;

    private bool isExpand = false;

    public void OnClickSelf()
    {
        isExpand = !isExpand;
        if (isExpand)
        {
            m_Text.text = condition + "\n" + "<color=#000000FF>" + stackTrace + "</color>" ;
        }
        else
        {
            m_Text.text = condition;
        }
    }
}
