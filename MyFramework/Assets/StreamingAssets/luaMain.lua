--主入口函数。从这里开始lua逻辑


Main = {}


function Main.main()
    print("普通测试 111111111111111 ")
    Util = HotFix.Util
    Util.LogErr("普通测试--------这是用来测试CustomSettings")	
end

function Main.start()
    print('普通测试--------这是测试start')
end

function Main.SetValue()
    DownPanel = HotFix.DownPanel
    DownPanel.SetProgressValue("MyFramwork测试1")
    DownPanel.SetFileValue("MyFramwork测试2")
    --DownPanel.GetTitle()
end
 
