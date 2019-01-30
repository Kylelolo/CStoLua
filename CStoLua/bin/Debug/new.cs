local tempUI = varGo:GetComponent(typeof(LuaBinding))
                        if  tempUI ~= nil  then

                            XLua.LuaTable tempTable = tempUI.scriptTable
                            tempTable:CallLuaFunction("SetSystemID",object[] { tempTable, varSystemID , varCallBack end)
                        end
