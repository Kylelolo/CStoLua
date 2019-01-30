using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
using System.Reflection;

public class ExportSymbol 
{

	[MenuItem("XLua/ExportSymbol")]
	public static void Export()
	{
		//导出类名
		{
			StreamWriter tmpStreamWrite = new StreamWriter (Application.dataPath + "/../Document/CStoLua/CStoLua/bin/Debug/CSTypes.txt",false);
			{
				var classes = Assembly.Load("Assembly-CSharp").GetTypes();
				tmpStreamWrite.WriteLine ("---------------------------------------");
				tmpStreamWrite.WriteLine ("--------  Assembly-CSharp  ------------");
				tmpStreamWrite.WriteLine ("---------------------------------------");
				foreach (var item in classes)
				{
					if (item.Name.Contains ("<") || item.Name.Contains ("$") || item.Name.Contains ("`")) {
						continue;
					}
					tmpStreamWrite.WriteLine (item.Name);
				}
			}

			{
				tmpStreamWrite.WriteLine ("---------------------------------------");
				tmpStreamWrite.WriteLine ("--------    UnityEngine    ------------");
				tmpStreamWrite.WriteLine ("---------------------------------------");
				var classes = Assembly.Load("UnityEngine").GetTypes();
				foreach (var item in classes)
				{
					if (item.Name.Contains ("<") || item.Name.Contains ("$") || item.Name.Contains ("`")) {
						continue;
					}
					tmpStreamWrite.WriteLine (item.Name);
				}
			}

			tmpStreamWrite.Flush ();
			tmpStreamWrite.Close ();
		}

		//导出类名全局替换的
		{
			StreamWriter tmpStreamWrite = new StreamWriter (Application.dataPath + "/../Document/CStoLua/CStoLua/bin/Debug/CSTypes_replace.txt",false);
			{
				var classes = Assembly.Load("Assembly-CSharp").GetTypes();
				tmpStreamWrite.WriteLine ("---------------------------------------");
				tmpStreamWrite.WriteLine ("--------  Assembly-CSharp  ------------");
				tmpStreamWrite.WriteLine ("---------------------------------------");
				foreach (var item in classes)
				{
					if (item.Name.Contains ("<") || item.Name.Contains ("$") || item.Name.Contains ("`")) {
						continue;
					}
					tmpStreamWrite.WriteLine (" as "+item.Name+";->");
					tmpStreamWrite.WriteLine (" as "+item.Name+" ;->");
					tmpStreamWrite.WriteLine ("("+item.Name+" ->(");
				}
			}

			{
				tmpStreamWrite.WriteLine ("---------------------------------------");
				tmpStreamWrite.WriteLine ("--------    UnityEngine    ------------");
				tmpStreamWrite.WriteLine ("---------------------------------------");
				var classes = Assembly.Load("UnityEngine").GetTypes();
				foreach (var item in classes)
				{
					if (item.Name.Contains ("<") || item.Name.Contains ("$") || item.Name.Contains ("`")) {
						continue;
					}
					tmpStreamWrite.WriteLine (" as "+item.Name+";->");
					tmpStreamWrite.WriteLine (" as "+item.Name+" ;->");
					tmpStreamWrite.WriteLine ("("+item.Name+" ->(");
				}
			}

			tmpStreamWrite.Flush ();
			tmpStreamWrite.Close ();
		}

		//导出函数名
		{
			//以下函数不导出Function
			Dictionary<string,bool> tmpDonotExportFunctionClass = new Dictionary<string, bool> ()
			{
				{"APIUpdaterRuntimeServices",true},
			};

			StreamWriter tmpStreamWrite = new StreamWriter (Application.dataPath + "/../Document/CStoLua/CStoLua/bin/Debug/CSFunctions.txt",false);

			{
				Dictionary<string,bool> tmpPublicMethodsName = new Dictionary<string, bool> ();
				Dictionary<string,bool> tmpStaticMethodsName = new Dictionary<string, bool> ();

				var classes = Assembly.Load("Assembly-CSharp").GetTypes();
				tmpStreamWrite.WriteLine ("---------------------------------------");
				tmpStreamWrite.WriteLine ("--------  Assembly-CSharp  ------------");
				tmpStreamWrite.WriteLine ("---------------------------------------");
				foreach (var item in classes)
				{
					if (item.Name.Contains ("<") || item.Name.Contains ("$") || item.Name.Contains ("`") || tmpDonotExportFunctionClass.ContainsKey(item.Name)) {
						continue;
					}

					{
						//导出Private
						MethodInfo[] tmpMethodArr= item.GetMethods (BindingFlags.Instance|BindingFlags.NonPublic);
						for (int i = 0; i < tmpMethodArr.Length; i++) {
							MethodInfo tmpMethodInfo = tmpMethodArr [i];

							if (tmpMethodInfo.MemberType == MemberTypes.Method) 
							{
								//if (tmpMethodInfo.IsPublic) 
								{
									if (tmpMethodInfo.IsStatic) {
										tmpStaticMethodsName [tmpMethodInfo.Name] = true;
										//									if (tmpMethodInfo.Name == "AddComponent") {
										//										int a = 0;
										//									}
									} else {
										tmpPublicMethodsName [tmpMethodInfo.Name] = true;
									}
								}
							}
						}
					}


					//导出其他
					{
						MethodInfo[] tmpMethodArr= item.GetMethods ();
						for (int i = 0; i < tmpMethodArr.Length; i++) {
							MethodInfo tmpMethodInfo = tmpMethodArr [i];

							if (tmpMethodInfo.MemberType == MemberTypes.Method) 
							{
								//if (tmpMethodInfo.IsPublic) 
								{
									if (tmpMethodInfo.IsStatic) {
										tmpStaticMethodsName [tmpMethodInfo.Name] = true;
										//									if (tmpMethodInfo.Name == "AddComponent") {
										//										int a = 0;
										//									}
									} else {
										tmpPublicMethodsName [tmpMethodInfo.Name] = true;
									}
								}
							}
						}
					}

				}

				//从Public里面去除Static
				//写入
				foreach (var item in tmpPublicMethodsName) {
					if (tmpStaticMethodsName.ContainsKey (item.Key)==false) {
						tmpStreamWrite.WriteLine ("."+item.Key+"("+"->" + ":"+item.Key+"(");
						tmpStreamWrite.WriteLine ("."+item.Key+" ("+"->" + ":"+item.Key+" (");
						tmpStreamWrite.WriteLine ("\t"+item.Key+"("+"->" + "\tself:"+item.Key+"(");
						tmpStreamWrite.WriteLine ("\t"+item.Key+" ("+"->" + "\tself:"+item.Key+"(");
						tmpStreamWrite.WriteLine ("    "+item.Key+"("+"->" + "    self:"+item.Key+"(");
						tmpStreamWrite.WriteLine ("    "+item.Key+" ("+"->" + "    self:"+item.Key+"(");
					}
				}
			}

			{
				Dictionary<string,bool> tmpPublicMethodsName = new Dictionary<string, bool> ();
				Dictionary<string,bool> tmpStaticMethodsName = new Dictionary<string, bool> ();

				var classes = Assembly.Load("UnityEngine").GetTypes();
				tmpStreamWrite.WriteLine ("---------------------------------------");
				tmpStreamWrite.WriteLine ("--------    UnityEngine    ------------");
				tmpStreamWrite.WriteLine ("---------------------------------------");
				foreach (var item in classes)
				{
					if (item.Name.Contains ("<") || item.Name.Contains ("$") || item.Name.Contains ("`") || tmpDonotExportFunctionClass.ContainsKey(item.Name)) {
						continue;
					}

					MethodInfo[] tmpMethodArr= item.GetMethods ();
					for (int i = 0; i < tmpMethodArr.Length; i++) {
						MethodInfo tmpMethodInfo = tmpMethodArr [i];
						if (tmpMethodInfo.MemberType == MemberTypes.Method) 
						{
							if (tmpMethodInfo.IsPublic) {
								if (tmpMethodInfo.IsStatic) {
									tmpStaticMethodsName [tmpMethodInfo.Name] = true;
//									if (tmpMethodInfo.Name == "AddComponent") {
//										int a = 0;
//									}
								} else {
									tmpPublicMethodsName [tmpMethodInfo.Name] = true;
								}
							}
						}
					}
						
				}

				//从Public里面去除Static
				//写入
				foreach (var item in tmpPublicMethodsName) {
					if (tmpStaticMethodsName.ContainsKey (item.Key)==false) {
						tmpStreamWrite.WriteLine ("."+item.Key+"("+"->" + ":"+item.Key+"(");
						tmpStreamWrite.WriteLine ("."+item.Key+" ("+"->" + ":"+item.Key+" (");
					}
				}
			}




			tmpStreamWrite.Flush ();
			tmpStreamWrite.Close ();
		}


		Debug.Log ("ExportSymbol Done");
	}
}
