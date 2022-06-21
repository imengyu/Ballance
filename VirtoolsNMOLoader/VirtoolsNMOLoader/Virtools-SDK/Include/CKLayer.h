/*************************************************************************/
/*	File : CKLayer.h													 */
/*	Author :  Cabrita Francisco											 */
/*																		 */
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKLayer_H

#define CKLayer_H "$Id:$"


#include "CKObject.h"
#include "CKSquare.h"

//_______________________________________
//          STATES FLAGS


#define LAYER_STATE_VISIBLE 0x00000001


/*************************************************
{filename:CKLayer}
Summary: Grid layer class.
						
Remarks:
	+ A CKlayer is only used to store an array of value for a given
	layer type in a grid.

{image:layer}
	
	+ Instances of CKLayer are created with the function CKGrid::AddLayer
	and remove the with CKGrid::RemoveLayer method

	+ Layer type is identified by an integer returned by GetType which can be converted in a string by
	using CKGridManager::GetTypeName

	+ Layer data is stored in an array[length][width] of CKSquare where CKSquare is defined as 

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}
	
		class CKSquare {
			  union{          
				int     ival;
				float   fval;
				CKDWORD dval;
				void    *ptr;
			  };
		};

{html:</td></tr></table>}


	+ The Class Identifier of a Layer is CKCID_LAYER

See also: CKGrid,CKGridManager
*************************************************/
class CKLayer : public CKObject {
public:
  //____________________________________________________
  //      TYPE

	virtual void SetType(int Type) = 0;

/************************************************
Summary: Returns the layer type

Return Value: layer type
Remarks:
	CKGridManager::GetTypeName
See also: CKGridManager::GetTypeName
************************************************/
  virtual int  GetType() = 0;
  
  //____________________________________________________
  //      FORMAT NOT USED


  virtual void SetFormat(int Format) = 0;

  virtual int  GetFormat() = 0;
  
  //____________________________________________________
  //      SQUARE

/************************************************
Summary: Sets the value of a specific square. (No validity check)

Input Arguments:
	x: the x coordinate in the layer coordinate system
	y: the y coordinate in the layer coordinate system
	val: A pointer to the value to set.
Remarks:
+ The SetValue function does not perform any check about the coordinates validity so
it is the application responsabilty to ensure x and y are valid coordinates or
use SetValue2 which will be slower.
+ val is supposed to be a pointer to a CKSquare, int, float or any 4 byte data...
+ The usual usage is to store integer value.
+ The content of iVal is used immediatly and is not needed anymore afterwards. 

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Width; x++) {
					int valueToSet = rand() & 255;

					layer->SetValue(x,y,&valueToSet);
				}
			}

{html:</td></tr></table>}

See also: GetValue, SetValue2, GetValue2, SetSquareArray, GetSquareArray
************************************************/
  virtual void SetValue( int x, int y, void *val ) = 0;

/************************************************
Summary: Gets the value of a specific square. (No validity check)

Arguments:
	x: the x coordinate in the grid coordinate system
	y: the y coordinate in the grid coordinate system
	val: a pointer to the variable that will get the value
Remarks:
+ The GetValue function does not perform any check about the coordinates validity so
it is the application responsabilty to ensure x and y are valid coordinates or
use GetValue2 which will be slower.
+ val is supposed to be a pointer to a CKSquare, int, float or any 4 byte data...

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

			int valueToGet;
			layer->GetValue(x,y,&valueToGet);

{html:</td></tr></table>}

See also: SetValue, GetValue2, SetValue2, SetSquareArray, GetSquareArray

************************************************/
  virtual void GetValue( int x, int y, void *val ) = 0;


  virtual CKBOOL SetValue2( int x, int y, void *val ) = 0;


  virtual CKBOOL GetValue2( int x, int y, void *val ) = 0;

/************************************************
Summary: Direct Access to the layer squares Array

Return Value: The adress of the CKSquare array.
Remarks:
Layer data is stored in an array[length][width] of CKSquare where CKSquare is defined as 

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}
	
		class CKSquare {
			  union{          
				int     ival;
				float   fval;
				CKDWORD dval;
				void    *ptr;
			  };
		};

{html:</td></tr></table>}	

The example shown in SetValue method can be coded more efficiently using this method.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					int valueToSet = rand() & 255;

					layer->SetValue(x,y,&valueToSet);
				}
			}

			becomes

			CKSquare* array = layer->GetSquareArray();

			for (int x = 0; x < Width; ++x) {
				for (int y = 0; y < Height; ++y , ++array) {
					int valueToSet = rand() & 255;

					array->ival = valueToSet;
				}
			}

{html:</td></tr></table>}
See also: SetSquareArray, GetValue, GetValue2, SetValue, SetValue2
************************************************/
  virtual CKSquare *GetSquareArray() = 0;


  virtual void SetSquareArray( CKSquare *sqarray ) = 0;


  //____________________________________________________
  //      STATE
/************************************************
Summary: Shows or hides a layer

Arguments:
	vis: default is TRUE (visible), else FALSE (hidden)
Remarks:
	The visibility flag is only used in the Dev Interface and 
has no impact on the processing.
See also: IsVisible
************************************************/
  virtual void SetVisible( CKBOOL vis=TRUE ) = 0;

/************************************************
Summary: Test if the layer is visible or not

Return Value:
	TRUE if visible, FALSE otherwise
Remarks:
	The visibility flag is only used in the Dev Interface and 
has no impact on the processing.
See also: SetVisible
************************************************/
  virtual CKBOOL IsVisible() = 0;


  virtual void InitOwner( CK_ID owner) = 0;

  virtual void SetOwner( CK_ID owner) = 0;

/************************************************
Summary: Returns the CKGrid using this layer. 

Return Value:
	owner CK_ID
See Also:CKContext::GetObject
************************************************/
  virtual CK_ID GetOwner() = 0;


  CKLayer(CKContext *Context,CKSTRING name=NULL) : CKObject(Context,name) {}	

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
static CKLayer* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_LAYER)?(CKLayer*)iO:NULL;
}

};

#endif
