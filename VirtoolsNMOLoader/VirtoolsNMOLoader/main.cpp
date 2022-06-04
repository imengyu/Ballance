#include "pch.h"
#include "main.h"

CKContext* VirtoolsContext = nullptr;

//Base system

BOOL _InitPlugins(CKPluginManager& iPluginManager, char* currentPath)
{
	char PluginPath[_MAX_PATH];
	char RenderPath[_MAX_PATH];
	char BehaviorPath[_MAX_PATH];
	char ManagerPath[_MAX_PATH];

	sprintf(PluginPath, "%s%s", currentPath, "Plugins");
	sprintf(RenderPath, "%s%s", currentPath, "RenderEngines");
	sprintf(ManagerPath, "%s%s", currentPath, "Managers");
	sprintf(BehaviorPath, "%s%s", currentPath, "BuildingBlocks");

	// we initialize plugins by parsing directories
	iPluginManager.ParsePlugins(RenderPath);
	iPluginManager.ParsePlugins(ManagerPath);
	iPluginManager.ParsePlugins(BehaviorPath);
	iPluginManager.ParsePlugins(PluginPath);
	return TRUE;
}

EXTERN_C API_EXPORT int Loader_Init(HWND hWnd, char* currentPath) {
  CKERROR err = CKStartUp();
  if (err != CK_OK)
    return 1;

	// retrieve the plugin manager ...
	CKPluginManager* pluginManager = CKGetPluginManager();

	//  ... to intialize plugins ...
	if (!_InitPlugins(*pluginManager, currentPath)) {
		MessageBoxA(NULL, "UNABLE_TO_INIT_PLUGINS", "Loader_Init", MB_OK | MB_ICONERROR);
		return FALSE;
	}

  if (CKCreateContext(&VirtoolsContext, hWnd, 0, 0) != CK_OK)
  {
    MessageBoxA(NULL, "CK Initialisation Problems", "Loader_Init", MB_OK);
    return 1;
  }

  return 0;
}
EXTERN_C API_EXPORT int Loader_Destroy() {
  CKERROR err = CKShutdown();
  if (err != CK_OK)
    return 1;
  return 0;
}
EXTERN_C API_EXPORT void Loader_Free(void* objPtr) {
	if (objPtr)
		delete objPtr;
}

//File reader

struct Loader_NmoFile {
	CKObjectArray* array;
};

EXTERN_C API_EXPORT void Loader_SolveNmoFileReset(void* filePtr) {
	Loader_NmoFile* file = (Loader_NmoFile*)filePtr;
	return file->array->Reset();
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFileGetNext(void* filePtr, int* classIdOut, char* nameOutBuffer) {
	Loader_NmoFile* file = (Loader_NmoFile*)filePtr;
	if (file->array->EndOfList())
		return nullptr;
	
	file->array->Next();

	CKObject* obj = file->array->GetData(VirtoolsContext);
	if (classIdOut)
		*classIdOut = obj->GetClassID();
	if (nameOutBuffer) {
		auto name = obj->GetName();
		if (name)
			strcpy(nameOutBuffer, name);
		else
			strcpy(nameOutBuffer, "");
	}
	return obj;
}
EXTERN_C API_EXPORT void Loader_SolveNmoFileDestroy(void* filePtr) {
	Loader_NmoFile* file = (Loader_NmoFile*)filePtr;

	// Delete Array
	DeleteCKObjectArray(file->array);

	delete file;
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFileRead(char* filePath) {

	// Load the file
	CKObjectArray* array = CreateCKObjectArray();
	CKERROR error = VirtoolsContext->Load(filePath, array);
	if (error != CK_OK) {
		printf("Loader_SolveNmoFileLogic: Error: %d\n", error);
		return nullptr;
	}

	Loader_NmoFile* file = new Loader_NmoFile();
	file->array = array;
  return file;
}

//Object parse

EXTERN_C API_EXPORT int Loader_CKGroupGetObjectCount(void* objPtr) {
	return ((CKGroup*)objPtr)->GetObjectCount();
}
EXTERN_C API_EXPORT void* Loader_CKGroupGetObjectW(void* objPtr, int pos) {
	return ((CKGroup*)objPtr)->GetObjectW(pos);
}
EXTERN_C API_EXPORT int Loader_CK3dEntityGetMeshCount(void* objPtr) {
	return ((CK3dEntity*)objPtr)->GetMeshCount();
}
EXTERN_C API_EXPORT void* Loader_CK3dEntityGetMeshCount(void* objPtr, int index) {
	return ((CK3dEntity*)objPtr)->GetMesh(index);
}
EXTERN_C API_EXPORT void* Loader_CKObjectGetName(void* objPtr) {
	return ((CKObject*)objPtr)->GetName();
}

struct Loader_MaterialInfo {
	float power;
	float ambient[4];
	float diffuse[4];
	float specular[4];
	float emissive[4];
	void* textureObject;
};
struct Loader_TextureInfo {
	int width;
	int height;
	int videoPixelFormat;
};
struct Loader_MeshInfo {
	int vertexCount;
	int faceCount;
	int channelCount;
};
struct Loader_3dEntityInfo {
	float position[3];
	float quaternion[4];
	float scale[3];
	int meshCount;
};

EXTERN_C API_EXPORT void* Loader_SolveNmoFileMesh(void* objPtr) {
	CKMesh* obj = (CKMesh*)objPtr;
	Loader_MeshInfo* info = new Loader_MeshInfo();
	if (obj) {
		info->vertexCount = obj->GetVertexCount();
		info->faceCount = obj->GetFaceCount();
		info->channelCount = obj->GetChannelCount();
	}
	return info;
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFile3dEntity(void* objPtr) {
	CK3dEntity* obj = (CK3dEntity*)objPtr;
	Loader_3dEntityInfo* info = new Loader_3dEntityInfo();
	if (obj) {
		VxVector pos;
		obj->GetPosition(&pos);
		info->position[0] = pos.x;
		info->position[1] = pos.y;
		info->position[2] = pos.z;
		VxQuaternion quat;
		obj->GetQuaternion(&quat);
		info->quaternion[0] = quat.x;
		info->quaternion[1] = quat.y;
		info->quaternion[2] = quat.z;
		info->quaternion[3] = quat.w;
		obj->GetScale(&pos);
		info->scale[0] = pos.x;
		info->scale[1] = pos.y;
		info->scale[2] = pos.z;
		info->meshCount = obj->GetMeshCount();
	}
	return info;
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFileMaterial(void* objPtr) {
	CKMaterial* obj = (CKMaterial*)objPtr;
	Loader_MaterialInfo* info = new Loader_MaterialInfo();
	if (obj) {
		auto color = obj->GetDiffuse();
		info->diffuse[0] = color.r;
		info->diffuse[1] = color.g;
		info->diffuse[2] = color.b;
		info->diffuse[3] = color.a;
		color = obj->GetAmbient();
		info->ambient[0] = color.r;
		info->ambient[1] = color.g;
		info->ambient[2] = color.b;
		info->ambient[3] = color.a;
		color = obj->GetSpecular();
		info->specular[0] = color.r;
		info->specular[1] = color.g;
		info->specular[2] = color.b;
		info->specular[3] = color.a;
		color = obj->GetEmissive();
		info->emissive[0] = color.r;
		info->emissive[1] = color.g;
		info->emissive[2] = color.b;
		info->emissive[3] = color.a;
		info->power = obj->GetPower();
		info->textureObject = obj->GetTexture();
	}

	return info;
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFileTexture(void* objPtr) {
	CKTexture* obj = (CKTexture*)objPtr;
	Loader_TextureInfo* info = new Loader_TextureInfo();
	if (obj) {
		info->width = obj->GetWidth();
		info->height = obj->GetHeight();
		info->videoPixelFormat = (int)obj->GetVideoPixelFormat();
	}

	return info;
}

EXTERN_C API_EXPORT void Loader_DirectReadCKMeshData(void* objPtr) {
	CKMesh* obj = (CKMesh*)objPtr;
	if (obj) {

	}
}

