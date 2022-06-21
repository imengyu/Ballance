/*************************************************************************/
/*	File : CKSpriteText.h												 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#if !defined(CKSPRITETEXT_H) || defined(CK_3DIMPLEMENTATION)
#ifndef CK_3DIMPLEMENTATION

#define CKSPRITETEXT_H "$Id:$"

#include "CKSprite.h"


#undef CK_PURE

#define CK_PURE = 0

/*************************************************
{filename:CKSpriteText}
Name: CKSpriteText

Summary: Text Only sprites.

Remarks:
	+ A SpriteText is a special kind of sprite that is used to draw text on screen.
	Methods provides acces to font,color,background color and transparency settings as 
	well as text alignment in the sprite.

	+ The size of image used to draw the text is given by CKSprite::Create.
	
	+ This class is provided more for debugging than for a real usage since changing
	the text every frame is a slow operation. Much faster text drawing can be achieved 
	if the text changes often, using bitmap fonts and textures (See BitmapTextDisplay behavior for example)



See also: CKSprite
*************************************************/
class CKSpriteText : public CKSprite {
public:
#endif
//-------------------------------------------
// Text

/************************************************
Summary: Sets the text drawn in the sprite.

Arguments:
	text : A CKSTRING containing the new text.
See also: GetText
************************************************/
virtual void SetText(CKSTRING text) CK_PURE;

/************************************************
Summary: Gets the text drawn in the sprite.

Return Value: A CKSTRING containing the current text.
See also: SetText
************************************************/
virtual CKSTRING	GetText() CK_PURE;

//-------------------------------------------
// Appearence

/************************************************
Summary: Sets the text color.

Arguments: 
	col: CKDWORD containing the new color ( 32 bit ARGB )
See also: GetTextColor,GetBackgroundColor,SetBackgroundColor
************************************************/
virtual void		SetTextColor(CKDWORD col) CK_PURE;

/************************************************
Summary: Gets the text color.

Return Value: Current text color.
See also: SetTextColor,GetBackgroundColor,SetBackgroundColor
************************************************/
virtual CKDWORD	GetTextColor() CK_PURE;

/************************************************
Summary: Sets the background color in the sprite.

Arguments: 
	col: CKDWORD containing the new color ( 32 bit ARGB )
See also: SetTextColor,GetTextColor, GetBackgroundColor
************************************************/
virtual void		SetBackgroundColor(CKDWORD col) CK_PURE;

/************************************************
Summary: Gets the background color.

Return Value: Current background color.
See also: SetTextColor,GetTextColor, SetBackgroundColor
************************************************/
virtual CKDWORD	GetBackgroundTextColor() CK_PURE;

/************************************************
Summary: Sets the font used to draw the text in the sprite.

Arguments: 
	FontName: Name of the font 
	FontSize: Size of the font
	Weight:	  Weight ot the font
	italic :  TRUE to create an italic font
	underline: TRUE to create an underlined font
Remarks:
	+ This function use the Win32 API CreateFont function to create the font that will be used to draw text in the sprite.
	+ See CreateFont in the Win32 API documentation for more details.
See also: RCKSpriteText
************************************************/
virtual void	SetFont(CKSTRING FontName,int FontSize=12,int Weight=400,CKBOOL italic=FALSE,CKBOOL underline=FALSE) CK_PURE;

/************************************************
Summary: Sets the drawing alignment for the sprite

Arguments:
	align : A combination of CKSPRITETEXT_ALIGNMENT flags specifying how to align the text inside the sprite.
See also: CKSPRITETEXT_ALIGNMENT, GetAlign
************************************************/
virtual void		SetAlign(CKSPRITETEXT_ALIGNMENT align) CK_PURE;

/************************************************
Summary: Gets the drawing alignment of the sprite

Return Value: Current alignment options.
See also: CKSPRITETEXT_ALIGNMENT, SetAlign
************************************************/
virtual CKSPRITETEXT_ALIGNMENT GetAlign() CK_PURE;


/************************************************
Summary: Sets the text drawn in the sprite.

Arguments:
	text : A CKSTRING containing the new text.
Remarks: Exists in U version for unicode strings.
See also: GetText
************************************************/
virtual void SetTextU(CKUSTRING text) CK_PURE;

/************************************************
Summary: Gets the text drawn in the sprite.

Return Value: A CKSTRING containing the current text.
Remarks: Exists in U version for unicode strings.

See also: SetText
************************************************/
virtual CKUSTRING	GetTextU() CK_PURE;

/*************************************************
Summary: Dynamic cast operator. 
Arguments:
	iO: A pointer to a CKObject to cast.
Return Value:
	iO casted to the appropriate class or NULL if iO is not from the required class .
Example:
	  CKObject* Object;
	  CKSpriteText* anim = CKSpriteText::Cast(Object);
Remarks:

*************************************************/
static CKSpriteText* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_SPRITETEXT)?(CKSpriteText*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif

