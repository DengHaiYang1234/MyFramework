--主入口函数。从这里开始lua逻辑


Main = {}


function Main.main()
    print("普通测试 111111111111111 ")
    Util = MyFramework.Util
    Util.LogErr("普通测试--------这是用来测试CustomSettings")	
end

function Main.start()
    print('普通测试--------这是测试start')
end

function Main.SetValue()
    DownPanel = MyFramework.DownPanel
    DownPanel.SetProgressValue("MyFramework")
    DownPanel.SetFileValue("MyFramework")
    --DownPanel.GetTitle()
end
 
