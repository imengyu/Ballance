/*************************************************************************/
/*	File : CKGrid.h														 */
/*	Author :  Cabrita Francisco											 */
/*																		 */
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#if !defined(CKGRID_H) || defined(CK_3DIMPLEMENTATION)
#ifndef CK_3DIMPLEMENTATION

#define CKGRID_H "$Id:$"

#include "CK3dEntity.h"
#include "CKLayer.h"


#undef CK_PURE

#define CK_PURE = 0

/*************************************************
{filename:CKGrid}
Summary: Class for Grids

Remarks:
	+ Grids are a simple and useful way of adding and retrieving information in 
	the 3D scene. A grid is an array of cells which can contain different values.

	+ A grid is considered to have limit in height defined by SetHeightValidity below 
	which points are considered as belonging to the grid.
	
{Image:Grid}

	+ A grid can contains several layers that store information in a array of squares.

	+ The CKGridManager is used to declare the existing types of layers and to acces the grids in their globality.
	
{Image:Grid2}

	+ Methods can convert from a given world position to a specific square in the grid.

	+ Grids can have classification (kind of attribute) to help organize very complex scene.
	
	+ Priority can be set on grids especially when several grids overlap so the CKgridManager::GetPreferredGrid returns the desired grid when finding on which grid a point should be tested.

	+ The Class Identifier of a Grid is CKCID_GRID



See also: CKLayer,CKGridManager
*************************************************/
class CKGrid : public CK3dEntity {
public:
#endif

  //____________________________________________________
  //      CONSTRUCT MESH & TEXTURE
/*************************************************
Summary: Construct the mesh materials, and texture associated with the grid

Arguments:
	Opacity: should be bound to range [0,1]
Remarks:
+ This function is called during the grid creation by the interface. If you make your own
grid using the interface and want it to be visible in the interface you must call this function 
after the creation.
+ This function is called in IsVisible(TRUE) if the grid was previously not visible.
+ When the grid is not visible [ IsVisible(FALSE) ], then the mesh and texture
are destroyed
See also: DestroyMeshTexture, IsVisible
*************************************************/
  virtual  void    ConstructMeshTexture( float opacity=0.5f ) CK_PURE;

/*************************************************
Summary: Destroy the mesh, materials and texture associated with the grid

Remarks:
+ This function is called in IsVisible(FALSE) if the grid was previously visible.
+When the grid is not visible [ IsVisible(FALSE) ], then the mesh and texture
are destroyed
See also: ConstructMeshTexture, IsVisible
*************************************************/
  virtual  void    DestroyMeshTexture() CK_PURE;

  //____________________________________________________
  //      ACTIVATION
/*************************************************
Summary: Returns whether the grid is active or not

Return Value:
	TRUE if the grid is active, FALSE otherwise.
Remarks:
+ Returns if the grid is active or not.
+ The grid is active by default when created.
+ This function just read the CK_SCENEOBJECT_ACTIVE flag of the Grid. This flag, 
as any other scene object can be modified from the "Level View" or using the CKScene::ModifyObjectFlags method. 
+ An inactive grid won't be parsed by the Grid Manager
*************************************************/
  virtual  CKBOOL		IsActive() CK_PURE;

  //____________________________________________________
  //      HEIGHT VALIDITY
/************************************************
Summary: Sets the Height of Validity of the grid.

Arguments:
	HeightValidity: the height of validity used to define the grid thickness.
Remarks:
	Changing the Height of Validity only changes the Y axis scale of the grid.
See also: GetHeightValidity
************************************************/
  virtual  void    SetHeightValidity(float HeightValidity) CK_PURE;

/************************************************
Summary: Returns the grid Height Validity of the grid.


Return Value: 
	Height of Validity
Remarks:
	Just retrieves the Y axis scale of the grid.
See also: SetHeightValidity
************************************************/
  virtual  float   GetHeightValidity() CK_PURE;


/************************************************
Summary: Returns the grid Width

Return Value: 
	Width in number of squares
Remarks:
	Width = Number of squares along the X axis
See also: GetLength
************************************************/
  virtual  int      GetWidth() CK_PURE;

/************************************************
Summary: Returns the grid Length

Return Value: 
	Length in number of squares
Remarks:
	Length = Number of squares along the Z axis
See also: GetWidth
************************************************/
  virtual  int      GetLength() CK_PURE;
  
  //____________________________________________________
  //      DIMENSIONS
/************************************************
Summary: Sets the grid's dimensions

Arguments:
	width: Number of square along X axis
	length: Number of square along Z axis
	sizeX: total world size of the grid's width
	sizeY: total world size of the grid's length
Remarks: 
This function simply sets the grid's scale so that the grids bounding box (X,Z) 
has a total size of (sizeX,sizeY).
This DOES NOT change the number of square in the grid. To achieve this you must manually remove
all the layers from the grid using RemoveAllLayers (and eventually storing them in memory if information
should be kept) then call SetDimensions and AddLayer to recreate the layers.
See also: GetWidth, GetLength
************************************************/
  virtual void SetDimensions(int width, int length, float sizeX, float sizeY) CK_PURE;
  
  //____________________________________________________
  //      COORDINATES
/************************************************
Summary: Converts a 3d position into the corresponding 2d grid coordinates

Arguments:
	pos3d: adress of the 3d position vector (expressed in world coordinate system)
	x: adress of the integer to be filled with the x coordinate 
	y: adress of the integer to be filled with the y coordinate
Return Value:
	A float that describe the validity height of the given 3d position : Between 0 and 1 the point is in the height validity.
See also: Get3dPosFrom2dCoords
************************************************/
  virtual float Get2dCoordsFrom3dPos(const VxVector *pos3d, int *x, int *y) CK_PURE;

/************************************************
Summary: Convert a 2 coordinate position on the grid, into a 3d position in world coordinate system

Arguments:
	pos3d: adresse of the 3d position vector (expressed in world coordinate system)
	x: x (width) coordinate in the grid square array
	z: z (height) coordinate in the grid square array
Remarks:
	The 3d position is the position middle point of the square(x,y) in the grid.
	returned Y position = 0 (in grid referential)
See also: Get2dCoordsForm3dPos
************************************************/
  virtual void  Get3dPosFrom2dCoords(VxVector *pos3d, int x, int z) CK_PURE;
  
  //____________________________________________________
  //      CLASSIFICATION

/************************************************
Summary: Adds a classification to the grid

Arguments:
	classification: attribute.
	ClassificationName: name of the Classification (eg: "Dwarf Grid", "Hero Grid", ...).
Return Value: CK_OK if successful, ERROR code otherwise.
Remarks:
+ Classification are attribute types (see CKAttributeManager) used to distinguish special kinds of grids.
+ These attribute must have been create with the CKGridManager::RegisterClassification method which will create the attribute with the attribute manager.
See also: RemoveClassification,HasCompatibleClass
************************************************/
  virtual CKERROR AddClassification(int classification) CK_PURE;
  virtual CKERROR AddClassification(CKSTRING ClassificationName) CK_PURE;

/************************************************
Summary: Removes a classification from the grid


Arguments:
	classification: index in the Attribut Manager of a attribut. 
	ClassificationName: Name of the Classification (eg: "Dwarf Grid", "Hero Grid", ...).

Return Value: CK_OK if successful, ERROR code otherwise.
Remarks:
+ Classification are attribute types (see CKAttributeManager) used to distinguish special kinds of grids.
+ These attribute must have been create with the CKGridManager::RegisterClassification method which will create the attribute with the attribute manager.
See also: AddClassification,HasCompatibleClass
************************************************/
  virtual CKERROR RemoveClassification(int classification) CK_PURE;
  virtual CKERROR RemoveClassification(CKSTRING ClassificationName) CK_PURE;

/************************************************
Summary: Check if the grid has a shared Classification.

Arguments:
	ent: 3d Entity to be checked (can be a grid). 
Return Value: CK_OK if successful, ERROR code otherwise.
Remarks:
	Checks if the entity and the grid share the same classification (attribute).
See also: AddClassification,RemoveClassification
************************************************/
  virtual CKBOOL HasCompatibleClass(CK3dEntity *ent) CK_PURE;  

  //____________________________________________________
  //      PRIORITY

/************************************************
Summary: Sets the Priority to the grid

Arguments:
	Priority: the priority
Remarks:
	Priority can be set on grids especially when several grids 
overlap so the CKgridManager::GetPreferredGrid returns the desired grid
when finding on which grid a point should be tested.
See also: GetPriority
************************************************/
  virtual void SetGridPriority(int Priority) CK_PURE;

/************************************************
Summary: Gets the Priority of the grid

Return Value: The grid priority
Remarks:
	Priority can be set on grids especially when several grids 
overlap so the CKgridManager::GetPreferredGrid returns the desired grid
when finding on which grid a point should be tested.
See also: SetPriority
************************************************/
  virtual int  GetGridPriority() CK_PURE;
  
  //____________________________________________________
  //      ORIENTATION
/************************************************
Summary: Sets the grid's Orientation mode

Arguments:
	orimode: The orientation mode
Remarks:
	A grid can be forced to be aligned on specific axis.
See also: GetOrientationMode,CK_GRIDORIENTATION
************************************************/
  virtual void SetOrientationMode( CK_GRIDORIENTATION orimode ) CK_PURE;

/************************************************
Summary: Gets the grid's Orientation mode

Return Value: the orientation mode (CK_GRIDORIENTATION)
Remarks:
	A grid can be forced to be aligned on specific axis.
See also: SetOrientationMode
************************************************/
  virtual CK_GRIDORIENTATION   GetOrientationMode() CK_PURE;

  //____________________________________________________
  //      LAYERS
/************************************************
Summary: Adds a layer to the grid

Arguments:
	type: Layer type. 
	TypeName: Name of the layer type (eg: "collision", "roughness", ...). The name
	of the layer must have been registered with CKGridManager::RegisterType.
	Format: Layer Format (by default and only valid value : CKGRID_LAYER_FORMAT_NORMAL).
Return Value: A pointer to the newly created layer if successful, NULL otherwise (for example,
  if the layer type was not registred, or if a layer with the same type is already
  included in the grid)
Remarks: 
  + SetDimensions must have been called before using this method.
See also: GetLayer, GetLayerCount, RemoveLayer, RemoveAllLayers
************************************************/
  virtual CKLayer  *AddLayer(int type, int Format=CKGRID_LAYER_FORMAT_NORMAL) CK_PURE;
  virtual CKLayer  *AddLayer(CKSTRING TypeName=NULL, int Format=CKGRID_LAYER_FORMAT_NORMAL) CK_PURE;

/************************************************
Summary: Gets a layer by its type number, or by its type name

Arguments:
	type: Layer type
	TypeName: Layer type name
Return Value: A pointer to the wanted layer if successful, NULL otherwise
Remarks: 
See also: AddLayer, GetLayerCount, RemoveLayer, RemoveAllLayers
************************************************/
  virtual CKLayer  *GetLayer(int type) CK_PURE;
  virtual CKLayer  *GetLayer(CKSTRING TypeName) CK_PURE;

/************************************************
Summary: Gets the number of layers in the grid

Return Value: the grid's layers count
Remarks:
See also: GetLayer,GetLayerByIndex
************************************************/
  virtual int GetLayerCount() CK_PURE;

/************************************************
Summary: Gets a layer by its index in the layers of the grid

Arguments:
	index: The index in the layer array, starting at 0
Return Value: A pointer to the wanted layer if successful, NULL otherwise
Remarks: 
See also: GetLayer, GetLayerCount,RemoveLayer
************************************************/
  virtual CKLayer  *GetLayerByIndex(int type) CK_PURE;

/************************************************
Summary: Removes a layer by its type number, or by its type name

Arguments:
	type: Type of the layer to be deleted
	TypeName: Name of the type of the layer to be deletet
Return Value: CKERROR: CK_OK if successful , Error Code otherwise
Remarks: 
See also: RemoveAllLayers, AddLayer, GetLayer, GetLayerCount
************************************************/
  virtual CKERROR RemoveLayer(int type) CK_PURE;
  virtual CKERROR RemoveLayer(CKSTRING TypeName) CK_PURE;

/************************************************
Summary: Removes all the layers from the grid

Return Value: CKERROR: CK_OK if successful , Error Code otherwise
Remarks: 
See also: RemoveLayer, AddLayer, GetLayer, GetLayerCount
************************************************/
  virtual CKERROR RemoveAllLayers( void ) CK_PURE;

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
static CKGrid* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_GRID)?(CKGrid*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif
