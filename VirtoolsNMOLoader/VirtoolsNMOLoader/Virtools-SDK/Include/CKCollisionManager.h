// CollisionManager.cpp : Defines the entry point for the DLL application.
//
#ifndef CKCOLLISION_MANAGER_H

#define CKCOLLISION_MANAGER_H

#include "CKBaseManager.h"

/************************************************
Name:ImpactDesc

Summary: Returned Information about a collision

Remarks:
This structure will be filled according to the specified CK_IMPACTINFO flags 
when calling CollisionManager::DetectCollision



See Also: CollisionManager,CollisionManager::DetectCollision
************************************************/
struct ImpactDesc {
	CK_ID	m_OwnerEntity;			// Child (if any) of the object tested that actually is in collision (can be the body part of a character)
	CK_ID	m_ObstacleTouched;		// Object with an attribute obstacle touched
	CK_ID	m_SubObstacleTouched;	// Child of the object touched that actually is in collision
	int		m_TouchedVertex;		// Nearest Vertex of the obstacle when the collision occurs
	int		m_TouchingVertex;		// Nearest Vertex of the tested object when the collision occurs
	int		m_TouchedFace;			// Nearest Face of the obstacle when the collision occurs
	int		m_TouchingFace;			// Nearest Face of the tested object when the collision occurs
	VxMatrix	m_ImpactWorldMatrix;// World Matrix of the tested object before the collision occurs (the matrix of the previous frame if it can't find anything better.)
	VxVector	m_ImpactPoint;		// Not Used Yet
	VxVector	m_ImpactNormal;		// Not Used Yet	
	CK_ID		m_Entity;			// object tested that actually is in collision
};

/*************************************************
{filename:CKCollisionManager}
Summary: Collision management

Remarks:

+For its main part, the collision manager lies on the obstacle attributes. 
These are of two types :

	+ Fixed Obstacle: defining the obstacle which not move from one frame to another
	+ Moving Obstacle: defining the objects susceptible to move during the processing

For these two attributes, You can precise the geometry precision 
(for now, only Bounding Box and Faces) and if you want to take into account
the children of the obstacle, the hierarchy flags must be checked.

+You can add this attributes the normal way, by adding the beobject the attribute
or by using the devoted collision manager functions : AddObstacle, AddObstacleByNames
and remove them the same way.
You can also iterate among the obstacles with the GetObstacle(fixed/moving)() functions.

+The main function of the Collision Manager is the DetectCollision()
The goal of this function is to find if the given object (which must be marked
as an obstacle) is in collision with another obstacle. It stops with the first obstacle found
and can provide several impact information if the user wants them.
The ImpactDesc is as follow :

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}
  
		  typedef struct {
			CK_ID	m_OwnerEntity;  // Child of the object tested or the object itself (can be the body part of a character)
			CK_ID	m_EntityTouched; // Precise entity touched, a child of one obstacle, or the obstacle itself (can be the body part of a character)
			CK_ID	m_ObstacleTouched; // Object with an attribute obstacle touched
			int		m_TouchedVertex;	// NOT USED YET
			int		m_TouchingVertex;	// NOT USED YET
			int		m_TouchedFace;		// NOT USED YET
			int		m_TouchingFace;		// NOT USED YET
			VxMatrix m_ImpactWorldMatrix; // a World Matrix describing the position of the tested object before it touched an obstacle
			VxVector m_ImpactPoint; // NOT USED YET
			VxVector m_ImpactNormal; // NOT USED YET
		} ImpactDesc;

{html:</td></tr></table>}

	+ The collision manager keeps all the objects marked as obstacle in sorted arrays
which allow it to find quickly which objects are overlapping with another one.
The DetectCollision() method use this functionnality to restrict the number
of complex intersection tests to perform. 
  
	+ One of the other functions set provided by the collision manager is the RayIntersection set.
These are three functions :
RayIntersection() testing the ray with all the obstacles
FixedRayIntersection() testing the ray with only the fixed obstacles
MovingRayIntersection() testing the ray with only the moving obstacles

	+ The collision manager gives also access to intersection tests between two entities,
at different precision level. These functions are :

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

			CKBOOL BoxBoxIntersection(ent1,ent2);
			CKBOOL BoxFaceIntersection(ent1,ent2);
			CKBOOL FaceFaceIntersection(ent1,ent2);

{html:</td></tr></table>}

These three functions operate at single entity level. For hierarchical detection, use :

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

		IsInCollisionWithHierarchy()
		IsHierarchyInCollisionWithHierarchy()

{html:</td></tr></table>}

which operate on hierarchy. The precision level is given as argument 
(for now, only CKCOLLISION_BOX and CKCOLLISION_FACE)

Finally, the collision manager gives basic geometric tests functions such as :

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

			FaceFaceIntersection();
			BoxBoxIntersection();

{html:</td></tr></table>}

The unique instance of CollisionManager can be retrieved by calling CKContext::GetManagerByGUID(COLLISION_MANAGER_GUID).

See also: CKFloorManager, CKAttributeManager
*************************************************/
class CKCollisionManager :public CKBaseManager {
public :
	
	CKCollisionManager(CKContext *Context, char* name):CKBaseManager(Context,COLLISION_MANAGER_GUID,name) {}
	
	virtual ~CKCollisionManager() {}
	
	//------------------------------------------------------------
	// Obstacles Management

/*************************************************
Summary: Declare a 3dEntity as an obstacle for the collision manager

Arguments:
	ent: the 3dEntity to declare as an obstacle
	moving: whether or not the declared 3dEntity will move during the processing
	precision: the geometric precision we want to use when considering this entity during the collision test
	hiera: whether or not we want to consider the entire hierarchy of entity
Remarks:
	The implementation of this method is in fact to set the FixedObstacle or MovingObstacle attribute, depending on 
	the value of the moving parameter. It is provided for ease of use in declaring obstacles.
See also: AddObstaclesByName,RemoveObstacle,RemoveAllObstacles,IsObstacle
*************************************************/
	virtual void AddObstacle(CK3dEntity *ent,CKBOOL moving=FALSE,CK_GEOMETRICPRECISION precision=CKCOLLISION_BOX,CKBOOL hiera=FALSE) = 0;
/*************************************************
Summary: Declare as obstacles every 3dEntity whose name contains a certain substring

Arguments:
	level: the level in which the manager will search the object to define
	substring: substring to search in the 3dEntities name (case sensitive)
	moving: whether or not the declared 3dEntities will move during the processing
	precision: the geometric precision we want to use for defining these entities during the collision test
	hiera: whether or not we want to consider the entire hierarchy of all the entities
Return Value: 
	Number of object defined as obstacles
Remarks:
	This method calls the AddObstacle methodfor each 3dEntity in level that match the substring parameter.
See also: AddObstacle,RemoveObstacle,RemoveAllObstacles,IsObstacle
*************************************************/
	virtual int	AddObstaclesByName(CKLevel* level,CKSTRING substring,CKBOOL moving=FALSE,CK_GEOMETRICPRECISION precision=CKCOLLISION_BOX,CKBOOL hiera=FALSE) = 0;

/*************************************************
Summary: Remove a 3dEntity from the collision manager

Arguments:
	ent: the 3dEntity to remove as an obstacle
Remarks:
	The implementation of this method is in fact to remove the FixedObstacle or MovingObstacle attribute if it was
	present, otherwise it does nothing.
See also: AddObstacle,AddObstaclesByName,RemoveAllObstacles,IsObstacle
*************************************************/
	virtual void RemoveObstacle(CK3dEntity *ent) = 0;
	
/*************************************************
Summary: Remove all the entities declared so far as obstacles from the collision manager

Arguments:
	level: whether only the current scene should be considered (FALSE) or the entire level
See also: AddObstacle,AddObstaclesByName,RemoveObstacle,IsObstacle
*************************************************/
	virtual void RemoveAllObstacles(CKBOOL level=TRUE) = 0;
	
/*************************************************
Summary: Remove all the entities declared so far as obstacles from the collision manager

Arguments:
	level: whether only the current scene should be considered (FALSE) or the entire level
Return Value:
	TRUE if ent is an obstacle, FALSE otherwise
See also: AddObstacle,AddObstaclesByName,RemoveObstacle,IsObstacle
*************************************************/
	virtual CKBOOL IsObstacle(CK3dEntity *ent,CKBOOL moving=FALSE) = 0;
	
	//------------------------------------------------------------
	// Obstacles Access
	
/*************************************************
Summary: Return the total numbers of objects declared as fixed obstacles

Arguments:
	level: whether or not you want to consider the obstacles of the entire level or just the ones from the current scene
Return Value:
	The total count of fixed obstacles.
See also: GetFixedObstacle,GetMovingObstacleCount
*************************************************/
	virtual int GetFixedObstacleCount(CKBOOL level=FALSE) = 0;
/*************************************************
Summary: Return the 'pos'th object declared as fixed obstacle

Arguments:
	pos: The index of the obstacle to return.
	level: whether or not you want to consider the obstacles of the entire level or just the ones from the current scene
Return Value:
	The 'pos'th fixed obstacle or NULL if it doesn't exist
See also: GetFixedObstacleCount
*************************************************/
	virtual CK3dEntity* GetFixedObstacle(int pos,CKBOOL level=FALSE) = 0;
	
/*************************************************
Summary: Return the total numbers of objects declared as moving obstacles

Arguments:
	level: whether or not you want to consider the obstacles of the entire level or just the ones from the current scene
Return Value:
	The total count of moving obstacles.
See also: GetMovingObstacle,GetFixedObstacleCount
*************************************************/
	virtual int GetMovingObstacleCount(CKBOOL level=FALSE) = 0;
/*************************************************
Summary: Return the 'pos'th object declared as moving obstacle

Arguments:
	pos: The index of the obstacle to return.
	level: whether or not you want to consider the obstacles of the entire level or just the ones from the current scene
Return Value:
	The 'pos'th moving obstacle or NULL if it doesn't exist
See also: GetMovingObstacleCount
*************************************************/
	virtual CK3dEntity* GetMovingObstacle(int pos,CKBOOL level=FALSE) = 0;
	
/*************************************************
Summary: Return the total numbers of objects declared as obstacles

Arguments:
	level: whether or not you want to consider the obstacles of the entire level or just the ones from the current scene
Return Value:
	The total count of obstacles whether fixed or moving.
See also: GetObstacle,GetMovingObstacle,GetMovingObstacleCount,GetFixedObstacle,GetFixedObstacleCount
*************************************************/
	virtual int GetObstacleCount(CKBOOL level=FALSE) = 0;

/*************************************************
Summary: Return the 'pos'th object declared as obstacle, wheter moving or not

Arguments:
	pos: The index of the obstacle to return.
	level: whether or not you want to consider the obstacles of the entire level or just the ones from the current scene
Return Value:
	The 'pos'th obstacle or NULL if it doesn't exist
See also: GetObstacleCount
*************************************************/
	virtual CK3dEntity* GetObstacle(int pos,CKBOOL level=FALSE) = 0;
	

	//----------------------------------------------------------------
	// Collision Detection functions
	//------------------------------
	// all these functions use the defined obstacles, fixed or moving
	// or the two at once if not precised in the fuction name
	
/*************************************************
Summary: Check if an entity is in collision with any of the declared obstacles, at the time the function is called.

Arguments:
	ent: the object to be checked (it needs to be defined as an obstacle, probably moving)
	precis_level: the geometric precision level you want t oforce for the tests. Use CKCOLLISION_NONE if you want the tests 
	to use the precision chosen for each obstacle. Otherwise, you can force the tests to be on bounding boxes for
	everyone by transmitting CKCOLLISION_BOX for example.
	replacementPrecision: an integer that describes the maximum number of tests to be executed to determine 
	the nearest safe position to the collision point, testing backwards to the starting point. (By safe 
	position, we mean a position with no collision at all. If it can not found anyone, the starting matrix will be given).
	detectionPrecision: an integer that describes the maximum number of tests to be executed to determine 
	if a collision occured starting with the starting point (before the behavioral process begins) and testing forward
	to the current point. The behavior stops testing at the first collision it encounters, then tests for a 
	safe position from that point.
	inmpactFlags: flags determining which information you want to be calculated and returned in the ImpactDesc structure.
	imp: pointer to an ImpactDesc structure that will be filled with all the information you asked, NULL if you don't need these info.
Return Value:
	TRUE if a collision occured, FALSE otherwise.
See also: ObstacleBetween
*************************************************/
	virtual CKBOOL DetectCollision(CK3dEntity *ent,CK_GEOMETRICPRECISION precis_level,int replacementPrecision,int detectionPrecision,CK_IMPACTINFO inmpactFlags,ImpactDesc* imp) = 0;
/*************************************************
Summary: Check if an entity, declared as an obstacle, is between two other obstacle objects.

Input Arguments:
	pos: first position
	endpos: second position
	width : width of the beam traced, added to the right and to the left of the base ray
	height : height of the beam traced, added to the top and to the bottom of the base ray
	iFace: FALSE to perform only bounding box tests , TRUE to perform face tests.
	iFirstContact: TRUE to stop the test at the first contact otherwise the contact nearest to pos is returned but it is slower.
	iIgnoreAlpha: if FALSE, face material and texture is taken into account and a test is perfom to check if the ray pass through a 
	color keyed pixel.
Output Arguments:
	oDesc: A structure that will be filled with detail about the intersection : position , face, etc...
Return Value:
	TRUE if an obstacle is found in between, FALSE otherwise
	or a 3DEntity if an obstacle is found, NULL otherwise, in the Ray version.
Remarks:
	The version with a width and an height will work with the bounding boxes
	of the obstacle whereas the pure ray version will work at the face level.
See also: DetectCollision
*************************************************/
	virtual CKBOOL ObstacleBetween(const VxVector& pos,const VxVector& endpos,float width,float height) = 0;
	
	//-------------------------------------------------------------
	// Advanced intersection test functions
	//----------------------------------
	// These functions test the static intersections between
	// two 3dEntity. They operate at a certain level of
	// geometry precision : Box-box, Box-Face and Face-Face
	
/*************************************************
Summary: Collision detection between two 3d entities (Box against Box)

Arguments:
	ent1: the first 3dEntity
	hiera1: whether or not to test the entire hierarchy of entity 1
	local1: do we want to test the local box (more precise, slower) or the world box (less precise, faster) of the first entity
	ent2: the second 3dEntity
	hiera2: whether or not we want to test the entire hierarchy of entity 2
	local2: do we want to test the local box(more precise, slower) or the world box (less precise, faster) of the second entity
Return Value: TRUE if they intersect, FALSE otherwise
See also: BoxFaceIntersection,FaceFaceIntersection
*************************************************/
	virtual CKBOOL BoxBoxIntersection(CK3dEntity* ent1,CKBOOL hiera1,CKBOOL local1,CK3dEntity* ent2,CKBOOL hiera2,CKBOOL local2) = 0;
/*************************************************
Summary: Collision detection between two 3d entities (Box against Faces)

Arguments:
	ent1: the first 3dEntity
	hiera1: whether or not to test the entire hierarchy of entity 1
	local1: do we want to test the local box(more precise, slower) or the world box (less precise, faster) of the first entity
	ent2: the second 3dEntity
Return Value: TRUE if they intersect, FALSE otherwise
See also: BoxBoxIntersection,FaceFaceIntersection
*************************************************/
	virtual CKBOOL BoxFaceIntersection(CK3dEntity* ent1,CKBOOL hiera1,CKBOOL local1,CK3dEntity* ent2) = 0;
/*************************************************
Summary: Collision detection between two 3d entities (Faces against Faces)

  Arguments:
	ent1: the first 3dEntity
	ent2: the second 3dEntity
Return Value: TRUE if they intersect, FALSE otherwise
See also: BoxBoxIntersection,BoxFaceIntersection
*************************************************/
	virtual CKBOOL FaceFaceIntersection(CK3dEntity* ent1,CK3dEntity* ent2) = 0;

/*************************************************
Summary: Check if two 3dEntities are in collision

Arguments:
	ent: first obstacle
	precis_level1: the geometric precision level you want to use for ent1.(CKCOLLISION_NONE is not a valid value)
	ent2: second obstacle
	precis_level2: the geometric precision level you want to use for ent2.(CKCOLLISION_NONE is not a valid value)
Return Value:
	TRUE if the two entites are colliding, FALSE otherwise.
Remarks:
	This method calls FaceFaceIntersection,BoxFaceIntersection or BoxBoxIntersection according
to given precision levels.
See also: IsInCollisionWithHierarchy,IsHierarchyInCollisionWithHierarchy
*************************************************/
	virtual CKBOOL IsInCollision(CK3dEntity *ent,CK_GEOMETRICPRECISION precis_level1,CK3dEntity *ent2,CK_GEOMETRICPRECISION precis_level2) = 0;
/*************************************************
Summary: Check if an 3dEntity is in collision with another and its hierarchy.

Arguments:
	ent: first obstacle
	precis_level1: the geometric precision level you want to use for ent1.(CKCOLLISION_NONE is not a valid value)
	ent2: second obstacle
	precis_level2: the geometric precision level you want to use for ent2 and its hierarchy.(CKCOLLISION_NONE is not a valid value)
Return Value:
	The pointer to the sub-object of entity 2 if the two entites are colliding, NULL otherwise.
Remarks:
	Check if two 3dEntities are in collision, the second one considered with all its sub-hierarchy. All the sub objects of 
	entity 2 are tested at the same level of precision : precis_level2
See also: IsInCollision,IsHierarchyInCollisionWithHierarchy
*************************************************/
	virtual CK3dEntity* IsInCollisionWithHierarchy(CK3dEntity *ent,CK_GEOMETRICPRECISION precis_level1,CK3dEntity *ent2,CK_GEOMETRICPRECISION precis_level2) = 0;
/*************************************************
Summary: Check if two hierarchies are in collision.

Arguments:
	ent: first obstacle
	precis_level1: the geometric precision level you want to use for ent1 and its hierarchy.(CKCOLLISION_NONE is not a valid value)
	ent2: second obstacle
	precis_level2: the geometric precision level you want to use for ent2 and its hierarchy.(CKCOLLISION_NONE is not a valid value)
	sub: A pointer to be filled with the children of ent1 that is colliding.
	subob: A pointer to be filled with the children of ent2 that is colliding.
Return Value:
	TRUE if the two entites are colliding, FALSE otherwise.
Remarks:
	Check if two 3dEntities are in collision, the two considered with all their sub-hierarchy.
See also: IsInCollision,IsHierarchyInCollisionWithHierarchy
*************************************************/
	virtual CKBOOL IsHierarchyInCollisionWithHierarchy(CK3dEntity *ent,CK_GEOMETRICPRECISION precis_level1,CK3dEntity *ent2,CK_GEOMETRICPRECISION precis_level2,CK3dEntity** sub, CK3dEntity** subob) = 0;

	virtual CK3dEntity* ObstacleBetween(const VxVector& pos,const VxVector& endpos,CKBOOL iFace = TRUE, CKBOOL iFirstContact = FALSE, CKBOOL iIgnoreAlpha = FALSE, VxIntersectionDesc* oDesc = NULL) = 0;
	
};
	
	
#endif