using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStoLua
{
    class Translator
    {
        public static string Func_2_Param(string varStr)
        {
            string re1 = ".*?"; // Non-greedy match on filler
            string re2 = "(?:[a-z][a-z0-9_]*)"; // Uninteresting: var
            string re3 = ".*?"; // Non-greedy match on filler
            string re4 = "(?:[a-z][a-z0-9_]*)"; // Uninteresting: var
            string re5 = ".*?"; // Non-greedy match on filler
            string re6 = "((?:[a-z][a-z0-9_]*))";   // Variable Name 1
            string re7 = "(\\()";   // Any Single Character 1
            string re8 = ".*?"; // Non-greedy match on filler
            string re9 = "(?:[a-z][a-z0-9_]*)"; // Uninteresting: var
            string re10 = ".*?";    // Non-greedy match on filler
            string re11 = "((?:[a-z][a-z0-9_]*))";  // Variable Name 2
            string re12 = "(,)";    // Any Single Character 2
            string re13 = ".*?";    // Non-greedy match on filler
            string re14 = "(?:[a-z][a-z0-9_]*)";    // Uninteresting: var
            string re15 = ".*?";    // Non-greedy match on filler
            string re16 = "((?:[a-z][a-z0-9_]*))";  // Variable Name 3
            string re17 = "(\\))";  // Any Single Character 3

            Regex r = new Regex(re1 + re2 + re3 + re4 + re5 + re6 + re7 + re8 + re9 + re10 + re11 + re12 + re13 + re14 + re15 + re16 + re17, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(varStr);
            if (m.Success)
            {
                String var1 = m.Groups[1].ToString();
                String c1 = m.Groups[2].ToString();
                String var2 = m.Groups[3].ToString();
                String c2 = m.Groups[4].ToString();
                String var3 = m.Groups[5].ToString();
                String c3 = m.Groups[6].ToString();

                string str = var1.ToString() + c1.ToString() + var2.ToString() + c2.ToString() + var3.ToString() + c3.ToString();
                //Console.Write(str);
                return str;
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
                string tmpTab = tmpOneLine.Substring(0, tmpBegin);

                tmpOneLine = tmpOneLine.Trim();
                if (tmpOneLine.StartsWith("GameObject"))
                {
                    tmpOneLine = tmpTab + "local" + tmpOneLine.Substring(10);
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
                if (varIndex + 1< varSourceArr.Length&&varSourceArr[varIndex + 1].Trim() == "else")
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

        
       
    }
}
