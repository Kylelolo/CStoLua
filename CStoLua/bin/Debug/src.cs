LuaBinding tempUI = varGo.GetComponent<LuaBinding>();
                        if (tempUI != null)
                        {
                            XLua.LuaTable tempTable = tempUI.scriptTable;
                            tempTable.CallLuaFunction("SetSystemID", new object[] { tempTable, (int)varSystemID , varCallBack });
                        }