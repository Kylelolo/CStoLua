using UnityEngine
using System.Collections
using System.Collections.Generic

 class UI_WaiGuan : UI_WaiGuanBase,IEventReceiver

    List<ResourcesManager.LoadTask> self.mListLoadTask =List<ResourcesManager.LoadTask>()
    ResourcesManager.LoadTask self.mLoadTaskUI = nil
    local self.mSystemID = SystemID.None
    local self.mSystemID2 = SystemID.WaiGuan_Wing
    local IsFrist = true
    System.Action<GameObject> mOpenSystemAction

    local mLastObj

    local mButton_FaQi

     local mWaiGuanMoreTran
     local mHuanHuaMoreTran

    #region Event

      function OnEnable()

		base:OnEnable ()
        self:RegisterEventReceiver()
    end
	  function OnDisable()

		base:OnDisable ()
        self:UnRegisterEventReceiver()
    end

     function RegisterEventReceiver()

EventDispatch:RegisterEventCallback(EventId.Pet_ReturnPetGotoBattle, self,self.)

EventDispatch:RegisterEventCallback(EventId.Mount_GuideClose, self,self.)
    end

     function UnRegisterEventReceiver()

EventDispatch:UnRegisterEventCallback(EventId.Pet_ReturnPetGotoBattle, self.)

EventDispatch:UnRegisterEventCallback(EventId.Mount_GuideClose, self.)
    end

     function OnDispatchEvent(varEventId, object varData)

        switch (varEventId)

            case EventId.Pet_ReturnPetGotoBattle:

                    self:CloseGuidePetGotoBattle()
                end
                break
            case EventId.Mount_GuideClose:

                    self:CloseGuideMount()
                end
                break
        end
    end
    #endregion

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
    --  Use this for initialization
	  function Start ()

		base:Start ()
        if  Debuger.EnableLog  then

            Debuger.Log(self.mToggle_Wing.name + self.mToggle_Wing.value .."---------------2----------------")
        end
        if  Helper.SetToggleState(SystemID.WaiGuan_Pet, self.mToggle_Pet.gameObject)  then

            self.mSystemID2 = SystemID.WaiGuan_Pet
        end
        if  Helper.SetToggleState(SystemID.WaiGuan_WaiGuan_Mount, self.mToggle_Mount.gameObject)  then

            self.mSystemID2 = SystemID.WaiGuan_WaiGuan_Mount
        end
        -- Helper.SetToggleState(SystemID.WaiGuan_Wing, self.mToggle_Wing.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_ChengHao, self.mToggle_Title.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_FaBao, self.mToggle_FaQi.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_WaiGuan_LingQi, self.mToggle_LingQi.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_WaiGuan_ZuJi, self.mToggle_ZuJi.gameObject)
        Helper.SetToggleState(SystemID.WaiGuan_HuanHua, self.mToggle_HuanHua.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_HuanHua_ShiZhuang, self.mToggle_Clothes.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_HuanHua_Equip, self.mToggle_Wuqi_HuanHua.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_HuanHua_Mount, self.mToggle_Mount_HuanHua.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_HuanHua_LingQi, self.mToggle_LingQi_HuanHua.gameObject)
        -- Helper.SetToggleState(SystemID.WaiGuan_HuanHua_ZuJi, self.mToggle_ZuJi_HuanHua.gameObject)


        local tempToggleNum = 0
        self:GetToggleNum(SystemID.WaiGuan_Pet, self.mToggle_Pet.gameObject,ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_WaiGuan_Mount, self.mToggle_Mount.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_Wing, self.mToggle_Wing.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_ChengHao, self.mToggle_Title.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_FaBao, self.mToggle_FaQi.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_WaiGuan_LingQi, self.mToggle_LingQi.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_WaiGuan_ZuJi, self.mToggle_ZuJi.gameObject, ref tempToggleNum)

        self.mWaiGuanMoreTran.transform.localScale = tempToggleNum > 4 ? Vector3.one : Vector3.zero

        tempToggleNum = 0
        self:GetToggleNum(SystemID.WaiGuan_HuanHua_ShiZhuang, self.mToggle_Clothes.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_HuanHua_Equip, self.mToggle_Wuqi_HuanHua.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_HuanHua_Mount, self.mToggle_Mount_HuanHua.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_HuanHua_LingQi, self.mToggle_LingQi_HuanHua.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_HuanHua_ZuJi, self.mToggle_ZuJi_HuanHua.gameObject, ref tempToggleNum)
        self:GetToggleNum(SystemID.WaiGuan_HuanHua_Wings, self.mToggle_Wing_HuanHua.gameObject, ref tempToggleNum)

        self.mHuanHuaMoreTran.transform.localScale = tempToggleNum > 4 ? Vector3.one : Vector3.zero


        self.mGrid_WaiGuan_Toggle.enabled = true
        self.mGrid_HuanHua_Toggle.enabled = true
        if  self.mSystemID == SystemID.None  then

            self.mToggle_WaiGuan.value = true

        else

            if  self.mSystemID / 1000000 == 132  then

                self.mToggle_HuanHua.value = true

            else

                self.mToggle_WaiGuan.value = true
            end
        end
        -- self.mToggle_LingQi.gameObject:SetActive(false)

        self:ShowGuide()
    end
	

    function SetEvent()

        self.mToggle_Mount.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_Mount.value  then

                self:OpenUI(UI_Panel_Mount.gameObject)
                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Mount")
            end
        end))
        self.mToggle_Pet.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_Pet.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Pet")
                if  CS.SystemFunctionData.GetSingleton():GetSystemInfoBool(SystemID.WaiGuan_Pet)  then

                    self:OpenUI(UI_Panel_Pet.gameObject)

                else

                    QiuMo. Systemfuciton tmpSystemfuciton = QiuMo.GameDataManager.LocalTypeSystemfuction:GetSystemfuciton(SystemID.WaiGuan_Pet)
                    if tmpSystemfuciton~=nil  then

                        WindowManager:OpenFlyUpFont(tmpSystemfuciton.mDescribe)
                    end
                end
            end
        end))
        self.mToggle_Wing.onChange:Add(new EventDelegate (function ()

            if  Debuger.EnableLog  then

                Debuger.Log(self.mToggle_Wing.name+mToggle_Wing.value .."---------------5----------------")
            end
            if  self.mToggle_Wing.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Wing")
                self:OpenUI(UI_Wings.gameObject)
            end
        end))
        self.mToggle_Title.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_Title.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Title")
                self:OpenUI(UI_Title.gameObject)
            end
        end))
        self.mToggle_FaQi.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_FaQi.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_FaQi")
                self:OpenUI(UI_FaBao.gameObject)
            end
        end))
        -- self.mButton_FaQi.onClick:Add(new EventDelegate (function ()
        -- {
        --     if (PlayerManager.GetSingleton().PlayerFullRoleInfoSelf.zhuansheng < 1 or PlayerManager.GetSingleton().PlayerFullRoleInfoSelf.level < 50)
        --     {
        --         WindowManager:OpenFlyUpFont(Localization.Get("WaiGuan_FaQiWeiJieSuo"))
        --     }
        --     else
        --     {
        --         self.mToggle_FaQi.enabled = true
        --         self.mToggle_FaQi.value = true
        --     }
        -- end))
        self.mToggle_LingQi.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_LingQi.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_LingQi")
                self:OpenUI(UI_XianQi.gameObject)
            end
        end))
        self.mToggle_ZuJi.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_ZuJi.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_ZuJi")
                self:OpenUI(UI_ZuJi.gameObject)
            end
        end))
        self.mToggle_WaiGuan.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_WaiGuan.value  then

                Panel_WaiGuan.gameObject:SetActive(true)
                if  IsFrist and self.mSystemID ~= SystemID.None  then

                    -- StartCoroutine(StartUI(self.mSystemID))
						new CoroutineTask(StartUI(self.mSystemID))

                else

                    -- StartCoroutine(StartUI(self.mSystemID2))
						new CoroutineTask(StartUI(self.mSystemID2))
                end

            else

                Panel_WaiGuan.gameObject:SetActive(false)
            end
        end))
        self.mToggle_HuanHua.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_HuanHua.value  then

                Panel_HuanHua.gameObject:SetActive(true)
                if  IsFrist and self.mSystemID ~= SystemID.None  then

                    -- StartCoroutine(StartUI(self.mSystemID))
						new CoroutineTask(StartUI(self.mSystemID))

                else

                    -- StartCoroutine(StartUI(SystemID.WaiGuan_HuanHua_ShiZhuang))
						new CoroutineTask(StartUI(SystemID.WaiGuan_HuanHua_ShiZhuang))
                end

            else

                Panel_HuanHua.gameObject:SetActive(false)
            end
        end))
        self.mClose.onClick:Add(new EventDelegate (function ()

            WindowManager:CloseWindow(UI_Path.UI_WaiGuan_Path)
        end))
        self.mToggle_Clothes.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_Clothes.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Clothes")
                self:OpenUI(UI_ClothesFashion.gameObject)
            end
        end))
        self.mToggle_Mount_HuanHua.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_Mount_HuanHua.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Mount_HuanHua")
                self:OpenUI(UI_FantasticChange.gameObject)
            end
        end))
        self.mToggle_Wuqi_HuanHua.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_Wuqi_HuanHua.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Wuqi_HuanHua")
                self:OpenUI(UI_WeaponFashion.gameObject)
            end
        end))
        self.mToggle_LingQi_HuanHua.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_LingQi_HuanHua.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Wuqi_HuanHua")
                self:OpenUI(UI_XianQiFashion.gameObject)
            end
        end))
        self.mToggle_ZuJi_HuanHua.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_ZuJi_HuanHua.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Wuqi_HuanHua")
                self:OpenUI(UI_FootPrintFashion.gameObject)
            end
        end))
        self.mToggle_Wing_HuanHua.onChange:Add(new EventDelegate (function ()

            if  self.mToggle_Wing_HuanHua.value  then

                -- self.mLabel_Title.text = Localization.Get("WaiGuan_Wuqi_HuanHua")
                self:OpenUI(UI_WingsFashion.gameObject)
            end
        end))
        self.mScrollBar_Toggle_WaiGuan.onChange:Add(EventDelegate(function()

            if  self.mScrollBar_Toggle_WaiGuan.value < 0.1   then

                self.mButton_Up_WaiGuan.gameObject:SetActive(false)

            else

                self.mButton_Up_WaiGuan.gameObject:SetActive(true)
            end
            if  self.mScrollBar_Toggle_WaiGuan.value > 0.9   then

                self.mButton_Down_WaiGuan.gameObject:SetActive(false)

            else

                self.mButton_Down_WaiGuan.gameObject:SetActive(true)
            end
        end))
        self.mScrollBar_Toggle_HuanHua.onChange:Add(EventDelegate(function()

            if  self.mScrollBar_Toggle_HuanHua.value < 0.1   then

                self.mButton_Up_HuanHua.gameObject:SetActive(false)

            else

                self.mButton_Up_HuanHua.gameObject:SetActive(true)
            end
            if  self.mScrollBar_Toggle_HuanHua.value > 0.9   then

                self.mButton_Down_HuanHua.gameObject:SetActive(false)

            else

                self.mButton_Down_HuanHua.gameObject:SetActive(true)
            end
        end))
    end

    IEnumerator StartUI(varSystemID)

        yield return nil

        yield return nil
        switch (varSystemID)

            case SystemID.WaiGuan_WaiGuan:
                self.mToggle_Mount.value = true
                if  EventDelegate.IsValid(self.mToggle_Mount.onChange)  then

                    EventDelegate.Execute(self.mToggle_Mount.onChange)
                end
                break
            case SystemID.WaiGuan_WaiGuan_Mount:
            case SystemID.WaiGuan_WaiGuan_Mount_Mount:
                self.mToggle_Mount.value = true
                if  EventDelegate.IsValid(self.mToggle_Mount.onChange)  then

                    EventDelegate.Execute(self.mToggle_Mount.onChange)
                end
                break
            case SystemID.WaiGuan_Pet:
                self.mToggle_Pet.value = true
                if  EventDelegate.IsValid(self.mToggle_Pet.onChange)  then

                    EventDelegate.Execute(self.mToggle_Pet.onChange)
                end
                break
            case SystemID.WaiGuan_Wing:
                self.mToggle_Wing.value = true
                if  EventDelegate.IsValid(self.mToggle_Wing.onChange)  then

                    EventDelegate.Execute(self.mToggle_Wing.onChange)
                end
                break
            case SystemID.WaiGuan_ChengHao:
                self.mScrollBar_Toggle_WaiGuan.value = 1
                self.mToggle_Title.value = true
                break
            case SystemID.WaiGuan_FaBao:  -- 外观-法宝
            case SystemID.WaiGuan_FaBao_JiHuo:  -- 外观-法宝
            case SystemID.WaiGuan_FaBao_JiHuo1:  -- 外观-法宝1
            case SystemID.WaiGuan_FaBao_JiHuo2:  -- 外观-法宝2
            case SystemID.WaiGuan_FaBao_JiHuo3:  -- 外观-法宝3
            case SystemID.WaiGuan_FaBao_CuiLian:-- 外观-法器-淬炼
            case SystemID.WaiGuan_FaBao_CuiLian1:  -- 外观-法宝1-淬炼
            case SystemID.WaiGuan_FaBao_CuiLian2:  -- 外观-法宝2-淬炼
            case SystemID.WaiGuan_FaBao_CuiLian3:  -- 外观-法宝3-淬炼
            case SystemID.WaiGuan_FaBao_ZhuLing: -- 外观-法器-注灵
            case SystemID.WaiGuan_FaBao_ZhuLing1: -- 外观-法器-注灵
            case SystemID.WaiGuan_FaBao_ZhuLing2: -- 外观-法器-注灵
            case SystemID.WaiGuan_FaBao_ZhuLing3: -- 外观-法器-注灵
                self.mToggle_FaQi.value = true
                self.mScrollBar_Toggle_WaiGuan.value = 0.35
                if (varSystemID == SystemID.WaiGuan_FaBao_JiHuo
                        or varSystemID == SystemID.WaiGuan_FaBao_JiHuo1
                        or varSystemID == SystemID.WaiGuan_FaBao_JiHuo2
                        or varSystemID == SystemID.WaiGuan_FaBao_JiHuo3
                        or varSystemID == SystemID.WaiGuan_FaBao_CuiLian
                        or varSystemID == SystemID.WaiGuan_FaBao_CuiLian1
                        or varSystemID == SystemID.WaiGuan_FaBao_CuiLian2
                        or varSystemID == SystemID.WaiGuan_FaBao_CuiLian3
                        or varSystemID == SystemID.WaiGuan_FaBao_ZhuLing
                        or varSystemID == SystemID.WaiGuan_FaBao_ZhuLing1
                        or varSystemID == SystemID.WaiGuan_FaBao_ZhuLing2
                        or varSystemID == SystemID.WaiGuan_FaBao_ZhuLing3)

                    local tempUi_FaoBao = UI_FaBao:GetComponent(typeof(Ui_FaoBao))
                    if  tempUi_FaoBao ~= nil  then

                        tempUi_FaoBao:SetToggle(varSystemID)
                    end
                end
                break
            case SystemID.WaiGuan_WaiGuan_LingQi:
            case SystemID.WaiGuan_WaiGuan_LingQi_LingQi:
                self.mToggle_LingQi.value = true
                self.mScrollBar_Toggle_WaiGuan.value = 0.68
                break
            case SystemID.WaiGuan_WaiGuan_ZuJi:
                self.mToggle_ZuJi.value = true
                break
            case SystemID.WaiGuan_HuanHua:
                self.mToggle_Clothes.value = true
                if  EventDelegate.IsValid(self.mToggle_Clothes.onChange)  then

                    EventDelegate.Execute(self.mToggle_Clothes.onChange)
                end
                break
            case SystemID.WaiGuan_HuanHua_ShiZhuang:
                self.mToggle_Clothes.value = true
                if  EventDelegate.IsValid(self.mToggle_Clothes.onChange)  then

                    EventDelegate.Execute(self.mToggle_Clothes.onChange)
                end
                break
            case SystemID.WaiGuan_HuanHua_Equip:
                self.mToggle_Wuqi_HuanHua.value = true
                break
            case SystemID.WaiGuan_HuanHua_Mount:
                self.mToggle_Mount_HuanHua.value = true
                break
            case SystemID.WaiGuan_HuanHua_LingQi:
                self.mToggle_LingQi_HuanHua.value = true
                break
            case SystemID.WaiGuan_HuanHua_ZuJi:
                self.mToggle_ZuJi_HuanHua.value = true
                break
            case SystemID.WaiGuan_HuanHua_Wings:
                self.mToggle_Wing_HuanHua.value = true
                break

        end
        if  IsFrist  then

            self.mToggle_Mount.optionCanBeNone = false
            self.mToggle_Pet.optionCanBeNone = false
            self.mToggle_Wing.optionCanBeNone = false
            self.mToggle_Title.optionCanBeNone = false
            self.mToggle_FaQi.optionCanBeNone = false
            self.mToggle_LingQi.optionCanBeNone = false
            self.mToggle_ZuJi.optionCanBeNone = false
            self.mToggle_Clothes.optionCanBeNone = false
            self.mToggle_Wuqi_HuanHua.optionCanBeNone = false
            self.mToggle_Mount_HuanHua.optionCanBeNone = false
            self.mToggle_ZuJi_HuanHua.optionCanBeNone = false
            self.mToggle_Wing_HuanHua.optionCanBeNone = false
            self.mToggle_LingQi_HuanHua.optionCanBeNone = false
            self.mToggle_WaiGuan.optionCanBeNone = false
            self.mToggle_HuanHua.optionCanBeNone = false

            self.mToggle_Mount.onChange:Add(EventDelegate(function()

                if self.mToggle_Mount.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_Pet.onChange:Add(EventDelegate(function()

                if  self.mToggle_Pet.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_Wing.onChange:Add(EventDelegate(function()

                if  self.mToggle_Wing.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_Title.onChange:Add(EventDelegate(function()

                if  self.mToggle_Title.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_FaQi.onChange:Add(EventDelegate(function()

                if  self.mToggle_FaQi.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_LingQi.onChange:Add(EventDelegate(function()

                if  self.mToggle_LingQi.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_ZuJi.onChange:Add(EventDelegate(function()

                if  self.mToggle_ZuJi.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_Clothes.onChange:Add(EventDelegate(function()

                if  self.mToggle_Clothes.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_Wuqi_HuanHua.onChange:Add(EventDelegate(function()

                if  self.mToggle_Wuqi_HuanHua.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_Mount_HuanHua.onChange:Add(EventDelegate(function()

                if  self.mToggle_Mount_HuanHua.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_LingQi_HuanHua.onChange:Add(EventDelegate(function()

                if  self.mToggle_LingQi_HuanHua.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_ZuJi_HuanHua.onChange:Add(EventDelegate(function()

                if  self.mToggle_ZuJi_HuanHua.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_Wing_HuanHua.onChange:Add(EventDelegate(function()

                if  self.mToggle_Wing_HuanHua.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_WaiGuan.onChange:Add(EventDelegate(function()

                if  self.mToggle_WaiGuan.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))
            self.mToggle_HuanHua.onChange:Add(EventDelegate(function()

                if  self.mToggle_HuanHua.value  then

                    AudioManager:PlayAudio(AudioPath.ClickItem)
                end
            end))

            IsFrist = false

            self:ShowGuide()
        end

    end

    function GetToggleNum(varSystemId, GameObject varGameObject, ref int varNum)

        if  Helper.SetToggleState(varSystemId, varGameObject)  then

            varNum++
        end
    end


     function OpenSubsystem(varSystemID, UIToggle varToggle)


        if  self.mSystemID == varSystemID  then

            varToggle.value = true
            if  self.mOpenSystemAction ~= nil  then

                mOpenSystemAction(self.mLastObj)
                self.mOpenSystemAction = nil
            end
        end
    end
    --- <summary>
    --- 设置要打开的子模块
    --- </summary>
    --- <param name="varSystemID"></param>
     function SetOpenSubsystemID(varSystemID,varAction)

        self.mSystemID = varSystemID
        self.mOpenSystemAction = varAction;-- 很大可能为空

    end

     local mLastPath
    function OpenUI(varObj)

        UI_Panel_Mount.gameObject:SetActive(false)
        UI_Panel_Pet.gameObject:SetActive(false)
        UI_Wings.gameObject:SetActive(false)
        UI_Title.gameObject:SetActive(false)
        UI_FaBao.gameObject:SetActive(false)
        UI_ClothesFashion.gameObject:SetActive(false)
        UI_WeaponFashion.gameObject:SetActive(false)
        UI_FantasticChange.gameObject:SetActive(false)
        UI_ZuJi.gameObject:SetActive(false)
        UI_XianQi.gameObject:SetActive(false)
        UI_FootPrintFashion.gameObject:SetActive(false)
        UI_XianQiFashion.gameObject:SetActive(false)
        UI_WingsFashion.gameObject:SetActive(false)
        varObj:SetActive(true)
        if  self.mOpenSystemAction ~= nil  then

            mOpenSystemAction(varObj)
            self.mOpenSystemAction = nil
        end
    end

    function ShowGuide()

        -- 玄兵
        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_FaBao)  then

            self.mScrollBar_Toggle_WaiGuan.value = 0.34
            GuideManager.GetSingleton():OpenTipsGuide(self.mToggle_FaQi.transform, nil,2, 0.3 )
        end
        -- 伙伴解锁
        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_PetUnLock)  then

            -- if (self.mToggle_Pet.value == false)
            -- {
            --     GuideManager.GetSingleton():OpenGudieNoMaskForce(self.mToggle_Pet.transform, nil, 0.3 )
            -- }
        end
        -- 伙伴成长
        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_PetChengZhang)  then

            if  self.mToggle_Pet.value == false  then

                GuideManager.GetSingleton():OpenTipsGuide(self.mToggle_Pet.transform, nil,2, 0.5 )
            end
        end

        -- 伙伴融合
        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_PetRongHe)  then

            if  self.mToggle_Pet.value == false  then

                GuideManager.GetSingleton():OpenTipsGuide(self.mToggle_Pet.transform, nil,3, 0.3 )
            end
        end

        -- 伙伴灵修
        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_PetLingXiu)  then

            if  self.mToggle_Pet.value == false  then

                GuideManager.GetSingleton():OpenTipsGuide(self.mToggle_Pet.transform, nil, 2,0.3 )
            end
        end
        -- 伙伴功法
        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_PetSkill)  then

            if  self.mToggle_Pet.value == false  then

                GuideManager.GetSingleton():OpenTipsGuide(self.mToggle_Pet.transform, nil,2, 0.3 )
            end
        end

        -- 仙器
        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_XianQi)  then

            self.mScrollBar_Toggle_WaiGuan.value = 1
            -- Invoke("SetScrollBarValue",0.2 )
			DelayTaskManager.GetSingleton().AddDelayTask(0.2 ,function()
			
				self:SetScrollBarValue()
			end)
            GuideManager.GetSingleton():OpenTipsGuide(self.mToggle_LingQi.transform, nil, 2, 0.5 )
        end
        -- 足迹
        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_Foot)  then

            GuideManager.GetSingleton():OpenTipsGuide(self.mToggle_ZuJi.transform, nil, 2, 0.3 )
        end
    end

     function SetScrollBarValue()

        self.mScrollBar_Toggle_WaiGuan.value = 1
    end

    -- 关闭外观
    function CloseGuidePetGotoBattle()

        if  GuideManager.GetSingleton():JudgeHave(GuideEnum.Guide_PetUnLock)  then

            GuideManager.GetSingleton():OpenTipsGuide(self.mClose.transform,function ()

                GuideManager.GetSingleton():CloseGuide()
                GuideManager.GetSingleton():RemoveGuide(GuideEnum.Guide_PetUnLock)
            },7, 0)
        end
    end
    -- 关闭外观
    function CloseGuideMount()

        GuideManager.GetSingleton():OpenTipsGuide(self.mClose.transform, nil, 4, 0, GuideEnum.Guide_MountUnLock)
    end

function OnDestroy()

		base:OnDestroy ()
        if  self.mLastObj ~= nil  then

            ResourcesManager.GetSingleton():ReleaseShort(self.mLastPath)
GameObject.Destroy(self.mLastObj)
        end

        for (i = 0; i < self.mListLoadTask.Count; i++)

            if  mListLoadTask[i] ~= nil  then

                mListLoadTask[i].mStatus = ResourcesManager.LoadTaskStatus.Cancel
            end
        end

        WindowManager:CloseWindow(UI_Path.UI_Medicine_Path)
    end
end
