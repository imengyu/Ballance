/*************************************************************************/
/*	File : CKGridManager.h												 */
/*	Author :  Cabrita Francisco											 */
/*																		 */
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKGridManager_H

#define CKGridManager_H "$Id:$"


#define GRID_FLAG_SCENE 0

#define GRID_FLAG_LEVEL 1

#include "CKGrid.h"
#include "CKBaseManager.h"


#define CKPGUID_LAYERSQUARETYPE_ENUM			CKGUID(0x454d7d88,0x210e3f33)

#define CKPGUID_LINKERGRAPH_ENUM				CKGUID(0x16a90c1b,0x740d0f12)


#define DEFAULT_LAYER_NAME "- default -"

/*************************************************
Name: CKGridManager
Summary: Grids Management


Remarks:
+ The Grid Manager manages the layer types : they are defined by a name to which is associated an
integer (what is called the layer type). The layer type has also a type of parameter
that is associated with (SetAssociatedParam) most of the time the layers use integer values
(CKPGUID_INT).

+ For every grid you can then decide to create one of more layer on this grid using the
existing layer type.

{Image:Grid2}  

+ For Dev Interface purposes a layer type can also be given a color with SetAssociatedColor

+ The Grid Manager is the entry point to acces a grid given a custom point in Space.

+ The Grid Manager, manage all grids and is the gateway between a point in 3D Space and
the correspondant grid. The Grid Manager, also manages the Layer Types (eg: fire, danger, grass)

See also: CKGrid
*************************************************/
class CKGridManager :public CKBaseManager {
public:
  
  
  //____________________________________________________
  //      TYPE

/*************************************************
Summary: Returns the layer type given its name.

Arguments:
	TypeName: Name of the layer type to return (ex: "collision", "Walls", ...)
Return Value:
	Layer type integer value if successful, 0 otherwise.
Remarks:
	A 0 return value means the TypeName type was not registered yet.
See also: GetTypeName,SetTypeName,RegisterType
*************************************************/
  virtual int        GetTypeFromName(CKSTRING TypeName)=0;
/*************************************************
Summary: Returns the name of a layer type.

Arguments:
	type: Layer Type
Return Value:
	Name of the layer type or NULL if the layer type is invalid.
See also: GetTypeFromName,SetTypeName,RegisterType
*************************************************/
  virtual CKSTRING   GetTypeName(int type)=0;
/*************************************************
Summary: Changes the name of a layer type.

Arguments:
	iType: Layer Type
	Name: New name for the layer type.
Return Value:
	CK_OK if successful or an error code otherwise.
See also: GetTypeFromName,GetTypeName,RegisterType
*************************************************/
  virtual CKERROR    SetTypeName(int iType, CKSTRING Name)=0;
/*************************************************
Summary: Register a new layer type.

Arguments:
	TypeName: name for the layer type.
Return Value:
	Newly created layer type. 
See also: GetTypeFromName,GetTypeName,RegisterType,UnRegisterType,SetAssociatedParam,SetAssociatedColor
*************************************************/
  virtual int        RegisterType(CKSTRING TypeName)=0;

  virtual int        UnRegisterType(CKSTRING TypeName)=0;

/*************************************************
Summary: Sets the type of data associated with a layer type.

Input Arguments:
	type: Layer Type
	param: CKGUID of the parameter type associated with the layer type.
Return Value:
	CK_OK if successful or an error code otherwise (invalid layer type).
Remarks:
	Most of the time an integer data (CKPGUID_INT) is associated with layer types. Some exception
	such as the path finding use a Linker type (CKPGUID_LINKERGRAPH_ENUM) when linking grids together.
See also: GetTypeFromName,GetTypeName,RegisterType,SetAssociatedColor
*************************************************/
  virtual CKERROR    SetAssociatedParam(int type, CKGUID param)=0;
/*************************************************
Summary: Gets the type of data associated with a layer type.

Input Arguments:
	type: Layer Type
Return Value:
	CKGUID of the parameter type associated with the layer type.
Remarks:
	Most of the time an integer data (CKPGUID_INT) is associated with layer types. Some exception
	such as the path finding use a Linker type (CKPGUID_LINKERGRAPH_ENUM) when linking grids together.
See also: GetTypeFromName,GetTypeName,RegisterType,SetAssociatedParam
*************************************************/
  virtual CKGUID     GetAssociatedParam(int type)=0;

/*************************************************
Summary: Sets the color associated with a layer type.

Input Arguments:
	type: Layer Type
	col: Color to associate to this layer type.
Return Value:
	CK_OK if successful or an error code otherwise (invalid layer type).
Remarks:
	The color association is used in the Dev Interface to draw the layer data.
See also: GetTypeFromName,GetTypeName,RegisterType,GetAssociatedColor
*************************************************/
  virtual CKERROR    SetAssociatedColor(int type, VxColor *col)=0;
/*************************************************
Summary: Returns the color associated with a layer type.

Input Arguments:
	type: Layer Type
Output Arguments:
	col: Color associated with layer type.
Return Value:
	CK_OK if successful or an error code otherwise (invalid layer type).
Remarks:
	The color association is used in the Dev Interface to draw the layer data.
See also: GetTypeFromName,GetTypeName,RegisterType,SetAssociatedColor
*************************************************/
  virtual CKERROR    GetAssociatedColor(int type, VxColor *col)=0;
/*************************************************
Summary: Returns the number of registered layer types.

Return Value:
	Number of existing layer type. 
See also: GetTypeFromName,GetTypeName,RegisterType,UnRegisterType
*************************************************/
  virtual int		GetLayerTypeCount()=0;



  //____________________________________________________
  //      CLASSIFICATION

/*************************************************
Summary: Returns an attribute given a classification name

See also: GetClassificationName,RegisterClassification,GetGridClassificationCatego,CKAttributeManager
*************************************************/
  virtual int        GetClassificationFromName(CKSTRING ClassificationName)=0;
/*************************************************
Summary: Returns the classification name associated with an attribute

See also: GetClassificationFromName,RegisterClassification,GetGridClassificationCatego,CKAttributeManager
*************************************************/
  virtual CKSTRING   GetClassificationName(int Classification)=0;
/*************************************************
Summary: Register a new classification name (new attribute)

See also: GetClassificationFromName,GetClassificationName,GetGridClassificationCatego,CKAttributeManager
*************************************************/
  virtual int		RegisterClassification(CKSTRING ClassificationName)=0;
/*************************************************
Summary: Gets the attribute category used by the grid manager

See also: GetClassificationFromName,GetClassificationName,RegisterClassification,CKAttributeManager
*************************************************/
  virtual int		GetGridClassificationCatego()=0;

  //____________________________________________________
  //      GRID FINDING

/*************************************************
Summary: Gets the list of CKGrids

Arguments:
	flag: GRID_FLAG_SCENE to return the list of grids in the current scene or 
		GRID_FLAG_LEVEL to return the global list of grids.
Return Value:
  A XObjectPointerArray reference to the scene or global list of CKGrids. 
See also: GetPreferredGrid,IsIngrid
*************************************************/
  virtual const XObjectPointerArray&	GetGridArray( int flag=GRID_FLAG_SCENE )=0;

/*************************************************
Summary: Gets the nearest grid from a 3d position expressed in a referential

Arguments:
	pos: the 3d position expressed in the referential ref
	ref: the referential of the position (NULL by default = World)
Return Value:
  the nearest grid if successful, NULL if no grid was found.
See also: GetPreferredGrid,IsIngrid
*************************************************/
  virtual CKGrid     *GetNearestGrid( VxVector *pos, CK3dEntity *ref=NULL )=0;

/*************************************************
Summary: Gets the grid containing a point given its priority.

Arguments:
	pos: position to consider, expressed in the referential ref
	ref: the referential in which the pos are expressed (NULL by default = World)
Return Value: 
	The grid with the highest priority that contains the point pos.
See also:GetNearestGrid,IsIngrid
*************************************************/
  virtual CKGrid     *GetPreferredGrid( VxVector *pos, CK3dEntity *ref=NULL )=0; 
/*************************************************
Summary: Returns whether a given point is inside a grid range.

Arguments:
	grid: Grid in which the point should be tested.
	pos: position to consider, expressed in the referential ref
	ref: the referential in which the pos are expressed (NULL by default = World)
Return Value: 
	TRUE is pos is inside the grid range.
See also:GetNearestGrid,GetPreferredGrid
*************************************************/
  virtual CKBOOL     IsInGrid( CKGrid *grid, VxVector *pos, CK3dEntity *ref=NULL )=0;

 /*************************************************
Summary: Returns the number of grids

Input Arguments:
	flag: GRID_FLAG_SCENE to return the number of grid in the current scene or 
		GRID_FLAG_LEVEL to return the total number of grids.
Return Value:
	Number of grids
See also: GetGridObject
*************************************************/  
  virtual int GetGridObjectCount( int flag=GRID_FLAG_SCENE )=0;

/*************************************************
Summary: Returns the Nth grid in the scene or level. 

Input Arguments:
	pos: index of the grid to return.
	flag: GRID_FLAG_SCENE to return the number of grid in the current scene or 
		GRID_FLAG_LEVEL to return the total number of grids.
Return Value:
	A pointer to a CKGrid.
See also: GetGridObjectCount
*************************************************/
  virtual CKGrid* GetGridObject(int pos, int flag=GRID_FLAG_SCENE )=0;

  //____________________________________________________
  //      OTHER ...

/*************************************************
Summary: Fills grids with an object shape. 

Input Arguments:
	ent: Entity to use to fill grids.
	layerType: Layer type on which values should be written.
	fillVal: A pointer to the value to set in the layerType layer.
See also: CKLayer::SetValue
*************************************************/
  virtual void FillGridWithObjectShape (CK3dEntity *ent, int layerType, void *fillVal)=0;
  virtual void FillGridWithObjectShape (CK3dEntity *ent, int solidLayerType, int shapeLayerType, void *fillVal)=0;

  //____________________________________________________
  //	  LINKER GRAPH
  //virtual void InitLinkerIterator (int layerType, unsigned int flags, LinkerGraphIterator &it)=0;

#ifdef DOCJETDUMMY // Docjet secret macro
#else

  virtual void Init()=0;
  virtual CKERROR OnCKInit()=0; 
  virtual CKERROR PostClearAll()=0;
  virtual CKERROR PostLoad()=0;
  virtual CKERROR PreSave()=0;
  virtual CKERROR OnCKReset()=0;
  virtual CKERROR PreProcess()=0;

  virtual CKDWORD GetValidFunctionsMask()	{ return CKMANAGER_FUNC_PostClearAll|
													 CKMANAGER_FUNC_OnCKInit|
													 CKMANAGER_FUNC_PreSave|
													 CKMANAGER_FUNC_PostLoad|
													 CKMANAGER_FUNC_OnCKReset|
													 CKMANAGER_FUNC_PreProcess;
													                      }
  virtual CKERROR LoadData(CKStateChunk *chunk,CKFile* LoadedFile)=0;
  virtual CKStateChunk* SaveData(CKFile *SavedFile)=0;

//--------------------------------------------------------
////               Private Part                     


  
  CKGridManager(CKContext *Context,CKGUID guid,char* name);
  
  virtual ~CKGridManager() {}
	
  virtual void ClearData()=0;
	
  virtual void InitData()=0;

	
  int m_RemapCount;
	
  int *m_Remap; // Array for remapping of the layer type parameters  {secret}

#endif // Docjet secret macro
};

// CK2 VERSION ...
#endif
