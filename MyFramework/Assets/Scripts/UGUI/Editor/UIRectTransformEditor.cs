//using UnityEngine;


//namespace UnityEditor
//{
//    ////告诉编辑器类，它是编辑器的运行时类型。(子类一并继承)
//    //[CustomEditor(typeof(RectTransform),true)]
//    //public class UIRectTransformEditor : DecoratorEditor
//    //{
//    //    public GameObject rect;
//    //    public UIRectTransformEditor() : base("RectTransformEditor")
//    //    {
//    //    }

//    //    public override void OnInspectorGUI()
//    //    {
//    //        base.OnInspectorGUI();
//    //        var rectTransform = serializedObject.targetObject as RectTransform;

//    //        GUILayout.BeginHorizontal();
//    //        if (GUILayout.Button("铺满父节点"))
//    //        {
//    //            rectTransform.localScale = Vector3.one;
//    //            rectTransform.anchorMax = Vector3.one;
//    //            rectTransform.anchorMin = Vector3.zero;
//    //            rectTransform.offsetMax = Vector3.zero;
//    //            rectTransform.offsetMin = Vector3.zero;
//    //        }

//    //        if (GUILayout.Button("置零"))
//    //        {
//    //            rectTransform.anchoredPosition = Vector2.zero;
//    //            rectTransform.localScale = Vector3.one;
//    //        }

//    //        if (GUILayout.Button("置为第一个子节点大小"))
//    //        {
//    //            if (rectTransform.childCount > 0)
//    //            {
//    //                var child = rectTransform.GetChild(0);
//    //                rectTransform.sizeDelta = (child as RectTransform).sizeDelta;
//    //            }
//    //        }
//    //        GUILayout.EndHorizontal();



//    //        GUILayout.BeginHorizontal();
//    //        if (GUILayout.Button("缩放值置为1"))
//    //        {
//    //            rectTransform.localScale = Vector3.one;
//    //        }

//    //        if (GUILayout.Button("取整"))
//    //        {
//    //            Vector2 pos = rectTransform.anchoredPosition;
//    //            pos.x = Mathf.CeilToInt(pos.x);
//    //            pos.y = Mathf.CeilToInt(pos.y);
//    //            rectTransform.anchoredPosition = pos;
//    //        }
//    //        GUILayout.EndHorizontal();

//    //    }




//    //}
//}


