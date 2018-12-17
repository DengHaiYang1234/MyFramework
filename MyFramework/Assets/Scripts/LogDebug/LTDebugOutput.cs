using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;
using System.IO;
using UnityEngine.UI;

public class LTDebugOutput : MonoBehaviour
{
    #region 错误日志data
    public class LogData
    {
        public Color TextColor;
        public string Condition;
        public string StackTrace;
    }
    #endregion
    #region private
    private bool _ShowDebugPanel = false;
    private bool _hasInit = false;
    private string _outPutPath = null;
    private List<LogData> _printDebugTxt = new List<LogData>();
    #endregion
    #region public
    public static LTDebugOutput Instance;
    [HeaderAttribute("Debug ScrollView Panel")]
    public GameObject m_LogPanel;
    [HeaderAttribute("Debug根节点")]
    public GameObject DebugRootPanel;
    [HeaderAttribute("是否开启Debug")]
    public bool isOpen = true;
    [HeaderAttribute("Debug输出Compent")]
    public LTDebugText m_LogPrefab;
    [HeaderAttribute("开启线程接收")]
    public bool ThreadRecrive = false;
    [HeaderAttribute("是否继续打印Log")]
    public bool isLoging = true;
    [HeaderAttribute("暂停开始按钮")]
    public Text m_StopButtonText;
    #endregion


    private void Awake()
    {
        Instance = this;
        UnityEngine.Debug.logger.filterLogType = LogType.Log;
        //接收Debug
        Application.logMessageReceived += Application_LogMessageReceived;
        //开启线程接收Debug
        Application.logMessageReceivedThreaded += Application_LogMessageReceivedThread;

        if (Application.platform == RuntimePlatform.WindowsEditor) CloseLog();
    }

    public void Init()
    {
        #region 在项目中创建Debug保存文件
        _hasInit = true;
        //Debug 文件根目录
        string binRootPath = SDRootPath.Instance.GetBinFileRootPath();
        if (!Directory.Exists(binRootPath))
            Directory.CreateDirectory(binRootPath);
        //Debug Log文件根目录
        string logPath = SDRootPath.Instance.GetLogFolderPath();
        if (!Directory.Exists(logPath))
            Directory.CreateDirectory(logPath);
        //Debug Log文件 
        _outPutPath = SDRootPath.Instance.GetCurLogFilePath();
        if (Directory.Exists(SDRootPath.Instance.GetBinFileRootPath()))
        {
            if (File.Exists(_outPutPath))
                File.Delete(_outPutPath);
        }
        else
        {
            Directory.CreateDirectory(SDRootPath.Instance.GetBinFileRootPath());
        }

        //保存Debug至文件
        MyDebug.Init(_outPutPath);
        #endregion
    }

    private void Update()
    {
        //输出Debug
        lock (_printDebugTxt)
        {
            var cnt = Mathf.Min(_printDebugTxt.Count, 10);
            for (int i = 0; i < cnt; i++)
            {
                LogData data = _printDebugTxt[i];
                AddLine(data.Condition, data.StackTrace, data.TextColor);
            }
            //范围删除
            _printDebugTxt.RemoveRange(0, cnt);
        }
    }

    private void Application_LogMessageReceived(string condition,string stackTrace,LogType type)
    {
        if (!isOpen) return;

        if (!m_LogPanel.gameObject.activeSelf) m_LogPanel.SetActive(true);

        if (!isLoging) return;

        switch (type)
        {
            case LogType.Log:
                AddLine("[I]" + condition, stackTrace, Color.green);
                break;
            case LogType.Warning:
                //AddLine("[W]" + condition, stackTrace, Color.yellow);
                break;
            case LogType.Assert:
                AddLine("[A]" + condition, stackTrace, Color.black);
                break;
            case LogType.Error:
                AddLine("[E]" + condition, stackTrace, Color.red);
                break;
            case LogType.Exception:
                AddLine("[X]" + condition, stackTrace, Color.magenta);
                break;

        }
    }

    private void Application_LogMessageReceivedThread(string condition, string stackTrace, LogType type)
    {
        if (!ThreadRecrive) return;

        if (!isLoging) return;

        switch (type)
        {
            case LogType.Log:
                AddLineThread("[I]" + condition, stackTrace, Color.green);
                break;
            case LogType.Warning:
                //AddLineThread("[W]" + condition, stackTrace, Color.yellow);
                break;
            case LogType.Assert:
                AddLineThread("[A]" + condition, stackTrace, Color.black);
                break;
            case LogType.Error:
                AddLineThread("[E]" + condition, stackTrace, Color.red);
                break;
            case LogType.Exception:
                AddLineThread("[X]" + condition, stackTrace, Color.magenta);
                break;
        }
    }
    
    void UpdateDebugPanelShow()
    {
        DebugRootPanel.SetActive(_ShowDebugPanel);
    }

    /// <summary>
    /// 设置log输出文本及相关属性
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="stackTrace"></param>
    /// <param name="color"></param>
    private void AddLine(string msg, string stackTrace, Color color)
    {
        m_LogPrefab.m_Text.color = color;
        m_LogPrefab.m_Text.text = msg;
        m_LogPrefab.condition = msg;
        m_LogPrefab.stackTrace = stackTrace;

        GameObject tempObj = GameObject.Instantiate(m_LogPrefab.gameObject);
        tempObj.transform.SetParent(m_LogPrefab.transform.parent, false);
        tempObj.SetActive(true);
    }

    private void AddLineThread(string msg,string stackTrace,Color color)
    {
        var data = new LogData() {Condition = msg, StackTrace = stackTrace, TextColor = color};
        lock (_printDebugTxt) _printDebugTxt.Add(data);
    }

    void CloseLog()
    {
        if (m_LogPanel.gameObject.activeSelf)
            m_LogPanel.SetActive(false);
    }

    public void OnClickClearLog()
    {
        ClearLog();
        CloseLog();
    }

    private void ClearLog()
    {
        for (int i = 1; i < m_LogPrefab.transform.parent.childCount; ++i)
            Destroy(m_LogPrefab.transform.parent.GetChild(i).gameObject);
    }

    public void OnClickCloseLog()
    {
        CloseLog();
    }

    public void OnClickStopLog()
    {
        isLoging = !isLoging;
        m_StopButtonText.text = isLoging ? "暂停" : "继续";
    }

    public void OnClickOpenDebugButton()
    {
        Debug.LogError("m_LogPanel.gameObject.activeSelf:" + m_LogPanel.gameObject.activeSelf);
        if (m_LogPanel.gameObject.activeSelf)
            return;

        ClearLog();
        OnClickStopLog();
        _ShowDebugPanel = !_ShowDebugPanel;
        UpdateDebugPanelShow();
    }

}
