#include "pch.h"
#include "main.h"

CKContext* VirtoolsContext = nullptr;
int VirtoolsLastEror = 0;
int Init = 0;

extern "C" void MyInitCustomPlayerContextEx(int* arg0, DWORD arg1);
extern "C" void MyCKInitCustomPlayer(BOOL arg0);

extern "C" __declspec(dllexport) void InitCustomPlayerContextEx(int* arg0, DWORD arg1) {
	MyInitCustomPlayerContextEx(arg0, arg1);
}
extern "C" __declspec(dllexport) void CKInitCustomPlayer(BOOL arg0) {
	MyCKInitCustomPlayer(arg0);
}

//Base system

int _InitRenderEngines(CKPluginManager& iPluginManager)
{
	// here we look for the render engine (ck2_3d)
	int count = iPluginManager.GetPluginCount(CKPLUGIN_RENDERENGINE_DLL);
	for (int i = 0; i < count; i++) {
		CKPluginEntry* desc = iPluginManager.GetPluginInfo(CKPLUGIN_RENDERENGINE_DLL, i);
		CKPluginDll* dll = iPluginManager.GetPluginDllInfo(desc->m_PluginDllIndex);
		XDWORD pos = dll->m_DllFileName.RFind('\\');
		if (pos == XString::NOTFOUND)
			continue;
		XString str = dll->m_DllFileName.Substring(pos + 1);
		if (_strnicmp(str.CStr(), "ck2_3d", strlen("ck2_3d")) == 0)
			return i;
	}

	return -1;
}
BOOL _InitPlugins(CKPluginManager& iPluginManager, char* currentPath)
{
	char PluginPath[_MAX_PATH];
	char RenderPath[_MAX_PATH];
	char ManagerPath[_MAX_PATH];
	char BuildingBlocksPath[_MAX_PATH];

	sprintf(BuildingBlocksPath, "%s%s", currentPath, "BuildingBlocks");
	sprintf(PluginPath, "%s%s", currentPath, "Plugins");
	sprintf(RenderPath, "%s%s", currentPath, "RenderEngines");
	sprintf(ManagerPath, "%s%s", currentPath, "Managers");

	// we initialize plugins by parsing directories
	iPluginManager.ParsePlugins(RenderPath);
	iPluginManager.ParsePlugins(ManagerPath);
	iPluginManager.ParsePlugins(PluginPath);
	iPluginManager.ParsePlugins(BuildingBlocksPath);
	return TRUE;
}

char* UnicodeToAnsi(const wchar_t* szStr)
{
	int nLen = WideCharToMultiByte(CP_ACP, 0, szStr, -1, NULL, 0, NULL, NULL);
	if (nLen == 0)
		return NULL;
	char* pResult = new char[nLen];
	WideCharToMultiByte(CP_ACP, 0, szStr, -1, pResult, nLen, NULL, NULL);
	return pResult;
}

EXTERN_C API_EXPORT int Loader_Init(HWND hWnd, wchar_t* ck2fullPath) {

	if (Init)
		return 0;

	char* ck2fullPathAnsi = UnicodeToAnsi(ck2fullPath);
	char dirPath[512];
	strcpy(dirPath, ck2fullPathAnsi);
	delete ck2fullPathAnsi;
	for (int i = strlen(dirPath) - 1; i >= 0; i--)
		if (dirPath[i] == '\\') {
			dirPath[i + 1] = '\0';
			break;
		}

  CKERROR err = CKStartUp(GetModuleHandle("VirtoolsNMOLoader.dll"));
	if (err != CK_OK) {
		VirtoolsLastEror = err;
		return 1;
	}

	// retrieve the plugin manager ...
	CKPluginManager* pluginManager = CKGetPluginManager();

	//  ... to intialize plugins ...
	if (!_InitPlugins(*pluginManager, dirPath)) {
		MessageBoxA(NULL, "UNABLE_TO_INIT_PLUGINS", "Loader_Init", MB_OK | MB_ICONERROR);
		return FALSE;
	}
	
	// ... and the render engine.
	int renderEngine = _InitRenderEngines(*pluginManager);
	if (renderEngine == -1) {
		MessageBox(NULL, "UNABLE_TO_LOAD_RENDERENGINE", "Loader_Init", MB_OK | MB_ICONERROR);
		return FALSE;
	}

	err = CKCreateContext(&VirtoolsContext, hWnd);
  if (err != CK_OK)
  {
    MessageBoxA(NULL, "CK Initialisation Problems", "Loader_Init", MB_OK);
    return 1;
  }

	Init = 1;
  return 0;
}
EXTERN_C API_EXPORT int Loader_Destroy() {
	if (!Init)
		return 0;
  CKERROR err = CKShutdown();
	if (err != CK_OK) {
		VirtoolsLastEror = err;
		return 1;
	}
	Init = 0;
  return 0;
}
EXTERN_C API_EXPORT void Loader_Free(void* objPtr) {
	if (objPtr)
		delete objPtr;
}
EXTERN_C API_EXPORT int Loader_GetLastError() {
	return VirtoolsLastEror;
}

//File reader

struct Loader_NmoFile {
	CKObjectArray* array;
	CKFile* ckfile;
	CKLevel* cklevel;
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
	if (!obj)
		return nullptr;
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

	VirtoolsContext->DeleteCKFile(file->ckfile);
	VirtoolsContext->ClearAll();
	delete file;
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFileRead(wchar_t* filePath, int *outErrCode) {

	// create a ckfile
	CKFile* f = VirtoolsContext->CreateCKFile();
	DWORD res = CKERR_INVALIDFILE;
	char* filePathAnsi = UnicodeToAnsi(filePath);
	res = f->OpenFile(filePathAnsi, (CK_LOAD_FLAGS)(CK_LOAD_GEOMETRY | CK_LOAD_CHECKDEPENDENCIES));
	delete filePathAnsi;
	if (res != CK_OK) {
		VirtoolsContext->DeleteCKFile(f);
		VirtoolsLastEror = res;
		if (outErrCode)
			*outErrCode = res;
		printf("Loader_SolveNmoFileRead: Error: %d\n", res);
		return nullptr;
	}

	// Load the file
	CKObjectArray* array = CreateCKObjectArray();
	res = f->LoadFileData(array);
	if (res != CK_OK) {
		VirtoolsContext->DeleteCKFile(f);
		DeleteCKObjectArray(array);
		printf("Loader_SolveNmoFileRead: Error: %d\n", res);
		VirtoolsLastEror = res;
		if (outErrCode)
			*outErrCode = res;
		return nullptr;
	}

	CKLevel* m_Level = VirtoolsContext->GetCurrentLevel();
	Loader_NmoFile* file = new Loader_NmoFile();
	file->array = array;
	file->ckfile = f;
	file->cklevel = m_Level;
  return file;
}

//Object parse

EXTERN_C API_EXPORT int Loader_CKGroupGetObjectCount(void* objPtr) {
	return ((CKGroup*)objPtr)->GetObjectCount();
}
EXTERN_C API_EXPORT void* Loader_CKGroupGetObject(void* objPtr, int pos, int* classIdOut, char* nameOutBuffer) {
	auto obj = ((CKGroup*)objPtr)->GetObject(pos);
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
EXTERN_C API_EXPORT int Loader_CK3dEntityGetMeshCount(void* objPtr) {
	return ((CK3dEntity*)objPtr)->GetMeshCount();
}
EXTERN_C API_EXPORT void* Loader_CK3dEntityGetMeshObj(void* objPtr, int index) {
	return ((CK3dEntity*)objPtr)->GetMesh(index);
}
EXTERN_C API_EXPORT void* Loader_CKMeshGetMaterialObj(void* objPtr, int index, int* outMaterialFacesCount, void** outMaterialFacesPtr) {
	auto mat = ((CKMesh*)objPtr)->GetMaterial(index);
	if (mat) {
		int count = 0;
		auto indices = ((CKMesh*)objPtr)->GetMaterialFaces(index, count);
		auto triangles = new int[count * 3];
		for (int i = 0; i < count; i++) {
			int a, b, c;
			((CKMesh*)objPtr)->GetFaceVertexIndex(indices[i], a, b, c);
			triangles[i * 3] = a;
			triangles[i * 3 + 1] = b;
			triangles[i * 3 + 2] = c;
		}
		*outMaterialFacesPtr = triangles;
		*outMaterialFacesCount = count;
	}
	return mat;
}
EXTERN_C API_EXPORT void* Loader_CKObjectGetName(void* objPtr) {
	return ((CKObject*)objPtr)->GetName();
}
EXTERN_C API_EXPORT int Loader_CKObjectIsHidden(void* objPtr) {
	return ((CKObject*)objPtr)->IsVisible() == FALSE;
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
		quat.ToEulerAngles(&info->euler[0], &info->euler[1], &info->euler[2]);
		obj->GetScale(&pos);
		info->scale[0] = pos.x;
		info->scale[1] = pos.y;
		info->scale[2] = pos.z;
		info->meshCount = obj->GetMeshCount();
	}
	return info;
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFileMesh(void* objPtr) {
	CKMesh* obj = (CKMesh*)objPtr;
	Loader_MeshInfo* info = new Loader_MeshInfo();
	if (obj) {
		info->vertexCount = obj->GetVertexCount();
		info->faceCount = obj->GetFaceCount();
		info->channelCount = obj->GetChannelCount();
		info->materialCount = obj->GetMaterialCount();
	}
	return info;
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFileMaterial(void* objPtr , void* nmoFilePtr) {
	CKMaterial* obj = (CKMaterial*)objPtr;
	Loader_NmoFile* nmoFile = (Loader_NmoFile*)nmoFilePtr;
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

		CKTexture* texture = obj->GetTexture();
		info->textureObject = texture;
	}

	return info;
}
EXTERN_C API_EXPORT void* Loader_SolveNmoFileTexture(void* objPtr) {
	CKTexture* obj = (CKTexture*)objPtr;
	Loader_TextureInfo* info = new Loader_TextureInfo();
	if (obj) {
		info->width = obj->GetWidth();
		info->height = obj->GetHeight();
		info->videoPixelFormat = obj->GetDesiredVideoFormat();
		if (info->width > 0 && info->height > 0)
			info->bufferSize = info->width * info->height * sizeof(unsigned int);
		else
			info->bufferSize = 0;
	}
	return info;
}

EXTERN_C API_EXPORT void Loader_DirectReadCKMeshData(void* objPtr, float* vertices, int* triangles, float* normals, float** uvs) {
	CKMesh* obj = (CKMesh*)objPtr;
	if (obj) {
		int vertexCount = obj->GetVertexCount();
		int faceCount = obj->GetFaceCount();
		int channelCount = obj->GetChannelCount();
		VxVector pos;
		for (int i = 0; i < vertexCount; i++) {
			obj->GetVertexPosition(i, &pos); //vertices
			vertices[i * 3] = pos.x;
			vertices[i * 3 + 1] = pos.y;
			vertices[i * 3 + 2] = pos.z;
			obj->GetVertexNormal(i, &pos); //normals
			normals[i * 3] = pos.x;
			normals[i * 3 + 1] = pos.y;
			normals[i * 3 + 2] = pos.z;

			if (channelCount > 0)
				for (int j = 0; j < channelCount; j++)
					obj->GetVertexTextureCoordinates(i, &uvs[j][i * 2], &uvs[j][i * 2 + 1], j); //uv
			else
				obj->GetVertexTextureCoordinates(i, &uvs[0][i * 2], &uvs[0][i * 2 + 1]); //uv0
			
		}
		for (int i = 0; i < faceCount; i++) {
			int a, b, c;
			obj->GetFaceVertexIndex(i, a, b, c);
			triangles[i * 3] = a;
			triangles[i * 3 + 1] = b;
			triangles[i * 3 + 2] = c;
		}
	}
}
EXTERN_C API_EXPORT void Loader_DirectReadCKTextureData(void* objPtr, Loader_TextureInfo* info, unsigned int* dataBuffer) {
	CKTexture* obj = (CKTexture*)objPtr;	
	if (obj && info && info->bufferSize) {
		/*auto ptr = obj->LockSurfacePtr();
		memcpy(dataBuffer, ptr, info->bufferSize);
		obj->ReleaseSurfacePtr();*/
		for (int x = 0; x < info->width; x++) {
			for (int y = 0; y < info->height; y++) {
				auto color = obj->GetPixel(x, y);
				dataBuffer[(x + y * info->width)] = color;
			}
		}
	}
}
