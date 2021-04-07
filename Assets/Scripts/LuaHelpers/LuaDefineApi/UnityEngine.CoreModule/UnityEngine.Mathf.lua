---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Mathf : ValueType
---@field public PI number 
---@field public Infinity number 
---@field public NegativeInfinity number 
---@field public Deg2Rad number 
---@field public Rad2Deg number 
---@field public Epsilon number 
local Mathf={ }
---
---@public
---@param value number 
---@return number 
function Mathf.ClosestPowerOfTwo(value) end
---
---@public
---@param value number 
---@return boolean 
function Mathf.IsPowerOfTwo(value) end
---
---@public
---@param value number 
---@return number 
function Mathf.NextPowerOfTwo(value) end
---
---@public
---@param value number 
---@return number 
function Mathf.GammaToLinearSpace(value) end
---
---@public
---@param value number 
---@return number 
function Mathf.LinearToGammaSpace(value) end
---
---@public
---@param kelvin number 
---@return Color 
function Mathf.CorrelatedColorTemperatureToRGB(kelvin) end
---
---@public
---@param val number 
---@return number 
function Mathf.FloatToHalf(val) end
---
---@public
---@param val number 
---@return number 
function Mathf.HalfToFloat(val) end
---
---@public
---@param x number 
---@param y number 
---@return number 
function Mathf.PerlinNoise(x, y) end
---
---@public
---@param f number 
---@return number 
function Mathf.Sin(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Cos(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Tan(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Asin(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Acos(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Atan(f) end
---
---@public
---@param y number 
---@param x number 
---@return number 
function Mathf.Atan2(y, x) end
---
---@public
---@param f number 
---@return number 
function Mathf.Sqrt(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Abs(f) end
---
---@public
---@param value number 
---@return number 
function Mathf.Abs(value) end
---
---@public
---@param a number 
---@param b number 
---@return number 
function Mathf.Min(a, b) end
---
---@public
---@param values Single[] 
---@return number 
function Mathf.Min(values) end
---
---@public
---@param a number 
---@param b number 
---@return number 
function Mathf.Min(a, b) end
---
---@public
---@param values Int32[] 
---@return number 
function Mathf.Min(values) end
---
---@public
---@param a number 
---@param b number 
---@return number 
function Mathf.Max(a, b) end
---
---@public
---@param values Single[] 
---@return number 
function Mathf.Max(values) end
---
---@public
---@param a number 
---@param b number 
---@return number 
function Mathf.Max(a, b) end
---
---@public
---@param values Int32[] 
---@return number 
function Mathf.Max(values) end
---
---@public
---@param f number 
---@param p number 
---@return number 
function Mathf.Pow(f, p) end
---
---@public
---@param power number 
---@return number 
function Mathf.Exp(power) end
---
---@public
---@param f number 
---@param p number 
---@return number 
function Mathf.Log(f, p) end
---
---@public
---@param f number 
---@return number 
function Mathf.Log(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Log10(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Ceil(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Floor(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Round(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.CeilToInt(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.FloorToInt(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.RoundToInt(f) end
---
---@public
---@param f number 
---@return number 
function Mathf.Sign(f) end
---
---@public
---@param value number 
---@param min number 
---@param max number 
---@return number 
function Mathf.Clamp(value, min, max) end
---
---@public
---@param value number 
---@param min number 
---@param max number 
---@return number 
function Mathf.Clamp(value, min, max) end
---
---@public
---@param value number 
---@return number 
function Mathf.Clamp01(value) end
---
---@public
---@param a number 
---@param b number 
---@param t number 
---@return number 
function Mathf.Lerp(a, b, t) end
---
---@public
---@param a number 
---@param b number 
---@param t number 
---@return number 
function Mathf.LerpUnclamped(a, b, t) end
---
---@public
---@param a number 
---@param b number 
---@param t number 
---@return number 
function Mathf.LerpAngle(a, b, t) end
---
---@public
---@param current number 
---@param target number 
---@param maxDelta number 
---@return number 
function Mathf.MoveTowards(current, target, maxDelta) end
---
---@public
---@param current number 
---@param target number 
---@param maxDelta number 
---@return number 
function Mathf.MoveTowardsAngle(current, target, maxDelta) end
---
---@public
---@param from number 
---@param to number 
---@param t number 
---@return number 
function Mathf.SmoothStep(from, to, t) end
---
---@public
---@param value number 
---@param absmax number 
---@param gamma number 
---@return number 
function Mathf.Gamma(value, absmax, gamma) end
---
---@public
---@param a number 
---@param b number 
---@return boolean 
function Mathf.Approximately(a, b) end
---
---@public
---@param current number 
---@param target number 
---@param currentVelocity Single& 
---@param smoothTime number 
---@param maxSpeed number 
---@return number 
function Mathf.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed) end
---
---@public
---@param current number 
---@param target number 
---@param currentVelocity Single& 
---@param smoothTime number 
---@return number 
function Mathf.SmoothDamp(current, target, currentVelocity, smoothTime) end
---
---@public
---@param current number 
---@param target number 
---@param currentVelocity Single& 
---@param smoothTime number 
---@param maxSpeed number 
---@param deltaTime number 
---@return number 
function Mathf.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed, deltaTime) end
---
---@public
---@param current number 
---@param target number 
---@param currentVelocity Single& 
---@param smoothTime number 
---@param maxSpeed number 
---@return number 
function Mathf.SmoothDampAngle(current, target, currentVelocity, smoothTime, maxSpeed) end
---
---@public
---@param current number 
---@param target number 
---@param currentVelocity Single& 
---@param smoothTime number 
---@return number 
function Mathf.SmoothDampAngle(current, target, currentVelocity, smoothTime) end
---
---@public
---@param current number 
---@param target number 
---@param currentVelocity Single& 
---@param smoothTime number 
---@param maxSpeed number 
---@param deltaTime number 
---@return number 
function Mathf.SmoothDampAngle(current, target, currentVelocity, smoothTime, maxSpeed, deltaTime) end
---
---@public
---@param t number 
---@param length number 
---@return number 
function Mathf.Repeat(t, length) end
---
---@public
---@param t number 
---@param length number 
---@return number 
function Mathf.PingPong(t, length) end
---
---@public
---@param a number 
---@param b number 
---@param value number 
---@return number 
function Mathf.InverseLerp(a, b, value) end
---
---@public
---@param current number 
---@param target number 
---@return number 
function Mathf.DeltaAngle(current, target) end
---
UnityEngine.Mathf = Mathf