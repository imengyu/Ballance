/*************************************************************************/
/*	File : CKBodyPart.h				 				 					 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#if !defined(CKBODYPART_H) || defined(CK_3DIMPLEMENTATION)

#define CKBODYPART_H "$Id:$"
#ifndef CK_3DIMPLEMENTATION

#include "CK3dObject.h"
#include "CKAnimation.h"


#define CKIKJOINTLIMIT(axe)		(CK_IKJOINT_LIMIT_X<<(axe-1))	

#define CKIKJOINTACTIVE(axe)	(CK_IKJOINT_ACTIVE_X<<(axe-1))	

#define CKIKJOINTEASE(axe)		(CK_IKJOINT_EASE_X<<(axe-1))	


typedef enum CK_IKJOINT_FLAGS {
	CK_IKJOINT_ACTIVE_X	= 0x001,
	CK_IKJOINT_ACTIVE_Y	= 0x002,
	CK_IKJOINT_ACTIVE_Z	= 0x004,
	CK_IKJOINT_ACTIVE	= 0x007,

	CK_IKJOINT_LIMIT_X	= 0x010,
	CK_IKJOINT_LIMIT_Y	= 0x020,
	CK_IKJOINT_LIMIT_Z	= 0x040,
	CK_IKJOINT_LIMIT	= 0x070,

	CK_IKJOINT_EASE_X	= 0x100,
	CK_IKJOINT_EASE_Y	= 0x200,
	CK_IKJOINT_EASE_Z	= 0x400,
	CK_IKJOINT_EASE		= 0x700
} CK_IKJOINT_FLAGS;




typedef struct CKIkJoint {
						DWORD		m_Flags;		// CK_IKJOINT_FLAGS
						VxVector	m_Min;			// Minimum range if rotations are limited
						VxVector	m_Max;			// Maximum range if rotations are limited
						VxVector	m_Damping;		// damping factor for each axis
} CKIkJoint;



#undef CK_PURE

#define CK_PURE = 0


/*************************************************
{filename:CKBodypart}
Name: CKBodyPart
Summary: Class for 3D objects being part of a character.

Remarks:
	+ The CKBodyPart class is designed for characters parts. In addition to the virtual
	base class CK3dObject it provides functions to retrieve the character a bodypart is part of.

	+ All the IK related methods (GetRotationJoint,FitToJoint) of the CKBodyPart class
	are now deprecated 

	+ The class id of CKBodyPart is CKCID_BODYPART.
See also: CK3dObject,CKCharacter::AddBodyPart()
*************************************************/
class CKBodyPart:public CK3dObject {
public:
#endif

/*************************************************
Summary: Returns the character to which this bodypart belongs to.
Return Value:
	A pointer to the owner CKCharacter.
See also: CKCharacter
*************************************************/
virtual CKCharacter* GetCharacter() const CK_PURE;

/*************************************************
Summary: Sets the only animation that can modify this bodypart.

Arguments:
	anim: A pointer to the new exclusive CKAnimation or NULL to remove any exclusive animation. 
Remarks:
	+ Since a character can have several animations playing at the same moment, some animations
	may refer to the same bodypart. This function forces one of these animations to be the only one 
	that can modify an object.
See Also:CKAnimation
*************************************************/
virtual void SetExclusiveAnimation(const CKAnimation *anim) CK_PURE;


virtual CKAnimation* GetExclusiveAnimation() const CK_PURE;


virtual void GetRotationJoint(CKIkJoint *rotjoint) const CK_PURE;

virtual void SetRotationJoint(const CKIkJoint *rotjoint) CK_PURE;

virtual CKERROR FitToJoint() CK_PURE;

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
static CKBodyPart* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_BODYPART)?(CKBodyPart*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif

