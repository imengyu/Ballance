/*************************************************************************/
/*	File : CKPatchMesh.h												 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#if !defined(CKPATCHMESH_H) || defined(CK_3DIMPLEMENTATION)

#define CKPATCHMESH_H "$Id:$"
#ifndef CK_3DIMPLEMENTATION

#include "CKMesh.h"

/***************************************************************************
Summary: Structure holding the details of a patch

Remarks:
+ This structure carries the details associated with patch, such as type, 
smoothing group, edges, vertices, vectors, material.
{Image:CKPatch}
+ The corner control points of a patch are called verts. 
+ The edge control points  and interiors are called vecs
+ Interior points are automatically computed when tesselating a patch
but the result is stored inside the vecs array.
+ The smoothing group is a DWORD mask, if the binary AND of the smoothing group value of
two adjacent patches is not 0 then they are smoothed together otherwise there is a sharp edge
between the two patches. The default value of a patch smoothing group is 0xFFFFFFFF which means
all patches are smoothed together.
See Also: CKPatchMesh,CKPatchEdge,CKTVPatch
****************************************************************************/
typedef struct CKPatch {
		CKDWORD type;			// CK_PATCH_FLAGS ( 3 for a tri patch , 4 for a quad patch) 	 
		CKDWORD SmoothingGroup;	// Smoothing group of this patch,
		short int v[4];			// 3 or 4 indices of the corner verts.
		short int vec[8];		// 6 or 8 indices of the edge vecs. 
		short int interior[4];	// Can have three or four interior vecs.
		short int edge[4];		// Indices of the 3 or 4 edges of this patches
		CK_ID Material;			// Material CK_ID for this patch. (CKPatchMesh::SetPatchMaterial) 
		VxVector* auxs;			// An array of 9 auxiliary points instancied at runtime for tri patches (used internally DO NOT modify)
		
		CKPatch() 
		{ SmoothingGroup = 0xFFFFFFFF; Material = 0; auxs = NULL;} 
		~CKPatch() { delete[] auxs;}	
} CKPatch;


/***************************************************************************
Summary: Structure holding the details of the edge of a patch

Remarks:
	+ A patch edge holds the index of the verts and vecs it is made of. It also 
	contains the two patches that share this edge (patch2 is -1) when this edge
	belongs to only one patch.
See Also: CKPatchMesh,CKPatch,CKTVPatch.
****************************************************************************/
typedef struct CKPatchEdge {
		short int v1;		// Index of first corner vert
		short int vec12;	// First vec between v1 and v2
		short int vec21;	// Second vec between v1 and v2
		short int v2;		// Index of second corner vert
		short int patch1;	// Index of first patch
		short int patch2;	// Index of second patch
} CKPatchEdge;

/***************************************************************************
Summary: Structure holding the texture coordinates details of a patch

Remarks:
	+ The CKTVPatch structure holds the 3 or 4 indices of the texture coordinates
	of the corner points of a patch.
See Also:CKPatch,CKPatchMesh
****************************************************************************/
typedef struct CKTVPatch {
		short int tv[4];	// Index of textures coordinates 
} CKTVPatch;



typedef enum CK_PATCHMESH_FLAGS {
		CK_PATCHMESH_UPTODATE	  =1,		//	Indicates the mesh is uptodate.
		CK_PATCHMESH_BUILDNORMALS =2,		//  Flag to indicate normals to be computed.
		CK_PATCHMESH_MATERIALSUPTODATE =4,	//  Flag to indicate patch materials have been changed.
		CK_PATCHMESH_AUTOSMOOTH		   =8,	//  Automatically computes vecs and interiors from control point position.
} CK_PATCHMESH_FLAGS;


/***************************************************************************
{filename:CK_PATCH_FLAGS}
Summary: Type of a patch (Triangle or Quad).

See Also:CKPatch,CKPatchMesh
****************************************************************************/
typedef enum CK_PATCH_FLAGS {
		CK_PATCH_TRI	=3,			// Triangle Patch. 
		CK_PATCH_QUAD	=4,			// Quad Patch.	 
} CK_PATCH_FLAGS;


#undef CK_PURE

#define CK_PURE = 0


/**************************************************************************
{filename:CKPatchMesh}
Summary: Representation of the geometry of a 3Dobject using bezier patches.

Remarks:
+ The CKPatchMesh Class derives from the CKMesh class.
{Image:CKPatch}
+ It is made up of array of bezier patches (triangular or quadrangular) which 
can be tesselated given an iteration count to construct a standard mesh made up of triangles.
{Image:PatchTess}
+ In the current implementation only the uniform tesselation is supported. It generates 
a list of strip primitives per material for maximum performances.

+ The advantages of a patch mesh is that it use less memory on disk.

+ Its class id is CKCID_PATCHMESH

See also: Using Meshes,CK3dEntity,CKMesh,CKPatch
*****************************************************************************/
class CKPatchMesh:public CKMesh {
public:
#endif

/**************************************************************************
Summary: Converts a mesh(CKMesh) to a bezier patch(CKPatchMesh) 
Remarks:
	+ This method is not implemented
{Secret}
*****************************************************************************/
virtual	CKERROR  FromMesh(CKMesh* m) CK_PURE; 

/**************************************************************************
Summary: Converts a bezier patch(CKPatchMesh) to a  mesh(CKMesh).
Remarks:
	+ This method is not implemented
{Secret}
*****************************************************************************/
virtual	CKERROR  ToMesh(CKMesh* m,int stepcount) CK_PURE; 

/**************************************************************************
Summary: Sets the number of iteration steps to be used for tesselation.

Arguments:
	count: Step count for tesselation
See also: GetIterationCount
*****************************************************************************/
virtual	void   SetIterationCount(int count) CK_PURE;

/**************************************************************************
Summary: Gets the number of iteration steps used for tesselation.

Return Value:
	Step count for tesselation.
See also: SetIterationCount
*****************************************************************************/
virtual	int	   GetIterationCount() CK_PURE;

//-------------------------------------
// Mesh building

/************************************************
Summary: Builds the base mesh for rendering.

Remarks: 
+ This method computes the base mesh vertices and face data to be rendered.
+ This method is automatically called when vertices have been moved (notified by CKMesh::ModifierVertexMove for example)
or the tesselation level has changed before rendering.
See Also: CleanRenderMesh
************************************************/
virtual	void BuildRenderMesh() CK_PURE;


virtual	void CleanRenderMesh() CK_PURE;

virtual	void Clear() CK_PURE;

virtual	void ComputePatchAux(int index) CK_PURE;

virtual	void ComputePatchInteriors(int index) CK_PURE;

virtual	CKDWORD  GetPatchFlags() CK_PURE;

virtual	void	 SetPatchFlags(CKDWORD Flags) CK_PURE;

//-------------------------------------------
// Control Points

/************************************************
Summary: Sets verts and vecs counts of the patchmesh.

Arguments:
	VertCount: Number of verts (corner control points) to be set. 
	VecCount: Number of vects (edge and interior control points) to be set.
Remarks:
	+ See CKPatch for more details on the difference between verts and vecs.
See Also: CKPatch,GetVertCount,SetVert,GetVert,SetVec,GetVec
************************************************/
virtual	void		SetVertVecCount(int VertCount,int VecCount) CK_PURE;

/************************************************
Summary: Gets verts (corner control points) count of the mesh.

Return Value:
	Number of verts in the patchmesh.
See Also: CKPatch,GetVecCount,SetVert,GetVert,SetVec,GetVec
************************************************/
virtual	int		GetVertCount() CK_PURE;

/************************************************
Summary: Sets a corner control point position.

Arguments:
	index: Index of the vert whose position is to be set.
	cp:    Position.
See Also: GetVert, GetVerts,GetVertCount
************************************************/
virtual	void	SetVert(int index,VxVector* cp) CK_PURE;	

/************************************************
Summary: Gets a corner control point position.

Arguments:
	index: Index of the vert whose position is to be get.
	cp:    Position to be filled.
See Also: SetVert, GetVerts,GetVertCount
************************************************/
virtual	void	GetVert(int index,VxVector* cp) CK_PURE;

/************************************************
Summary: Gets all corner control points 

Return Value:
	A pointer to the list of verts (corner control points).
See Also: SetVert,GetVert,GetVertCount
************************************************/
virtual	VxVector*	GetVerts() CK_PURE;

/************************************************
Summary: Gets vecs (edge and interior control points) count of the mesh.

Return Value:
	Number of verts in the patchmesh.
See Also: CKPatch,GetVecCount,SetVert,GetVert,SetVec,GetVec
************************************************/
virtual	int		GetVecCount() CK_PURE;

/************************************************
Summary: Sets a edge control point position.

Arguments:
	index: Index of the vec whose position is to be set.
	cp:    Position.
See Also: SetVec, GetVecs,GetVecCount
************************************************/
virtual	void	SetVec(int index,VxVector* cp) CK_PURE;	

/************************************************
Summary: Gets a edge control point position.

Arguments:
	index: Index of the vec whose position is to be get.
	cp:    Position to be filled.
See Also: SetVec, GetVecs,GetVecCount
************************************************/
virtual	void	GetVec(int index,VxVector* cp) CK_PURE;

/************************************************
Summary: Gets all edge and interiors  control points 

Return Value:
	A pointer to the list of vecs (edge and interior control points).
See Also: SetVec,GetVec,GetVecCount
************************************************/
virtual	VxVector*	GetVecs() CK_PURE;

//--------------------------------------------
// Edges

/************************************************
Summary: Sets the number of edges.

Arguments:
	count: Number of edges to be set.
See Also: GetEdgeCount,SetEdge,GetEdge,GetEdges
************************************************/
virtual	void SetEdgeCount(int count) CK_PURE;

/************************************************
Summary: Returns the number of edges.

Return Value:
	Number of edges.
See Also: SetEdgeCount,SetEdge,GetEdge,GetEdges
************************************************/
virtual	int	GetEdgeCount() CK_PURE;

/************************************************
Summary: Sets a given edge data.

Arguments:
	index: Index of the edge to be set.
	edge:  A pointer to a CKPatchEdge structure containing the edge data.
See Also: SetEdgeCount,GetEdgeCount,GetEdge,GetEdges,CKPatchEdge
************************************************/
virtual	void	SetEdge(int index,CKPatchEdge* edge) CK_PURE;

/************************************************
Summary: Gets a given edge data.

Arguments:
	index: Index of the edge to be get.
	edge:  A pointer to a CKPatchEdge structure that will be filled with the edge data.
See Also: SetEdgeCount,GetEdgeCount,SetEdge,GetEdges,CKPatchEdge
************************************************/
virtual	void	GetEdge(int index,CKPatchEdge* edge) CK_PURE;

/************************************************
Summary: Gets a pointer to the list of edges.

See Also: SetEdgeCount,GetEdgeCount,SetEdge,GetEdge,CKPatchEdge
************************************************/
virtual	CKPatchEdge* GetEdges() CK_PURE;

//---------------------------------------------
// Patches

/************************************************
Summary: Sets the number of patches.

Arguments:
	count: Number of patches to be set for the patchmesh.
See Also: GetPatchCount,SetPatch,GetPatch,GetPatches
************************************************/
virtual	void	SetPatchCount(int count) CK_PURE;

/************************************************
Summary: Gets the number of patches.

Return Value:
	Number of patches.
See Also: SetPatchCount,SetPatch,GetPatch,GetPatches
************************************************/
virtual	int		GetPatchCount() CK_PURE;

/************************************************
Summary: Sets the description of a given patch.

Arguments:
	index: Index of the patch to be set.
	p: A pointer to a CKPatch structure containing the description of the patch.
See Also: CKPatch,SetPatchCount,GetPatch,GetPatches,SetPatchSM,SetPatchMaterial
************************************************/
virtual	void	SetPatch(int index,CKPatch* p) CK_PURE;

/************************************************
Summary: Gets the description of a given patch.

Arguments:
	index: Index of the patch to be set.
	p: A pointer to a CKPatch structure to be filled with the description of the patch.
See Also: CKPatch,SetPatchCount,SetPatch,GetPatches,SetPatchSM,SetPatchMaterial
************************************************/
virtual	void	GetPatch(int index,CKPatch* p) CK_PURE;

/************************************************
Summary: Gets the smoothing group of a patch.

Arguments:
	Index: Index of patch whose smoothing group is to be obtained.
Return Value:
	Smoothing group of the patch.
Remarks:
	+ The smoothing group is a DWORD mask, if the binary AND of the smoothing group value of
	two adjacent patches is not 0 then they are smoothed together otherwise there is a sharp edge
	between the two patches. The default value of a patch smoothing group is 0xFFFFFFFF which means
	all patches are smoothed together.
See Also: SetPatchSM,SetPatch,CKPatch
************************************************/
virtual	CKDWORD	GetPatchSM(int index) CK_PURE;

/************************************************
Summary: Sets the smoothing group for the patch.

Arguments:
	index: Index of patch whose smoothing group is to be set.
	smoothing: Smoothing group of the patch to be set.
Remarks:
	+ The smoothing group is a DWORD mask, if the binary AND of the smoothing group value of
	two adjacent patches is not 0 then they are smoothed together otherwise there is a sharp edge
	between the two patches. The default value of a patch smoothing group is 0xFFFFFFFF which means
	all patches are smoothed together.
See Also: GetPatchSM,SetPatch,CKPatch
************************************************/
virtual	void	SetPatchSM(int index,CKDWORD smoothing) CK_PURE;

/************************************************
Summary: Gets the material used on a given patch.

Arguments:
	Index: Index of patch whose material is to be obtained.
Return Value:
	Pointer to the material.
See Also: SetPatchMaterial,CKPatch
************************************************/
virtual	CKMaterial*	GetPatchMaterial(int index) CK_PURE;

/************************************************
Summary: Sets the material used for a given patch.

Arguments:
	index: Index of patch whose material is to be obtained.
	mat: A Pointer to the material to use for this patch.
See Also: GetPatchMaterial,CKPatch
************************************************/
virtual	void	SetPatchMaterial(int index,CKMaterial* mat) CK_PURE;

/************************************************
Summary: Gets a pointer to the list of patches.

See Also: CKPatch,SetPatchCount,SetPatch,GetPatch
************************************************/
virtual	CKPatch* GetPatches() CK_PURE;

//------------------------------------------- 
// Texture Patches

/************************************************
Summary: Sets the number of texture patches

Arguments: 
	count: Number of texture patches to be set.
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
Remarks:
	+ The number of texture patches can be 0 (no texturing ) otherwise it must be equal
	to the number of patches.
See Also: SetPatchCount,GetTVPatchCount
************************************************/
virtual	void	SetTVPatchCount(int count,int Channel=-1) CK_PURE;

/************************************************
Summary: Gets the number of texture patches

Arguments: 
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
Remarks:
	+ The number of texture patches can be 0 (no texturing ) otherwise it must be equal
	to the number of patches.
See Also: SetPatchCount,SetTVPatchCount
************************************************/
virtual	int		GetTVPatchCount(int Channel=-1) CK_PURE;

/************************************************
Summary: Sets the mapping of a given patch.

Arguments: 
	index: Index of the patch which mapping should be set.
	tvpatch: A pointer to a CKTVPatch structure that contains the indices of the texture coordinates of the corner points of the patch.
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
See Also: GetTVPatch, GetTVPatches,GetTVPatchCount,SetTVPatchCount,SetTVCount
************************************************/
virtual	void	SetTVPatch(int index,CKTVPatch* tvpatch,int Channel=-1) CK_PURE;

/************************************************
Summary: Gets the mapping of a given patch.

Arguments: 
	index: Index of the patch which mapping should be set.
	tvpatch: A pointer to a CKTVPatch structure that will be filled with the indices of the texture coordinates of the corner points of the patch.
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
See Also: SetTVPatch, GetTVPatches,GetTVPatchCount,SetTVPatchCount,SetTVCount
************************************************/
virtual	void	GetTVPatch(int index,CKTVPatch* tvpatch,int Channel=-1) CK_PURE;

/************************************************
Summary: Gets a pointer to the list of texture patches.

Arguments: 
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
See Also: SetTVPatch, GetTVPatch,GetTVPatchCount,SetTVPatchCount,SetTVCount
************************************************/
virtual	CKTVPatch* GetTVPatches(int Channel=-1) CK_PURE;

//-----------------------------------------
// Texture Verts 

/************************************************
Summary: Sets the number of the texture coordinates for a given channel.

Arguments: 
	count: Number of texture vertices to be set.
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
See Also: GetTVCount,GetTVPatchCount,SetTV,GetTV,GetTVs
************************************************/
virtual	void	SetTVCount(int count,int Channel=-1) CK_PURE;

/************************************************
Summary: Returns the number of the texture coordinates for a given channel.

Arguments: 
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
Return Value:
	Number of texture coordinates.
See Also: SetTVCount,SetTV,GetTV,GetTVs,GetTVPatchCount
************************************************/
virtual	int		GetTVCount(int Channel=-1) CK_PURE;

/************************************************
Summary: Sets the texture coordinate values.

Arguments: 
	index: Index of the texture vertex whose value has to be set.
	u: U texture coordinate value.
	v: V texture coordinate value.
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
See Also: GetTV, GetTVs,SetTVCount,SetTVPatch
************************************************/
virtual	void	SetTV(int index,float u,float v,int Channel=-1) CK_PURE;

/************************************************
Summary: Gets the texture coordinate values.

Arguments: 
	index: Index of the texture vertex whose value has to be retrieve.
	u: U texture coordinate value.
	v: V texture coordinate value.
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
See Also: GetTV, GetTVs,SetTVCount,SetTVPatch
************************************************/
virtual	void	GetTV(int index,float* u,float* v,int Channel=-1) CK_PURE;

/************************************************
Summary: Gets a pointer to the list of texture coordinates.

Arguments: 
	Channel: Index of the channel (-1 for default texture coordinates or >=0 for an additionnal material channel).
See Also: GetTV, SetTV,GetTVCount,GetTVPatchCount
************************************************/
virtual	VxUV*	GetTVs(int Channel=-1) CK_PURE;


/*************************************************
Summary: Dynamic cast operator. 
Arguments:
	iO: A pointer to a CKObject to cast.
Return Value:
	iO casted to the appropriate class or NULL if iO is not from the required class .
Example:
	  CKObject* Object;
	  CKAnimation* anim = CKAnimation::Cast(Object);
Remarks:

*************************************************/
static CKPatchMesh* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_PATCHMESH)?(CKPatchMesh*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif

