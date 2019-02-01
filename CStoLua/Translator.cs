using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace CStoLua
{
    class Translator
    {
        /// <summary>
        /// 格式化代码
        /// 
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_Format(string varStr, string[] varSourceArr, int varIndex)
        {
            string tmpOneLine = varStr;

            //存储前面的空格对齐
            string tmpTabs = string.Empty;

            int tmpIndexofFirstNoEmptyChar = GetIndexofFirstNoEmptyChar(varStr);
            if (tmpIndexofFirstNoEmptyChar >= 0)
            {
                tmpTabs = varStr.Substring(0, tmpIndexofFirstNoEmptyChar);
            }

            //if (self.mStarted == false) { 给 { 左边插入换行符
            if(containsNewLine(varStr)==false)
            {
                StringBuilder tmpStringBuilder = new StringBuilder();
                for (int i = 0; i < tmpOneLine.Length; i++)
                {
                    string tmpStrCombine = string.Empty;

                    char tmpChar = tmpOneLine[i];
                    if (tmpChar == '{')
                    {
                        if (haveCharInLeft(tmpOneLine, i))
                        {
                            //如果左边有字母
                            tmpStrCombine = "\n" + tmpTabs;
                        }
                        tmpStrCombine = tmpStrCombine + "{";
                        if (haveCharInRight(tmpOneLine, i))
                        {
                            if(rightIsCharIgnoreEmpty(tmpOneLine,i,'\n'))
                            {
                                tmpStrCombine = tmpStrCombine + tmpTabs + "\t";
                            }
                            else
                            {
                                tmpStrCombine = tmpStrCombine + "\n" + tmpTabs + "\t";
                            }
                            
                        }
                    }
                    else if (tmpChar == '}')
                    {
                        if (haveCharInLeft(tmpOneLine, i))
                        {
                            //如果左边有字母
                            if (leftIsCharIgnoreEmpty(tmpOneLine, i, '\n'))
                            {
                                tmpStrCombine = tmpTabs;
                            }
                            else
                            {
                                tmpStrCombine = "\n" + tmpTabs;
                            }
                                
                        }
                        tmpStrCombine = tmpStrCombine + "}";
                        if (haveCharInRight(tmpOneLine, i))
                        {
                            if (leftIsCharIgnoreEmpty(tmpOneLine, i, '\n'))
                            {
                                tmpStrCombine = tmpStrCombine + tmpTabs + "\t";
                            }
                            else
                            {
                                tmpStrCombine = tmpStrCombine + "\n" + tmpTabs + "\t";
                            }
                                
                        }
                    }
                    else
                    {
                        tmpStrCombine = tmpChar.ToString();
                    }

                    tmpStringBuilder.Append(tmpStrCombine);
                }
                tmpOneLine = tmpStringBuilder.ToString();
            }

            //if(a<b)
            //	a++;
            //if 语句，只有条件，没有大括号
            //下一句不是空格 也不是大括号 符合条件
            //给第二行添加大括号
            {
                string tmpOneLine_Trimed = tmpOneLine.Trim();
                if(tmpOneLine_Trimed.StartsWith("if"))
                {
                    if(CharCountSame(tmpOneLine_Trimed,'(',')') && CharCountSame(tmpOneLine_Trimed, '{', '}'))
                    {
                        //判断下一行是否有大括号
                        int tmpIndex = varIndex+1;
                        while (tmpIndex< varSourceArr.Length)
                        {
                            if (varSourceArr[tmpIndex].Trim() == string.Empty)
                            {
                                tmpIndex++;
                                continue;
                            }

                            if(varSourceArr[tmpIndex].Trim().StartsWith("{")==false)
                            {
                                //添加{ }
                                varSourceArr[tmpIndex] = tmpTabs+ "{\n" + varSourceArr[tmpIndex] + "\n"+tmpTabs+"}";
                                //varSourceArr[tmpIndex] = "{" + varSourceArr[tmpIndex] +"}";
                            }

                            break;
                        }
                    }
                }
            }

            return tmpOneLine;
        }


        

        /// <summary>
        /// 替换函数定义的一行
        /// public override void Awake()
        /// public void Awake()
        /// void Awake()
        ///   string Awake()
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_FunctionDeclare(string varStr)
        {


            int tmpWordNum = 0;//统计(左边的单词数量)
            bool tmpBeginCountWordNum = false;//跳过一行前面的空格 与 tab 等，开始统计左边单词数量,>=2是基本条件
            bool tmpFinishCountWordNum = false;

            int tmpFunctionNameBeginIndex = 0;//函数名从第几位开始
            int tmpLeftParenthesesIndex = 0;//( 在第几位

            for (int i = 0; i < varStr.Length; i++)
            {
                char tmpChar = varStr[i];

                if (tmpFinishCountWordNum)
                {
                    //检查( 右边字符
                    //这里过滤= 就会过滤掉带有默认参数的函数
                    if (tmpChar == ';' || tmpChar == '=')
                    {
                        tmpWordNum = 0;
                        break;
                    }
                    continue;
                }

                //--------------------------
                //当 检查到 ( 之后，下面的代码就不会执行了
                // 只会走上面的检查 ( 右边的字符串
                //------------------------

                //检查( 左边字符
                if(tmpChar=='(')
                {
                    if(tmpBeginCountWordNum)
                    {
                        tmpWordNum++;
                    }
                    tmpLeftParenthesesIndex = i;
                    tmpFinishCountWordNum = true;
                    if(tmpWordNum<2)
                    {
                        break;
                    }
                    continue;
                }

                //函数定义式  ( 左边不能有其他特殊字符
                if(tmpChar==' ' || tmpChar=='\n' || tmpChar=='\t')
                {
                    if (tmpBeginCountWordNum==false)
                    {
                        continue;
                    }
                }
                else
                {
                    //检查 ( 左边的单词是否合格
                    string tmpSubString_TrimLeft = varStr.Substring(i);
                    if (tmpSubString_TrimLeft.StartsWith("if(")
                        || tmpSubString_TrimLeft.StartsWith("if ")
                        || tmpSubString_TrimLeft.StartsWith("else(")
                        || tmpSubString_TrimLeft.StartsWith("else ")
                        || tmpSubString_TrimLeft.StartsWith("for(")
                        || tmpSubString_TrimLeft.StartsWith("for ")
                        || tmpSubString_TrimLeft.StartsWith("foreach(")
                        || tmpSubString_TrimLeft.StartsWith("foreach ")
                        || tmpSubString_TrimLeft.StartsWith("new "))
                    {
                        tmpWordNum = 0;
                        break;
                    }
                    if (isChar(tmpChar)==false)
                    {
                        tmpWordNum = 0;
                        break;
                    }
                }

                tmpBeginCountWordNum = true;

                

                if(tmpChar==' ')
                {
                    tmpWordNum++;
                    tmpBeginCountWordNum = false;

                    //每次遇到空格，都更新tmpFunctionNameBeginIndex,这个Index是带空格的，所以下面往前面加上function的时候不用带空格
                    if(rightIsCharIgnoreEmpty(varStr,i,'(')==false)
                    {
                        tmpFunctionNameBeginIndex = i;
                    }
                }
            }

            if(tmpWordNum>=2 && tmpFinishCountWordNum)
            {
                


                string tmpFuncName = varStr.Substring(tmpFunctionNameBeginIndex, tmpLeftParenthesesIndex - tmpFunctionNameBeginIndex);
                //Console.WriteLine(tmpFuncName);

                string tmpTabs = varStr.Substring(0,GetIndexofFirstNoEmptyChar(varStr));

                string tmpNewLine = tmpTabs + "function" + varStr.Substring(tmpFunctionNameBeginIndex);
                Console.WriteLine(tmpNewLine);
                varStr = tmpNewLine;


                //StreamWriter tmpStreamWrite = new StreamWriter("./testfunctions.cs", true);
                //tmpStreamWrite.WriteLine(varStr);
                //tmpStreamWrite.Flush();
                //tmpStreamWrite.Close();
            }

            return varStr;
        }

        public static string T_AddComponent(string varStr)
        {
            string tmpOneLine = varStr;
            if (tmpOneLine.Contains(".AddComponent<"))
            {
                int tmpBegin = tmpOneLine.IndexOf('<') + 1;
                int tmpEnd = tmpOneLine.IndexOf('>');
                string tmpTypeName = tmpOneLine.Substring(tmpBegin, tmpEnd - tmpBegin);

                tmpOneLine = tmpOneLine.Replace(".AddComponent<" + tmpTypeName + ">();", ":AddComponent(typeof(" + tmpTypeName + "))");

                //替换为local
                {
                    //先存储前面的占位符空格
                    tmpBegin = tmpOneLine.IndexOf(tmpTypeName);
                    string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                    string tmpOneLineForLocalReplace = tmpOneLine.Trim();
                    if (tmpOneLineForLocalReplace.StartsWith(tmpTypeName))
                    {
                        //临时变量
                        tmpOneLineForLocalReplace = tmpOneLineForLocalReplace.Substring(tmpTypeName.Length);
                        tmpOneLine = tmpTab+ "local" + tmpOneLineForLocalReplace;
                    }
                    else
                    {
                        //可能是全局变量，也可能是之前定义的变量，这里不好判断
                    }
                }

            }
            return tmpOneLine;
        }



        public static string T_GetComponent(string varStr)
        {
            string tmpOneLine = varStr;
            if (tmpOneLine.Contains(".GetComponent<"))
            {
                int tmpBegin = tmpOneLine.IndexOf('<') + 1;
                int tmpEnd = tmpOneLine.IndexOf('>');
                string tmpTypeName = tmpOneLine.Substring(tmpBegin, tmpEnd - tmpBegin);

                tmpOneLine = tmpOneLine.Replace(".GetComponent<" + tmpTypeName + ">();", ":GetComponent(typeof(" + tmpTypeName + "))");

                //替换为local
                {
                    //先存储前面的占位符空格
                    tmpBegin = tmpOneLine.IndexOf(tmpTypeName);
                    string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                    string tmpOneLineForLocalReplace = tmpOneLine.Trim();
                    if (tmpOneLineForLocalReplace.StartsWith(tmpTypeName))
                    {
                        //临时变量
                        tmpOneLineForLocalReplace = tmpOneLineForLocalReplace.Substring(tmpTypeName.Length);
                        tmpOneLine = tmpTab+ "local" + tmpOneLineForLocalReplace;
                    }
                    else
                    {
                        //可能是全局变量，也可能是之前定义的变量，这里不好判断
                    }
                }

            }
            return tmpOneLine;
        }

        public static string T_GetComponentsInChildren(string varStr)
        {
            string tmpOneLine = varStr;
            if (tmpOneLine.Contains(".GetComponentsInChildren<"))
            {
                int tmpBegin = tmpOneLine.IndexOf('<') + 1;
                int tmpEnd = tmpOneLine.IndexOf('>');
                string tmpTypeName = tmpOneLine.Substring(tmpBegin, tmpEnd - tmpBegin);

                tmpOneLine = tmpOneLine.Replace(".GetComponentsInChildren<" + tmpTypeName + ">();", ":GetComponentsInChildren(typeof(" + tmpTypeName + "))");

                //替换为local
                {
                    //先存储前面的占位符空格
                    tmpBegin = tmpOneLine.IndexOf(tmpTypeName);
                    string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                    string tmpOneLineForLocalReplace = tmpOneLine.Trim();
                    if (tmpOneLineForLocalReplace.StartsWith(tmpTypeName))
                    {
                        //临时变量
                        tmpOneLineForLocalReplace = tmpOneLineForLocalReplace.Substring(tmpTypeName.Length);
                        tmpOneLine = tmpTab + "local" + tmpOneLineForLocalReplace;

                        //去除[]
                        tmpOneLine = tmpOneLine.Replace("[]", "");
                    }
                    else
                    {
                        //可能是全局变量，也可能是之前定义的变量，这里不好判断
                    }
                }

            }
            return tmpOneLine;
        }


        public static string T_as_GameObject(string varStr)
        {
            string tmpOneLine = varStr;


            string tmpTag = " as GameObject;";
            if (tmpOneLine.Contains(tmpTag))
            {
                tmpOneLine = tmpOneLine.Replace(tmpTag, "");

                //先存储前面的占位符空格
                int tmpBegin = tmpOneLine.IndexOf("GameObject");
                if(tmpBegin>0)
                {
                    string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                    string tmpTrimedLine= tmpOneLine.Trim();
                    if (tmpTrimedLine.StartsWith("GameObject"))
                    {
                        tmpOneLine = tmpTab + "local" + tmpTrimedLine.Substring(10);
                    }
                }
                
            }
            return tmpOneLine;
        }

        public static string T_NewGameObject(string varStr)
        {
            string tmpOneLine = varStr;

            if (tmpOneLine.Contains(" GameObject()"))
            {
                tmpOneLine=tmpOneLine.Replace("new ", "");

                //先存储前面的占位符空格
                int tmpBegin = tmpOneLine.IndexOf("GameObject");
                string tmpTab = tmpOneLine.Substring(0, tmpBegin );

                tmpOneLine = tmpOneLine.Trim();
                if (tmpOneLine.StartsWith("GameObject"))
                {
                    tmpOneLine = tmpTab+ "local" + tmpOneLine.Substring(10);
                }
            }
            return tmpOneLine;
        }


        public static string T_NotEqual(string varStr)
        {
            string tmpOneLine = varStr;
            tmpOneLine = tmpOneLine.Replace("!=", "~=");
            return tmpOneLine;
        }

        public static string T_null(string varStr)
        {
            string tmpOneLine = varStr;
            tmpOneLine = tmpOneLine.Replace("null", "nil");
            return tmpOneLine;
        }

        /// <summary>
        /// 大括号{}
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_Brace(string varStr, string[] varSourceArr, int varIndex)
        {
            string tmpOneLine = varStr;
            if (tmpOneLine.Trim() == "{")
            {
                tmpOneLine = tmpOneLine.Replace("{", "");
            }
            if (tmpOneLine.Trim() == "}")
            {
                if ( (varIndex + 1< varSourceArr.Length&&((varSourceArr[varIndex + 1].Trim().StartsWith("else") || varSourceArr[varIndex + 1].Trim().StartsWith("else if"))))
                    || (varIndex + 2 < varSourceArr.Length && (varSourceArr[varIndex + 1].Trim()==""&&(varSourceArr[varIndex + 2].Trim().StartsWith("else") || varSourceArr[varIndex + 2].Trim().StartsWith("else if"))))

                    )
                {
                    tmpOneLine = tmpOneLine.Replace("}", "");
                }
                else
                {
                    tmpOneLine = tmpOneLine.Replace("}", "end");
                }
            }
            return tmpOneLine;
        }

        /// <summary>
        /// 替换 && ||
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_And_OR(string varStr)
        {
            string tmpOneLine = varStr;
            tmpOneLine = tmpOneLine.Replace("&&", "and");
            tmpOneLine = tmpOneLine.Replace("||", "or");
            return tmpOneLine;
        }

        public static string T_UIPath_UI_Path(string varStr)
        {
            string tmpOneLine = varStr;
            tmpOneLine = tmpOneLine.Replace("UIPath","UI_Path");
            return tmpOneLine;
        }



        /// <summary>
        /// 给if 后面添加 then
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_if_add_then(string varStr,string[] varSourceArr,int varIndex)
        {
            
            string tmpOneLine = varStr;

            if (tmpOneLine.Contains("if"))
            {
                //先存储前面的占位符空格
                int tmpBegin = tmpOneLine.IndexOf("if");
                string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                tmpOneLine = tmpOneLine.Trim();
                if (tmpOneLine.StartsWith("if") && tmpOneLine.EndsWith(")"))
                {
                    //如果下一行是 { ，才说明当前行是完整的if
                    if ( (varIndex + 1>= varSourceArr.Length || varSourceArr[varIndex + 1].Trim().StartsWith("{"))
                        || (varIndex + 2 >= varSourceArr.Length || (varSourceArr[varIndex + 1].Trim()==string.Empty && varSourceArr[varIndex + 2].Trim().StartsWith("{"))))
                    {
                        tmpOneLine = tmpTab + tmpOneLine + " then";
                    }
                    else
                    {
                        //说明if语句有换行
                        return varStr;
                    }


                    //如果这一行if 的只有一对小括号 那么就去掉开始的( 和结束的 )
                    if (GetCharCount(tmpOneLine, '(') ==  GetCharCount(tmpOneLine, ')'))
                    {
                        //tmpOneLine = tmpOneLine.Replace("(", "");
                        //tmpOneLine = tmpOneLine.Replace(")", "");

                        int tmpBeginIndex = tmpOneLine.IndexOf('(');
                        int tmpEndIndex = tmpOneLine.LastIndexOf(')');
                        StringBuilder tmpStringBuilder = new StringBuilder(tmpOneLine);
                        tmpStringBuilder[tmpBeginIndex] = ' ';
                        tmpStringBuilder[tmpEndIndex] = ' ';
                        tmpOneLine = tmpStringBuilder.ToString();
                    }

                }
                else
                {
                    return varStr;
                }
            }


            

            return tmpOneLine;
        }


        /// <summary>
        /// 给 else if 后面加 then
        /// </summary>
        /// <param name="varStr"></param>
        /// <param name="varSourceArr"></param>
        /// <param name="varIndex"></param>
        /// <returns></returns>
        public static string T_else_if_then(string varStr, string[] varSourceArr, int varIndex)
        {
            string tmpOneLine = varStr;

            if (tmpOneLine.Contains("else if"))
            {
                //先存储前面的占位符空格
                int tmpBegin = tmpOneLine.IndexOf("else if");
                string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                tmpOneLine = tmpOneLine.Trim();
                if (tmpOneLine.StartsWith("else if") && tmpOneLine.EndsWith(")"))
                {
                    //如果下一行是 { ，才说明当前行是完整的else if
                    if ((varIndex + 1 >= varSourceArr.Length || varSourceArr[varIndex + 1].Trim().StartsWith("{"))
                        || (varIndex + 2 >= varSourceArr.Length || (varSourceArr[varIndex + 1].Trim() == string.Empty && varSourceArr[varIndex + 2].Trim().StartsWith("{"))))
                    {
                        tmpOneLine = tmpTab + tmpOneLine + " then";
                    }
                    else
                    {
                        //说明if语句有换行
                        return varStr;
                    }


                    //如果这一行if 的只有一对小括号 那么就去掉开始的( 和结束的 )
                    if (GetCharCount(tmpOneLine, '(') == GetCharCount(tmpOneLine, ')'))
                    {
                        //tmpOneLine = tmpOneLine.Replace("(", "");
                        //tmpOneLine = tmpOneLine.Replace(")", "");

                        int tmpBeginIndex = tmpOneLine.IndexOf('(');
                        int tmpEndIndex = tmpOneLine.LastIndexOf(')');
                        StringBuilder tmpStringBuilder = new StringBuilder(tmpOneLine);
                        tmpStringBuilder[tmpBeginIndex] = ' ';
                        tmpStringBuilder[tmpEndIndex] = ' ';
                        tmpOneLine = tmpStringBuilder.ToString();
                    }

                }
                else
                {
                    return varStr;
                }
            }




            return tmpOneLine;
        }


        /// <summary>
        /// 处理Destroy 添加GameObject.
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_Destroy(string varStr)
        {
            string tmpOneLine = varStr;

            if (tmpOneLine.Contains("Destroy("))
            {
                //先存储前面的占位符空格
                int tmpBegin = tmpOneLine.IndexOf("Destroy(");
                string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                tmpOneLine = tmpOneLine.Trim();
                if (tmpOneLine.StartsWith("Destroy("))
                {
                    tmpOneLine = tmpTab + "GameObject." + tmpOneLine;
                    tmpOneLine=tmpOneLine.Replace(";", "");
                }
            }
            return tmpOneLine;
        }

        /// <summary>
        /// 处理 EventDispatch
        /// EventDispatch.GetSingleton().RegisterEventReceiver(EventID.Login_RequestConnectLoginServer, this);
        /// EventDispatch.GetSingleton().UnRegisterEventReceiver(EventID.Login_RequestConnectLoginServer, this);
        /// EventDispatch:RegisterEventCallback(EventId.Login_RequestVerifyVersion,self,self.Login_RequestVerifyVersion)
        /// EventDispatch:UnRegisterEventCallback(EventId.LuaFuLi_SingleInfo_Return,self.LuaFuLi_SingleInfo_Return)
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_EventDispatch(string varStr)
        {
            string tmpOneLine = varStr;

            if (tmpOneLine.Contains("EventDispatch.GetSingleton().RegisterEventReceiver"))
            {
                //先存储前面的占位符空格
                int tmpBegin = tmpOneLine.IndexOf("EventDispatch.GetSingleton().RegisterEventReceiver");
                string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                tmpOneLine = tmpOneLine.Trim();
                if (tmpOneLine.StartsWith("EventDispatch.GetSingleton().RegisterEventReceiver"))
                {
                    //tmpOneLine = tmpTab + "GameObject." + tmpOneLine;
                    //tmpOneLine = tmpOneLine.Replace(";", "");

                    tmpOneLine = tmpOneLine.Replace("EventDispatch.GetSingleton().RegisterEventReceiver(", "EventDispatch:RegisterEventCallback(");
                    tmpOneLine = tmpOneLine.Replace("this", "self,self.");
                }
            }

            if (tmpOneLine.Contains("EventDispatch.GetSingleton().UnRegisterEventReceiver("))
            {
                //先存储前面的占位符空格
                int tmpBegin = tmpOneLine.IndexOf("EventDispatch.GetSingleton().UnRegisterEventReceiver(");
                string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                tmpOneLine = tmpOneLine.Trim();
                if (tmpOneLine.StartsWith("EventDispatch.GetSingleton().UnRegisterEventReceiver("))
                {
                    //tmpOneLine = tmpTab + "GameObject." + tmpOneLine;
                    //tmpOneLine = tmpOneLine.Replace(";", "");

                    tmpOneLine = tmpOneLine.Replace("EventDispatch.GetSingleton().UnRegisterEventReceiver(", "EventDispatch:UnRegisterEventCallback(");
                    tmpOneLine = tmpOneLine.Replace("this", "self.");
                }
            }
            return tmpOneLine;
        }

        /// <summary>
        /// 处理LastIndexOf("
        ///  local tempIndex = varEffectName.LastIndexOf("/");
        ///  local tempIndex =string.LastIndexOf(varEffectName,"/")
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_LastIndexOf(string varStr)
        {
            string tmpOneLine = varStr;

            string tmpTag = ".LastIndexOf(";

            if (tmpOneLine.Contains(tmpTag))
            {
                //取出varEffectName
                string tmpVarName = GetStringBetween(varStr, "=", tmpTag);
                if (tmpVarName == null)
                {
                    return tmpOneLine;
                }
                string tmpIndexofTag = GetStringBetween(varStr, tmpTag, ");");
                if (tmpIndexofTag == null)
                {
                    return tmpOneLine;
                }

                string tmpBegin = GetStartTo(varStr, "=");
                if (tmpBegin == null)
                {
                    return tmpOneLine;
                }

                tmpOneLine = tmpBegin + "=string.LastIndexOf(" + tmpVarName + "," + tmpIndexofTag + ")";
            }
            return tmpOneLine;
        }


        /// <summary>
        /// 处理Substring("
        ///   local tmpEffectName = varEffectName.Substring(tempIndex + 1);
        ///   local tmpWindowName =string.sub(varWindowName,tempIndex)
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_Substring(string varStr)
        {
            string tmpOneLine = varStr;

            string tmpTag = ".Substring(";

            if (tmpOneLine.Contains(tmpTag))
            {
                //取出varEffectName
                string tmpVarName = GetStringBetween(varStr, "=", tmpTag);
                if (tmpVarName == null)
                {
                    return tmpOneLine;
                }
                string tmpIndexofTag = GetStringBetween(varStr, tmpTag, ");");
                if (tmpIndexofTag == null)
                {
                    return tmpOneLine;
                }

                string tmpBegin = GetStartTo(varStr, "=");
                if (tmpBegin == null)
                {
                    return tmpOneLine;
                }

                tmpOneLine = tmpBegin + "=string.sub(" + tmpVarName + "," + tmpIndexofTag + ")";
            }
            return tmpOneLine;
        }


        /// <summary>
        /// 去除行尾分号
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_RemoveSimicolon(string varStr)
        {
            string tmpOneLine = varStr;

            tmpOneLine = tmpOneLine.TrimEnd(new char[] {' ' });
            if (tmpOneLine.EndsWith(";"))
            {
                tmpOneLine = tmpOneLine.Substring(0, tmpOneLine.Length - 1);
            }
            return tmpOneLine;
        }


        /// <summary>
        /// Action转function
        /// .MoveToPoint(tmpSpeed,(varGo) =>
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_Action(string varStr)
        {
            string tmpOneLine = varStr;
            string tmpTag = "=>";
            if (tmpOneLine.Contains(tmpTag))
            {
                //找到=>
                int tmpIndexofTag = varStr.IndexOf(tmpTag);

                //往回找到第一个 ,
                int tmpIndexofFirstDouhaoBeforeTag = -1;
                for (int i = tmpIndexofTag; i >=0; i--)
                {
                    if(varStr[i]==',')
                    {
                        tmpIndexofFirstDouhaoBeforeTag = i;
                        break;
                    }
                }

                if (tmpIndexofFirstDouhaoBeforeTag > 0)
                {
                    //提取到中间的变量 (varGo) 带括号
                    string tmpVarWithKuohao = varStr.Substring(tmpIndexofFirstDouhaoBeforeTag + 1, tmpIndexofTag - tmpIndexofFirstDouhaoBeforeTag - 1);


                    tmpOneLine = varStr.Substring(0, tmpIndexofFirstDouhaoBeforeTag + 1) + "function" + tmpVarWithKuohao
                        + varStr.Substring(tmpIndexofTag + tmpTag.Length);
                    int a = 0;
                }
                else
                {
                    //tmpIndexofFirstDouhaoBeforeTag = 0;
                    ////没有找到, 说明是单独一行
                    ////提取到中间的变量 (varGo) 带括号
                    //string tmpVarWithKuohao = varStr.Substring(tmpIndexofFirstDouhaoBeforeTag, tmpIndexofTag - tmpIndexofFirstDouhaoBeforeTag - 1);


                    //tmpOneLine = "function" + tmpVarWithKuohao
                    //    + varStr.Substring(tmpIndexofTag + tmpTag.Length);
                }
            }
            return tmpOneLine;
        }

        /// <summary>
        /// 去除float
        /// 0.01f
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static string T_float(string varStr)
        {
            string tmpOneLine = varStr;

            char[] tmpCharArr= tmpOneLine.ToArray();

            //从左往右查找，找到数字，然后后一位是f的，替换这个f
            for (int i = 0; i < tmpCharArr.Length; i++)
            {
                char c = tmpCharArr[i];
                if(c>=48 && c<=57)
                {
                    //找到数字 判断后一位
                    if (i + 1 < varStr.Length)
                    {
                        c = varStr[i + 1];
                        if (c == 'f')
                        {
                            tmpCharArr[i + 1] = ' ';
                        }
                    }
                    
                }
            }

            tmpOneLine = string.Concat(tmpCharArr);

            return tmpOneLine;
        }

        

        /// <summary>
        /// 判断是否含有多个查找的标记
        /// </summary>
        /// <param name="varStr"></param>
        /// <param name="varTag"></param>
        /// <returns></returns>
        public static bool ContainsMoreThanOne(string varStr, string varTag)
        {
            if(varStr.IndexOf(varTag)==varStr.LastIndexOf(varTag))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取A和B之间的字符串
        /// </summary>
        /// <param name="varStr"></param>
        /// <param name="varBegin"></param>
        /// <param name="varEnd"></param>
        /// <returns></returns>
        public static string GetStringBetween(string varStr, string varBegin, string varEnd)
        {
            //包含多个varBegin varEnd的不进行处理
            if (ContainsMoreThanOne(varStr, varBegin) || ContainsMoreThanOne(varStr, varEnd))
            {
                return null;
            }
            int tmpBegin = varStr.IndexOf(varBegin)+ varBegin.Length;
            int tmpEnd = varStr.IndexOf(varEnd);
            int tmpLength = tmpEnd - tmpBegin;
            return varStr.Substring(tmpBegin, tmpLength);
        }


        public static string GetStartTo(string varStr, string varBegin)
        {
            if (ContainsMoreThanOne(varStr, varBegin))
            {
                return null;
            }
            return varStr.Substring(0, varStr.IndexOf(varBegin));
        }

        /// <summary>
        /// 计算一个字符出现的次数
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static int GetCharCount(string varStr,char c)
        {
            int tmpCount = 0;
            for (int i = 0; i < varStr.Length; i++)
            {
                if(varStr[i]==c)
                {
                    tmpCount++;
                }
            }
            return tmpCount;
        }

        /// <summary>
        /// 判断字符串中两个字符数量相等
        /// </summary>
        /// <param name="varStr"></param>
        /// <param name="varCharA"></param>
        /// <param name="varCharB"></param>
        /// <returns></returns>
        public static bool CharCountSame(string varStr,char varCharA,char varCharB)
        {
            return GetCharCount(varStr, varCharA) == GetCharCount(varStr, varCharB);
        }

        /// <summary>
        /// 找到第一个非空字符
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static int GetIndexofFirstNoEmptyChar(string varStr)
        {
            int tmpIndex = 0;
            for (int i = 0; i < varStr.Length; i++)
            {
                char c = varStr[i];
                if (c!=' ' && c!='\t' && c!='\n')
                {
                    return tmpIndex;
                }
                tmpIndex++;
            }
            return -1;
        }

        /// <summary>
        /// 找到字一个字母的index
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static int GetIndexofFirstChar(string varStr)
        {
            int tmpIndex = 0;
            for (int i = 0; i < varStr.Length; i++)
            {
                char c = varStr[i];
                if ((c>=65 && c<=90) || (c>=97 && c<=122))
                {
                    return tmpIndex;
                }
                tmpIndex++;
            }
            return -1;
        }

        //判断是否是字母
        public static bool isChar(char c)
        {
            return (c >= 65 && c <= 90) || (c >= 97 && c <= 122);
        }

        //判断指定位置右边是否有字母
        public static bool haveCharInRight(string varStr,int varIndex)
        {
            for (int i = varIndex; i < varStr.Length; i++)
            {
                if(isChar(varStr[i]))
                {
                    return true;
                }

            }
            return false;
        }

        //判断指定位置左边是否有字母
        public static bool haveCharInLeft(string varStr, int varIndex)
        {
            for (int i = varIndex; i >=0; i--)
            {
                if (isChar(varStr[i]))
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 当前 右边 除了空格，是指定的字符
        /// </summary>
        /// <param name="varStr"></param>
        /// <param name="varIndex"></param>
        /// <param name="varChar"></param>
        /// <returns></returns>
        public static bool rightIsCharIgnoreEmpty(string varStr,int varIndex,char varChar)
        {
            varIndex = varIndex + 1;
            for (int i = varIndex; i < varStr.Length; i++)
            {
                if(varStr[i]==' ' || varStr[i] == '\t')
                {
                    continue;
                }
                if (varStr[i]== varChar)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 当前 左边 除了空格，是指定的字符
        /// </summary>
        /// <param name="varStr"></param>
        /// <param name="varIndex"></param>
        /// <param name="varChar"></param>
        /// <returns></returns>
        public static bool leftIsCharIgnoreEmpty(string varStr, int varIndex, char varChar)
        {
            for (int i = varIndex-1; i >=0; i--)
            {
                if (varStr[i] == ' ' || varStr[i] == '\t')
                {
                    continue;
                }
                if (varStr[i] == varChar)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 是否包含换行符
        /// </summary>
        /// <param name="varStr"></param>
        /// <returns></returns>
        public static bool containsNewLine(string varStr)
        {
            for (int i = 0; i < varStr.Length; i++)
            {
                if(varStr[i]=='\n')
                {
                    return true;
                }
            }
            return false;
        }
    }
}
