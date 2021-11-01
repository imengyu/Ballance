---递归显示Table
---@param data any
---@param max_level number 最大显示层级
---@param prefix string 前缀
function VarDump(data, max_level, prefix)   
  if type(prefix) ~= "string" then   
      prefix = ""  
  end   
  if type(data) ~= "table" then   
      print(prefix .. tostring(data))   
  else  
      print(data)   
      if max_level ~= 0 then   
          local prefix_next = prefix .. "    "  
          print(prefix .. "{")   
          for k,v in pairs(data) do  
              io.stdout:write(prefix_next .. k .. " = ")   
              if type(v) ~= "table" or (type(max_level) == "number" and max_level <= 1) then   
                  print(v)   
              else  
                  if max_level == nil then   
                    VarDump(v, nil, prefix_next)   
                  else  
                    VarDump(v, max_level - 1, prefix_next)   
                  end   
              end   
          end   
          print(prefix .. "}")   
      end   
  end   
end

---完美打印一个Table的方案
---@param t any
function print_r( t )  
  local print_r_cache={}
  local function sub_print_r(t,indent)
      if (print_r_cache[tostring(t)]) then
          print(indent.."*"..tostring(t))
      else
          print_r_cache[tostring(t)]=true
          if (type(t)=="table") then
              for pos,val in pairs(t) do
                  if (type(val)=="table") then
                      print(indent.."["..pos.."] => "..tostring(t).." {")
                      sub_print_r(val,indent..string.rep(" ",string.len(pos)+8))
                      print(indent..string.rep(" ",string.len(pos)+6).."}")
                  elseif (type(val)=="string") then
                      print(indent.."["..pos..'] => "'..val..'"')
                  else
                      print(indent.."["..pos.."] => "..tostring(val))
                  end
              end
          else
              print(indent..tostring(t))
          end
      end
  end
  if (type(t)=="table") then
      print(tostring(t).." {")
      sub_print_r(t,"  ")
      print("}")
  else
      sub_print_r(t,"  ")
  end
  print()
end