/*************************************************************************/
/*	File : CKSquare.h													 */
/*	Author :  Cabrita Francisco											 */
/*																		 */
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKSquare_H

#define CKSquare_H "$Id:$"


class CKSquare {
public:
  ////////////////////////////////////////////////////////
  ////            Members of a Square                 ////
  ////////////////////////////////////////////////////////

  union{          // Value
    int     ival;
    float   fval;
    CKDWORD dval;
    void    *ptr;
  };

};

#endif
