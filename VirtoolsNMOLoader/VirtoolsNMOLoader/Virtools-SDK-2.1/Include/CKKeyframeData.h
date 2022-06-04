/*************************************************************************/
/*	File : CKKeyframeData.h							 					 */
/*	Author :  Romain Sididris											 */
/*																		 */
/*	Definition of base Keyframe Key types and base animation controller	 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKKEYFRAMEDATA_H
#define CKKEYFRAMEDATA_H "$Id:$"

#include "CKDefines.h"

/*************************************************************************
Summary : Base class for animation keys.

Remarks:
This is the base class for all animation key. It contains only the time at which the key occurs.

See Also: CKObjectAnimation,CKAnimController,CKPositionKey,CKRotationKey,CKTCBPositionKey,CKTCBRotationKey
*************************************************************************/
class CKKey
{
public:
    // Summary:Returns the time for this key.
    float GetTime() { return TimeStep; }
    // Summary:Sets the time for this key.
    void SetTime(float t) { TimeStep = t; }

    CKKey(float Time = 0) { TimeStep = Time; }

public:
    float TimeStep; // Time of this key
};

//------------------- Linear Keys  ------------------------------//

/*************************************************************
Summary : Linear Rotation Key

Remarks:
This is the base class for all rotation keys. It contains only
a VxQuaternion with the rotation information.
See Also: CKKey,CKPositionKey,CKTCBRotationKey
*************************************************************/
class CKRotationKey : public CKKey
{
public:
    // Summary:Returns the rotation value for this key.
    const VxQuaternion &GetRotation() { return Rot; }
    // Summary:Sets the rotation value for this key.
    void SetRotation(const VxQuaternion &rot) { Rot = rot; }

    CKRotationKey() : CKKey(0) {}
    CKRotationKey(float Time, VxQuaternion &rot) : CKKey(Time) { Rot = rot; }

    CKBOOL Compare(CKRotationKey &key, float Threshold)
    {
        return ((XFabs(TimeStep - key.TimeStep) <= Threshold) &&
                (XFabs(Rot.x - key.Rot.x) <= Threshold) &&
                (XFabs(Rot.y - key.Rot.y) <= Threshold) &&
                (XFabs(Rot.z - key.Rot.z) <= Threshold) &&
                (XFabs(Rot.w - key.Rot.w) <= Threshold));
    }

public:
    VxQuaternion Rot; // Rotation info for this key.
};

/*************************************************************
Summary : Linear Position Key

Remarks:
This is the base class for all position keys. It contains only
a VxVector with the position information.
See Also: CKKey,CKRotationKey,CKTCBPositionKey,CKBezierPositionKey
*************************************************************/
class CKPositionKey : public CKKey
{
public:
    // Summary:Returns the position value for this key.
    const VxVector &GetPosition() { return Pos; }
    // Summary:Sets the position value for this key.
    void SetPosition(const VxVector &pos) { Pos = pos; }

    CKPositionKey() : CKKey(0) {}
    CKPositionKey(float Time, VxVector &pos) : CKKey(Time) { Pos = pos; }

    CKBOOL Compare(CKPositionKey &key, float Threshold)
    {
        return ((XFabs(TimeStep - key.TimeStep) <= Threshold) &&
                (XFabs(Pos.x - key.Pos.x) <= Threshold) &&
                (XFabs(Pos.y - key.Pos.y) <= Threshold) &&
                (XFabs(Pos.z - key.Pos.z) <= Threshold));
    }

public:
    VxVector Pos; // Position info for this key.
};

typedef CKPositionKey CKScaleKey;

typedef CKRotationKey CKScaleAxisKey;

//------------------- TCB  Keys  ------------------------------//

/*************************************************************
Summary : TCB Position Key

Remarks:
This class defines a TCB position key which contains
tension,continuity,bias for the key. easeto and easefrom
parameters defines the respectively incoming and outgoing accelerations.
See Also: CKKey,CKPositionKey,CKTCBRotationKey,CKBezierPositionKey
*************************************************************/
class CKTCBPositionKey : public CKPositionKey
{
public:
    CKTCBPositionKey() {}

    CKTCBPositionKey(float Time, VxVector &pos, float t = 0, float c = 0, float b = 0, float easeTo = 0, float easeFrom = 0) : CKPositionKey(Time, pos), tension(t), continuity(c), bias(b), easeto(easeTo), easefrom(easeFrom) {}

    CKBOOL Compare(CKTCBPositionKey &key, float Threshold)
    {
        if (tension != key.tension || continuity != key.continuity || bias != key.bias || easeto != key.easeto || easefrom != key.easefrom)
            return FALSE;
        return CKPositionKey::Compare(key, Threshold);
    }

public:
    float tension, continuity, bias;
    float easeto, easefrom;
};

/*************************************************************
Summary : TCB Rotation Key

Remarks:
This class defines a TCB rotation key which contains
tension,continuity,bias for the key. easeto and easefrom
parameters defines the respectively incoming and outgoing accelerations.
See Also: CKKey,CKRotationKey,CKTCBPositionKey,CKBezierPositionKey
*************************************************************/
class CKTCBRotationKey : public CKRotationKey
{
public:
    CKTCBRotationKey() {}
    CKTCBRotationKey(float Time, VxQuaternion &rot, float t = 0, float c = 0, float b = 0, float easeTo = 0, float easeFrom = 0) : CKRotationKey(Time, rot), tension(t), continuity(c), bias(b), easeto(easeTo), easefrom(easeFrom) {}

    CKBOOL Compare(CKTCBRotationKey &key, float Threshold)
    {
        if (tension != key.tension || continuity != key.continuity || bias != key.bias || easeto != key.easeto || easefrom != key.easefrom)
            return FALSE;
        return CKRotationKey::Compare(key, Threshold);
    }

public:
    float tension, continuity, bias;
    float easeto, easefrom;
};

typedef CKTCBPositionKey CKTCBScaleKey;

typedef CKTCBRotationKey CKTCBScaleAxisKey;

//------------------- Bezier  Keys  ------------------------------//

#define INTAN_MASK   0xFFFF
#define OUTTAN_MASK  0xFFFF0000L
#define OUTTAN_SHIFT 16

typedef enum CKBEZIERKEY_FLAGS
{
    BEZIER_KEY_AUTOSMOOTH = 0x0001, // compute smooth tangents
    BEZIER_KEY_LINEAR     = 0x0002, // linear to/from pt
    BEZIER_KEY_STEP       = 0x0004, // step to/from pt
    BEZIER_KEY_FAST       = 0x0008, // compute fast (2* (keyn-keyp)) tangent
    BEZIER_KEY_SLOW       = 0x0010, // compute slow (0) Tangent
    BEZIER_KEY_TANGENTS   = 0x0020, // Use tangent value
} CKBEZIERKEY_FLAGS;

class CKBezierKeyFlags
{
public:
    //
    CKBEZIERKEY_FLAGS GetInTangentMode() { return (CKBEZIERKEY_FLAGS)(m_Flags & INTAN_MASK); }
    CKBEZIERKEY_FLAGS GetOutTangentMode() { return (CKBEZIERKEY_FLAGS)((m_Flags & OUTTAN_MASK) >> OUTTAN_SHIFT); }
    void SetInTangentMode(CKBEZIERKEY_FLAGS f) { m_Flags = ((m_Flags & ~INTAN_MASK) | (f & INTAN_MASK)); }
    void SetOutTangentMode(CKBEZIERKEY_FLAGS f) { m_Flags = ((m_Flags & ~OUTTAN_MASK) | ((f << OUTTAN_SHIFT) & OUTTAN_MASK)); }

    friend int operator!=(const CKBezierKeyFlags &f1, const CKBezierKeyFlags &f2) { return f1.m_Flags != f2.m_Flags; }
    CKBezierKeyFlags() { m_Flags = 0; }

protected:
    CKDWORD m_Flags;
};

/*************************************************************
Summary : Bezier Position Key

Remarks:
+ This class defines a Bezier position key which contains incoming and ougoing tangent vectors for the key.
+ The key also contain a CKBezierKeyFlags which defines how the incoming and outgoing tangents are computed. For example if the incoming tangent mode is set to BEZIER_KEY_TANGENTS we will use the In vector as incoming tangent otherwise the incoming tangent will be automatically computed according to the given mode.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

            class CKBezierKeyFlags {
                CKBEZIERKEY_FLAGS GetInTangentMode();
                CKBEZIERKEY_FLAGS GetOutTangentMode();
                void SetInTangentMode(CKBEZIERKEY_FLAGS f);
                void SetOutTangentMode(CKBEZIERKEY_FLAGS f);
            }

            typedef enum CKBEZIERKEY_FLAGS {
                BEZIER_KEY_AUTOSMOOTH	= 0x0001,	// compute smooth tangents
                BEZIER_KEY_LINEAR		= 0x0002,	// linear to/from pt
                BEZIER_KEY_STEP			= 0x0004,	// step to/from pt
                BEZIER_KEY_FAST			= 0x0008,	// Fast Tangent
                BEZIER_KEY_SLOW			= 0x0010,	// 0 Tangent
                BEZIER_KEY_TANGENTS		= 0x0020,	// Use tangent value (In or Out)
            } CKBEZIERKEY_FLAGS;

{html:</td></tr></table>}

See Also: CKKey,CKPositionKey,CKTCBPositionKey
*************************************************************/
class CKBezierPositionKey : public CKPositionKey
{
public:
    // Summary: Returns the tangents computation mode.
    CKBezierKeyFlags &GetFlags() { return Flags; }
    CKBezierPositionKey()
    {
        Flags.SetInTangentMode(BEZIER_KEY_AUTOSMOOTH);
        Flags.SetOutTangentMode(BEZIER_KEY_AUTOSMOOTH);
    }
    CKBezierPositionKey(float Time, VxVector &pos, const CKBezierKeyFlags &flags, VxVector &in, VxVector &out) : CKPositionKey(Time, pos), Flags(flags), In(in), Out(out) {}
    CKBezierPositionKey(float Time, VxVector &pos, const CKBezierKeyFlags &flags) : CKPositionKey(Time, pos), Flags(flags) {}
    CKBezierPositionKey(float Time, VxVector &pos, VxVector &in, VxVector &out) : CKPositionKey(Time, pos), In(in), Out(out)
    {
        Flags.SetInTangentMode(BEZIER_KEY_TANGENTS);
        Flags.SetOutTangentMode(BEZIER_KEY_TANGENTS);
    }

    CKBOOL Compare(CKBezierPositionKey &key, float Threshold)
    {
        if (Flags != key.Flags)
            return FALSE;
        if ((Flags.GetInTangentMode() == BEZIER_KEY_TANGENTS) && (In != key.In))
            return FALSE;
        if ((Flags.GetOutTangentMode() == BEZIER_KEY_TANGENTS) && (Out != key.Out))
            return FALSE;
        return CKPositionKey::Compare(key, Threshold);
    }

public:
    CKBezierKeyFlags Flags; // Flags specifying how incoming and outgoing tangents are computed.
    VxVector In;            // Incoming tangent vector.
    VxVector Out;           // Outgoing tangent vector.
};

typedef CKBezierPositionKey CKBezierScaleKey;

//------------------ Morph Keys --------------------------------//

/*************************************************************
Summary : Morph Key

Remarks:

+ A Morph key contains a list of vertex for a given time but can also contain normal information.
+ If normal are also stored, they used a VxCompressedVector (1 DWORD) per normal.
+ The number of vertices in a key is stored by its owner CKMorphController.(CKMorphController::SetMorphVertexCount)

See Also: CKKey,CKRotationKey,CKTCBPositionKey,CKBezierPositionKey
*************************************************************/
class CKMorphKey : public CKKey
{
public:
    VxVector *PosArray;            // List of vertex positions
    VxCompressedVector *NormArray; // List of vertex normals

    CKBOOL Compare(CKMorphKey &key, int NbVertex, float Threshold)
    {
        if (XFabs(TimeStep - key.TimeStep) > Threshold)
            return FALSE;
        if (PosArray)
        {
            if (!key.PosArray)
                return FALSE;
            for (int i = 0; i < NbVertex; ++i)
            {
                if (PosArray[i] != key.PosArray[i])
                    return FALSE;
            }
        }
        if (NormArray)
        {
            if (!key.NormArray)
                return FALSE;
            for (int i = 0; i < NbVertex; ++i)
            {
                if (NormArray[i].xa != key.NormArray[i].xa)
                    return FALSE;
                if (NormArray[i].ya != key.NormArray[i].ya)
                    return FALSE;
            }
        }
        return TRUE;
    }
};

/****************************************************************
Summary: List of availables animations controllers

Remarks:
+ CKANIMATION_CONTROLLER contains the list of available animation controllers.
+ It is used when creating or deleting a controller on a CKObjectAnimation.

When deleting a controller with CKObjectAnimation::DeleteController only the base type is used (CKANIMATION_CONTROLLER_MASK).

See Also : CKAnimController,CKObjectAnimation::CreateController,CKObjectAnimation::DeleteController
*****************************************************************/
typedef enum CKANIMATION_CONTROLLER
{
    CKANIMATION_CONTROLLER_POS     = 0x00000001,    // Base type mask for position controllers
    CKANIMATION_CONTROLLER_ROT     = 0x00000002,    // Base type mask for rotation controllers
    CKANIMATION_CONTROLLER_SCL     = 0x00000004,    // Base type mask for scale controller
    CKANIMATION_CONTROLLER_SCLAXIS = 0x00000008,    // Base type mask for off-axis scale controller (Shear)
    CKANIMATION_CONTROLLER_MORPH   = 0x00000010,    // Base type mask for morph controller
    CKANIMATION_CONTROLLER_MASK    = 0x000000FF,    // Mask for all controller types

    CKANIMATION_LINPOS_CONTROL     = 0x637c4301,    // LinearPosition Controller
    CKANIMATION_LINROT_CONTROL     = 0x49ed4002,    // Linear Rotation Controller
    CKANIMATION_LINSCL_CONTROL     = 0x654a3a04,    // Linear Scale Controller
    CKANIMATION_LINSCLAXIS_CONTROL = 0x2f200b08,    // Linear Off-Axis Scale Controller (Shear)

    CKANIMATION_TCBPOS_CONTROL     = 0x347e4a01,    // TCB Position Controller
    CKANIMATION_TCBROT_CONTROL     = 0x45b52a02,    // TCB Rotation Controller
    CKANIMATION_TCBSCL_CONTROL     = 0x1b545904,    // TCB Scale Controller
    CKANIMATION_TCBSCLAXIS_CONTROL = 0x32595908,    // TCB Off-Axis Scale Controller (Shear)

    CKANIMATION_BEZIERPOS_CONTROL  = 0x921ab801,    // Bezier Position Controller
    CKANIMATION_BEZIERSCL_CONTROL  = 0x18ab4404,    // Bezier Scale Controller

    CKANIMATION_MORPH_CONTROL      = 0x73847810     // Morph Controller
} CKANIMATION_CONTROLLER;

//--------- Base Animation Controller -------------------//

/**********************************************************
Summary : Base animation controller class.

Remarks:
+ All the animation controllers derives from this class. It provides the base methods to add,remove keys, save them to a memory buffer and perform the interpolation.
+ The interpolation methods are automatically called by the framework when using animations.

See Also:Animation Keys,CKMorphController
**********************************************************/
class CKAnimController
{
public:
    CKAnimController(CKDWORD Type) : m_Type(Type), m_NbKeys(0) {}

    virtual ~CKAnimController() {}

    /*****************************************************
    Summary: Evaluates the result of the controller at a given time.

    Arguments:
        TimeStep: Time at which the interpolation should be done.
        res: See Remarks.
    Return Value:
        TRUE if successful, FALSE otherwise (no keys for example).
    Remarks:
    According to the type of controller res is a pointer to:

    + A VxVector for position and scale controllers
    + A VxQuaternion for rotation and off axis scale controllers.
    + This method can not be called in this version for morph controllers (See CKMorphController)
    See Also:AddKey
    ******************************************************/
    virtual CKBOOL Evaluate(float TimeStep, void *res) = 0;

    /*****************************************************
    Summary: Adds a key to this controller.

    Arguments:
        key: A pointer to a CKKey (derived class) that contain the information.
    Return Value:
        Index of the generated key.
    Remarks:
        + The type of key depends on both the type of the controller (Position,Rotation,Scale,etc..) and the type
        of interpolation used (linear,TCB,Bezier).
        + The keys added by this method are automatically sorted by increasing time
        value so there is not a specific order in which keys should be added.
        + Since keys can be re-ordered when AddKey is called the returned index is
        only valid until next call to AddKey.
    See Also:Animation Keys,GetKey,GetKeyCount
    ******************************************************/
    virtual int AddKey(CKKey *key) = 0;
    /*****************************************************
    Summary: Retuns a given key

    Arguments:
        index: Index of the key to be returned.
    Remarks:
        + The type of returned key depends on both the type of the controller (Position,Rotation,Scale,etc..) and the type
        of interpolation used (linear,TCB,Bezier).
        + The returned pointer must not be deleted nor should its time value be modified
        but its content value (position,scale,tangents,etc...) can be changed.
    Return Value:
        A pointer to the 'index'th key.
    See Also:Animation Keys,GetKeyCount
    ******************************************************/
    virtual CKKey *GetKey(int index) = 0;
    /*****************************************************
    Summary: Removes a given key from the list.

    Arguments:
        index: Index of the key to be deleted.
    See Also:Animation Keys,AddKey,GetKeyCount
    ******************************************************/
    virtual void RemoveKey(int index) = 0;

    /*****************************************************
    Summary: Stores all controller data in a memory buffer.

    Arguments:
        Buffer: A pointer to the memory buffer where the controller data should be stored.
    Return Value:
        Size in bytes of the data written (or needed) to Buffer.
    Remarks:
    Call this method in two steps:

    + First with Buffer set to NULL, so that the method will return the needed size
    to allocate for buffer.
    + Then with a valid pointer to a memory buffer allocated with a least
    the returned size.
    See Also:ReadKeysFrom
    ******************************************************/
    virtual int DumpKeysTo(void *Buffer) = 0; // Pass NULL to get the size
                                              /*****************************************************
                                              Summary: Reads controller data from a memory buffer.
                                          
                                              Arguments:
                                                  Buffer: A pointer to the memory buffer that was previously written DumpKeysTo.
                                              Return Value:
                                                  Size in bytes of the data read from Buffer.
                                              See Also:DumpKeysTo
                                              ******************************************************/
    virtual int ReadKeysFrom(void *Buffer) = 0;

    virtual CKBOOL Compare(CKAnimController *control, float Threshold = 0.0f) = 0;

    virtual CKBOOL Clone(CKAnimController *control)
    {
        m_NbKeys = control->m_NbKeys;
        m_Length = control->m_Length;
        return TRUE;
    }

    // Summary: Returns the number of keys of this controller.
    //
    // See Also:GetKey
    int GetKeyCount() { return m_NbKeys; }
    // Summary: Returns the CKANIMATION_CONTROLLER type of this controller.
    CKDWORD GetType() { return m_Type; }

    // Summary: Sets the time length of this controller.
    void SetLength(float l) { m_Length = l; }
    // Summary: Returns the time length of this controller.
    float GetLength() { return m_Length; }

public:
    CKDWORD m_Type;
    int m_NbKeys;
    float m_Length;
};

//------------------ Morph Controller ---------------------------/

/**********************************************************
Summary : Morph controller class.

Remarks:
A Morph controller contains a list of morph keys (CKMorphKey)
and perfoms linear interpolation between these keys to
get the final result.

See Also:Animation Keys,CKAnimController
**********************************************************/
class CKMorphController : public CKAnimController
{
public:
    CKMorphController() : CKAnimController(CKANIMATION_MORPH_CONTROL) {}
    /*********************************************************
    Summary: Adds a morph key.

    Arguments:
        TimeStep: Time value for the key to generate.
        key: A pointer to a CKMorphKey that contains the vertex positions.
        AllocateNormals: TRUE if the new key should contain normal information.
    Return Value:
        Index of the generated key.
    See Also: CKMorphKey.
    ********************************************************/
    virtual int AddKey(float TimeStep, CKBOOL AllocateNormals) = 0;
    virtual int AddKey(CKKey *key, CKBOOL AllocateNormals) = 0;
    virtual int AddKey(CKKey *key) { return AddKey(key, TRUE); }

    virtual CKKey *GetKey(int index) = 0;

    virtual void RemoveKey(int index) = 0;

    virtual CKBOOL Evaluate(float TimeStep, void *res) = 0;
    /************************************************
    Summary: Evaluates the vertex position at a given time

    Arguments:
        TimeStep: Time value at which interpolation should be done.
        VertexPtr: A pointer to positions that will be filled.
        VertexStride: Amount in bytes between each position in VertexPtr.
        NormalPtr: A pointer to an array of VxCompressedVector that will be filled with the resulting normals.
    *************************************************/
    virtual CKBOOL Evaluate(float TimeStep, int VertexCount, void *VertexPtr, CKDWORD VertexStride, VxCompressedVector *NormalPtr) = 0;
    // Summary: Sets the number of vertices per key.
    virtual void SetMorphVertexCount(int count) = 0;
};

#endif