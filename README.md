# CStoLua

## (1)导出C# 函数名、类名
将UnityEditorCode/ExportSymbol.cs 拷贝到Unity工程Editor目录，等待编译完成。

点击XLua/ExportSymbol 菜单，导出C# 函数名、类名 到代码中设定的目录(需要将代码中目录设置为Clone之后的项目目录)。

## (2)执行代码转换
拷贝需要转换的代码，黏贴到bin目录下的src.cs 文件中。

运行CStoLua.exe，执行转换。

生成的lua代码存放在new.cs中。


效果预览：

C#代码

```
void Start()
{
	base.Start ();
	Init();
	mLabel_Enter = mButton_Enter.transform.GetComponent<UILabel>();
	SetButtonEvent();
	RegisterEventReceiver();
	PBMessage.GMRoleNameRequest tempData = new PBMessage.GMRoleNameRequest();
	tempData.m_RoleProfession = mProfessional;
	RequestName();

	//开始倒计时自动创建角色
	mAutoCreateRoleTimeOut = 40;

	//加载特效场景
	EventDispatch.GetSingleton().DispatchEvent(EventID.LoadSelfAnimEnd, null);
}

```

生成Lua代码
```
void Start()

	base:Start ()
	Init()
	mLabel_Enter = mButton_Enter.transform:GetComponent(typeof(UILabel))
	SetButtonEvent()
	self:RegisterEventReceiver()
	PBMessage.GMRoleNameRequest tempData =PBMessage.GMRoleNameRequest()
	tempData.m_RoleProfession = mProfessional
	self:RequestName()

	-- 开始倒计时自动创建角色
	mAutoCreateRoleTimeOut = 40

	-- 加载特效场景
	SLuaHelper:DispatchEventTwoSide(EventId.LoadSelfAnimEnd, nil)
end
```

## (3) 自定义替换

在bin目录的 replace.txt 中添加自定义替换。

例如在C#中Instantiate 之后，需要 as GameObject，但是Lua中不需要。

在 replace.txt 中添加替换规则。

```
as GameObject;->
```


