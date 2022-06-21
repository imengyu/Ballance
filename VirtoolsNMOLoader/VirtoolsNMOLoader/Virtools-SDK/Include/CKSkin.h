/*************************************************************************/
/*	File : CKSkin.h														 */
/*	Author :Romain Sididris												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKSKIN_H

#define CKSKIN_H "$Id:$"

#include "CKDefines.h"

/******************************************************
Summary: Bone information in a Skin 

Remarks: 
+A bone uses a CK3dEntity to represents its orientation
+The CKSkin class returns instance of this class to describe
a given bone with CKSkin::GetBoneData.
See Also:CK3dEntity,CKSkin,CKSkinVertexData
******************************************************/
class CKSkinBoneData {
public:
/*********************************************
Summary: Sets the 3D Entity that represents this bone orientation.
Arguments:
	ent: A pointer to the CK3dEntity that represents this bone orientation.
See Also:GetBone
**********************************************/
	virtual void SetBone(CK3dEntity* ent) =0;

/*********************************************
Summary: Returns the 3D Entity that represents this bone orientation.
Return Value:
	A Pointer to the CK3dEntity that gives the bone orientation.
See Also:SetBone
**********************************************/	
	virtual CK3dEntity* GetBone() =0;
/*********************************************
Summary: Sets the bone original orientation.
Arguments:
	M: A VxMatrix that represents the bone original inverse world matrix.
Remarks:
+The vertices of a skin are transformed according to 
the relative transformation between the original bone orientation
and its current orientation.
+In the same manner the original position of vertices must be given,
the original orientation (Inverse World Matrix) of the bones must
be given with this method.
See Also:CKSkin,CKSkinVertexData::SetInitialPos,CK3dEntity::GetInverseWorldMatrix
**********************************************/	
	virtual void SetBoneInitialInverseMatrix(const VxMatrix& M) =0;
/*********************************************
Summary: Gets the bone original orientation.
Return Value: A VxMatrix that represents the bone original inverse world matrix.
Remarks:
+The vertices of a skin are transformed according to 
the relative transformation between the original bone orientation
and its current orientation.
See Also:CKSkin,SetBoneInitialInverseMatrix
**********************************************/	
	virtual VxMatrix& GetBoneInitialInverseMatrix() = 0;
};

/******************************************************
Summary: Vertex information in a Skin 

Remarks: 
+ Each vertex in a skin can be influenced by one or more bones. 
+ Each of theses bones influences the vertex with a given weight, the sum
of all weights should be 1.
+ The CKSkin class returns instance of this class to describe
a given vertex with CKSkin::GetVertexData.
See Also:CK3dEntity,CKSkin,CKSkinBoneData
******************************************************/
class CKSkinVertexData {
public:
/*********************************************
Summary: Sets the number of bones that influence this vertex.
Arguments:
	BoneCount: Number of bones that influence this vertex.	
See Also:GetBoneCount,SetBone,GetBone,GetWeight
**********************************************/	
	virtual void SetBoneCount(int BoneCount) =0;
/*********************************************
Summary: Gets the number of bones that influence this vertex.
Return Value:
	Number of bones that influence this vertex.	
See Also:SetBoneCount,SetBone,GetBone,GetWeight
**********************************************/	
	virtual int GetBoneCount() =0;
/*********************************************
Summary: Gets the index of the n-th bone that influences this vertex.
Arguments:
	n: Number of the bone index to be returned.
Return Value:
	Index of the n-th bone that influence this vertex, this index can be used
	to retrieve the bone data with CKSkin::GetBoneData.
See Also:GetBoneCount,SetBone,GetWeight,CKSkin::GetBoneData.
**********************************************/	
	virtual int GetBone(int n) =0;
/*********************************************
Summary: Sets the index of the n-th bone that influences this vertex.
Arguments:
	n: Number of the bone which index should be set.
	BoneIdx: Index of the n-th bone that influence this vertex in the skin data.
See Also:GetBoneCount,GetBone,GetWeight,CKSkin::GetBoneData.
**********************************************/	
	virtual void SetBone(int n,int BoneIdx) =0;
	
/*********************************************
Summary: Gets the influence of the n-th bone.
Arguments:
	n: Number of the bone which index should be set.
Return Value:
	Influence of the n-th bone.
See Also:GetBoneCount,GetBone,SetWeight
**********************************************/	
	virtual float GetWeight(int n) =0;
/*********************************************
Summary: Sets the influence of the n-th bone.
Arguments:
	n: Number of the bone which index should be set.
	Weight: Influence of the bone.
See Also:GetBoneCount,GetBone,GetWeight
**********************************************/	
	virtual void SetWeight(int n,float Weight) =0;

/*********************************************
Summary: Gets the original position of this vertex.
Return Value:
	Position of the vertex when all bones are at their initial matrices.
See Also:SetInitialPos,CKBoneData::SetBoneInitialInverseMatrix
**********************************************/	
	virtual VxVector& GetInitialPos() = 0;
/*********************************************
Summary: Gets the original position of this vertex.
Arguments:
	pos: Original position of the vertex.
Remarks:
+ The vertices of a skin are transformed according to 
the relative transformation between the original bone orientation
and its current orientation.
+ In the same manner the original orientation of bones must be given,
the original position of vertices must
be given with this method.
See Also:SetInitialPos,CKBoneData::SetBoneInitialInverseMatrix
**********************************************/	
	virtual void SetInitialPos(VxVector& pos) = 0;
};


/***********************************************************
Summary : Skin


Remarks:

+ In a standard mesh the vertices are all transformed by the same 3d Entity world matrice 
to be rendered. Skinning is the technique of having different vertices of a mesh be 
transformed by multiple transform matrices each of these matrices being given a different influence (weight).

+ The CKSkin class contains a list of vertices and a list of bones (3dEntities) that influence
these vertices. 

+ A skin is attached to a given 3d entity, each time the 3d Entity is rendered 
and if one of the bones has moved the entity's current mesh is recomputed according 
to the skin data.

+ It can be created with the CK3dEntity::CreateSkin method.

+ The sample exporter (see Available Source Code) shows an example
of creation of CKSkin.

See Also:CK3dEntity::CreateSkin
************************************************************/
class CKSkin : public VxPoolObject {
public:
/*********************************************
Summary: Sets the owner 3d Entity initial orientation.

Arguments:
	Mat: Original world matrix of the owner 3dEntity.
See Also:CK3dEntity::CreateSkin
**********************************************/
	virtual void SetObjectInitMatrix(const VxMatrix& Mat) = 0;
/*********************************************
Summary: Gets the owner 3d Entity initial orientation.

Arguments:
Mat: Original world matrix of the owner 3dEntity.
See Also:CK3dEntity::CreateSkin
**********************************************/
	virtual void GetObjectInitMatrix(VxMatrix& Mat) = 0;
/*********************************************
Summary: Sets the number of bones in the skin.

Arguments:
	BoneCount: Number of bones that influence the skin.
Remarks:
	+ Each bone information can be retrieved with GetBoneData method.
See Also:SetVertexCount,GetBoneCount,GetBoneData
**********************************************/
	virtual void SetBoneCount(int BoneCount) = 0;
/*********************************************
Summary: Sets the number of vertices in the skin.

Arguments:
	Count: Number of vertices.
Remarks:
	+ Each vertex information (Number of bones that influence the vertex,weights) can be retrieved with GetVertexData method.
See Also:GetVertexCount,GetBoneCount,GetVertexData
**********************************************/
	virtual void SetVertexCount(int Count) = 0;
/*********************************************
Summary: Gets the number of bones in the skin.

Return Value:
	Number of bones that influence the skin.
Remarks:
	+ Each bone information can be retrieved with GetBoneData method.
See Also:SetVertexCount,SetBoneCount,GetBoneData
**********************************************/
	virtual int  GetBoneCount() const = 0;
/*********************************************
Summary: Gets the number of vertices in the skin.

Return Value:
	Number of vertices.
Remarks:
	+ Each vertex information (Number of bones that influence the vertex,weights) can be retrieve with GetVertexData method.
See Also:SetVertexCount,GetVertexData
**********************************************/
	virtual int GetVertexCount() const = 0;

/*********************************************
Summary: Precomputes the bones transformation matrices.

Remarks:
+ This method computes the appropriate matrices for each bone (3d Entity)
used by the skin.
+ It must called before evaluating the vertices transformed position with CalcPoints.
See Also:CalcPoints
**********************************************/
	virtual void ConstructBoneTransfoMatrices(CKContext* context) =0;
	
/*********************************************
Summary: Computes the tranformed vertices positions.

Arguments:
	VertexCount: Number of vertices to be computed.
	VertexPtr: A pointer to a strided array of VxVector to be filled withe transformed positions.
	VStride: Amount in bytes between each VxVector in the VertexPtr buffer.
Remarks:
+ This method computes the positions of the skin vertices according to the 
current bones matrices.
+ ConstructBoneTransfoMatrices must have been called before evaluating the vertices transformed positions.
See Also:ConstructBoneTransfoMatrices
**********************************************/
	virtual CKBOOL CalcPoints(int VertexCount,BYTE* VertexPtr,CKDWORD VStride) = 0;
	
/*********************************************
Summary: Gets description of the n-th bone.

Arguments:
	BoneIdx: Index of the bone which description should be returned
Return Value:
	A pointer to CKSkinBoneData that contains the bone information (CK3dEntity,orginal orientation..).
See Also:CKSkinBoneData,SetBoneCount,GetBoneCount
**********************************************/
	virtual CKSkinBoneData* GetBoneData(int BoneIdx) = 0;
/*********************************************
Summary: Gets description of the n-th vertex.

Arguments:
	VertexIdx: Index of the vertex which description should be returned
Return Value:
	A pointer to CKSkinVertexData that contains the vertex information (Number of bones that influence the vertex,weights,etc..).
See Also:CKSkinVertexData,SetVertexCount,GetVertexCount
**********************************************/
	virtual CKSkinVertexData* GetVertexData(int VertexIdx) =0;
/*********************************************
Summary: Changes the order of the vertices.

Arguments:
	permutation: An array of integer containing the new index of each vertex
See Also:SetVertexCount,GetVertexCount
**********************************************/
	virtual void RemapVertices(const XArray<int>& permutation) =0;

/*********************************************
Summary: Specify if normals should be skinned too.

Arguments:
	Count: must be equal to the number of vertices or 0 to disable the skinning of normals.
Remarks:
	When set to 0 the normals of the mesh are automatically recomputed at each skinning 
	using the face normals otherwise the normals are skinned as the vertex positions.
See Also:SetVertexCount,GetVertexCount,SetNormal,GetNormal
**********************************************/
	virtual void SetNormalCount(int Count) =0;

	virtual int GetNormalCount() =0;

/*********************************************
Summary: SetNormal.

Arguments:
	Index: Index of the normal to set or retrieve.
	Norm: Vector representing the normal to set.
See Also: SetNormalCount
**********************************************/
	virtual void SetNormal(int Index,const VxVector& Norm) =0;

	virtual VxVector& GetNormal(int Index) =0;

/*********************************************
Summary: Computes the tranformed vertices positions and Normals.

Return Value:
	TRUE if successful.
Arguments:
	VertexCount: Number of vertices to be computed.
	VertexPtr: A pointer to a strided array of VxVector to be filled with transformed positions.
	VStride: Amount in bytes between each VxVector in the VertexPtr buffer.
	NormalPtr: A pointer to a strided array of VxVector to be filled with transformed normals.
	NStride: Amount in bytes between each VxVector in the NormalPtr buffer.
Remarks:
+This method computes the positions of the skin vertices according to the 
current bones matrices.
+ConstructBoneTransfoMatrices must have been called before evaluating the vertices transformed positions.
See Also:ConstructBoneTransfoMatrices
**********************************************/
	virtual CKBOOL CalcPoints(int VertexCount,BYTE* VertexPtr,CKDWORD VStride,BYTE* NormalPtr,CKDWORD NStride) = 0;

	
/*************************************************
Summary: returns a pointer to the internal bones matrix array

*************************************************/
virtual VxMatrix * GetTransformedBonesArray() = 0;


/*************************************************
Summary: Sets the referential in which the bones matrix array is calculated

*************************************************/
virtual void SetHardwareSkinningRef(CK_BONES_REFERENTIAL ref) =0;

/*************************************************
Summary: Gets the referential in which the bones matrix array is calculated

*************************************************/
virtual CK_BONES_REFERENTIAL GetHardwareSkinningRef() =0;

/*************************************************
Summary: reposition vertices and normals to their original positions

*************************************************/
virtual void RestoreOriginalVNPositions(CK3dEntity* Entity)=0;

/*************************************************
Summary: returns the maximum number of bones per vertex

*************************************************/
virtual int GetMaxBonesPerVertex() = 0;


virtual void ConstructRstBoneTransfoMatrices(CKContext* context) =0;
virtual void EnsureLessThanNBonesPerVertex(int MaxBonesPerVertex = 4) = 0;

#if defined(_XBOX) && (_XBOX_VER<200)

virtual void EnsureLessThanNBones(int MaxBones = 44)=0;
#else

virtual void EnsureLessThanNBones(int MaxBones = 84)=0;
#endif
};


#endif