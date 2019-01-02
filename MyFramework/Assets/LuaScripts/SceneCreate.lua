SceneCreate = {}
local this = SceneCreate
function SceneCreate.instance()
	this = SceneCreate
	return this
end
--启动事件--
function SceneCreate:Awake(obj)
	local gameObject = obj
	local transform = obj.transform
	
	--玩家名字
	this.mRoleName = transform:Find("middleUnder/InputName/Label"):GetComponent("UILabel")
	--名字切换按钮
	this.mBtnRandomName = transform:Find("middleUnder/BtnRandomName").gameObject
	
	--职业1
	this.mBtnJob1 = transform:Find("leftEdge/BtnJob1").gameObject
	--职业2
	this.mBtnJob2 = transform:Find("leftEdge/BtnJob2").gameObject
	--职业3
	this.mBtnJob3 = transform:Find("leftEdge/BtnJob3").gameObject
	--职业4
	this.mBtnJob4 = transform:Find("leftEdge/BtnJob4").gameObject
	--特效
	this.uiEffect = transform:Find("leftEdge/uiEffect").gameObject
	
	--英雄名字
	this.mHeroName = transform:Find("rightEdge/plIntro/heroName"):GetComponent("UILabel")
	--英雄职业
	this.mlbJob = transform:Find('rightEdge/plIntro/spTitle/lbJob'):GetComponent('UILabel')
	this.mSpAttr = transform:Find('rightEdge/plIntro/spAttr').gameObject
	--英雄简介
	this.mJobIntro = transform:Find("rightEdge/plIntro/JobIntro"):GetComponent("UILabel")
	--开始游戏按钮
	this.mBtnStart = transform:Find("rightEdge/plIntro/BtnStart").gameObject
	
	this.SwitchAction = transform:Find('SwitchAction').gameObject
	
end

-- start --
--------------------------------
-- 选择分类
-- @function [parent=#SceneCreate] onSwitchType
-- @param obj table 对象
-- end --
function SceneCreate:onSwitchType(obj)
	if this.mNowClick ~= nil then
		this.mNowClick:GetComponent('BoxCollider').enabled = true
		this.mNowClick.transform:Find('defSp').gameObject:SetActive(true)
		this.mNowClick.transform:Find('selectSp').gameObject:SetActive(false)
		this.mNowClick.transform.localPosition = Vector3.New(this.mNowClick.transform.localPosition.x - 50, this.mNowClick.transform.localPosition.y, 0)
	end
	this.mNowClick = obj
	this.mNowClick:GetComponent('BoxCollider').enabled = false
	this.mNowClick.transform:Find('defSp').gameObject:SetActive(false)
	this.mNowClick.transform:Find('selectSp').gameObject:SetActive(true)
	this.mNowClick.transform.localPosition = Vector3.New(this.mNowClick.transform.localPosition.x + 50, this.mNowClick.transform.localPosition.y, 0)
end

-- start --
--------------------------------
-- 释放
-- @function [parent=#SceneCreate] OnDestroy
-- end --
function SceneCreate:OnDestroy()
	this.mNowClick = nil
	ControllerManager.GetCtrl(ControllerName.Choose):OnDestroy()
	package.loaded['View.SceneCreate'] = nil
	package.loaded['_G'] ['SceneCreate'] = nil
	this = nil
	
end 