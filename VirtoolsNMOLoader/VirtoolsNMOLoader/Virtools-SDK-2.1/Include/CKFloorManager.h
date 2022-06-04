/*************************************************************************/
/*	File : CKFloorManager.h					 		 					 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKFLOORMANAGER_H
#define CKFLOORMANAGER_H "$Id:$"

#include "CKBaseManager.h"
#include "CKDefines.h"
#include "VxMath.h"

#define CKPGUID_FLOORGEOMETRY CKDEFINEGUID(0x7c1930a1, 0x42361a45)
#define CKPGUID_FLOOR         CKDEFINEGUID(0x7b447672, 0x5798572a)

/************************************************
Summary: Returned Information about a nearest floor

Remarks:
This structure will be filled when calling CKFloorManager::GetNearestFloors
with the details about floor entities that are below and above the requested position.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

        class CKFloorPoint {
        public:
            CK_ID		m_UpFloor;
            int			m_UpFaceIndex;
            VxVector	m_UpNormal;
            float		m_UpDistance;

            CK_ID		m_DownFloor;
            int			m_DownFaceIndex;
            VxVector	m_DownNormal;
            float		m_DownDistance;
        };

{html:</td></tr></table>}

See Also: CKFloorManager,CKFloorManager::GetNearestFloors
************************************************/
class CKFloorPoint
{
public:
    CKFloorPoint() : m_UpFloor(0), m_UpFaceIndex(-1), m_UpDistance(100000000.0f),
                     m_DownFloor(0), m_DownFaceIndex(-1), m_DownDistance(-100000000.0f){};

    void Clear()
    {
        m_UpFloor = 0;
        m_UpFaceIndex = -1;
        m_UpNormal.Set(0, 0, 0);
        m_UpDistance = 100000000.0f;
        m_DownFloor = 0;
        m_DownFaceIndex = -1;
        m_DownNormal.Set(0, 0, 0);
        m_DownDistance = -100000000.0f;
    }
    CK_ID m_UpFloor;	 // CK_ID of the 3dEntity above the point
    int m_UpFaceIndex;	 // Index of the face above the point.
    VxVector m_UpNormal; // Normal vector
    float m_UpDistance;	 // Distance from point to upper floor.

    CK_ID m_DownFloor;	   // CK_ID of the 3dEntity below the point
    int m_DownFaceIndex;   // Index of the face below the point.
    VxVector m_DownNormal; // Normal vector
    float m_DownDistance;  // Distance from point to lower floor.
};

/*************************************************
{filename:CKFloorManager}
Summary: Floors management


Remarks:

+ The floor manager will keep the count of the floor objects, marked with the Floor attribute, or registered
directly to the FloorManager. These floors will be used for intersection tests, at a precision level depending
on their attribute, wheen looking for the floor under an object or a character, so that it can follow the landscape.

+ The unique instance of CKFloorManager can be retrieved by calling CKContext::GetManagerByGUID(FLOOR_MANAGER_GUID).

See also: CKCollisionManager, CKAttributeManager
*************************************************/
class CKFloorManager : public CKBaseManager
{
public:
    CKFloorManager(CKContext *Context, char *name) : CKBaseManager(Context, FLOOR_MANAGER_GUID, name) {}

    virtual ~CKFloorManager() {}

    /************************************************
    Summary: Returns the nearest floors (up floor and down floor, if availables).

    Arguments:
        iPosition: a vector in world space to be tested.
        oFP:	a pointer on an existing CKFloorPoint structure.
        iExcludeFloor: if given, floor to exclude from the detection
    Return Value:
        CKFLOOR_NOFLOOR if no floor is available,
        CKFLOOR_DOWN if the down floor is the nearest (or only)
        CKFLOOR_UP if the up floor is the nearest (or only)
    Remarks:

    +The structure is filled with the nearest floor downward the point and the nearest floor upward the point. The distance from the intersection point,
    the index of the face and its normal (in the floor referential) are also given.

    +Faces with an angle greater than the angle limit will be rejected.

    +The test for floors will be done on the Y world axis, so you
    cannot for example use this function for doing a looping in a
    rollercoaster for example.

    See Also:CKFloorPoint
    ************************************************/
    virtual CK_FLOORNEAREST GetNearestFloors(const VxVector &iPosition, CKFloorPoint *oFP, CK3dEntity *iExcludeFloor = NULL) = 0;

    /************************************************
    Summary: Constrain a point in the edges of the current floors, with a given radius.

    Arguments:
        iPosition: a vector in world space to be tested.
        iRadius: float describing the radius of the object to constrain
        oPosition: the new position of the vector, in world space, if constraint has been done.
        iExcludeAttribute: attribute that will exclude a found floor. from the process
    Return Value:
        TRUE if the position has been constrained, FALSE otherwise,

    Remarks:

    See Also:GetNearestFloors
    ************************************************/
    virtual CKBOOL ConstrainToFloor(const VxVector &iPosition, float iRadius, VxVector *oPosition, CKAttributeType iExcludeAttribute = -1) = 0;

    /************************************************
    Summary: Sets/Gets the angle above which faces are not considered as floors.

    Arguments:
        angle: angle in radian, between 0 and PI/2
    Remarks:
        By default, this angle is set to PI/4
    ************************************************/
    virtual void SetLimitAngle(float angle) = 0;

    virtual float GetLimitAngle() = 0;

    /************************************************
    Summary: Declares objects as floors, based on their name.

    Arguments:
        level: level containg the objects to parse.
        floorname: string to be seach in the object name.
        geo: geometry precision of the floor (CKFLOOR_FACES or CKFLOOR_BOX).
        moving: will the floor be moving or not.
        type: integer provided by the user to "type" the floors.
        hiera: should the hierarchy of the floor considered too.
        first: should the intersection test at the first contact (asuming that
        the floor faces does not overlap in the Y axis).
    Return Value:
        count of the declared floors.
    See Also:AddFloorObject,RemoveFloorObject,GetFloorObjectCount,GetFloorObject
    ************************************************/
    virtual int AddFloorObjectsByName(CKLevel *level, CKSTRING floorname, CK_FLOORGEOMETRY geo, CKBOOL moving, int type, CKBOOL hiera, CKBOOL first) = 0;

    /************************************************
    Summary: Declares an object as a floor.

    Arguments:
        ent: object to register as floor.
        geo: geometry precision of the floor (CKFLOOR_FACES or CKFLOOR_BOX).
        moving: will the floor be moving or not.
        type: integer provided by the user to "type" the floors.
        hiera: should the hierarchy of the floor considered too.
        first: should the intersection test at the first contact (assuming that
        the floor faces does not overlap in the Y axis).
    See Also:RemoveFloorObject,AddFloorObjectsByName,GetFloorObjectCount,GetFloorObject
    ************************************************/
    virtual void AddFloorObject(CK3dEntity *ent, CK_FLOORGEOMETRY geo, CKBOOL moving, int type, CKBOOL hiera, CKBOOL first) = 0;
    /************************************************
    Summary: Unregister a floor object.

    Arguments:
        ent: object to unregister as a floor.
    See Also:AddFloorObject,AddFloorObjectsByName,GetFloorObjectCount,GetFloorObject
    ************************************************/
    virtual void RemoveFloorObject(CK3dEntity *ent) = 0;
    /************************************************
    Summary: Returns the floor object count.

    Return Value:
        Number of floor objects.
    See Also:AddFloorObject,AddFloorObjectsByName,RemoveFloorObject,GetFloorObject
    ************************************************/
    virtual int GetFloorObjectCount() = 0;
    /************************************************
    Summary: Returns the floor object at the given index.

    Arguments:
        pos : index of the floor to obtain.
    Return Value:
        The floor at the given index.
    See Also:AddFloorObject,AddFloorObjectsByName,RemoveFloorObject,GetFloorObject
    ************************************************/
    virtual CK3dEntity *GetFloorObject(int pos) = 0;

    /*************************************************
    Summary: Returns the current threshold use by the cache system of the FloorManager

    Return Value:
        A floating point value representing the threshold in all the 2 axis, x and z.
    Remarks:
        Return the current threshold use by the cache system of the FloorManager. A cached point is
        reused if a newly asked point fell in the threshold proximity of it.
    See also: GetNearestFloors,
    *************************************************/
    virtual float GetCacheThreshold() = 0;
    /*************************************************
    Summary: Set the current threshold use by the cache system of the FloorManager

    Arguments:
        t: A floating point value representing the threshold in all the 2 axis, x and z.
    Remarks:
        Set the current threshold use by the cache system of the FloorManager. When a call to
        GetNearestFloor is performed, the position given is kept with the nearest face found from the
        nearest floor found. When GetNearestFloors is called again, if first test all the kept point,
        with a fixed threshold, to accelerate the detection when the object has not moved, or just a little.
    See also: FloorManager, FloorManager::GetNearestFloors
    *************************************************/
    virtual void SetCacheThreshold(float t) = 0;
    /*************************************************
    Summary: Gets the current number of positions kept by the cache.

    Return Value:
        The current number of positions kept by the cache.
    Remarks:
        Gets the current number of positions kept by the cache.
    See also: GetCacheThreshold,SetCacheThreshold,GetNearestFloors,SetCacheSize
    *************************************************/
    virtual int GetCacheSize() = 0;

    /*************************************************
    Summary: Sets the number of positions kept by the cache.

    Arguments:
        size: The current number of positions kept by the cache.
    Remarks:
        Sets the current number of positions kept by the cache.
        Warning : It clear the current cache as well...
    See also: GetCacheThreshold,SetCacheThreshold,GetNearestFloors,GetCacheSize
    *************************************************/
    virtual void SetCacheSize(int size) = 0;

    virtual CKBOOL ReadAttributeValues(CK3dEntity *ent, CKDWORD *geo = NULL, CKBOOL *moving = NULL, int *type = NULL, CKBOOL *hiera = NULL, CKBOOL *first = NULL) = 0;

    virtual int GetFloorAttribute() = 0;

    /************************************************
    Summary: Constrain a moving point in the edges of the current floors, with a given radius.

    Arguments:
        iOldPosition: the old position in world space to be tested.
        iPosition: the current position in world space to be tested.
        iRadius: float describing the radius of the object to constrain
        oPosition: the new position of the vector, in world space, if constraint has been done.
        iExcludeAttribute: attribute that will exclude a found floor. from the process
    Return Value:
        TRUE if the position has been constrained, FALSE otherwise,

    Remarks:

    See Also:GetNearestFloors
    ************************************************/
    virtual CKBOOL ConstrainToFloor(const VxVector &iOldPosition, const VxVector &iPosition, float iRadius, VxVector *oPosition, CKAttributeType iExcludeAttribute = -1) = 0;

    /************************************************
    Summary: Returns the nearest floor (up floor or down floor, if available).

    Arguments:
        iPosition: a vector in world space to be tested.
        oFP:	a pointer on an existing CKFloorPoint structure.
        iExcludeFloor: if given, floor to exclude from the detection
    Return Value:
        CKFLOOR_NOFLOOR if no floor is available,
        CKFLOOR_DOWN if the down floor is the nearest (or only)
        CKFLOOR_UP if the up floor is the nearest (or only)
    Remarks:

    +Faces with an angle greater than the angle limit will be rejected.

    +The test for floors will be done on the Y world axis, so you
    cannot for example use this function for doing a looping in a
    rollercoaster for example.

    ************************************************/
    virtual CK_FLOORNEAREST GetNearestFloor(const VxVector &iPosition, CK3dEntity **oFloor, int *oFaceIndex = NULL, VxVector *oNormal = NULL, float *oDistance = NULL, CK3dEntity *iExcludeFloor = NULL) = 0;
};

#endif
