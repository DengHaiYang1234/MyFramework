# MyFramework
当前框架简介：  
## 一、日志系统：  
    DebugOutput.cs管理类，目前支持日志暂停，清除，开启/关闭，会以UI的形式输出，并会将Debug日志存放至本地。便于手机查找BUG.  
## 二、资源打包：（以下打包默认为生产AssetBundle资源）  
    入口：MyAssetBundleMenu.cs  
    介绍：提供一键打包以及自定义打包（约定需打包的资源必须放于data目录下，Lua脚本需单独建立Lua目录）。并会生成manifest配置文件，可查看打包信息。 在打包过程中，会自动维护依赖关系及避免资源重复打包。  
    1.一键打包：  
      各种资源会以默认的粒度进行打包（例：prefab采用细粒度，图集采用适中粒度，声音采用粗粒度）   
    2.自定义打包：  
      （1）选择Assets/CheckBuildPattern/ShowOrUpdateBuildPattern生成一份默认的打包方式文件（buildPattern），查看或修改资源打包信息。  
      （2）选择Assets/BuildManifest生成对应的打包配置文件（manifest），查看是否符合预期。  
      （3）最后BuildAll。  
        
    注意：  
      （1）buildPattern存在的参数中，BuildType是决定采用什么方式打包。BuildAssetsWithAssetBundleName（粗粒度），BuildAssetsWithDirectroyName（适中粒度），BuildAssetsWithFilename（细粒度），BuildLua（打包lua文件，实际采用的也是适中粒度）。  
      （2）manifest中的信息，表示打包之后的AssetBundle就是这种资源分配的。  
      （3）打包后的资源路径并非存放于StreamingAssets中，发布平台需执行MyAssetsBundle/CopyAssetsToStreamingAssets。  
## 三、资源加载：（直接使用项目资源或启用AssetBundle加载模式）  
    入口：ResourceManager.cs  
    FrameworkDefaultSetting.cs中可以设置一些默认的资源加载属性。     
    主要干了什么：     
      1.首先加载的资源是AssetBundle及AssetBundleManifest。     
      2.其次加载资源是配置文件manifest（加载完成后会理解卸载）。     
      3.加载指定资源时，会加载其依赖资源，且必须等其依赖加载完成之后才会返回指定资源。   
      4.MyAsset和MyBundle，对应asset和bundle对象，会自动维护其依赖数量。     
      5.MyAssets和MyBundles，对应所有的asset和bundle的管理类，维护asset或bundle的加载及卸载。   
      6.加载asset或bundle有同步和异步两种方式。   
      7.使用方便，只需调用ResourceManager.LoadAsset<T>（name）,只需传入加载对象类型及名称即可。   
## 四、热更新：   
    入口：HotManager.cs   
    主要干了什么：（以下更新配置文件都指的是file.txt，存有当前所有AssetBundle路径及对应MD5）   
      1.首先检测是否存在本地（或者包体内）的更新配置文件，若没有那么就远端下载，若有就缓存本地配置文件内的所有信息。   
      2.下载并读取远端更新配置文件   
      3.比对本地与远端配置文件的差异。首先检测版本文件是否相同（若相同就表示不用更新），若存在差异，就添加至线程队列，开始执行下载（删除本地文件，下载远端文件并保存至本地）。   
      4.最后删除本地配置文件，下载远端配置文件。   
    注意：   
      1.Application .streamingAssetsPath（只读目录）  Application.persistentDataPath（读写目录）     
      2.注意各平台的包体资源路径（Android：Application.dataPath + "!/assets/"，IOS：Application.dataPath + "/Raw/"，PC： Application.dataPath + "/StreamingAssets/"）    
## 五、状态机：    
    基类：StateBase.cs   
    介绍：添加状态StateMachine.Add(XXXState) => 构造函数初始化XXXState类（例：public CheckStage(FrameworkMain owner) : base((int) MyStage.check, owner{})，主要是方便访问共有类及确定当前状态ID） => 切换状态只需调用StateMachine.ChangeState（ID）（指定状态的id）       
## 六、事件：  
    入口：EventDispatchCenter  
    主要干了什么：  
      1.事件注册：LTEventCenter. Regist（string notifyID, Action<object> action, bool inQueue），notifyID：确定事件ID，action：事件回调，inQueue：是否按照队列执行）。  
      2.事件派发：LTEventCenter. SendNotify（string notifyID, object obj），notifyID：确定事件ID，obj：回调参数。  
      3.取消事件注册：LTEventCenter. UnRegist（string notifyID, Action<object> action），删除指定事件ID的回调。    
