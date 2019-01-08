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
    DownPanel.SetProgressValue("成功完成热更")
    DownPanel.SetFileValue("!!!!!!!!!!!!!!!!")
    DownPanel.SetSprite("BG_1")
    --DownPanel.GetTitle()
end