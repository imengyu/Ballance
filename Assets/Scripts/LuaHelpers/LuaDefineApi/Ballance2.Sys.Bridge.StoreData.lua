---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StoreData
---@field public Empty StoreData 空数据
---@field public Name string 名称
---@field public DataRaw Object 原始数据
---@field public DataType number 数据类型
---@field public DataArray List`1 数组类型
local StoreData={ }
---判断是否为空
---@public
---@return boolean 
function StoreData:IsNull() end
---释放
---@public
---@return void 
function StoreData:Destroy() end
---注册数据更新观察者
---@public
---@param observer StoreOnDataChanged 观察者
---@return void 
function StoreData:RegisterDataObserver(observer) end
---移除已经注册的数据更新观察者
---@public
---@param observer StoreOnDataChanged 观察者
---@return void 
function StoreData:UnRegisterDataObserver(observer) end
---通知数据更新观察者数据已经更改
---@public
---@param oldV Object 旧的值
---@param newV Object 新的值
---@return void 
function StoreData:NotificationDataObserver(oldV, newV) end
---重新通知数据更新观察者数据已经更改
---@public
---@return void 
function StoreData:ReNotificationAllDataObserver() end
---设置数据提供者
---@public
---@param context number 数据安全上下文（注册成功后须使用此上下文才能进行私有数据更新）
---@param provider StoreDataProvider 数据提供者
---@return number 
function StoreData:SetDataProvider(context, provider) end
---取消设置数据提供者
---@public
---@param context number 数据安全上下文
---@return void 
function StoreData:RemoveRegisterProvider(context) end
---设置数据的值
---@public
---@param context number 数据安全上下文
---@param data Object 新值
---@return boolean 
function StoreData:SetData(context, data) end
---
---@public
---@param data StoreData 
---@return boolean 
function StoreData:DataArrayAdd(data) end
---
---@public
---@param data StoreData 
---@return void 
function StoreData:DataArrayRemove(data) end
---
---@public
---@param index number 
---@return void 
function StoreData:DataArrayRemoveAt(index) end
---
---@public
---@param index number 
---@param count number 
---@return void 
function StoreData:DataArrayRemoveAt(index, count) end
---
---@public
---@param index number 
---@param data StoreData 
---@return void 
function StoreData:DataArrayInsert(index, data) end
---
---@public
---@return number 
function StoreData:DataArrayGetCount() end
---
---@public
---@param index number 
---@return StoreData 
function StoreData:DataArrayGet(index) end
---
---@public
---@return void 
function StoreData:DataArrayClear() end
---
---@public
---@return number 
function StoreData:IntData() end
---
---@public
---@return number 
function StoreData:LongData() end
---
---@public
---@return number 
function StoreData:FloatData() end
---
---@public
---@return string 
function StoreData:StringData() end
---
---@public
---@return boolean 
function StoreData:BoolData() end
---
---@public
---@return number 
function StoreData:DoubleData() end
---
---@public
---@return Object 
function StoreData:Data() end
---
---@public
---@return StoreData[] 
function StoreData:ArrayData() end
---
---@public
---@return List`1 
function StoreData:ListArrayData() end
---
---@public
---@return Color 
function StoreData:ColorData() end
---
---@public
---@return Material 
function StoreData:MaterialData() end
---
---@public
---@return Texture 
function StoreData:TextureData() end
---
---@public
---@return Texture2D 
function StoreData:Texture2DData() end
---
---@public
---@return Vector2 
function StoreData:Vector2Data() end
---
---@public
---@return Vector3 
function StoreData:Vector3Data() end
---
---@public
---@return Vector4 
function StoreData:Vector4Data() end
---
---@public
---@return Quaternion 
function StoreData:QuaternionData() end
---
---@public
---@return Sprite 
function StoreData:SpriteData() end
---
---@public
---@return Rigidbody 
function StoreData:RigidbodyData() end
---
---@public
---@return Rigidbody2D 
function StoreData:Rigidbody2DData() end
---
---@public
---@return RectTransform 
function StoreData:RectTransformData() end
---
---@public
---@return Transform 
function StoreData:TransformData() end
---
---@public
---@return Camera 
function StoreData:CameraData() end
---
---@public
---@return GameObject 
function StoreData:GameObjectData() end
---
---@public
---@return Object 
function StoreData:ObjectData() end
---
---@public
---@return AudioClip 
function StoreData:AudioClipData() end
---
---@public
---@return AudioSource 
function StoreData:AudioSourceData() end
---
---@public
---@return MonoBehaviour 
function StoreData:MonoBehaviourData() end
---
---@public
---@return GamePackage 
function StoreData:GamePackageData() end
---将当前数据转为字符串表达形式
---@public
---@return string 字符串
function StoreData:ToString() end
---全局数据共享存储类
Ballance2.Sys.Bridge.StoreData = StoreData