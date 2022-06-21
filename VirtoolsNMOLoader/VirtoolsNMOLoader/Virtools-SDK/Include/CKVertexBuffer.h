/*************************************************************************/
/*	File : CKVertexBuffer.h												 */
/*	Author :  Romain Sididris											 */	
/*	Last Modification : 15/07/01										 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2001, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKVERTEXBUFFER_H

#define CKVERTEXBUFFER_H "$Id:$"

#include "CKDefines.h"

/*****************************************************************
Summary: CKVertexBuffer::Lock behavior flags.

See Also: CKVertexBuffer::Lock
******************************************************************/
typedef enum CKLOCKFLAGS {
	CK_LOCK_DEFAULT		= 0x00000000,   // No assumption
	CK_LOCK_NOOVERWRITE	= 0x00000001,	// Write operation will not overwrite any vertex used in a pending drawing operation. 
	CK_LOCK_DISCARD		= 0x00000002,	// No need for the previous memory
} CKLOCKFLAGS;

/*****************************************************************
Summary: CKVertexBuffer::Check return flags.

******************************************************************/
typedef enum CKVB_STATE {
	CK_VB_OK		= 0x00000000,   // Vertex buffer format and content are up to date.
	CK_VB_LOST		= 0x00000001,	// Vertex buffer content must be updated. 
	CK_VB_FAILED	= 0x00000002,	// Vertex buffer can not be created with the given format or vertex count.
} CKVB_STATE;

/************************************************************
Summary: Vertex buffer class.

Remarks: 
+ The CKVertexBuffer class provides an interface to create and manage a vertex buffer.

+ For devices that supports vertex buffer in hardware this can enable very fast rendering 
as the vertex data is already on the video card.

+ For other devices the implementation will provide a system memory pointer to hold the vertex
data so that the user does not have to worry of the underlying implementation

+ A vertex buffer is created through the CKRenderManager::CreateVertexBuffer method.
and can be destroyed with the CKVertexBuffer::Destroy or CKRenderManager::DestroyVertexBuffer

+ The CKVertexBuffer class is mainly designed for static vertex buffers. If a vertex buffer is needed to be very frequently updated (dynamic vertex buffer) , the
RCKRenderContext::GetDrawPrimitiveStructure with the CKRST_DP_VBUFFER is a better solution and 
avoids the creation of a CKVertexBuffer instance.
	
See also: VxDrawPrimitiveData,CKRenderManager::CreateVertexBuffer,CKRenderManager::DestroyVertexBuffer
*************************************************************/
class CKVertexBuffer {
public:

	CKVertexBuffer() {}; 

	virtual ~CKVertexBuffer() {}; 
	
/**************************************************************************************
Summary: Destroys the content of the vertex buffer.
Remarks:
+ This method can be used to release memory taken by a vertex buffer 
See Also : Check,Draw
***************************************************************************************/
	virtual void Destroy() =0; 
/**************************************************************************************
Summary: Checks the state of the vertex buffer.
Return Value
	Current State of the vertex buffer: 
	CK_VB_LOST : The content of the vertex buffer was lost (device changes,etc..) and should filled back (Lock)
	CK_VB_OK: No Error , Vertex buffer can be rendered (Draw)
	CK_VB_FAILED: Vertex buffer could not be created
Arguments:
	Ctx: A pointer to a CKRenderContext on which this vertex buffer should be used.
	MaxVertexCount: Max number of vertex the buffer can contain.
	Format: A combination of CKRST_DPFLAGS giving the vertex format for this vertex buffer.
	Dynamic: A boolean indicating if this vertex buffer will be frequently written too...
Remarks:
+ There is not a method to create a vertex buffer, instead one should use this method 
to check the vertex buffer before each drawing.
+ The return value indicates whether the vertex buffer is up-to-date (CK_VB_OK) or if its content should 
be updated (CK_VB_LOST), or even if the creation of such a vertex buffer failed (CK_VB_FAILED). 
See Also : Lock,Unlock,Draw
***************************************************************************************/
	virtual CKVB_STATE			 Check(CKRenderContext* Ctx,CKDWORD MaxVertexCount,CKRST_DPFLAGS Format,CKBOOL Dynamic = FALSE) = 0;
	
/**************************************************************************************
Summary: Locks the content of a vertex buffer for a write operation.
Arguments:
	Ctx: A pointer to a CKRenderContext on which this vertex buffer should be used.
	StartVertex: Index of the first vertex to be locked.
	VertexCount: Number of vertices to be locked.
	LockFlags: Behavior of the lock operation.
Return Value:
A pointer to a VxDrawPrimitiveData structure that which pointers and strides have been setup
to point directly to the vertex buffer.
Remarks:
+ A Locked vertex buffer must be Unlocked before any draw operation can occur.
+ The LockFlags can give some hint to accelerate the copy in case of a dynamic vertex buffer otherwise it 
is meaningless and should be kept to its default value.
See Also : CKLOCKFLAGS
***************************************************************************************/
	virtual VxDrawPrimitiveData* Lock(CKRenderContext* Ctx,CKDWORD StartVertex,CKDWORD VertexCount,CKLOCKFLAGS LockFlags = CK_LOCK_DEFAULT) = 0;

	virtual void				 Unlock(CKRenderContext* Ctx) = 0 ;

/**************************************************************************************
Summary: Draws the vertex buffer on a given render context.
Arguments: 
	Ctx: A pointer to a CKRenderContext on which this vertex buffer should be used.
	pType: Type of primitive to draw.
	Indices: A pointer to a list of WORD that will be used as indices to vertices or NULL.
	Indexcount: Number of indices
	StartVertex: Index of the first vertex to be locked.
	VertexCount: Number of vertices to be locked.
Return Value:
	TRUE if successful.
Remarks:
+ This method renders a set of primitives (using Indices if given otherwise taking vertices in the order they are given).
+ Indices are relative to StartVertex.
+ Before calling this method you must ensure your vertex buffer is ready to be drawn on this context with the Check
method. 
See also:Custom Rendering,VXPRIMITIVETYPE,CKRenderContext::DrawPrimitive
************************************************************/
	virtual CKBOOL				 Draw(CKRenderContext* Ctx,VXPRIMITIVETYPE pType,CKWORD* Indices,int Indexcount,CKDWORD StartVertex,CKDWORD VertexCount) = 0;
};



#endif // CKVERTEXBUFFER_H