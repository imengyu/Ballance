
---Lua 表相关操作工具类
---@class Table
Table = {}

---获取元素在数组中的索引
---@param table table
---@param item any
---@return integer 索引，如果未找到，则返回-1
function Table.IndexOf(table, item)
  for i = 1, #table, 1 do
    if table[i] == item then
      return i
    end
  end
  return -1
end

---获取元素在数组中最后一个的索引
---@param table table
---@param item any
---@return integer 索引，如果未找到，则返回-1
function Table.IndexOf(table, item)
  for i = #table, 1, -1 do
    if table[i] == item then
      return i
    end
  end
  return -1
end

---深克隆一个表
---@param object table 原表
---@return table 返回新的表
function Table.DeepCopy(object)
  local lookup_table = {}
  local function _copy(object)
    if type(object) ~= "table" then
      return object
    elseif lookup_table[object] then
      return lookup_table[object]
    end
    local newObject = {}
    lookup_table[object] = newObject
    for key, value in pairs(object) do
      newObject[_copy(key)] = _copy(value)
    end
    return setmetatable(newObject, getmetatable(object))
  end
  return _copy(object)
end

---递归地比较两个表是否相等。
---此函数不比较元表。
---@param tbl1 table
---@param tbl2 table
---@return boolean 返回两个表是否全部相等
function Table.DeepCompare(tbl1, tbl2)
  if tbl1 == tbl2 then return true end
  for k, v in pairs(tbl1) do
    if type(v) == "table" and type(tbl2[k]) == "table" then
      if not Table.DeepCompare( v, tbl2[k] )  then return false end
    else
      if v ~= tbl2[k] then return false end
    end
  end
  for k in pairs(tbl2) do
    if tbl1[k] == nil then return false end
  end
  return true
end

---Shallow copy an array's values into a new array.
---This function is optimized specifically for arrays, and should be used in place of @{table.shallow_copy} for arrays.
---@param arr table 
---@return table copied array.
function Table.ArrayCopy(arr)
  local new_arr = {}
  for i = 1, #arr do
    new_arr[i] = arr[i]
  end
  return new_arr
end

---Recursively merge two or more tables.
---Values from earlier tables are overwritten by values from later tables, unless both values are tables, in which case
---they are recursively merged.
---Non-merged tables are deep-copied, so the result is brand-new.
---@tparam array tables An array of tables to merge.
---@treturn table The merged tables.
---@usage
---local tbl = {foo = "bar"}
---log(tbl.foo) ---logs "bar"
---log (tbl.bar) ---errors (key is nil)
---tbl = table.merge{tbl, {foo = "baz", set = 3}}
---log(tbl.foo) ---logs "baz"
---log(tbl.bar) ---logs "3"
function Table.DeepMerge(tables)
  local output = {}
  for _, tbl in ipairs(tables) do
    for k, v in pairs(tbl) do
      if type(v) == "table" then
        if type(output[k] or false) == "table" then
          output[k] = Table.DeepMerge{output[k], v}
        else
          output[k] = Table.DeepCopy(v)
        end
      else
        output[k] = v
      end
    end
  end
  return output
end

---Find the value in the table.
---@tparam table tbl The table to search.
---@tparam any value The value to match. Must have an `eq` metamethod set, otherwise will error.
---@treturn any|nil The key that matches the value, or `nil` if it was not found.
---@usage
---local tbl = {"foo", "bar"}
---local contains_foo = table.search(tbl, "foo") ---true
---local contains_baz = table.search(tbl, "baz") ---false
function Table.Find(tbl, value)
  for k, v in pairs(tbl) do
    if v == value then
      return k
    end
  end
end

---Call the given function for each item in the table, and abort if the function returns truthy.
---
---Calls `callback(value, key)` for each item in the table, and immediately ceases iteration if the callback
---returns truthy.
---@tparam table tbl
---@tparam function callback Receives `value`, `key`, and `tbl` as parameters.
---@treturn boolean Whether the callback returned truthy for any one item, and thus halted iteration.
---@usage
---local tbl = {1, 2, 3, 4, 5}
------run a function for each item (identical to a standard FOR loop)
---table.for_each(tbl, function(v) game.print(v) end)
------determine if any value in the table passes the test
---local value_is_even = table.for_each(tbl, function(v) return v % 2 == 0 end)
------determine if ALL values in the table pass the test (invert the test result and function return)
---local all_values_less_than_six = not table.for_each(tbl, function(v) return not (v < 6) end)
function Table.ForEach(tbl, callback)
  for k, v in pairs(tbl) do
    if callback(v, k) then
      return true
    end
  end
  return false
end

----Call the given function on a set number of items in a table, returning the next starting key.
---
---Calls `callback(value, key)` over `n` items from `tbl`, starting after `from_k`.
--
---The first return value of each invocation of `callback` will be collected and returned in a table keyed by the
---current item's key.
---
---The second return value of `callback` is a flag requesting deletion of the current item.
---
---The third return value of `callback` is a flag requesting that the iteration be immediately aborted. Use this flag to
---early return on some condition in `callback`. When aborted, `for_n_of` will return the previous key as `from_k`, so
---the next call to `for_n_of` will restart on the key that was aborted (unless it was also deleted).
--
---**DO NOT** delete entires from `tbl` from within `callback`, this will break the iteration. Use the deletion flag
---instead.
---@tparam table tbl The table to iterate over.
---@tparam any|nil from_k The key to start iteration at, or `nil` to start at the beginning of `tbl`. If the key does
---not exist in `tbl`, it will be treated as `nil`, _unless_ a custom `_next` function is used.
---@tparam number n The number of items to iterate.
---@tparam function callback Receives `value` and `key` as parameters.
---@tparam[opt] function _next A custom `next()` function. If not provided, the default `next()` will be used.
---@treturn any|nil Where the iteration ended. Can be any valid table key, or `nil`.
---Pass this as `from_k` in the next call to `for_n_of` for `tbl`.
---@treturn table The results compiled from the first return of `callback`.
---@treturn boolean Whether or not the end of the table was reached on this iteration.
---@usage
---local extremely_large_table = {
---  [1000] = 1,
---  [999] = 2,
---  [998] = 3,
---  ...,
---  [2] = 999,
---  [1] = 1000,
---}
---event.on_tick(function()
---  global.from_k = table.for_n_of(extremely_large_table, global.from_k, 10, function(v) game.print(v) end)
---end)
function Table.ForNOf(tbl, from_k, n, callback, _next)
  ---bypass if a custom `next` function was provided
  if not _next then
    ---verify start key exists, else start from scratch
    if from_k and not tbl[from_k] then
      from_k = nil
    end
    ---use default `next`
    _next = next
  end

  local delete
  local prev
  local abort
  local result = {}

  ---run `n` times
  for _ = 1, n, 1 do
    local v
    if not delete then
      prev = from_k
    end
    from_k, v = _next(tbl, from_k)
    if delete then
      tbl[delete] = nil
    end

    if from_k then
      result[from_k], delete, abort = callback(v, from_k)
      if delete then
        delete = from_k
      end
      if abort then break end
    else
      return from_k, result, true
    end
  end

  if delete then
    tbl[delete] = nil
    from_k = prev
  elseif abort then
    from_k = prev
  end
  return from_k, result, false
end

---Create a filtered version of a table based on the results of a filter function.
---
---Calls `filter(value, key)` on each element in the table, returning a new table with only pairs for which
---`filter` returned a truthy value.
---@tparam table tbl
---@tparam function filter Takes in `value`, `key`, and `tbl` as parameters.
---@tparam[opt] boolean array_insert If true, the result will be constructed as an array of values that matched the
---filter. Key references will be lost.
---@treturn table A new table containing only the filtered values.
---@usage
---local tbl = {1, 2, 3, 4, 5, 6}
---local just_evens = table.filter(tbl, function(v) return v % 2 == 0 end) ---{[2] = 2, [4] = 4, [6] = 6}
---local just_evens_arr = table.filter(tbl, function(v) return v % 2 == 0 end, true) ---{2, 4, 6}
function Table.Filter(tbl, filter, array_insert)
  local output = {}
  local i = 0
  for k, v in pairs(tbl) do
    if filter(v, k) then
      if array_insert then
        i = i + 1
        output[i] = v
      else
        output[k] = v
      end
    end
  end
  return output
end

---Invert the given table such that `[value] = key`, returning a new table.
---
---Non-unique values are overwritten based on the ordering from `pairs()`.
---@tparam table tbl
---@treturn table The inverted table.
---@usage
---local tbl = {"foo", "bar", "baz", set = "baz"}
---local inverted = table.invert(tbl) ---{foo = 1, bar = 2, baz = "set"}
function Table.Invert(tbl)
  local inverted = {}
  for k, v in pairs(tbl) do
    inverted[v] = k
  end
  return inverted
end

---Create a transformed table using the output of a mapper function.
---
---Calls `mapper(value, key)` on each element in the table, using the return as the new value for the key.
---@tparam table tbl
---@tparam function mapper Takes in `value`, `key`, and `tbl` as parameters.
---@treturn table A new table containing the transformed values.
---@usage
---local tbl = {1, 2, 3, 4, 5}
---local tbl_times_ten = table.map(tbl, function(v) return v * 10 end) ---{10, 20, 30, 40, 50}
function Table.Map(tbl, mapper)
  local output = {}
  for k, v in pairs(tbl) do
    output[k] = mapper(v, k)
  end
  return output
end

local function default_comp(a, b) return a < b end

---Partially sort an array.
---
---This function utilizes [insertion sort](https://en.wikipedia.org/wiki/Insertion_sort), which is _extremely_
---inefficient with large data sets. However, you can spread the sorting over multiple ticks, reducing the performance
---impact. Only use this function if `table.sort` is too slow.
---@tparam array arr
---@tparam number from_index The index to start iteration at (inclusive). Pass `nil` or a number less than `2` to begin
---at the start of the array.
---@tparam number iterations The number of iterations to perform. Higher is more performance-heavy. This number should
---be adjusted based on the performance impact of the custom `comp` function (if any) and the size of the array.
---@tparam[opt] function comp A comparison function for sorting. Must return truthy if `a < b`.
---@treturn number|nil The index to start the next iteration at, or `nil` if the end was reached.
function Table.PartialSort(arr, from_index, iterations, comp)
  comp = comp or default_comp
  local start_index = (from_index and from_index > 2) and from_index or 2
  local end_index = start_index + (iterations - 1)

  for j = start_index, end_index do
    local key = arr[j]
    if not key then return nil end
    local i = j - 1

    while i > 0 and comp(key, arr[i]) do
      arr[i + 1] = arr[i]
      i = i - 1
    end

    arr[i + 1] = key
  end

  return end_index + 1
end

---"Reduce" a table's values into a single output value, using the results of a reducer function.
---
---Calls `reducer(accumulator, value, key)` on each element in the table, returning a single accumulated output value.
---@tparam table tbl
---@tparam function reducer
---@tparam[opt] any initial_value The initial value for the accumulator. If not provided or is falsy, the first value
---in the table will be used as the initial `accumulator` value and skipped as `key`. Calling `reduce()` on an empty
---table without an `initial_value` will cause a crash.
---@treturn any The accumulated value.
---@usage
---local tbl = {10, 20, 30, 40, 50}
---local sum = table.reduce(tbl, function(acc, v) return acc + v end)
---local sum_minus_ten = table.reduce(tbl, function(acc, v) return acc + v end, -10)
function Table.Reduce(tbl, reducer, initial_value)
  local accumulator = initial_value
  for key, value in pairs(tbl) do
    if accumulator then
      accumulator = reducer(accumulator, value, key)
    else
      accumulator = value
    end
  end
  return accumulator
end

---Shallowly copy the contents of a table into a new table.
---
---The parent table will have a new table reference, but any subtables within it will still have the same table
---reference.
---
---Does not copy metatables.
---@tparam table tbl
---@tparam boolean use_rawset Use rawset to set the values (ignores metamethods).
---@treturn table The copied table.
function Table.ShallowCopy(tbl, use_rawset)
  local output = {}
  for k, v in pairs(tbl) do
    if use_rawset then
      rawset(output, k, v)
    else
      output[k] = v
    end
  end
  return output
end

---Shallowly merge two or more tables.
---Unlike @{Table.deepMerge}, this will only combine the top level of the tables.
function Table.ShallowMerge(tables)
  local output = {}
  for _, tbl in pairs(tables) do
    for key, value in pairs(tbl) do
      output[key] = value
    end
  end
  return output
end

---Retrieve a shallow copy of a portion of an array, selected from `start` to `end` inclusive.
---The original array **will not** be modified.
---@tparam array arr
---@tparam[opt=1] int start
---@tparam[opt=#arr] int stop Stop at this index. If negative, will stop `n` items from the end of the array.
---@treturn array A new array with the copied values.
---@usage
---local arr = {10, 20, 30, 40, 50, 60, 70, 80, 90}
---local sliced = table.slice(arr, 3, 7) ---{30, 40, 50, 60, 70}
---log(serpent.line(arr)) ---{10, 20, 30, 40, 50, 60, 70, 80, 90} (unchanged)
function Table.Slice(arr, start, stop)
  local output = {}
  local n = #arr

  start = start or 1
  stop = stop or n
  stop = stop < 0 and (n + stop + 1) or stop

  if start < 1 or start > n then
    return {}
  end

  local k = 1
  for i = start, stop do
    output[k] = arr[i]
    k = k + 1
  end
  return output
end

---Extract a portion of an array, selected from `start` to `end` inclusive.
---The original array **will** be modified.
---@tparam array arr
---@tparam[opt=1] int start
---@tparam[opt=#arr] int stop Stop at this index. If negative, will stop `n` items from the end of the array.
---@treturn array A new array with the extracted values.
---@usage
---local arr = {10, 20, 30, 40, 50, 60, 70, 80, 90}
---local spliced = table.splice(arr, 3, 7) ---{30, 40, 50, 60, 70}
---log(serpent.line(arr)) ---{10, 20, 80, 90} (values were removed)
function Table.Splice(arr, start, stop)
  local output = {}
  local n = #arr

  start = start or 1
  stop = stop or n
  stop = stop < 0 and (n + stop + 1) or stop

  if start < 1 or start > n then
    return {}
  end

  local k = 1
  for _ = start, stop do
    output[k] = table.remove(arr, start)
    k = k + 1
  end
  return output
end

return Table