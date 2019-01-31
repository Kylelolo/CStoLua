  function Awake()

	base:Awake ()
	Init()
	self.mWaiGuanMoreTran = self.mButton_Up_WaiGuan.transform.parent
	self.mHuanHuaMoreTran = self.mButton_Up_HuanHua.transform.parent
	if  Debuger.EnableLog  then
	
		Debuger.Log(self.mToggle_Wing.name + self.mToggle_Wing.value .."---------------1----------------")
	end
	self.mButton_FaQi = self.mToggle_FaQi.transform:GetComponent(typeof(UIButton))
	self:SetEvent()
end
