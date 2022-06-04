/*************************************************************************/
/*	File : CK2dEntity.h													 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#if !defined(CK2DENTITY_H) || defined(CK_3DIMPLEMENTATION)
#ifndef CK_3DIMPLEMENTATION

#define CK2DENTITY_H "$Id:$"

#include "CKBeObject.h"

#undef CK_PURE
#define CK_PURE = 0

/*************************************************
{filename:CK2dentity}
Name: CK2dentity


Summary: Base class for 2D objects.

Remarks:
    + This class provides the basic methods used in CKSprite and CKSpriteText
    to manipulate sprites : Position,Size, Visibility.

    + 2D Entities position and size can be given in two modes : Homogeneous coordinates (SetHomogeneousCoordinates)
    where the coordinates are in the range (0..1) in which 0 is the left corner of the screen and 1 is the right corner.
    This mode ensures that whatever the render context size is a sprite will have the same proportional position and size.
    The other mode is absolute (or screen) coordinates in which the coordinates are given in pixel.

    + 2d Entities are drawn according to their Z order (min Z first up to max Z).
    The Z order is only used to sort 2dentities among themselves not among 3dentities.

    + 2dEntities can be drawn before 3d entities if they are set as background otherwise
    they are drawn after 3dEntities.

    + 2dEntities can have material (and texture) associated to them to provide much more
    functionnalities than sprites but with the limitation of texture sizes (power of 2 dimensions).

    + The class id of CK2dEntity is CKCID_2DENTITY.

See also: CKSprite,CKSpriteText,Using 2D Entities
*************************************************/
class CK2dEntity : public CKRenderObject
{
public:
#endif

    /*************************************************
    Summary: Returns the position of 2d entity.
    Arguments:
        vect: Pointer to Vx2DVector for the entity coordinates, to be filled.
        hom: TRUE to return position in homogeneous coordinates, FALSE for screen coordinates.
        ref: Pointer to the reference 2d entity when screen coordinates are required.
    Return Value:
        CK_OK if successful or CKERR_INVALIDPARAMETER otherwise.
    Remarks:
        + For homogeneous coordinates, set hom to be TRUE and ref as NULL.
        + For screen cordinates, set hom to be FALSE and ref as pointer to the CK2dEntity (optionnaly).

    See Also: SetPosition,GetRect,GetSize
    *************************************************/
    virtual CKERROR GetPosition(Vx2DVector &vect, CKBOOL hom = FALSE, CK2dEntity *ref = NULL) CK_PURE;

    /*************************************************
    Summary: Sets the position of 2d entity.
    Arguments:
        vect: Pointer to Vx2DVector containing the entity coordinates
        hom: TRUE if given position is in homogeneous coordinates, FALSE for screen coordinates.
        KeepChildren: FALSE to update child entities psoitions, TRUE otherwise.
        ref: Pointer to the reference 2d entity for screen coordinates.
    Remarks:

    See Also: GetPosition,SetRect,SetSize
    *************************************************/
    virtual void SetPosition(const Vx2DVector &vect, CKBOOL hom = FALSE, CKBOOL KeepChildren = FALSE, CK2dEntity *ref = NULL) CK_PURE;

    /*************************************************
    Summary: Gets the size of 2d entity.
    Arguments:
        vect: Pointer to Vx2DVector for the entity size to be filled.
        hom: TRUE for Homogeneous coordinates mode, FALSE for screen coordinates mode.
    Return Value:
        CK_OK if successful or CKERR_INVALIDPARAMETER otherwise.
    Remarks:
        + The Vx2DVector argument will return coordinates in pixels or in percentage relative to the
        screen size, depending on the Homogeneous mode.

    See Also: SetSize,GetPosition,GetRect
    *************************************************/
    virtual CKERROR GetSize(Vx2DVector &vect, CKBOOL hom = FALSE) CK_PURE;

    /*************************************************
    Summary: Sets the size of 2d entity.
    Arguments:
        vect: Pointer to Vx2DVector containing the entity size.
        hom: TRUE for Homogeneous coordinates mode, FALSE for screen coordinates mode.
        KeepChildren: FALSE to update child entities positions, TRUE otherwise.
    Remarks:
        + The Vx2DVector is coordinates in pixels or percentage relative to the
        screen size, depending on the Homogeneous mode.

    See Also: GetSize,SetPosition,SetRect.
    *************************************************/
    virtual void SetSize(const Vx2DVector &vect, CKBOOL hom = FALSE, CKBOOL KeepChildren = FALSE) CK_PURE;

    //------------------------------------------------
    // Rect Access

    /*************************************************
    Summary: Sets the 2d entity rectangle in screen coordinates.
    Arguments:
        rect: 2d entity rectangle in screen coordinates.
        KeepChildren: FALSE to update child entities positions, TRUE otherwise.
    Remarks:
        + This method directly sets the position and size of the entity.

    See Also: GetRect,SetPosition,SetSize
    *************************************************/
    virtual void SetRect(const VxRect &rect, CKBOOL KeepChildren = FALSE) CK_PURE;

    /*************************************************
    Summary: Gets the 2d entity rectangle in screen coordinates.
    Arguments:
        rect: 2d entity rectangle in screen coordinates.
    Remarks:

    See Also: SetRect,SetHomogeneousRect,GetPosition,GetSize
    *************************************************/
    virtual void GetRect(VxRect &rect) CK_PURE;

    /*************************************************
    Summary: Sets the 2d entity rectangle in homogeneous coordinates.

    Return Value:
        CK_OK if successful, an error code otherwise
    Arguments:
        rect: 2d entity rectangle in homogeneous coordinates.
        KeepChildren: FALSE to update child entities positions, TRUE otherwise.
    See Also: GetHomogeneousRect,SetRect
    *************************************************/
    virtual CKERROR SetHomogeneousRect(const VxRect &rect, CKBOOL KeepChildren = FALSE) CK_PURE;

    /*************************************************
    Summary: Gets the 2d entity rectangle in homogeneous coordinates.
    Return Value:
        CK_OK if successful, an error code otherwise
    Arguments:
        rect: 2d entity rectangle in homogeneous coordinates.
    Remarks:

    See Also: SetHomogeneousRect,GetRect
    *************************************************/
    virtual CKERROR GetHomogeneousRect(VxRect &rect) CK_PURE;

    //------------------------------------------------
    // Source Rect Access

    /*************************************************
    Summary: Sets the cropping rectangle.
    Arguments:
        rect: 2d entity cropping rectangle in pixels.
    Remarks:
        + Cropping can be use to display only a subpart of a sprite. For example in a 32x32 sprite
        setting a cropping rectangle of (0,0) - (16,16) will only display the top left corner of the sprite.
        {Image:SpriteCropping}
        + Cropping must be enabled with UseSourceRect function for this function to work.

    See also: GetSourceRect, UseSourceRect, IsUsingSourceRect
    *************************************************/
    virtual void SetSourceRect(VxRect &rect) CK_PURE;

    /*************************************************
    Summary: Gets the cropping rectangle.
    Arguments:
        rect: 2d entity rectangle in screen coordinates.
    Remarks:

    See also: SetSourceRect, UseSourceRect, IsUsingSourceRect
    *************************************************/
    virtual void GetSourceRect(VxRect &rect) CK_PURE;

    /*************************************************
    Summary: Enables or disables cropping.
    Arguments:
        Use: TRUE to enable cropping, FALSE otherwise.
    Remarks:
        + When cropping is disabled the entire surface of the 2d entity is used to be drawn.
        If cropping is enabled only the rectangle specified with SetSourceRect will be used.
        + This method sets or removes the CK_2DENTITY_USESRCRECT flag.

    See also:SetSourceRect, GetSourceRect, IsUsingSourceRect
    *************************************************/
    virtual void UseSourceRect(CKBOOL Use = TRUE) CK_PURE;

    /*************************************************
    Summary: Checks if cropping is enabled or not.
    Return Value:
        TRUE if cropping is enabled, FALSE otherwise.
    Remarks:
        + When cropping is disabled the entire surface of the 2d entity is used to be drawn.
        If cropping is enabled only the rectangle specified with SetSourceRect will be used.

    See also:SetSourceRect, GetSourceRect, UseSourceRect
    *************************************************/
    virtual CKBOOL IsUsingSourceRect() CK_PURE;

    //------------------------------------------------
    // Pickability Access

    /*************************************************
    Summary: Enables or disables picking of the 2d entity.
    Arguments:
        Pick: TRUE to enables picking, FALSE to disable.
    Remarks:
        + If picking is disabled, the 2d entity will not be returned by CKRenderContext::Pick function.
        + This methods sets or removes the CK_2DENTITY_NOTPICKABLE flag.

    See also:SetFlags, IsPickable
    *************************************************/
    virtual void SetPickable(CKBOOL Pick = TRUE) CK_PURE;

    /*************************************************
    Summary: Checks if the 2d entity can be picked.
    Return Value:
        TRUE if the 2d Entity can be picked, FALSE otherwise.

    See also:SetPickable,SetFlags
    *************************************************/
    virtual CKBOOL IsPickable() CK_PURE;

    //------------------------------------------------
    // Background/foreground Sprites

    /*************************************************
    Summary: Sets the 2d entity background property
    Arguments:
        back :TRUE to set the entity as a background entity, FALSE otherwise.
    Remarks:
        + Background entities are drawn before any 3d geometry.
        + This method sets or removes the CK_2DENTITY_BACKGROUND flag.

    See also: SetFlags, IsBackground
    *************************************************/
    virtual void SetBackground(CKBOOL back = TRUE) CK_PURE;

    /*************************************************
    Summary: Checks if background property is set.
    Return Value:
        TRUE if the entity is rendered before 3d geometry, FALSE otherwise.
    Remarks:
        + Background entities are drawn before any 3d geometry.

    See also: SetFlags, SetBackground
    *************************************************/
    virtual CKBOOL IsBackground() CK_PURE;

    //------------------------------------------------
    // Clip To Parent

    /*************************************************
    Summary: Sets or Clears the clip to parent property

    Arguments:
        back :TRUE to clip the entity to its parent extents.
    Remarks:
        o If clipping is enabled the entity cannot be rendered outside of the extents of its parent.
        o This method sets or removes the CK_2DENTITY_CLIPTOPARENT flag.
    See also: SetFlags, IsClipToParent
    *************************************************/
    virtual void SetClipToParent(CKBOOL clip = TRUE) CK_PURE;

    /*************************************************
    Summary: Checks if entity is clipped to parent extents.
    Return Value:
        TRUE if the entity is clipped to its parent extents, FALSE otherwise.
    Remarks:

    See also: SetFlags, SetClipToParent
    *************************************************/
    virtual CKBOOL IsClipToParent() CK_PURE;

    //------------------------------------------------
    // Flag Access

    /*************************************************
    Summary: Sets the flags for 2d entity.
    Arguments:
        Flags: A combination of CK_2DENTITY_FLAGS to set to the entity.
    Remarks:
        + Most of the flags can be directly set or asked by the appropriate method
        of CK2dEntity.

    See Also: GetFlags, ModifyFlags,CK_2DENTITY_FLAGS
    *************************************************/
    virtual void SetFlags(CKDWORD Flags) CK_PURE;

    /*************************************************
    Summary: Modifies the flags of 2d entity.
    Arguments:
        add: Flags (CK_2DENTITY_FLAGS) to be added for 2d entity.
        remove: Flags to be removed from 2d entity.
    Remarks:
        + Most of the flags can be directly set or asked by the appropriate method
        of CK2dEntity.

    See Also: GetFlags, SetFlags,CK_2DENTITY_FLAGS
    *************************************************/
    virtual void ModifyFlags(CKDWORD add, CKDWORD remove = 0) CK_PURE;

    /*************************************************
    Summary: Returns the flags of 2d entity.
    Return Value:
        CK_2DENTITY_FLAGS Flags of the 2d entity.
    Remarks:
        + Most of the flags can be directly set or asked by the appropriate method
        of CK2dEntity.

    See Also: SetFlags, ModifyFlags,CK_2DENTITY_FLAGS
    *************************************************/
    virtual CKDWORD GetFlags() CK_PURE;

    //------------------------------------------------
    // Camera Ratio Offset

    /*************************************************
    Summary: Sets the origin of sprite position : Viewport or Context

    Arguments:
        Ratio: TRUE to enable ratio offset.
    Remarks:
    o When RatioOffset is enabled, the 2d entity position origin is on the top right corner of the camera viewport otherwise it is the top right corner of the render context window.
    o This method sets or removes the CK_2DENTITY_RATIOOFFSET flag.
    See also:SetFlags, IsRatioOffset
    *************************************************/
    virtual void EnableRatioOffset(CKBOOL Ratio = TRUE) CK_PURE;

    /*************************************************
    Summary: Checks if Ratio offset is enabled
    Return Value:
        TRUE if Ratio offset is enabled.
    Remarks:
        + When RatioOffset is enabled, the 2d entity position origin is
        on the top right corner of the camera viewport otherwise it is the
        top right corner of the render context window.

    See also:SetFlags, EnableRatioOffset
    *************************************************/
    virtual CKBOOL IsRatioOffset() CK_PURE;

    //------------------------------------------------
    // Parenting

    /*************************************************
    Summary: Sets the parent of a 2dEntity

    Arguments:
        parent: New parent of the 2D Entity, NULL to set this entity as a root.
    Return Value:
        TRUE if successful.
    See also:GetParent
    *************************************************/
    virtual CKBOOL SetParent(CK2dEntity *parent) CK_PURE;

    /*************************************************
    Summary: Returns the parent of a 2dEntity
    Return Value:
        The parent of the 2D Entity, NULL if not.
    Remarks:

    See also: SetParent
    *************************************************/
    virtual CK2dEntity *GetParent() const CK_PURE;

    /*************************************************
    Summary: Returns the number of children of the 2dEntity
    Return Value:
        Number of children of the 2dEntity.
    Remarks:

    See also: SetParent, GetChild
    *************************************************/
    virtual int GetChildrenCount() const CK_PURE;

    /*************************************************
    Summary: Returns the child of a 2dEntity
    Arguments:
        i: index of the child to return.
    Return Value:
        The ith child is available.
    Remarks:

    See also: SetParent, GetChildrenCount
    *************************************************/
    virtual CK2dEntity *GetChild(int i) const CK_PURE;

    /*************************************************
    Summary: Checks if the object is root object.
    Return Value:
        TRUE if the entity is root, FALSE otherwise.
    Remarks:
        This method returns TRUE if the entity does not have a parent.

    *************************************************/
    virtual CKBOOL IsRootObject() CK_PURE;
    /************************************************
    Summary: Parses the sub hierarchy of an entity.
    Arguments:
        current: Current object in the hierarchy (must start at NULL).
    Return Value: Child entity.

    Remarks:
        + This method enables you to iterate among the complete hierarchy  of an
        entity.


    Example:
            // This sample iterates the hierarchy of Entity
            // and search for a specific mesh...

            CK2dEntity* Child = NULL;
            while (Child = Entity->HierarchyParser(Child)) {
                // Do something on the child
            }


    See also: GetChildrenCount,GetChildren
    ************************************************/
    virtual CK2dEntity *HierarchyParser(CK2dEntity *current) const CK_PURE;

    //------------------------------------------------
    // Material Rendering

    /*************************************************
    Summary: Sets the material to use to render the 2d entity
    Arguments:
        mat: A pointer to the CKMaterial.
    Remarks:
        + 2D entities can use a material (and its texture) to render
        as an equivalent of a sprite.

    See Also: GetMaterial
    *************************************************/
    virtual void SetMaterial(CKMaterial *mat) CK_PURE;

    /*************************************************
    Summary: Returns the material used to render the 2d entity
    Return Value:
        A pointer to the material used or NULL otherwise.
    Remarks:

    See Also: SetMaterial
    *************************************************/
    virtual CKMaterial *GetMaterial() CK_PURE;

    //------------------------------------------------
    // Homogeneous Coordinates

    /*************************************************
    Summary: Sets homogeneous coordinates.

    Arguments:
        Coord: TRUE to enable homogeneous coordinates FALSE otherwise.
    Remarks:
    o In homogeneous coordinates mode the positions are given with coordinates between (0..1) relative to screen space.
    o This method sets or removes the CK_2DENTITY_USEHOMOGENEOUSCOORD flag.
    See Also:SetFlags,IsHomogeneousCoordinates,SetPosition,SetSize
    *************************************************/
    virtual void SetHomogeneousCoordinates(CKBOOL Coord = TRUE) CK_PURE;

    /*************************************************
    Summary: Checks whether homogeneous coordinates mode is enabled or not.
    Return Value:
        TRUE if coordinates are homogeneous, FALSE otherwise.
    Remarks:

    See Also: SetHomogeneousCoordinates
    *************************************************/
    virtual CKBOOL IsHomogeneousCoordinates() CK_PURE;

    //------------------------------------------------
    // Clip to Camera view

    /*************************************************
    Summary: Forces 2d entity to be clipped by camera viewport.
    Arguments:
        Clip: TRUE to enables clipping, FALSE otherwise.
    Remarks:
    o The camera viewport can be smaller than the rendercontext size according to camera
    apsect ratio. The default behavior of a 2d entity is to be clipped by the rendercontext
    size, but enabling this flag can force the entity to be clipped against camera viewport.
    o This method sets or removes the CK_2DENTITY_CLIPTOCAMERAVIEW flag.

    See also:SetFlags, IsClippedToCamera
    *************************************************/
    virtual void EnableClipToCamera(CKBOOL Clip = TRUE) CK_PURE;

    /*************************************************
    Summary: Checks whether camera clipping is enabled or not.
    Return Value:
        TRUE if clipping is enabled, FALSE otherwise.
    Remarks:
        + The camera viewport can be smaller than the rendercontext size according to camera
        apsect ratio. The default behavior of a 2d entity is to be clipped by the rendercontext
        size, but enabling this flag can force the entity to be clipped against camera viewport.

    See also:SetFlags, EnableClipToCamera
    *************************************************/
    virtual CKBOOL IsClippedToCamera() CK_PURE;

    //------------------------------------------------
    // Entity Rendering

    /*************************************************
    Summary: Render the 2d entity and its children
    Arguments:
        context: A pointer to a CKRenderContext on which this entity will be drawn

    Return Value:
        CK_OK if successful or an error code otherwise.

    Remarks:
    + This methods calls the pre and post render callbacks if there are any and
    draw the entity calling the Draw method.
    + If the entity has children they are rendered afterwards.

    See also: Draw
    *************************************************/
    virtual CKERROR Render(CKRenderContext *context) CK_PURE;

    //------------------------------------------------
    // Actual Entity Drawing

    /*************************************************
    Summary: Draws the 2d entity.
    Arguments:
        context: A pointer to a CKRenderContext on which this entity will be drawn
    Return Value:
        CK_OK if successful or an error code otherwise.
    Remarks:
        + This methods is automatically called by the Render method. It simply draws the
        entity on the context without rendering the children or calling the render callbacks.

    See Also: Render
    *************************************************/
    virtual CKERROR Draw(CKRenderContext *context) CK_PURE;

    //------------------------------------------------
    // Extents (Surface actually rendered)

    virtual void GetExtents(VxRect &srcrect, VxRect &rect) CK_PURE;

    virtual void SetExtents(const VxRect &srcrect, const VxRect &rect) CK_PURE;

    /*************************************************
    Summary: Restores initial size.
    Remarks:
        + This method is only implemented for sprite and restore the size
        of the entity to the size of the bitmap that was used to generate the sprite.

    See Also:
    *************************************************/
    virtual void RestoreInitialSize() CK_PURE;

    /*************************************************
    Summary: Dynamic cast operator.
    Arguments:
        iO: A pointer to a CKObject to cast.
    Return Value:
        iO casted to the appropriate class or NULL if iO is not from the required class .
    Example:
          CKObject* Object;
          CK2dEntity* ent = CK2dEntity::Cast(Object);
    Remarks:

    *************************************************/
    static CK2dEntity *Cast(CKObject *iO)
    {
        return CKIsChildClassOf(iO, CKCID_2DENTITY) ? (CK2dEntity *)iO : NULL;
    }

#ifndef CK_3DIMPLEMENTATION
};
#endif
#endif
