using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_WaiGuan : UI_WaiGuanBase,IEventReceiver 
{
    List<ResourcesManager.LoadTask> mListLoadTask = new List<ResourcesManager.LoadTask>();
    ResourcesManager.LoadTask mLoadTaskUI = null;
    SystemID mSystemID = SystemID.None;
    SystemID mSystemID2 = SystemID.WaiGuan_Wing;
    bool IsFrist = true;
    System.Action<GameObject> mOpenSystemAction;

    GameObject mLastObj;

    UIButton mButton_FaQi;

    private Transform mWaiGuanMoreTran;
    private Transform mHuanHuaMoreTran;

    #region Event

    public override void OnEnable()
    {
		base.OnEnable ();
        RegisterEventReceiver();
    }
	public override void OnDisable()
    {
		base.OnDisable ();
        UnRegisterEventReceiver();
    }

    public void RegisterEventReceiver()
    {
        EventDispatch.GetSingleton().RegisterEventReceiver(EventID.Pet_ReturnPetGotoBattle, this);

        EventDispatch.GetSingleton().RegisterEventReceiver(EventID.Mount_GuideClose, this);
    }

    public void UnRegisterEventReceiver()
    {
        EventDispatch.GetSingleton().UnRegisterEventReceiver(EventID.Pet_ReturnPetGotoBattle, this);

        EventDispatch.GetSingleton().UnRegisterEventReceiver(EventID.Mount_GuideClose, this);
    }

    public void OnDispatchEvent(EventID varEventId, object varData)
    {
        switch (varEventId)
        {
            case EventID.Pet_ReturnPetGotoBattle:
                {
                    CloseGuidePetGotoBattle();
                }
                break;
            case EventID.Mount_GuideClose:
                {
                    CloseGuideMount();
                }
                break;
        }
    }
    #endregion

	public override void Awake()
    {
		base.Awake ();
        Init();
        mWaiGuanMoreTran = mButton_Up_WaiGuan.transform.parent;
        mHuanHuaMoreTran = mButton_Up_HuanHua.transform.parent;
        if (Debuger.EnableLog)
            Debuger.Log(mToggle_Wing.name + mToggle_Wing.value + "---------------1----------------");
        mButton_FaQi = mToggle_FaQi.transform.GetComponent<UIButton>();
        SetEvent();
    }
    // Use this for initialization
	public override void Start ()
    {
		base.Start ();
        if (Debuger.EnableLog)
            Debuger.Log(mToggle_Wing.name + mToggle_Wing.value + "---------------2----------------");
        if (Helper.SetToggleState(SystemID.WaiGuan_Pet, mToggle_Pet.gameObject))
        {
            mSystemID2 = SystemID.WaiGuan_Pet;
        }
        if (Helper.SetToggleState(SystemID.WaiGuan_WaiGuan_Mount, mToggle_Mount.gameObject))
        {
            mSystemID2 = SystemID.WaiGuan_WaiGuan_Mount;
        }
        //Helper.SetToggleState(SystemID.WaiGuan_Wing, mToggle_Wing.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_ChengHao, mToggle_Title.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_FaBao, mToggle_FaQi.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_WaiGuan_LingQi, mToggle_LingQi.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_WaiGuan_ZuJi, mToggle_ZuJi.gameObject);
        Helper.SetToggleState(SystemID.WaiGuan_HuanHua, mToggle_HuanHua.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_HuanHua_ShiZhuang, mToggle_Clothes.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_HuanHua_Equip, mToggle_Wuqi_HuanHua.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_HuanHua_Mount, mToggle_Mount_HuanHua.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_HuanHua_LingQi, mToggle_LingQi_HuanHua.gameObject);
        //Helper.SetToggleState(SystemID.WaiGuan_HuanHua_ZuJi, mToggle_ZuJi_HuanHua.gameObject);


        int tempToggleNum = 0;
        GetToggleNum(SystemID.WaiGuan_Pet, mToggle_Pet.gameObject,ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_WaiGuan_Mount, mToggle_Mount.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_Wing, mToggle_Wing.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_ChengHao, mToggle_Title.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_FaBao, mToggle_FaQi.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_WaiGuan_LingQi, mToggle_LingQi.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_WaiGuan_ZuJi, mToggle_ZuJi.gameObject, ref tempToggleNum);

        mWaiGuanMoreTran.transform.localScale = tempToggleNum > 4 ? Vector3.one : Vector3.zero;

        tempToggleNum = 0;
        GetToggleNum(SystemID.WaiGuan_HuanHua_ShiZhuang, mToggle_Clothes.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_HuanHua_Equip, mToggle_Wuqi_HuanHua.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_HuanHua_Mount, mToggle_Mount_HuanHua.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_HuanHua_LingQi, mToggle_LingQi_HuanHua.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_HuanHua_ZuJi, mToggle_ZuJi_HuanHua.gameObject, ref tempToggleNum);
        GetToggleNum(SystemID.WaiGuan_HuanHua_Wings, mToggle_Wing_HuanHua.gameObject, ref tempToggleNum);

        mHuanHuaMoreTran.transform.localScale = tempToggleNum > 4 ? Vector3.one : Vector3.zero;


        mGrid_WaiGuan_Toggle.enabled = true;
        mGrid_HuanHua_Toggle.enabled = true;
        if (mSystemID == SystemID.None)
        {
            mToggle_WaiGuan.value = true;
        }
        else
        {
            if ((int)mSystemID / 1000000 == 132)
            {
                mToggle_HuanHua.value = true;
            }
            else
            {
                mToggle_WaiGuan.value = true;
            }
        }
        //mToggle_LingQi.gameObject.SetActive(false);

        ShowGuide();
    }
	

    void SetEvent()
    {
        mToggle_Mount.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_Mount.value)
            {
                OpenUI(UI_Panel_Mount.gameObject);
                //mLabel_Title.text = Localization.Get("WaiGuan_Mount");
            }
        }));
        mToggle_Pet.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_Pet.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Pet");
                if (SystemFunctionData.GetSingleton().GetSystemInfoBool(SystemID.WaiGuan_Pet))
                {
                    OpenUI(UI_Panel_Pet.gameObject);
                }
                else
                {
                    QiuMo. Systemfuciton tmpSystemfuciton = QiuMo.LocalTypeSystemfuction.GetSingleton().GetSystemfuciton((int)SystemID.WaiGuan_Pet);
                    if(tmpSystemfuciton!=null)
                    {
                        WindowManager.GetSingleton().OpenFlyUpFont(tmpSystemfuciton.mDescribe);
                    }
                }
            }
        }));
        mToggle_Wing.onChange.Add(new EventDelegate(delegate ()
        {
            if (Debuger.EnableLog)
                Debuger.Log(mToggle_Wing.name+mToggle_Wing.value + "---------------5----------------");
            if (mToggle_Wing.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Wing");
                OpenUI(UI_Wings.gameObject);
            }
        }));
        mToggle_Title.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_Title.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Title");
                OpenUI(UI_Title.gameObject);
            }
        }));
        mToggle_FaQi.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_FaQi.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_FaQi");
                OpenUI(UI_FaBao.gameObject);
            }
        }));     
        //mButton_FaQi.onClick.Add(new EventDelegate(delegate ()
        //{
        //    if (PlayerManager.GetSingleton().PlayerFullRoleInfoSelf.zhuansheng < 1 || PlayerManager.GetSingleton().PlayerFullRoleInfoSelf.level < 50)
        //    {
        //        WindowManager.GetSingleton().OpenFlyUpFont(Localization.Get("WaiGuan_FaQiWeiJieSuo"));
        //    }
        //    else
        //    {
        //        mToggle_FaQi.enabled = true;
        //        mToggle_FaQi.value = true;
        //    }
        //}));       
        mToggle_LingQi.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_LingQi.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_LingQi");
                OpenUI(UI_XianQi.gameObject);
            }
        }));
        mToggle_ZuJi.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_ZuJi.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_ZuJi");
                OpenUI(UI_ZuJi.gameObject);
            }
        }));
        mToggle_WaiGuan.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_WaiGuan.value)
            {
                Panel_WaiGuan.gameObject.SetActive(true);
                if (IsFrist && mSystemID != SystemID.None)
                {
                    //StartCoroutine(StartUI(mSystemID));
						new CoroutineTask(StartUI(mSystemID));
                }
                else
                {
                    //StartCoroutine(StartUI(mSystemID2));
						new CoroutineTask(StartUI(mSystemID2));
                }
            }
            else
            {
                Panel_WaiGuan.gameObject.SetActive(false);
            }
        }));
        mToggle_HuanHua.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_HuanHua.value)
            {
                Panel_HuanHua.gameObject.SetActive(true);
                if (IsFrist && mSystemID != SystemID.None)
                {
                    //StartCoroutine(StartUI(mSystemID));
						new CoroutineTask(StartUI(mSystemID));
                }
                else
                {
                    //StartCoroutine(StartUI(SystemID.WaiGuan_HuanHua_ShiZhuang));
						new CoroutineTask(StartUI(SystemID.WaiGuan_HuanHua_ShiZhuang));
                }
            }
            else
            {
                Panel_HuanHua.gameObject.SetActive(false);
            }
        }));
        mClose.onClick.Add(new EventDelegate(delegate ()
        {
            WindowManager.GetSingleton().CloseWindow(UIPath.UI_WaiGuan_Path);
        }));
        mToggle_Clothes.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_Clothes.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Clothes");
                OpenUI(UI_ClothesFashion.gameObject);
            }
        }));
        mToggle_Mount_HuanHua.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_Mount_HuanHua.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Mount_HuanHua");
                OpenUI(UI_FantasticChange.gameObject);
            }
        }));
        mToggle_Wuqi_HuanHua.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_Wuqi_HuanHua.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Wuqi_HuanHua");
                OpenUI(UI_WeaponFashion.gameObject);
            }
        }));
        mToggle_LingQi_HuanHua.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_LingQi_HuanHua.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Wuqi_HuanHua");
                OpenUI(UI_XianQiFashion.gameObject);
            }
        }));
        mToggle_ZuJi_HuanHua.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_ZuJi_HuanHua.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Wuqi_HuanHua");
                OpenUI(UI_FootPrintFashion.gameObject);
            }
        }));
        mToggle_Wing_HuanHua.onChange.Add(new EventDelegate(delegate ()
        {
            if (mToggle_Wing_HuanHua.value)
            {
                //mLabel_Title.text = Localization.Get("WaiGuan_Wuqi_HuanHua");
                OpenUI(UI_WingsFashion.gameObject);
            }
        }));
        mScrollBar_Toggle_WaiGuan.onChange.Add(new EventDelegate(() =>
        {
            if (mScrollBar_Toggle_WaiGuan.value < 0.1f)
            {
                mButton_Up_WaiGuan.gameObject.SetActive(false);
            }
            else
            {
                mButton_Up_WaiGuan.gameObject.SetActive(true);
            }
            if (mScrollBar_Toggle_WaiGuan.value > 0.9f)
            {
                mButton_Down_WaiGuan.gameObject.SetActive(false);
            }
            else
            {
                mButton_Down_WaiGuan.gameObject.SetActive(true);
            }
        }));
        mScrollBar_Toggle_HuanHua.onChange.Add(new EventDelegate(() =>
        {
            if (mScrollBar_Toggle_HuanHua.value < 0.1f)
            {
                mButton_Up_HuanHua.gameObject.SetActive(false);
            }
            else
            {
                mButton_Up_HuanHua.gameObject.SetActive(true);
            }
            if (mScrollBar_Toggle_HuanHua.value > 0.9f)
            {
                mButton_Down_HuanHua.gameObject.SetActive(false);
            }
            else
            {
                mButton_Down_HuanHua.gameObject.SetActive(true);
            }
        }));
    }

    IEnumerator StartUI(SystemID varSystemID)
    {
        yield return null;

        yield return null;
        switch (varSystemID)
        {
            case SystemID.WaiGuan_WaiGuan:
                mToggle_Mount.value = true;
                if (EventDelegate.IsValid(mToggle_Mount.onChange))
                    EventDelegate.Execute(mToggle_Mount.onChange);
                break;
            case SystemID.WaiGuan_WaiGuan_Mount:
            case SystemID.WaiGuan_WaiGuan_Mount_Mount:
                mToggle_Mount.value = true;
                if (EventDelegate.IsValid(mToggle_Mount.onChange))
                    EventDelegate.Execute(mToggle_Mount.onChange);
                break;
            case SystemID.WaiGuan_Pet:
                mToggle_Pet.value = true;
                if (EventDelegate.IsValid(mToggle_Pet.onChange))
                    EventDelegate.Execute(mToggle_Pet.onChange);
                break;
            case SystemID.WaiGuan_Wing:
                mToggle_Wing.value = true;
                if (EventDelegate.IsValid(mToggle_Wing.onChange))
                    EventDelegate.Execute(mToggle_Wing.onChange);
                break;
            case SystemID.WaiGuan_ChengHao:
                mScrollBar_Toggle_WaiGuan.value = 1f;
                mToggle_Title.value = true;
                break;
            case SystemID.WaiGuan_FaBao:  //外观-法宝
            case SystemID.WaiGuan_FaBao_JiHuo:  //外观-法宝
            case SystemID.WaiGuan_FaBao_JiHuo1:  //外观-法宝1
            case SystemID.WaiGuan_FaBao_JiHuo2:  //外观-法宝2
            case SystemID.WaiGuan_FaBao_JiHuo3:  //外观-法宝3
            case SystemID.WaiGuan_FaBao_CuiLian://外观-法器-淬炼
            case SystemID.WaiGuan_FaBao_CuiLian1:  //外观-法宝1-淬炼
            case SystemID.WaiGuan_FaBao_CuiLian2:  //外观-法宝2-淬炼
            case SystemID.WaiGuan_FaBao_CuiLian3:  //外观-法宝3-淬炼
            case SystemID.WaiGuan_FaBao_ZhuLing: //外观-法器-注灵
            case SystemID.WaiGuan_FaBao_ZhuLing1: //外观-法器-注灵
            case SystemID.WaiGuan_FaBao_ZhuLing2: //外观-法器-注灵
            case SystemID.WaiGuan_FaBao_ZhuLing3: //外观-法器-注灵
                mToggle_FaQi.value = true;
                mScrollBar_Toggle_WaiGuan.value = 0.35f;
                if (varSystemID == SystemID.WaiGuan_FaBao_JiHuo
                        || varSystemID == SystemID.WaiGuan_FaBao_JiHuo1
                        || varSystemID == SystemID.WaiGuan_FaBao_JiHuo2
                        || varSystemID == SystemID.WaiGuan_FaBao_JiHuo3
                        || varSystemID == SystemID.WaiGuan_FaBao_CuiLian
                        || varSystemID == SystemID.WaiGuan_FaBao_CuiLian1
                        || varSystemID == SystemID.WaiGuan_FaBao_CuiLian2
                        || varSystemID == SystemID.WaiGuan_FaBao_CuiLian3
                        || varSystemID == SystemID.WaiGuan_FaBao_ZhuLing
                        || varSystemID == SystemID.WaiGuan_FaBao_ZhuLing1
                        || varSystemID == SystemID.WaiGuan_FaBao_ZhuLing2
                        || varSystemID == SystemID.WaiGuan_FaBao_ZhuLing3)
                {
                    Ui_FaoBao tempUi_FaoBao = UI_FaBao.GetComponent<Ui_FaoBao>();
                    if (tempUi_FaoBao != null)
                    {
                        tempUi_FaoBao.SetToggle(varSystemID);
                    }
                }
                break;
            case SystemID.WaiGuan_WaiGuan_LingQi:
            case SystemID.WaiGuan_WaiGuan_LingQi_LingQi:
                mToggle_LingQi.value = true;
                mScrollBar_Toggle_WaiGuan.value = 0.68f;
                break;
            case SystemID.WaiGuan_WaiGuan_ZuJi:
                mToggle_ZuJi.value = true;
                break;
            case SystemID.WaiGuan_HuanHua:
                mToggle_Clothes.value = true;
                if (EventDelegate.IsValid(mToggle_Clothes.onChange))
                    EventDelegate.Execute(mToggle_Clothes.onChange);
                break;
            case SystemID.WaiGuan_HuanHua_ShiZhuang:
                mToggle_Clothes.value = true;
                if (EventDelegate.IsValid(mToggle_Clothes.onChange))
                    EventDelegate.Execute(mToggle_Clothes.onChange);
                break;
            case SystemID.WaiGuan_HuanHua_Equip:
                mToggle_Wuqi_HuanHua.value = true;
                break;
            case SystemID.WaiGuan_HuanHua_Mount:
                mToggle_Mount_HuanHua.value = true;
                break;
            case SystemID.WaiGuan_HuanHua_LingQi:
                mToggle_LingQi_HuanHua.value = true;
                break;
            case SystemID.WaiGuan_HuanHua_ZuJi:
                mToggle_ZuJi_HuanHua.value = true;
                break;
            case SystemID.WaiGuan_HuanHua_Wings:
                mToggle_Wing_HuanHua.value = true;
                break;

        }
        if (IsFrist)
        {
            mToggle_Mount.optionCanBeNone = false;
            mToggle_Pet.optionCanBeNone = false;
            mToggle_Wing.optionCanBeNone = false;
            mToggle_Title.optionCanBeNone = false;
            mToggle_FaQi.optionCanBeNone = false;
            mToggle_LingQi.optionCanBeNone = false;
            mToggle_ZuJi.optionCanBeNone = false;
            mToggle_Clothes.optionCanBeNone = false;
            mToggle_Wuqi_HuanHua.optionCanBeNone = false;
            mToggle_Mount_HuanHua.optionCanBeNone = false;
            mToggle_ZuJi_HuanHua.optionCanBeNone = false;
            mToggle_Wing_HuanHua.optionCanBeNone = false;
            mToggle_LingQi_HuanHua.optionCanBeNone = false;
            mToggle_WaiGuan.optionCanBeNone = false;
            mToggle_HuanHua.optionCanBeNone = false;

            mToggle_Mount.onChange.Add(new EventDelegate(() =>
            {
                if(mToggle_Mount.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_Pet.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_Pet.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_Wing.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_Wing.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_Title.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_Title.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_FaQi.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_FaQi.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_LingQi.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_LingQi.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_ZuJi.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_ZuJi.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_Clothes.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_Clothes.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_Wuqi_HuanHua.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_Wuqi_HuanHua.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_Mount_HuanHua.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_Mount_HuanHua.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_LingQi_HuanHua.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_LingQi_HuanHua.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_ZuJi_HuanHua.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_ZuJi_HuanHua.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_Wing_HuanHua.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_Wing_HuanHua.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_WaiGuan.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_WaiGuan.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));
            mToggle_HuanHua.onChange.Add(new EventDelegate(() =>
            {
                if (mToggle_HuanHua.value)
                    AudioManager.GetSingleton().PlayAudio(AudioPath.ClickItem);
            }));

            IsFrist = false;

            ShowGuide();
        }
        
    }

    void GetToggleNum(SystemID varSystemId, GameObject varGameObject, ref int varNum)
    {
        if (Helper.SetToggleState(varSystemId, varGameObject))
        {
            varNum++;
        }
    }


    private void OpenSubsystem(SystemID varSystemID, UIToggle varToggle)
    {

        if (mSystemID == varSystemID)
        {
            varToggle.value = true;
            if (mOpenSystemAction != null)
            {
                mOpenSystemAction(mLastObj);
                mOpenSystemAction = null;
            }
        }
    }
    /// <summary>
    /// 设置要打开的子模块
    /// </summary>
    /// <param name="varSystemID"></param>
    public void SetOpenSubsystemID(SystemID varSystemID, System.Action<GameObject> varAction)
    {
        mSystemID = varSystemID;
        mOpenSystemAction = varAction;//很大可能为空
        
    }

    private string mLastPath;
    void OpenUI(GameObject varObj)
    {
        UI_Panel_Mount.gameObject.SetActive(false);
        UI_Panel_Pet.gameObject.SetActive(false);
        UI_Wings.gameObject.SetActive(false);
        UI_Title.gameObject.SetActive(false);
        UI_FaBao.gameObject.SetActive(false);
        UI_ClothesFashion.gameObject.SetActive(false);
        UI_WeaponFashion.gameObject.SetActive(false);
        UI_FantasticChange.gameObject.SetActive(false);
        UI_ZuJi.gameObject.SetActive(false);
        UI_XianQi.gameObject.SetActive(false);
        UI_FootPrintFashion.gameObject.SetActive(false);
        UI_XianQiFashion.gameObject.SetActive(false);
        UI_WingsFashion.gameObject.SetActive(false);
        varObj.SetActive(true);
        if (mOpenSystemAction != null)
        {
            mOpenSystemAction(varObj);
            mOpenSystemAction = null;
        }
    }

    void ShowGuide()
    {
        //玄兵
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_FaBao))
        {
            mScrollBar_Toggle_WaiGuan.value = 0.34f;
            GuideManager.GetSingleton().OpenTipsGuide(mToggle_FaQi.transform, null,2, 0.3f);
        }
        //伙伴解锁
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_PetUnLock))
        {
            //if (mToggle_Pet.value == false)
            //{
            //    GuideManager.GetSingleton().OpenGudieNoMaskForce(mToggle_Pet.transform, null, 0.3f);
            //}
        }
        //伙伴成长
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_PetChengZhang))
        {
            if (mToggle_Pet.value == false)
            {
                GuideManager.GetSingleton().OpenTipsGuide(mToggle_Pet.transform, null,2, 0.5f);
            }
        }

        //伙伴融合
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_PetRongHe))
        {
            if (mToggle_Pet.value == false)
            {
                GuideManager.GetSingleton().OpenTipsGuide(mToggle_Pet.transform, null,3, 0.3f);
            }
        }

        //伙伴灵修
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_PetLingXiu))
        {
            if (mToggle_Pet.value == false)
            {
                GuideManager.GetSingleton().OpenTipsGuide(mToggle_Pet.transform, null, 2,0.3f);
            }
        }
        //伙伴功法
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_PetSkill))
        {
            if (mToggle_Pet.value == false)
            {
                GuideManager.GetSingleton().OpenTipsGuide(mToggle_Pet.transform, null,2, 0.3f);
            }
        }

        //仙器
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_XianQi))
        {
            mScrollBar_Toggle_WaiGuan.value = 1f;
            //Invoke("SetScrollBarValue",0.2f);
			DelayTaskManager.GetSingleton().AddDelayTask(0.2f,()=>{SetScrollBarValue();});
            GuideManager.GetSingleton().OpenTipsGuide(mToggle_LingQi.transform, null, 2, 0.5f);
        }
        //足迹
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_Foot))
        {
            GuideManager.GetSingleton().OpenTipsGuide(mToggle_ZuJi.transform, null, 2, 0.3f);
        }
    }

    private void SetScrollBarValue()
    {
        mScrollBar_Toggle_WaiGuan.value = 1f;
    }

    //关闭外观
    void CloseGuidePetGotoBattle()
    {
        if (GuideManager.GetSingleton().JudgeHave(GuideEnum.Guide_PetUnLock))
        {
            GuideManager.GetSingleton().OpenTipsGuide(mClose.transform, ()=> {
                GuideManager.GetSingleton().CloseGuide();
                GuideManager.GetSingleton().RemoveGuide(GuideEnum.Guide_PetUnLock);
            },7, 0);
        }
    }
    //关闭外观
    void CloseGuideMount()
    {
        GuideManager.GetSingleton().OpenTipsGuide(mClose.transform, null, 4, 0, GuideEnum.Guide_MountUnLock);
    }

	public override void OnDestroy()
    {
		base.OnDestroy ();
        if (mLastObj != null)
        {
            ResourcesManager.GetSingleton().ReleaseShort(mLastPath);
            Destroy(mLastObj);
        }

        for (int i = 0; i < mListLoadTask.Count; i++)
        {
            if (mListLoadTask[i] != null)
            {
                mListLoadTask[i].mStatus = ResourcesManager.LoadTaskStatus.Cancel;
            }
        }

        WindowManager.GetSingleton().CloseWindow(UIPath.UI_Medicine_Path);
    }
}
