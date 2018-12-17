using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Loom : MonoBehaviour
{
    public struct QueueCallItem
    {
        public Action<object> Action;
        public object Parameter;
    }

    private ArrayBufferStruct<QueueCallItem> _actions = new ArrayBufferStruct<QueueCallItem>(10000);

    private ArrayBufferStruct<QueueCallItem> _currentActions = new ArrayBufferStruct<QueueCallItem>(10000);

    private static bool initialize;
    
    private static Loom _current;

    public static Loom Current
    {
        get { Initialize();
            return _current;
        }
    }

    private void Awake()
    {
        _current = this;
        initialize = true;
    }

    public static void Initialize()
    {
        if (!initialize)
        {
            if (!Application.isPlaying)
                return;

            initialize = true;
            var g = new GameObject("Loom");
            g.hideFlags = HideFlags.NotEditable;
            DontDestroyOnLoad(g);
            _current = g.AddComponent<Loom>();
        }
    }

    /// <summary>
    /// 添加事件队列
    /// </summary>
    /// <param name="action"> callback </param>
    /// <param name="parmeter"> 参数 </param>
    public static void QueueOnMainThread(Action<object> action, object parmeter)
    {
        if (Current == null) return;
        lock (Current._actions)
        {
            if (Current._actions.Count >= Current._actions.MaxSize)  //该消息的绑定的回调数量大于总数量
            {
                MyDebug.LogErrorFormat(
                    "QueueOnMainThread called but Current._actions.Count{0} >= Current._actions.MaxSize{1} !",
                    Current._actions.Count, Current._actions.MaxSize);
                return;
            }

            //添加当前消息的回调
            Current._actions.Add(new QueueCallItem()
            {
                Action = action,
                Parameter = parmeter
            });
        }
    }

    private void Update()
    {
        lock (_actions)
        {
            _currentActions.Clear();
            AddRange(_actions,_currentActions);
            _actions.Clear();
        }

        for (int i = 0; i < _currentActions.Count; i++)
        {
            var a = _currentActions[i];
            try
            {
                a.Action(a.Parameter);  //执行每个回调
            }
            catch(Exception e)
            {
                MyDebug.LogException(e);
            }
        }
    }

    /// <summary>
    /// 添加至目标集合
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    private void AddRange(ArrayBufferStruct<QueueCallItem> source,ArrayBufferStruct<QueueCallItem> target)
    {
        for (int i = 0; i < source.Count; i++)
            target.Add(source[i]);
    }

    private void OnDisable()
    {
        if (_current == this)
            _current = null;
    }
}
