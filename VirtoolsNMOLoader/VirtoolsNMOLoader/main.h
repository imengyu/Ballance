#pragma once
#include "pch.h"

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
	int bufferSize;
};
struct Loader_MeshInfo {
	int vertexCount;
	int faceCount;
	int channelCount;
	int materialCount;
};
struct Loader_3dEntityInfo {
	float position[3];
	float quaternion[4];
	float scale[3];
	int meshCount;
};

#ifdef __cplusplus
EXTERN_C {
#endif //  __cplusplus
  API_EXPORT int Loader_Init(HWND hWnd, char* ck2fullPath);
  API_EXPORT int Loader_Destroy();
	API_EXPORT int Loader_GetLastError();
	API_EXPORT void Loader_SolveNmoFileReset(void* filePtr);
	API_EXPORT void* Loader_SolveNmoFileGetNext(void* filePtr, int* classIdOut, char* nameOutBuffer);
	API_EXPORT void Loader_SolveNmoFileDestroy(void* filePtr);
	API_EXPORT void* Loader_SolveNmoFileRead(char* filePath, int* outErrCode);
	API_EXPORT int Loader_CKGroupGetObjectCount(void* objPtr);
	API_EXPORT void* Loader_CKGroupGetObject(void* objPtr, int pos, int* classIdOut, char* nameOutBuffer);
	API_EXPORT int Loader_CK3dEntityGetMeshCount(void* objPtr);
	API_EXPORT void* Loader_CK3dEntityGetMeshObj(void* objPtr, int index);
	API_EXPORT void* Loader_CKObjectGetName(void* objPtr);
	API_EXPORT void* Loader_SolveNmoFileMesh(void* objPtr);
	API_EXPORT void* Loader_SolveNmoFile3dEntity(void* objPtr);
	API_EXPORT void* Loader_SolveNmoFileMaterial(void* objPtr);
	API_EXPORT void* Loader_SolveNmoFileTexture(void* objPtr);
	API_EXPORT void* Loader_CKMeshyGetMaterialObj(void* objPtr, int index);
	API_EXPORT void Loader_DirectReadCKMeshData(void* objPtr, float* vertices, int* triangles, float* normals, float** uvs);
	API_EXPORT void Loader_DirectReadCKTextureData(void* objPtr, Loader_TextureInfo* info, unsigned char* dataBuffer);
#ifdef __cplusplus
}

#endif //  __cplusplus
