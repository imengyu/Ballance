
---自动根据关卡主元件，生成归组信息。
---注：此函数将会覆盖原有Level json中的归组信息。
---@param level table Level json
---@param transform Transform 关卡主元件transform
function DoLevelAutoGroup(level, transform)
  local childCount = transform.childCount - 1

  level.internalObjects = {
    PS_LevelStart = '',
    PE_LevelEnd = '',
    PR_ResetPoints = {},
    PC_CheckPoints = {},
  }
  level.sectors = {}
  level.floors = {}
  level.groups = {}
  level.depthTestCubes = {}

  local groupsTemp = {}

  for i = 1, level.sectorCount, 1 do
    level.sectors[tostring(i)] = {}
  end

  for i = 0, childCount, 1 do
    local go = transform:GetChild(i).gameObject
    local name = go.name
    if name =='PS_LevelStart' then
      --开始火焰
      level.internalObjects.PS_LevelStart = 'PS_LevelStart'
    elseif name =='PE_LevelEnd' then
      --结束飞船
      level.internalObjects.PE_LevelEnd = 'PE_LevelEnd'
    elseif string.startWith(name, 'PR_ResetPoint:') then
      --出生点
      local sector = string.sub(name, 15)
      level.internalObjects.PR_ResetPoints[sector] = name
    elseif string.startWith(name, 'PC_CheckPoint:') then
      --检查点
      local sector = string.sub(name, 15)
      level.internalObjects.PC_CheckPoints[sector] = name
    elseif string.startWith(name, 'S_') then
      if go.activeSelf then
        --静态路面组
        local floor_type = string.sub(name, 3)
        local c_names = {}
        local c_transform =  transform:GetChild(i)
        local c_childCount = c_transform.childCount - 1
        for i = 0, c_childCount, 1 do
          local go = c_transform:GetChild(i).gameObject
          if go.activeSelf then
            go.name = go.name..floor_type..tostring(i)
            table.insert(c_names, go.name)
          end
        end
        table.insert(level.floors, {
          name = 'Phys_'..floor_type,
          objects = c_names
        })
      end
    elseif name == 'DepthTestCubes' then
      --坠落检测区
      local c_transform =  transform:GetChild(i)
      local c_childCount = c_transform.childCount - 1
      for i = 0, c_childCount, 1 do
        local go = c_transform:GetChild(i).gameObject
        go.name = go.name..'DepthTestCubes'..tostring(i)
        table.insert(level.depthTestCubes, go.name)
      end
    elseif string.contains(name, ':') then
      if go.activeSelf then
        local arr = string.split(name, ':')
        if #arr > 2 then
          local nname = arr[1]
          local sector = arr[#arr]
          if string.startWith(nname, 'P_') then
            --机关
            local gdata = groupsTemp[nname]
            local sdata = level.sectors[sector]
            if gdata == nil then
              gdata = {}
              groupsTemp[nname] = gdata
            end
            table.insert(gdata, name)
            table.insert(sdata, name)
          --elseif string.startWith(name, 'K_') then
            --Internal TODO
          end
        end
      end
    end
  end

  for key, value in pairs(groupsTemp) do
    table.insert(level.groups, {
      name = key,
      objects = value
    })
  end

end