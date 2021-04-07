---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Application
---@field public isLoadingLevel boolean 
---@field public streamedBytes number 
---@field public webSecurityEnabled boolean 
---@field public isPlaying boolean 
---@field public isFocused boolean 
---@field public buildGUID string 
---@field public runInBackground boolean 
---@field public isBatchMode boolean 
---@field public dataPath string 
---@field public streamingAssetsPath string 
---@field public persistentDataPath string 
---@field public temporaryCachePath string 
---@field public absoluteURL string 
---@field public unityVersion string 
---@field public version string 
---@field public installerName string 
---@field public identifier string 
---@field public installMode number 
---@field public sandboxType number 
---@field public productName string 
---@field public companyName string 
---@field public cloudProjectId string 
---@field public targetFrameRate number 
---@field public stackTraceLogType number 
---@field public consoleLogPath string 
---@field public backgroundLoadingPriority number 
---@field public genuine boolean 
---@field public genuineCheckAvailable boolean 
---@field public isShowingSplashScreen boolean 
---@field public platform number 
---@field public isMobilePlatform boolean 
---@field public isConsolePlatform boolean 
---@field public systemLanguage number 
---@field public internetReachability number 
---@field public isPlayer boolean 
---@field public levelCount number 
---@field public loadedLevel number 
---@field public loadedLevelName string 
---@field public isEditor boolean 
local Application={ }
---
---@public
---@param exitCode number 
---@return void 
function Application.Quit(exitCode) end
---
---@public
---@return void 
function Application.Quit() end
---
---@public
---@return void 
function Application.CancelQuit() end
---
---@public
---@return void 
function Application.Unload() end
---
---@public
---@param levelIndex number 
---@return number 
function Application.GetStreamProgressForLevel(levelIndex) end
---
---@public
---@param levelName string 
---@return number 
function Application.GetStreamProgressForLevel(levelName) end
---
---@public
---@param levelIndex number 
---@return boolean 
function Application.CanStreamedLevelBeLoaded(levelIndex) end
---
---@public
---@param levelName string 
---@return boolean 
function Application.CanStreamedLevelBeLoaded(levelName) end
---
---@public
---@param obj Object 
---@return boolean 
function Application.IsPlaying(obj) end
---
---@public
---@return String[] 
function Application.GetBuildTags() end
---
---@public
---@param buildTags String[] 
---@return void 
function Application.SetBuildTags(buildTags) end
---
---@public
---@return boolean 
function Application.HasProLicense() end
---
---@public
---@param script string 
---@return void 
function Application.ExternalEval(script) end
---
---@public
---@param delegateMethod AdvertisingIdentifierCallback 
---@return boolean 
function Application.RequestAdvertisingIdentifierAsync(delegateMethod) end
---
---@public
---@param url string 
---@return void 
function Application.OpenURL(url) end
---
---@public
---@param mode number 
---@return void 
function Application.ForceCrash(mode) end
---
---@public
---@param logType number 
---@return number 
function Application.GetStackTraceLogType(logType) end
---
---@public
---@param logType number 
---@param stackTraceType number 
---@return void 
function Application.SetStackTraceLogType(logType, stackTraceType) end
---
---@public
---@param mode number 
---@return AsyncOperation 
function Application.RequestUserAuthorization(mode) end
---
---@public
---@param mode number 
---@return boolean 
function Application.HasUserAuthorization(mode) end
---
---@public
---@param value LowMemoryCallback 
---@return void 
function Application.add_lowMemory(value) end
---
---@public
---@param value LowMemoryCallback 
---@return void 
function Application.remove_lowMemory(value) end
---
---@public
---@param value LogCallback 
---@return void 
function Application.add_logMessageReceived(value) end
---
---@public
---@param value LogCallback 
---@return void 
function Application.remove_logMessageReceived(value) end
---
---@public
---@param value LogCallback 
---@return void 
function Application.add_logMessageReceivedThreaded(value) end
---
---@public
---@param value LogCallback 
---@return void 
function Application.remove_logMessageReceivedThreaded(value) end
---
---@public
---@param functionName string 
---@param args Object[] 
---@return void 
function Application.ExternalCall(functionName, args) end
---
---@public
---@param o Object 
---@return void 
function Application.DontDestroyOnLoad(o) end
---
---@public
---@param filename string 
---@param superSize number 
---@return void 
function Application.CaptureScreenshot(filename, superSize) end
---
---@public
---@param filename string 
---@return void 
function Application.CaptureScreenshot(filename) end
---
---@public
---@param value UnityAction 
---@return void 
function Application.add_onBeforeRender(value) end
---
---@public
---@param value UnityAction 
---@return void 
function Application.remove_onBeforeRender(value) end
---
---@public
---@param value Action`1 
---@return void 
function Application.add_focusChanged(value) end
---
---@public
---@param value Action`1 
---@return void 
function Application.remove_focusChanged(value) end
---
---@public
---@param value Action`1 
---@return void 
function Application.add_deepLinkActivated(value) end
---
---@public
---@param value Action`1 
---@return void 
function Application.remove_deepLinkActivated(value) end
---
---@public
---@param value Func`1 
---@return void 
function Application.add_wantsToQuit(value) end
---
---@public
---@param value Func`1 
---@return void 
function Application.remove_wantsToQuit(value) end
---
---@public
---@param value Action 
---@return void 
function Application.add_quitting(value) end
---
---@public
---@param value Action 
---@return void 
function Application.remove_quitting(value) end
---
---@public
---@param value Action 
---@return void 
function Application.add_unloading(value) end
---
---@public
---@param value Action 
---@return void 
function Application.remove_unloading(value) end
---
---@public
---@param handler LogCallback 
---@return void 
function Application.RegisterLogCallback(handler) end
---
---@public
---@param handler LogCallback 
---@return void 
function Application.RegisterLogCallbackThreaded(handler) end
---
---@public
---@param index number 
---@return void 
function Application.LoadLevel(index) end
---
---@public
---@param name string 
---@return void 
function Application.LoadLevel(name) end
---
---@public
---@param index number 
---@return void 
function Application.LoadLevelAdditive(index) end
---
---@public
---@param name string 
---@return void 
function Application.LoadLevelAdditive(name) end
---
---@public
---@param index number 
---@return AsyncOperation 
function Application.LoadLevelAsync(index) end
---
---@public
---@param levelName string 
---@return AsyncOperation 
function Application.LoadLevelAsync(levelName) end
---
---@public
---@param index number 
---@return AsyncOperation 
function Application.LoadLevelAdditiveAsync(index) end
---
---@public
---@param levelName string 
---@return AsyncOperation 
function Application.LoadLevelAdditiveAsync(levelName) end
---
---@public
---@param index number 
---@return boolean 
function Application.UnloadLevel(index) end
---
---@public
---@param scenePath string 
---@return boolean 
function Application.UnloadLevel(scenePath) end
---
UnityEngine.Application = Application