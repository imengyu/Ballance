------------------------------------------------------------------------------
-- EventEmitter Class in Node.js Style
-- LICENSE: MIT
-- Simen Li <simenkid@gmail.com>
------------------------------------------------------------------------------

local classic = require('classic')

local PFX = '__lsn_'
local PFX_LEN = #PFX

local function rmEntry(tbl, pred)
  local x, len = 0, #tbl
  for i = 1, len do
      local trusy, idx = false, (i - x)
      if (type(pred) == 'function') then trusy = pred(tbl[idx])
      else trusy = tbl[idx] == pred
      end

      if (tbl[idx] ~= nil and trusy) then
          tbl[idx] = nil
          table.remove(tbl, idx)
          x = x + 1
      end
  end
  return tbl
end

---EventEmitter
---@class EventEmitter : ClassicObject
EventEmitter = classic:extend()

function EventEmitter:new()
  EventEmitter.super.new(self)
  self._on = {}
  self.defaultMaxListeners = 10
end
function EventEmitter:evTable(ev)
  if (type(self._on[ev]) ~= 'table') then self._on[ev] = {} end
  return self._on[ev]
end

function EventEmitter:getEvTable(ev)
  return self._on[ev]
end

-- ************************************************************************ --
-- ** Public APIs                                                         * --
-- ************************************************************************ --

---添加事件监听器。
---@param ev string 事件名
---@param listener function 回调
---@return EventEmitter
function EventEmitter:addListener(ev, listener)
  local pfx_ev = PFX .. tostring(ev)
  local evtbl = self:evTable(pfx_ev)
  local maxLsnNum = self.currentMaxListeners or self.defaultMaxListeners
  local lsnNum = self:listenerCount(ev)
  table.insert(evtbl, listener)

  if (lsnNum > maxLsnNum) then  print('WARN: Number of ' .. string.sub(pfx_ev, PFX_LEN + 1) .. " event listeners: " .. tostring(lsnNum)) end
  return self
end

---添加事件监听器。
---@param ev string 事件名
---@param listener function 回调
---@return EventEmitter
function EventEmitter:on(ev, listener)
  return self:addListener(ev, listener)
end

---执行指定事件的每个监听器。
---@param ev string 事件名
---@param ... any
---@return EventEmitter
function EventEmitter:emit(ev, ...)
  local pfx_ev = PFX .. tostring(ev)
  local evtbl = self:getEvTable(pfx_ev)
  if (evtbl ~= nil) then
      for _, lsn in ipairs(evtbl) do
          local status, err = pcall(lsn, ...)
          if not (status) then print(string.sub(_, PFX_LEN + 1) .. " emit error: " .. tostring(err)) end
      end
  end

  -- one-time listener
  pfx_ev = pfx_ev .. ':once'
  evtbl = self:getEvTable(pfx_ev)

  if (evtbl ~= nil) then
      for _, lsn in ipairs(evtbl) do
          local status, err = pcall(lsn, ...)
          if not (status) then print(string.sub(_, PFX_LEN + 1) .. " emit error: " .. tostring(err)) end
      end

      rmEntry(evtbl, function (v) return v ~= nil  end)
      self._on[pfx_ev] = nil
  end
  return self
end

---获取监听器的默认限制的数量。
---@return integer
function EventEmitter:getMaxListeners()
  return self.currentMaxListeners or self.defaultMaxListeners
end

---返回指定事件的监听器数量。
---@param ev string 事件名
---@return integer
function EventEmitter:listenerCount(ev)
  local totalNum = 0
  local pfx_ev = PFX .. tostring(ev)
  local evtbl = self:getEvTable(pfx_ev)

  if (evtbl ~= nil) then totalNum = totalNum + #evtbl end

  pfx_ev = pfx_ev .. ':once'
  evtbl = self:getEvTable(pfx_ev)

  if (evtbl ~= nil) then totalNum = totalNum + #evtbl end

  return totalNum
end

---返回指定事件的监听器数组。
---@param ev string 事件名
---@return table
function EventEmitter:listeners(ev)
  local pfx_ev = PFX .. tostring(ev)
  local evtbl = self:getEvTable(pfx_ev)
  local clone = {}

  if (evtbl ~= nil) then
      for i, lsn in ipairs(evtbl) do table.insert(clone, lsn) end
  end

  pfx_ev = pfx_ev .. ':once'
  evtbl = self:getEvTable(pfx_ev)

  if (evtbl ~= nil) then
      for i, lsn in ipairs(evtbl) do table.insert(clone, lsn) end
  end

  return clone
end

---为指定事件注册一个单次监听器，即 监听器最多只会触发一次，触发后立刻解除该监听器。
---@param ev string 事件名
---@param listener function 监听器
---@return EventEmitter
function EventEmitter:once(ev, listener)
  local pfx_ev = PFX .. tostring(ev) .. ':once'
  local evtbl = self:evTable(pfx_ev)
  local maxLsnNum = self.currentMaxListeners or self.defaultMaxListeners
  local lsnNum = self:listenerCount(ev)
  if (lsnNum > maxLsnNum) then print('WARN: Number of ' .. ev .. " event listeners: " .. tostring(lsnNum)) end

  table.insert(evtbl, listener)
  return self
end

---移除所有事件的所有监听器， 如果未指定事件，则移除指定事件的所有监听器。
---@param ev string 事件名
---@return EventEmitter
function EventEmitter:removeAllListeners(ev)
  if ev ~= nil then
      local pfx_ev = PFX .. tostring(ev)
      local evtbl = self:evTable(pfx_ev)
      rmEntry(evtbl, function (v) return v ~= nil  end)

      pfx_ev = pfx_ev .. ':once'
      evtbl = self:evTable(pfx_ev)
      rmEntry(evtbl, function (v) return v ~= nil  end)
      self._on[pfx_ev] = nil
  else
      for _pfx_ev, _t in pairs(self._on) do self:removeAllListeners(string.sub(_pfx_ev, PFX_LEN + 1)) end
  end

  for _pfx_ev, _t in pairs(self._on) do
      if (#_t == 0) then self._on[_pfx_ev] = nil end
  end

  return self
end

---移除指定事件的某个监听器，监听器必须是该事件已经注册过的监听器。
---@param ev string 事件名
---@param listener function 监听器
---@return EventEmitter
function EventEmitter:removeListener(ev, listener)
  local pfx_ev = PFX .. tostring(ev)
  local evtbl = self:evTable(pfx_ev)
  local lsnCount = 0
  assert(listener ~= nil, "listener is nil")
  -- normal listener
  rmEntry(evtbl, listener)

  if (#evtbl == 0) then self._on[pfx_ev] = nil end

  -- emit-once listener
  pfx_ev = pfx_ev .. ':once'
  evtbl = self:evTable(pfx_ev)
  rmEntry(evtbl, listener)

  if (#evtbl == 0) then self._on[pfx_ev] = nil end
  return self
end

---默认情况下， EventEmitters 如果你添加的监听器超过 10 个就会输出警告信息。 setMaxListeners 函数用于改变监听器的默认限制的数量。
---@param n any
---@return EventEmitter
function EventEmitter:setMaxListeners(n)
  self.currentMaxListeners = n
  return self
end

return EventEmitter