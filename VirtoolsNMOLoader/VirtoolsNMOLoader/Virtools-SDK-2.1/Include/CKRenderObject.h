/*************************************************************************/
/*	File : CKRenderObject.h												 */
/*	Author :  Aymeric BARD												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKRenderObject_H
#define CKRenderObject_H "$Id:$"

#include "CKBeObject.h"

/***************************************************************************
{filename:CKRenderObject}
Summary: Base class for CKObjects that can be rendered in a CKRenderContext (3D and 2D)

Remarks:
    + This class presents the methods common to all objects that can be rendered onto a
    CKRenderContext (CK2dEntity and CK3dEntity).

    + Callback functions can be added to be called before,after and even instead of the
    default rendering function.

  See also: CK3dEntity, CKRenderContext,CK2dEntity
*******************************************************************************/
class CKRenderObject : public CKBeObject
{
public:
    /*************************************************
    Summary: Checks if this render object is referenced in a CKRenderContext.
    Arguments:
      context: A pointer to the context.
    Return Value: TRUE if this object is in context, FALSE otherwise.
    Remarks:
        + To be rendered when calling CKRenderContext::Render a 2D Entity or 3D Entity needs
        to be referenced by the render context. Objects referenced in a scene are automatically
        placed in a render context when the scene becomes active. But additionnal objects to be rendered
        but not present in the scene should be added to the render context with CKRenderContext::AddObject
        or rendered manually with CK3dEntity::Render or CK2dEntity::Render

    See also: CKRenderContext
    *************************************************/
    virtual CKBOOL IsInRenderContext(CKRenderContext *context) = 0;

    virtual CKBOOL IsRootObject() = 0;

    virtual CKBOOL IsToBeRendered() = 0;

    //------------------------------------------------
    // Z order Access

    /*************************************************
    Summary: Sets the Z order (priority) for this object.
    Remarks:
        + 2D and 3D entities are drawn separately and in a hierarchical order. The z order is a priority
        used to sort the rendering of objects at the same level in the hierarchy.
        + 2D Entities are drawn from minimum Z order to maximum Z order
        + On the contrary 3D Entities use the Z order a priority drawing object with a higher priority first
        + Valid range for the Z order is from -10000 to 10000
    *************************************************/
    virtual void SetZOrder(int Z) = 0;

    virtual int GetZOrder() = 0;

    virtual CKBOOL IsToBeRenderedLast() = 0;

    //-------------------------------------------------------
    // RENDERING CALLBACKS

    /*************************************************
    Summary: Adds a pre-render callback function.

    Arguments:
        Function: A function of type CK_RENDEROBJECT_CALLBACK which will be called before the object is rendereded.
        Argument: Argument that will be passed to the function.
        Temp : TRUE if the callback should be removed after being called.
    Return Value: TRUE if successful, FALSE otherwise.
    See also: CK_RENDEROBJECT_CALLBACK, RemovePreRenderCallBack
    *************************************************/
    virtual CKBOOL AddPreRenderCallBack(CK_RENDEROBJECT_CALLBACK Function, void *Argument, CKBOOL Temp = FALSE) = 0;

    /*************************************************
    Summary: Removes a pre-render callback function.

    Arguments:
        Function: Pre-render call back function to be removed.
        Argument: Argument that was passed to AddPreRenderCallBack.
    Return Value: TRUE if successful, FALSE otherwise.
    See also: CK_RENDEROBJECT_CALLBACK, AddPreRenderCallBack
    *************************************************/
    virtual CKBOOL RemovePreRenderCallBack(CK_RENDEROBJECT_CALLBACK Function, void *Argument) = 0;

    /*************************************************
    Summary: Sets the rendering function.

    Arguments:
        Function: A function of type CK_RENDEROBJECT_CALLBACK which will be called to render the object.
        Argument: Argument that will be passed to the function.
    Return Value: TRUE if successful, FALSE otherwise.
    See also: CK_RENDEROBJECT_CALLBACK, RemoveRenderCallBack
    *************************************************/
    virtual CKBOOL SetRenderCallBack(CK_RENDEROBJECT_CALLBACK Function, void *Argument) = 0;

    /*************************************************
    Summary: Removes the rendering function.

    Return Value: TRUE if successful, FALSE otherwise.
    Remarks:
        + This restores the default rendering method on this object.
    See also: CK_RENDEROBJECT_CALLBACK, AddRenderCallBack
    *************************************************/
    virtual CKBOOL RemoveRenderCallBack() = 0;

    /*************************************************
    Summary: Adds a post-render callback function.

    Arguments:
        Function: A function of type CK_RENDEROBJECT_CALLBACK which will be called after the object has been rendered.
        Argument: Argument that will be passed to the function.
        Temp : TRUE if the callback should be removed after being called.
    Return Value: TRUE if successful, FALSE otherwise.
    See also: CK_RENDEROBJECT_CALLBACK, RemovePostRenderCallBack
    *************************************************/
    virtual CKBOOL AddPostRenderCallBack(CK_RENDEROBJECT_CALLBACK Function, void *Argument, CKBOOL Temp = FALSE) = 0;

    /*************************************************
    Summary: Removes a post-render callback function.

    Arguments:
        Function: Post-render call back function to be removed.
        Argument: Argument that was passed to AddPostRenderCallBack.
    Return Value: TRUE if successful, FALSE otherwise.
    See also: CK_RENDEROBJECT_CALLBACK, AddPostRenderCallBack
    *************************************************/
    virtual CKBOOL RemovePostRenderCallBack(CK_RENDEROBJECT_CALLBACK Function, void *Argument) = 0;

    /*************************************************
    Summary: Removes all callbacks from CKRenderObject

    Remarks:
        + This methods removes all the existing call backs on this object.
    SeeAlso: SetRenderCallBack, AddPreRenderCallBack, AddPostRenderCallBack
    *************************************************/
    virtual void RemoveAllCallbacks() = 0;

    CKRenderObject() {}
    CKRenderObject(CKContext *Context, CKSTRING name = NULL) : CKBeObject(Context, name) {}

    // Dynamic Cast method (returns NULL if the object can't be casted)
    static CKRenderObject *Cast(CKObject *iO)
    {
        return CKIsChildClassOf(iO, CKCID_RENDEROBJECT) ? (CKRenderObject *)iO : NULL;
    }
};

#endif
