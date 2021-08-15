using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngineInternal_GITextureType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngineInternal.GITextureType");
		addMember(l,0,"Charting");
		addMember(l,1,"Albedo");
		addMember(l,2,"Emissive");
		addMember(l,3,"Irradiance");
		addMember(l,4,"Directionality");
		addMember(l,5,"Baked");
		addMember(l,6,"BakedDirectional");
		addMember(l,7,"InputWorkspace");
		addMember(l,8,"BakedShadowMask");
		addMember(l,9,"BakedAlbedo");
		addMember(l,10,"BakedEmissive");
		addMember(l,11,"BakedCharting");
		addMember(l,12,"BakedTexelValidity");
		addMember(l,13,"BakedUVOverlap");
		addMember(l,14,"BakedLightmapCulling");
		LuaDLL.lua_pop(l, 1);
	}
}
