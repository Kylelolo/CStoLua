using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CStoLua
{
    class Program
    {
        static string[] mSourceStrArr;

        static Dictionary<string, string> mReplaceData = new Dictionary<string, string>();
        static void LoadGlobalReplaceData()
        {
            {
                string[] tmpReplaceDataArr = File.ReadAllLines("./replace.txt");
                for (int i = 0; i < tmpReplaceDataArr.Length; i++)
                {
                    string tmpOneLine = tmpReplaceDataArr[i];
                    string[] tmpKeyValue = tmpOneLine.Split(new string[] { "->" }, StringSplitOptions.None);
                    if (tmpKeyValue.Length != 2)
                    {
                        continue;
                    }
                    if (mReplaceData.ContainsKey(tmpKeyValue[0]) == false)
                    {
                        mReplaceData.Add(tmpKeyValue[0], tmpKeyValue[1]);
                    }
                }
            }
            {
                string[] tmpReplaceDataArr = File.ReadAllLines("./CSTypes_replace.txt");
                for (int i = 0; i < tmpReplaceDataArr.Length; i++)
                {
                    string tmpOneLine = tmpReplaceDataArr[i];
                    string[] tmpKeyValue = tmpOneLine.Split(new string[] { "->" }, StringSplitOptions.None);
                    if (tmpKeyValue.Length != 2)
                    {
                        continue;
                    }
                    if (mReplaceData.ContainsKey(tmpKeyValue[0]) == false)
                    {
                        mReplaceData.Add(tmpKeyValue[0], tmpKeyValue[1]);
                    }

                }
            }
            {
                string[] tmpReplaceDataArr = File.ReadAllLines("./CSFunctions.txt");
                for (int i = 0; i < tmpReplaceDataArr.Length; i++)
                {
                    string tmpOneLine = tmpReplaceDataArr[i];
                    string[] tmpKeyValue = tmpOneLine.Split(new string[] { "->" }, StringSplitOptions.None);
                    if (tmpKeyValue.Length != 2)
                    {
                        continue;
                    }
                    if (mReplaceData.ContainsKey(tmpKeyValue[0]) == false)
                    {
                        mReplaceData.Add(tmpKeyValue[0], tmpKeyValue[1]);
                    }
                    
                }
            }
            {
                string[] tmpReplaceDataArr = File.ReadAllLines("./CSMemberVariables.txt");
                for (int i = 0; i < tmpReplaceDataArr.Length; i++)
                {
                    string tmpOneLine = tmpReplaceDataArr[i];
                    string[] tmpKeyValue = tmpOneLine.Split(new string[] { "->" }, StringSplitOptions.None);
                    if (tmpKeyValue.Length != 2)
                    {
                        continue;
                    }
                    if (mReplaceData.ContainsKey(tmpKeyValue[0]) == false)
                    {
                        mReplaceData.Add(tmpKeyValue[0], tmpKeyValue[1]);
                    }

                }
            }
        }

        static List<string> mRemoveBeginData = new List<string>();
        static void LoadRemoveBeginData()
        {
            {
                string[] tmpReplaceDataArr = File.ReadAllLines("./removebegin.txt");
                for (int i = 0; i < tmpReplaceDataArr.Length; i++)
                {
                    string tmpOneLine = tmpReplaceDataArr[i];
                    tmpOneLine = tmpOneLine.Trim();
                    if (mRemoveBeginData.Contains(tmpOneLine) == false)
                    {
                        mRemoveBeginData.Add(tmpOneLine);
                    }
                }
            }
            {
                string[] tmpReplaceDataArr = File.ReadAllLines("./CSTypes.txt");
                for (int i = 0; i < tmpReplaceDataArr.Length; i++)
                {
                    string tmpOneLine = tmpReplaceDataArr[i];
                    tmpOneLine = tmpOneLine.Trim();
                    if (mRemoveBeginData.Contains(tmpOneLine)==false)
                    {
                        mRemoveBeginData.Add(tmpOneLine);
                    }
                    
                }
            }

            
        }

        static string translat(string tmpOneLine, string[] mSourceStrArr, int i)
        {
            tmpOneLine = Translator.T_AddComponent(tmpOneLine);
            tmpOneLine = Translator.T_GetComponent(tmpOneLine);
            tmpOneLine = Translator.T_GetComponentsInChildren(tmpOneLine);

            tmpOneLine = Translator.T_as_GameObject(tmpOneLine);
            tmpOneLine = Translator.T_NewGameObject(tmpOneLine);
            tmpOneLine = Translator.T_NotEqual(tmpOneLine);
            tmpOneLine = Translator.T_null(tmpOneLine);
            tmpOneLine = Translator.T_Brace(tmpOneLine, mSourceStrArr, i);


            tmpOneLine = Translator.T_if_add_then(tmpOneLine, mSourceStrArr, i);
            tmpOneLine = Translator.T_else_if_then(tmpOneLine, mSourceStrArr, i);

            tmpOneLine = Translator.T_And_OR(tmpOneLine);
            tmpOneLine = Translator.T_UIPath_UI_Path(tmpOneLine);

            tmpOneLine = Translator.T_Destroy(tmpOneLine);

            tmpOneLine = Translator.T_LastIndexOf(tmpOneLine);
            tmpOneLine = Translator.T_Substring(tmpOneLine);

            tmpOneLine = Translator.T_Action(tmpOneLine);

            tmpOneLine = Translator.T_float(tmpOneLine);


            tmpOneLine = Translator.T_EventDispatch(tmpOneLine);
            


            //配置替换,在上面的处理代码中有一些被误杀的，也在这里修正
            foreach (var item in mReplaceData)
            {
                tmpOneLine = tmpOneLine.Replace(item.Key, item.Value);
            }

            //替换以这个开头的 
            //例如 UIEventListener tmpUIEventListener = UIEventListener.Get(varTran.gameObject);
            tmpOneLine = RemoveBegin(tmpOneLine);

            tmpOneLine = Translator.T_RemoveSimicolon(tmpOneLine);

            return tmpOneLine;
        }

        static void Main(string[] args)
        {
            string tmpExeDir= AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Directory.SetCurrentDirectory(tmpExeDir);
            Console.WriteLine("WorkingPath:" + tmpExeDir);

            LoadGlobalReplaceData();
            LoadRemoveBeginData();







            //先格式化
            {
                mSourceStrArr = File.ReadAllLines("./src.cs");
                for (int i = 0; i < mSourceStrArr.Length; i++)
                {
                    string tmpOneLine = mSourceStrArr[i];

                    tmpOneLine = Translator.T_Format(tmpOneLine, mSourceStrArr, i);

                    Console.WriteLine(tmpOneLine);

                    mSourceStrArr[i] = tmpOneLine;
                }
                //写文件
                File.WriteAllLines("./src_format.cs", mSourceStrArr);
            }


            //转换
            {
                mSourceStrArr = File.ReadAllLines("./src_format.cs");
                for (int i = 0; i < mSourceStrArr.Length; i++)
                {
                    string tmpOneLine = mSourceStrArr[i];

                    tmpOneLine = translat(tmpOneLine, mSourceStrArr, i);
                    tmpOneLine = translat(tmpOneLine, mSourceStrArr, i);

                    Console.WriteLine(tmpOneLine);

                    mSourceStrArr[i] = tmpOneLine;
                }

                //写文件
                File.WriteAllLines("./new.lua", mSourceStrArr);

                Console.WriteLine("---------------SUCCESS-----------------");
                Console.ReadLine();
            }
        }


        public static string RemoveBegin(string varStr)
        {
            string tmpOneLine = varStr;


            foreach (var item in mRemoveBeginData)
            {
                if (tmpOneLine.Contains(item))
                {
                    //先存储前面的占位符空格
                    int tmpBegin = tmpOneLine.IndexOf(item);
                    string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                    string tmpOneLineForLocalReplace = tmpOneLine.Trim();
                    if (tmpOneLineForLocalReplace.StartsWith(item+" "))
                    {
                        tmpOneLineForLocalReplace = tmpOneLineForLocalReplace.Substring(item.Length);
                        tmpOneLine = tmpTab + "local" + tmpOneLineForLocalReplace;
                    }
                }
            }

            return tmpOneLine;
        }

    }
}
