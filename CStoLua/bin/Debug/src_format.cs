public override void Awake()
{
	base.Awake ();
	Init();
	mWaiGuanMoreTran = mButton_Up_WaiGuan.transform.parent;
	mHuanHuaMoreTran = mButton_Up_HuanHua.transform.parent;
	if (Debuger.EnableLog)
	{
		Debuger.Log(mToggle_Wing.name + mToggle_Wing.value + "---------------1----------------");
	}
	mButton_FaQi = mToggle_FaQi.transform.GetComponent<UIButton>();
	SetEvent();
}
