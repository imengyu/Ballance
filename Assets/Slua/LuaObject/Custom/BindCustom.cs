﻿using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_Ballance2_Utils_CommonUtils.reg,
				Lua_Ballance2_Utils_ConverUtils.reg,
				Lua_Ballance2_Utils_DebugUtils.reg,
				Lua_Ballance2_Utils_FileUtils.reg,
				Lua_Ballance2_Utils_Log.reg,
				Lua_Ballance2_Utils_LogLevel.reg,
				Lua_Ballance2_Utils_LuaUtils.reg,
				Lua_Ballance2_Utils_StringSpliter.reg,
				Lua_Ballance2_Utils_StringUtils.reg,
				Lua_Ballance2_Sys_GameManager.reg,
				Lua_Ballance2_Sys_Utils_CloneUtils.reg,
				Lua_Ballance2_Sys_Utils_Web_FileRequestUtil.reg,
				Lua_Ballance2_Sys_UI_Progress.reg,
				Lua_Ballance2_Sys_UI_SplitView.reg,
				Lua_Ballance2_Sys_UI_SplitViewDragger.reg,
				Lua_Ballance2_Sys_UI_ToggleEx.reg,
				Lua_Ballance2_Sys_UI_Window.reg,
				Lua_Ballance2_Sys_UI_WindowState.reg,
				Lua_Ballance2_Sys_UI_WindowType.reg,
				Lua_Ballance2_Sys_UI_Utils_EventTriggerListener.reg,
				Lua_Ballance2_Sys_UI_Utils_UIAnchorPosUtils.reg,
				Lua_Ballance2_Sys_UI_Utils_UIPivot.reg,
				Lua_Ballance2_Sys_UI_Utils_UIAnchor.reg,
				Lua_Ballance2_Sys_UI_Utils_UIContentSizeUtils.reg,
				Lua_Ballance2_Sys_UI_Utils_UIFadeManager.reg,
				Lua_Ballance2_Sys_UI_Utils_UIRayIgnore.reg,
				Lua_Ballance2_Sys_UI_UISystem_LayoutParams.reg,
				Lua_Ballance2_Sys_UI_UISystem_LayoutDirection.reg,
				Lua_Ballance2_Sys_UI_UISystem_UIElement.reg,
				Lua_Ballance2_Sys_UI_UISystem_UIVisible.reg,
				Lua_Ballance2_Sys_UI_UISystem_UIRootImpl.reg,
				Lua_Ballance2_Sys_UI_UISystem_Layout_UILayout.reg,
				Lua_Ballance2_Sys_UI_UISystem_Layout_UILayoutUtils.reg,
				Lua_Ballance2_Sys_UI_UISystem_Layout_LayoutAxis.reg,
				Lua_Ballance2_Sys_UI_UISystem_Layout_LayoutType.reg,
				Lua_Ballance2_Sys_UI_UISystem_Layout_LayoutGravity.reg,
				Lua_Ballance2_Sys_Services_GameService.reg,
				Lua_Ballance2_Sys_Services_GameMediator.reg,
				Lua_Ballance2_Sys_Services_GamePackageManager.reg,
				Lua_Ballance2_Sys_Services_GameUIManager.reg,
				Lua_Ballance2_Sys_Res_GamePathManager.reg,
				Lua_Ballance2_Sys_Res_GameStaticResourcesPool.reg,
				Lua_Ballance2_Sys_Package_GamePackage.reg,
				Lua_Ballance2_Sys_Package_GameAssetBundlePackage.reg,
				Lua_Ballance2_Sys_Package_GamePackageCodeType.reg,
				Lua_Ballance2_Sys_Package_GamePackageType.reg,
				Lua_Ballance2_Sys_Package_GamePackageStatus.reg,
				Lua_Ballance2_Sys_Package_GamePackageRunTime.reg,
				Lua_Ballance2_Sys_Package_GamePackageBaseInfo.reg,
				Lua_Ballance2_Sys_Package_GamePackageDependencies.reg,
				Lua_Ballance2_Sys_Debug_GameError.reg,
				Lua_Ballance2_Sys_Debug_GameErrorInfo.reg,
				Lua_Ballance2_Sys_Debug_GameErrorChecker.reg,
				Lua_Ballance2_Sys_Bridge_GameAction.reg,
				Lua_Ballance2_Sys_Bridge_GameActionCallResult.reg,
				Lua_Ballance2_Sys_Bridge_GameActionStore.reg,
				Lua_Ballance2_Sys_Bridge_GameEvent.reg,
				Lua_Ballance2_Sys_Bridge_GameEventNames.reg,
				Lua_Ballance2_Sys_Bridge_StoreData.reg,
				Lua_Ballance2_Sys_Bridge_StoreDataType.reg,
				Lua_Ballance2_Sys_Bridge_StoreDataAccess.reg,
				Lua_Ballance2_Sys_Bridge_Store.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_LuaVarObjectInfo.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_LuaVarObjectType.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaObjectHost.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaWapperEvents_GameLuaObjectAnimatorEventCaller.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaWapperEvents_GameLuaObjectEventTriggerCaller.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaWapperEvents_GameLuaObjectMouseEventCaller.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaWapperEvents_GameLuaObjectOtherEventCaller.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaWapperEvents_GameLuaObjectParticleEventCaller.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaWapperEvents_GameLuaObjectPhysics2DEventCaller.reg,
				Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaWapperEvents_GameLuaObjectPhysicsEventCaller.reg,
				Lua_Ballance2_Sys_Bridge_Handler_GameHandler.reg,
				Lua_Ballance2_Sys_Bridge_Handler_GameHandlerList.reg,
				Lua_Ballance2_Config_GameConst.reg,
				Lua_Ballance2_Config_GameSettingsManager.reg,
				Lua_Ballance2_Config_GameSettingsActuator.reg,
				Lua_Ballance2_Sys_UI_Utils_UIFadeManager_FadeType.reg,
				Lua_Ballance2_Sys_UI_Utils_UIFadeManager_FadeObject.reg,
				Lua_System_Collections_Generic_List_1_int.reg,
				Lua_System_String.reg,
			};
			return list;
		}
	}
}
