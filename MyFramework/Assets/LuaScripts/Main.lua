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
    DownPanel.SetProgressValue("手机Lua成功")
    DownPanel.SetFileValue("11111111")
    --DownPanel.GetTitle()
end