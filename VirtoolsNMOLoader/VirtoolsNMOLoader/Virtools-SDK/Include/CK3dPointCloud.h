/*************************************************************************/
/*	File : CK3dPointCloud.h												 */
/*	Author : Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2003, All Rights Reserved.					 */	
/*************************************************************************/
#if (!defined(CK3DPOINTCLOUD_H) || defined(CK_3DIMPLEMENTATION))

#define CK3DPOINTCLOUD_H "$Id:$"
// When included from the implementation side the
// sub includes,  PURE definition and class  statement must to be removed


#ifndef CK_3DIMPLEMENTATION


/***********************************************************
Summary: An iterator to iterate on a cloud of points.
{filename:CloudSelection}
Remarks:
See Also:CK3dPointCloud
************************************************************/
class CloudSelection {
public:
	enum EDrawMode {
		ePoint	  		= 0,
		eSprites		= 1,
		ePrimitive		= 2, 
	};
	enum Flags {
		eLighting		= 1<<4,
		eVBUptodate		= 1<<5, 
		eVisible		= 1<<6,
		eUseMatColor	= 1<<7,
	};
	XString					m_Name;
	int						m_MaxSize;
	int						m_Flags;
	CKMaterial*				m_DisplayMaterial;
	CKMesh*					m_DisplayMesh;


	virtual const XString& GetName() const = 0;	
	virtual void		SetName(const XString& iName) = 0;	

	virtual CKMaterial* GetDisplayMaterial() const = 0;	
	virtual void		SetDisplayMaterial(CKMaterial* iMaterial) = 0;	

	virtual void   UseMaterialColor(CKBOOL iUse) = 0;
	virtual CKBOOL   IsUsingMaterialColor()  const = 0;

	virtual CKBOOL		IsVisible() const = 0;	
	virtual void		SetVisible(CKBOOL IsVisible) = 0;	

	virtual CKBOOL		IsLightingEnabled() const = 0;	
	virtual void		EnableLighting(CKBOOL iEnable = TRUE) = 0;	

	virtual int			GetPointCount() const = 0;	
	virtual const VxVector&	GetPointPosition(int iPointIndex) const = 0;	
	virtual int			GetPointOriginalIndex(int iPointIndex) const = 0;	

	virtual int			GetMaximumPointCount() const = 0;	
	virtual void		SetMaximumPointCount(int iMaxPointCount) = 0;	

	virtual int			GetDrawMode() const = 0;	
	virtual void		SetDrawMode(int iMode) = 0;	

	virtual CKMesh*		GetDisplayMesh() const = 0;	
	virtual void		SetDisplayMesh(CKMesh* iMesh) = 0;	


	virtual void		ClearSelection() = 0;	
};


/***********************************************************
Summary: An iterator to iterate on a cloud of points.
{filename:CloudIterator}
Remarks:
It can be used to iterate the octree that is used to store the data. 
This tree is made up of node that contain up to 8 children nodes  

One can ask an iterator from the tree root node (See CK3dPointCloud::GetRootIterator)
and then iterate using Depth first (NextNode) or custom (ToChildByIndex) iteration.

Warning: Only a single iterator should be created at a time on a single CK3dPointCloud.

See Also:CK3dPointCloud
************************************************************/
class CloudIterator {
public:
//Summary: Return TRUE if the iterator is on a tree node.
//
	virtual XBOOL IsNode() = 0;	
//Summary: Return TRUE if the iterator is on a tree leaf.
//
	virtual XBOOL IsLeaf() = 0;	
//Summary: Return the current node bounding box in cloud local coordinates (as visible in the 3D view).
//
	virtual void  GetBox(VxBbox& oBox) = 0;	
//Summary: Return the current node bounding box in cloud local coordinates (as in the original .asc file).
//
	virtual void  GetRealBox(VxBbox& oBox) = 0;	

//--- Linear iteration (depth first or width first)

//Summary: Advance the iterator to the next element (node or leaf) in a Depth First order.
//
	virtual XBOOL NextNode() = 0;	
//Summary: Advance the iterator to the next point
//
	virtual XBOOL NextPoint() = 0;

//--- Node iteration

//Summary: Move this iterator back the parent node
//Return Value: TRUE if sucessful, FALSE if there the iterator is on tree root.
//
	virtual XBOOL	ToParent() = 0;	
//Summary: Return the number of children of the current node
//
	virtual int		GetChildrenCount() = 0;	
//Summary: Advance the iterator down to the iChildIndex children
//Return Value: TRUE if sucessful, FALSE if no child exist.
//
	virtual XBOOL	ToChildByIndex(int iChildIndex) = 0;	
	
//Summary: Return if a child exists for one of the 8 possible branches of a node.
//
	virtual XBOOL IsChildHere(int iBranchIndex) = 0;	
//Summary: Advance the iterator down to a children given its position in the 8 possible branches.
//Return Value: TRUE if sucessful, FALSE if no child exist.
//
	virtual XBOOL ToChildByBranch(int iBranchIndex) = 0;	

//Summary: Returns the position of the currently iterated point (as visible in the 3D view).
//
	virtual VxVector GetCurrentPointPosition() = 0;	
//Summary: Returns the position of the currently iterated point (as it was stored in the .asc file).
//
	virtual VxVector GetCurrentPointRealPosition() = 0;	
//Summary: Returns the global index of the currently iterated point.
//
	virtual int      GetCurrentPointIndex() = 0;	

};


/*******************************************************
Summary: Attributes on points
Remarks:
Every points can store attributes on it. By default 4 types of attribute are defined
(not necessarily used): A color , normal , intensity and render group.	
This attributes can be integer or float values. For integer the number of bits used to store
the value can be specified in the BitsPerValue member thus allowing efficient storage.
See Also:CK3dPointCloud
*******************************************************/
struct CloudAttributeDescription {
	
	CloudAttributeDescription():ValueType(ATTRIBUT_INT),BitsPerValue(0),Flags(ATTRIBUT_RESERVED) {}
#ifdef DOCJETDUMMY // DOCJET secret macro
#else
	enum AttValueType {
		ATTRIBUT_INT	= 0,	// A int (with n bit per value) will be stored for each point		
		ATTRIBUT_FLOAT	,		// A float (32 bit)  will be stored for each point	
		ATTRIBUT_COLOR	,		// A 16 bit color 	
		ATTRIBUT_NORMAL	,		// A 10 bit compressed normal 	
	};
	enum AttFlags {
		ATTRIBUT_RESERVED	= 1<<0,	//
		ATTRIBUT_DYNAMIC	= 1<<1,	// Attribute values are frequently updated		
		ATTRIBUT_PUTINVB	= 1<<2,	// This attribut values will be put in the vertex buffers with positions
	};
#endif
	AttValueType	ValueType;			// Type of the value stored 
										// Can be ATTRIBUT_INT , ATTRIBUT_FLOAT , ATTRIBUT_COLOR (A 16 bit 565 color) or ATTRIBUT_NORMAL (A 16 bit compressed normal)
	XString			Name;				// Name of the attribute	
	int				BitsPerValue;		// Summary:Number of bits taken by the value
										// Remarks:This value is only used when using an ATTRIBUT_INT or ATTRIBUT_ENUM storage. If this not given (0) 32 bits will be used for each value.
	DWORD			Flags;				// Reserved
};

#include "CK3dEntity.h"


#undef CK_PURE

#define CK_PURE = 0


/***************************************************************************
{filename:CK3dPointCloud}
Name: CK3dPointCloud

Summary: Cloud of 3D points.
Remarks:
This class is used to manage and display clouds of 3D points.
These points are stored in a tree structure for efficient memory management and can have 
attribute attached like colors, normal or user defined attributes.

One of these attribute is called the render group (see GetPointRenderGroup). 
Points can be placed in different render group to have different render options 
(rendering as a sprite , size of this sprite , usage of a given material, etc..).

The class id of CK3dPointCloud is CKCID_3DPOINTCLOUD.

See also: CK3dEntity
***************************************************************************/
class CK3dPointCloud : public CK3dEntity {
#endif
public :

/***************************************************************
Summary: Default Point cloud attributes.
Remarks:
	This enumeration represents the attribute type that are always 
created for a 3D point cloud (which does not mean they are used)
*****************************************************************/
enum DefaultCloudAttribute {
	CLOUD_ATTRIBUTE_UNUSED		 = 0,	// Attribute 0 is not used
	CLOUD_ATTRIBUTE_COLOR		 = 1,	// Color information is stored as 16 bits color 
	CLOUD_ATTRIBUTE_NORMAL		 = 2,	// Normal information is stored as compressed on 10 bits
	CLOUD_ATTRIBUTE_INTENSITY	 = 3,	// An intensity stored on 8 bits 
};	

/************************************************************
Summary: Create the cloud using a .Asc file.

Remarks:
This method searches for a file named  iFileName.Desc in the same directory
than the iFileName file. 
This *.Desc* file contains the information about the number of points, bounding box, etc...
and its syntax is the following:

		Version 0
		Points 2399432
		Format 6
		Precision (0.110841,0.122853,0.114422)
		BoxMin (503847.437500,78931.140625,19900.824219)
		BoxMax (521581.968750,98587.679688,38208.335938)
		Translate (-512000,-88000,-29000)
		Rescale 0.05

No comments are allowed in a *.Desc* file for the moment....  
This information allows for a quick one-step processing of the .asc file. If the *.Desc* file is not found 
the file is loaded in a two-step process to identify the bounding volumes
The asc file is a text-mode file which contains a list of point and its syntax looks like this:

		10.0	11.0	 5.0
		8.002	13.15	 14.2
		etc...	

The data contained in a .asc file is expected to be in the form :
 X Y Z (floats) [R G B (0..255)] [NX NY NZ (floats)] [I(0..255)]

If the appropriate fields are found (color, normal and/or intensity) the relevant attribute types
are created and marked as used on the point cloud.
See Also: CreateFromPointList
*************************************************************/
virtual CKBOOL CreateFromAscFile(const char* iFileName) CK_PURE;

/************************************************************
Summary: Create the cloud using an array of points.

Arguments:
	iPointCount: Number of points in the iPoints array.
	iPoints: A point to the list of point in the cloud.
	iNormals: (Optionnal, can be NULL) A pointer to an array of VxVector containing the points normals.
	iColors: (Optionnal, can be NULL) A pointer to an array of DWORD containing the points colors (32 bit ARGB).
	iIntensities: (Optionnal, can be NULL) A pointer to an array of Bytes containing the points intensity.
	iPrecision: A vector containing the error that is acceptable between the value given in iPoints and the values that will get stored in the tree. 
Remarks:
This methods creates a new cloud using the data given in the different arrays.
Only the iPointCount, iPoints and iPrecision parameters are mandatory.
See Also: CreateFromAscFile
*************************************************************/
virtual CKBOOL CreateFromPointList(int iPointCount,VxVector* iPoints,VxVector* iNormals,DWORD* iColors,BYTE* iIntensities,VxVector& iPrecision) CK_PURE;

/****************************************************************
Summary: Enables or disables the lighting of the points

Input Arguments:
	iEnable: TRUE to set lighting on , FALSE to set it off.
Remarks:
	If the data set does not contain any normal information the 
lighting can not be enabled.
See Also: IsLightingEnabled,HasNormals
*****************************************************************/
virtual void   EnableLighting(CKBOOL iEnable = TRUE) CK_PURE;

/****************************************************************
Summary: Returns if the point are lit.

Remarks:
	If the data set does not contain any normal information the 
lighting can not be enabled.
See Also: EnableLighting,HasNormals
*****************************************************************/
virtual CKBOOL IsLightingEnabled()  const CK_PURE;

/****************************************************************
Summary: Sets the size factor for points drawn as sprites.

Remarks:
This value is taken as a multiplication factor of the size of the point sprites.
To be drawn as sprites a point must belong to a render group which rendering
is set to Sprite mode (see CloudRenderGroup).
See Also: SetRenderGroup,GetGlobalPointSize,CloudRenderGroup
*****************************************************************/
virtual void   SetGlobalPointSize(float iPointSize) CK_PURE;

/****************************************************************
Summary: Returns the size factor for points drawn as sprites.

Remarks:
This value is taken as a multiplication factor of the size of the point sprites.
To be drawn as sprites a point must belong to a render group which rendering
is set to Sprite mode (see CloudRenderGroup).
See Also: SetRenderGroup,SetGlobalPointSize,CloudRenderGroup
*****************************************************************/
virtual float  GetGlobalPointSize()  const CK_PURE;

virtual void   ShowMainCloud(CKBOOL iShow) CK_PURE;
virtual CKBOOL  IsMainCloudVisible() const CK_PURE;

virtual void   SetPickingPrecision(float iPointDistance) CK_PURE;
virtual float  GetPickingPrecision() const CK_PURE;

virtual void   UsePointSprite(CKBOOL iUse) CK_PURE;
virtual CKBOOL  IsUsingPointSprite() const CK_PURE;

virtual void   SetGlobalMaterial(CKMaterial* iMaterial) CK_PURE;
virtual CKMaterial*  GetGlobalMaterial()  const CK_PURE;

virtual void   UseMaterialColor(CKBOOL iUse) CK_PURE;
virtual CKBOOL   IsUsingMaterialColor()  const CK_PURE;


virtual void   SetLOD(int iLod) CK_PURE;
virtual int  GetLOD()  const CK_PURE;




// Number of points...
/****************************************************************
Summary: Returns the number of points stored.

Remarks:
See Also: GetMemorySize,GetVertexBufferCount,GetVertexBufferMemorySize
*****************************************************************/
virtual CKDWORD   GetPointsCount()  const CK_PURE;
/****************************************************************
Summary: Returns the memory taken to store all the points and their attributes.

Remarks:
See Also: GetPointsCount,GetVertexBufferCount,GetVertexBufferMemorySize
*****************************************************************/
virtual CKDWORD   GetMemorySize()  const CK_PURE;

/****************************************************************
Summary: Returns the number of vertex buffers allocated to display the cloud.

Remarks:
The number of vertex buffer that will be generated depends on the number of 
vertices that can be placed in a single vertex buffer. This value is controled 
by a global variable CK2_3D\CloudVerticesPerVB.
See Also: GetPointsCount,GetMemorySize,GetVertexBufferMemorySize
*****************************************************************/
virtual CKDWORD   GetVertexBufferCount()  const CK_PURE;
/****************************************************************
Summary: Returns the memory taken to store all the vertex buffers.

Remarks:
This method returns the size (in bytes) taken by all the currently allocated
vertex buffers to display the cloud.
See Also: GetPointsCount,GetVertexBufferCount,GetMemorySize
*****************************************************************/
virtual CKDWORD   GetVertexBufferMemorySize() const CK_PURE;


/****************************************************************
Summary: Returns the offset that was applied to all the points when loaded from a .asc file.

Remarks:
When loading from a .asc file an offset and scale factor can be applied to all the points 
if a *.Desc* was used. 
This scale factor and offset can be taken into account when accessing the point position with 
an iterator (see CloudIterator::GetCurrentPointPosition and GetCurrentPointRealPosition )
See Also: CreateFromAscFile, GetLoadScaleFactor
*****************************************************************/
virtual const VxVector&  GetLoadOffset() const CK_PURE;

/****************************************************************
Summary: Returns the scale factor that was applied to all the points when loaded from a .asc file.

Remarks:
When loading from a .asc file an offset and scale factor can be applied to all the points 
if a *.Desc* was used. 
This scale factor and offset can be taken into account when accessing the point position with 
an iterator (see CloudIterator::GetCurrentPointPosition and GetCurrentPointRealPosition )
See Also: CreateFromAscFile, GetLoadOffset
*****************************************************************/
virtual float   GetLoadScaleFactor() const CK_PURE;


//-------------------------------------------

/****************************************************************
Summary: Sets the position of the referential.

Remarks:
	This methods the referential of the cloud without actually moving the points
themselves. It is different from the CK3dEntity::SetPosition method which moves
the whole cloud.
*****************************************************************/
virtual void SetReferentialPosition(const VxVector& iPosition,CK3dEntity* iRef = NULL) CK_PURE;

/****************************************************************
Summary: Returns an iterator on the root of the tree.

See Also: CloudIterator,SelectSphere,SelectHalfSpace,SelectCylinder,RayPick
*****************************************************************/
virtual CloudIterator* GetRootIterator() CK_PURE;


/****************************************************************
Summary: Returns an iterator on the root of the tree.

See Also: CloudIterator,SelectSphere,SelectHalfSpace,SelectCylinder,RayPick
*****************************************************************/
virtual CloudSelection* CreateSelection(const XString& iName, int iMaxSize = 250000) CK_PURE;

virtual int				GetSelectionCount() const CK_PURE;
virtual CloudSelection* GetSelection(int iIndex) const CK_PURE;
virtual CloudSelection* GetSelection(const XString& iName) const CK_PURE;

virtual void			RemoveSelection(CloudSelection* iSelection) CK_PURE;
virtual void			RemoveSelection(const XString& iName) CK_PURE;


/****************************************************************
Summary: Selects a list of points within a box.

Arguments:
	iBox: Axis aligned selection box given in the cloud local space.
	iOBox: Oriented selection box given in the cloud local space.
	iSelection: Index of the selection which should contain .
Remarks:
	This method returns an iterator which can then be used to acces all the points 
that are inside the given box or NULL if no such point exists.
See Also: CloudIterator,SelectSphere,SelectHalfSpace,SelectCylinder,RayPick
*****************************************************************/
virtual int SelectPoint(CKRenderContext* iContext,const Vx2DVector& iScreenPosition,CloudSelection* iSelection, BOOL iClearSelection = TRUE) CK_PURE;

/****************************************************************
Summary: Selects a list of points within a box.

Arguments:
	iBox: Axis aligned selection box given in the cloud local space.
	iOBox: Oriented selection box given in the cloud local space.
	iSelection: Pointer on the CloudSelection in which the result should be stored. This selection must have been previously created with the CreateSelection Method
Remarks:
	This method returns the number of points that were put in the selection.
See Also: CloudIterator,SelectSphere,RayPick
*****************************************************************/
virtual int SelectBox(const VxBbox& iBox,CloudSelection* iSelection, BOOL iClearSelection = TRUE) CK_PURE;
virtual int SelectBox(const VxOBB& iOBox,CloudSelection* iSelection, BOOL iClearSelection = TRUE) CK_PURE;

/****************************************************************
Summary: Selects a list of points within a sphere.

Arguments:
	iSphere: Selection sphere given in the cloud local space.
	iPutInRenderGroup: If >= 0 this value gives the render group in which the selected point should be put.	Default is -1, that is do not change the point render group.
Remarks:
	This method returns an iterator which can then be used to acces all the points 
that are inside the given sphere or NULL if no such point exists.
See Also: CloudIterator,SelectBox,SelectHalfSpace,SelectCylinder,RayPick
*****************************************************************/
virtual int SelectSphere(const VxSphere& iSphere,CloudSelection* iSelection, BOOL iClearSelection = TRUE) CK_PURE;

virtual int SelectCone(const VxCone& iCone,CloudSelection* iSelection, BOOL iClearSelection = TRUE) CK_PURE;


virtual int SelectConvexObject(CK3dEntity* iEnt,CloudSelection* iSelection, BOOL iClearSelection = TRUE) CK_PURE;
virtual int SelectConvexObjectGroup(CKGroup* iGroup,CloudSelection* iSelection, BOOL iClearSelection = TRUE) CK_PURE;
virtual int SelectConvexObjectGroup(const XObjectPointerArray& iObjects,CloudSelection* iSelection, BOOL iClearSelection = TRUE) CK_PURE;

//---------- Base Attributes -------

/****************************************************************
Summary: Returns if a normal is stored per point.

Remarks:
	This method is equivalent to calling IsCloudAttributeUsed(CLOUD_ATTRIBUTE_NORMAL).
	Normals must exist if lighting is to be enabled.
See Also: HasColors,UseNormals,GetPointNormal,IsCloudAttributeUsed
*****************************************************************/
virtual CKBOOL	  HasNormals() const CK_PURE;
/****************************************************************
Summary: Returns if a color is stored per point.

Remarks:
	This method is equivalent to calling IsCloudAttributeUsed(CLOUD_ATTRIBUTE_COLOR).
	If not color is stored per point, the default color is full white.
See Also: HasNormals,UseNormals,GetPointNormal,IsCloudAttributeUsed
*****************************************************************/
virtual CKBOOL	  HasColors() const CK_PURE;

/****************************************************************
Summary: Ensure memory is allocatd to store normals.

Remarks:
	This method is equivalent to calling UseCloudAttribute(CLOUD_ATTRIBUTE_NORMAL).
	This ensures that memory is allocated to store a normal for each point.
	A normal is compressed on 10 bits for efficiency purposes.
See Also: HasNormals,GetPointNormal,UseCloudAttribute
*****************************************************************/
virtual void	  UseNormals(CKBOOL iUseNormals) CK_PURE;

/****************************************************************
Summary: Ensure memory is allocatd to store colors.

Remarks:
	This method is equivalent to calling UseCloudAttribute(CLOUD_ATTRIBUTE_COLOR).
	This ensures that memory is allocated to store a color for each point.
	A color is stored internally as a 16 bit ARGB 1555 color.
See Also: HasColors,GetPointColor,UseCloudAttribute
*****************************************************************/
virtual void	  UseColors(CKBOOL iUseColors) CK_PURE;


/****************************************************************
Summary: Returns the color of a point in ARGB 8888 format.

Remarks:
	If there is no color stored in the cloud , the returned value is 0xFFFFFFFF.
Note: Colors are internally encoded in 16 bit.
See Also: HasColors,UseColor,SetPointColor,GetPointIntAttribute
*****************************************************************/
virtual DWORD	  GetPointColor(int iPointIndex) const CK_PURE;

/****************************************************************
Summary: Sets the color of a point in ARGB 8888 format.

Remarks:
If there is no color stored in the cloud , this function does nothing.
Modifying the color of a point can be a slow operation, if used inside a vertex buffer,
as the whole vertex buffer will be reloaded in video memory.
Note: Colors are internally encoded in 16 bit.

See Also: HasColors,UseColor,GetPointColor
*****************************************************************/
virtual void	  SetPointColor(int iPointIndex,DWORD iColor) CK_PURE;

/****************************************************************
Summary: Returns the normal of a point.

Remarks:
	If there is no normal stored in the cloud, the returned value is the vector (0,0,0) .
See Also: HasNormals,UseNormals
*****************************************************************/
virtual VxVector  GetPointNormal(int iPointIndex) const CK_PURE;

/****************************************************************
Summary: Sets the normal of a point.

Remarks:
If there is no normal stored in the cloud , this function does nothing.
Modifying the normal of a point can be a slow operation, if used inside a vertex buffer,
as the whole vertex buffer will be reloaded in video memory.

See Also: HasColors,UseColor
*****************************************************************/
virtual void	  SetPointNormal(int iPointIndex,const VxVector& iNormal) CK_PURE;

/****************************************************************
Summary: Returns the intensity of a point.

Remarks:
If there is no intensity stored in the cloud, the returned value is 0.
The intensity is a information stored on a byte (0..255) that was retrieved when reading 
a .asc file for example.
See Also: GetPointIntAttribute,SetPointIntensity
*****************************************************************/
virtual int			GetPointIntensity(int iPointIndex) const CK_PURE;

/****************************************************************
Summary: Sets the intensity of a point.

Remarks:
If there is no intensity stored in the cloud, the function does nothing.
Modifying the normal of a point can be a slow operation, if used inside a vertex buffer,
as the whole vertex buffer will be reloaded in video memory.
See Also: GetPointIntAttribute,GetPointIntensity
*****************************************************************/
virtual void		SetPointIntensity(int iPointIndex, BYTE iNewIntensity) CK_PURE;


//---------- User Attributes -------

/********************************************************************
Summary: Register a new type of attribute to use on the cloud. 

Arguments:
	iAttributeDesc: A CloudAttributeDescription that defines the attribute to create.
Remarks:
The CloudAttributeDescription structure identifies the type of data that will be stored (integer or float)
along with a description. 
An attribute can be stored on all points or only on a subset of points.

This function returns an index that can be later used to access data on points.
See Also:UnRegisterCloudAttribute,GetCloudAttributeIndexByName
*********************************************************************/
virtual int  	  RegisterCloudAttribute(const CloudAttributeDescription& iAttributeDesc) CK_PURE;

/********************************************************************
Summary: Removes an attribute type from the cloud. 

Arguments:
	iAttributeIndex: Index of the attribute to remove (Can be returned from a name with GetCloudAttributeIndexByName).
Remarks:
	The default attributes (Color,Normal,Intensity and render group) can not be removed.
Removing an attribute can change the indices that were attributed to all the user attribute.
See Also:RegisterCloudAttribute,GetCloudAttributeIndexByName
*********************************************************************/
virtual void  	  UnRegisterCloudAttribute(int iAttributeIndex) CK_PURE;

/********************************************************************
Summary: Returns an attribute type from the cloud given its name. 

Arguments:
	iName: Name of the attribute type which index should be returned.
Remarks:
See Also:RegisterCloudAttribute,UnRegisterCloudAttribute,GetCloudAttributeCount
*********************************************************************/
virtual int  	  GetCloudAttributeIndexByName(const XBaseString& iName)  const CK_PURE;

/********************************************************************
Summary: Returns the number of attribute types on the cloud. 

Remarks:
The default attributes (Color,Normal,Intensity and render group) can not be removed which means
there should be at least 4 default groups.
See Also:RegisterCloudAttribute,GetCloudAttributeIndexByName
*********************************************************************/
virtual int  	  GetCloudAttributeCount()  const CK_PURE;

/********************************************************************
Summary: Returns the description of an attribute type. 

Input Arguments:
	iAttributeIndex: Index of the attribute which description should be returned (Can be returned from a name with GetCloudAttributeIndexByName).
Remarks:
The 4 first attributes are the Color,Normal,Intensity and render group attributes.
The description of an attribute type gives the type of value stored on the points in the cloud. 
See Also:CloudAttributeDescription,SetCloudAttributeDescription,GetCloudAttributeIndexByName
*********************************************************************/
virtual const CloudAttributeDescription&   GetCloudAttributeDescription(int iAttributeIndex)  const CK_PURE;

/********************************************************************
Summary: Sets the description of an attribute type. 

Input Arguments:
	iAttributeIndex: Index of the attribute which description should be returned (Can be returned from a name with GetCloudAttributeIndexByName).
Remarks:
The 4 first attributes are the Color,Normal,Intensity and render group attributes.
The description of an attribute type gives the type of value stored on the points in the cloud.
See Also:CloudAttributeDescription,GetCloudAttributeDescription,GetCloudAttributeIndexByName
*********************************************************************/
virtual void	  SetCloudAttributeDescription(int iAttributeIndex,const CloudAttributeDescription& iDescription) CK_PURE;

/***********************************************************************
Summary: Returns whether a attribute type is used.

Remarks:
	Having an attribute defined on a cloud does not mean it is used. An attribute type must be marked 
as used with this method before points can hold data for this attribute.
Warning: When an attribute is marked as "non used" all the memory that was
used to store the point data is released, therefore switching back and forth to "used" - "not used"
will not keep the value initially stored on the points.
See Also:UseCloudAttribute
***********************************************************************/
virtual CKBOOL	  IsCloudAttributeUsed(int iAttributeIndex)  const CK_PURE;

/***********************************************************************
Summary: Sets the usage of a given attribute type.

Remarks:
	Having an attribute defined on a cloud does not mean it is used. An attribute type must be marked 
as used with this method before points can hold data for this attribute.
Warning: When an attribute is marked as "non used" all the memory that was
used to store the point data is released, therefore switching back and forth to "used" - "not used"
will not keep the value initially stored on the points.
See Also:IsCloudAttributeUsed
***********************************************************************/
virtual void	  UseCloudAttribute(int iAttributeIndex,BOOL iUse) CK_PURE;


/********************************************************************
Summary: Returns whether the values of an attribute type will be inside the vertex buffer.  

Input Arguments:
	iAttributeIndex: Index of the attribute for which we want to known if values will be present in VB.
Remarks:
See Also:CloudAttributeDescription,SetCloudAttributeInVB
*********************************************************************/
virtual CKBOOL   IsCloudAttributeInVB(int iAttributeIndex)  const CK_PURE;

/********************************************************************
Summary: Sets whether the values of an attribute type should be put inside the vertex buffer.  

Input Arguments:
	iAttributeIndex: Index of the attribute which VB presence should be modified.
	iPutInVB: TRUE if attribute values should uploaded in the vertex buffer , FALSE otherwise
Remarks:
See Also:CloudAttributeDescription,IsCloudAttributeInVB
*********************************************************************/
virtual void	  SetCloudAttributeInVB(int iAttributeIndex,CKBOOL iPutInVB) CK_PURE;


/***********************************************************************
Summary: Gets the value of an attribute on a point (stored as an integer).

Input arguments:
	iPointIndex: Index of the point (0..GetPointsCount()).
	iAttributeIndex: Index of the attribute to get the value of.
Remarks:
This method should only be used if the value stored by the attribute
is an integer as described in the CloudAttributeDescription.
An attribute index can be retrieve from its name using GetCloudAttributeIndexByName.
See Also:SetAttributeOnPoint,SetPointIntAttribute,GetPointFloatAttribute,GetCloudAttributeIndexByName
***********************************************************************/
virtual int  	  GetPointIntAttribute(int iPointIndex,int iAttributeIndex)  const CK_PURE;

/***********************************************************************
Summary: Gets the value of an attribute on a point (stored as a float).

Input arguments:
	iPointIndex: Index of the point (0..GetPointsCount()).
	iAttributeIndex: Index of the attribute to get the value of.
Remarks:
This method should only be used if the value stored by the attribute
is a float as described in the CloudAttributeDescription.
An attribute index can be retrieve from its name using GetCloudAttributeIndexByName.
See Also:SetAttributeOnPoint,GetPointIntAttribute,SetPointFloatAttribute,GetCloudAttributeIndexByName
***********************************************************************/
virtual float  	  GetPointFloatAttribute(int iPointIndex,int iAttributeIndex)  const CK_PURE;

/***********************************************************************
Summary: Sets the value of an attribute on a point (stored as an integer).

Input arguments:
	iPointIndex: Index of the point (0..GetPointsCount()).
	iAttributeIndex: Index of the attribute to set the value of.
	iValue: integer Value
Remarks:
This method should only be used if the value stored by the attribute
is an integer as described in the CloudAttributeDescription.
An attribute index can be retrieve from its name using GetCloudAttributeIndexByName.
See Also:SetAttributeOnPoint,GetPointIntAttribute,GetPointFloatAttribute,GetCloudAttributeIndexByName
***********************************************************************/
virtual void  	  SetPointIntAttribute(int iPointIndex,int iAttributeIndex, int iValue) CK_PURE;

/***********************************************************************
Summary: Sets the value of an attribute on a point (stored as a float).

Input arguments:
	iPointIndex: Index of the point (0..GetPointsCount()). 
	iAttributeIndex: Index of the attribute to set the value of.
	iValue: float Value
Remarks:
This method should only be used if the value stored by the attribute
is a float as described in the CloudAttributeDescription.
An attribute index can be retrieve from its name using GetCloudAttributeIndexByName.
See Also:SetAttributeOnPoint,SetPointIntAttribute,GetPointFloatAttribute,GetCloudAttributeIndexByName
***********************************************************************/
virtual void  	  SetPointFloatAttribute(int iPointIndex,int iAttributeIndex, float iValue) CK_PURE;


virtual void  	  SetClipPlaneCount(int iClipPlaneCount) CK_PURE;
virtual int  	  GetClipPlaneCount() const CK_PURE;

virtual void  	  SetClipPlane(int iClipPlaneIndex,const VxPlane& iPlaneEquation) CK_PURE;
virtual void  	  GetClipPlane(int iClipPlaneIndex,VxPlane& oPlaneEquation) const CK_PURE;

virtual CKBOOL	  LoadAttributesFromTextFile(int iAttributeIndex,const char* iFileName,float iOffset = 0.0f, float iScale = 1.0f) CK_PURE;

/************************************************************
Summary: Create the cloud using a data array of points.

Arguments:
	iArray: A pointer to a CKDataArray that contain the points to load.
	iColumn1: Index of the column that contains the points to load.
	iColumn2: Optionnal index of the additionnal column when using 3 columns containing floats.
	iColumn3: Optionnal index of the additionnal column when using 3 columns containing floats.

Remarks:
This methods creates a new cloud using the data given in the input iArray.
Only point position (a vector) can be specified through this method, additionnal attributes
can be read later using LoadAttributesFromArray. 
The valid format for a data array is to contain either:
- a column containing strings (slower), 
- a column containing parameters of type vector.
- 3 columns containing floats.
Any other data types will produce an error.

See Also: CreateFromAscFile,LoadAttributesFromTextFile,LoadAttributesFromArray
*************************************************************/
virtual CKBOOL	  LoadAttributesFromArray(int iAttributeIndex,CKDataArray* iArray, int iColumn = 0,float iOffset = 0.0f, float iScale = 1.0f) CK_PURE;


/************************************************************
Summary: Create the cloud using a data array of points.

Arguments:
	iArray: A pointer to a CKDataArray that contain the points to load.
	iColumn1: Index of the column that contains the points to load.
	iColumn2: Optionnal index of the additionnal column when using 3 columns containing floats.
	iColumn3: Optionnal index of the additionnal column when using 3 columns containing floats.

Remarks:
This methods creates a new cloud using the data given in the input iArray.
Only point position (a vector) can be specified through this method, additionnal attributes
can be read later using LoadAttributesFromArray. 
The valid format for a data array is to contain either:
- a column containing strings (slower), 
- a column containing parameters of type vector.
- 3 columns containing floats.
Any other data types will produce an error.

See Also: CreateFromAscFile,LoadAttributesFromTextFile,LoadAttributesFromArray
*************************************************************/
virtual CKBOOL CreateFromArray(CKDataArray* iArray,int iColumn1 = 0,int iColumn2 = -1,int iColumn3 = -1) CK_PURE;

/************************************************************
Summary: Deletes all the data stored by the cloud.

Arguments:
	iKeepAttributes: Keeps the attributes definitions.
	iKeepSelection: Keeps the selections definitions.
Remarks:

See Also: CreateFromAscFile,CreateFromArray
*************************************************************/
virtual CKBOOL ClearAllData(CKBOOL iKeepAttributes = TRUE, CKBOOL iKeepSelection = TRUE) CK_PURE;


virtual void   SetLODPointCount(int iLod,int iPointCount) CK_PURE;

virtual int  GetLODPointCount(int iLod)  const CK_PURE;

/*************************************************
Summary: Dynamic cast operator. 
Arguments:
	iO: A pointer to a CKObject to cast.
Return Value:
	iO casted to the appropriate class or NULL if iO is not from the required class .
Example:
	  CKObject* Object;
	  CK3dPointCloud* anim = CK3dPointCloud::Cast(Object);
Remarks:

*************************************************/
static CK3dPointCloud* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_3DPOINTCLOUD)?(CK3dPointCloud*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif

#endif
