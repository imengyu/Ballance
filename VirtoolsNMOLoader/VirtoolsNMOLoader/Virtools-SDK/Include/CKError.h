/*************************************************************************/
/*	File : CKError.h				 				 					 */
/*	List of possible Error Codes										 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 1999, All Rights Reserved.						 */	
/*************************************************************************/

#ifndef CKERROR_H

#define CKERROR_H "$Id:$"


CKSTRING CKErrorToString(CKERROR err);

//----------------------------------------------------------////
//			Error Codes										////
//----------------------------------------------------------////
// Note : When adding Error code, update the CKErrorToString function in CKMain.cpp
// and documentation in Papers\overview.gls

// Operation successful

#define CK_OK										 0

// One of the parameter passed to the function was invalid

#define CKERR_INVALIDPARAMETER						-1

// One of the parameter passed to the function was invalid

#define CKERR_INVALIDPARAMETERTYPE					-2

// The parameter size was invalid

#define CKERR_INVALIDSIZE							-3

// The operation type didn't exist

#define CKERR_INVALIDOPERATION						-4

// The function used to execute the operation is not yet implemented

#define CKERR_OPERATIONNOTIMPLEMENTED				-5

// There was not enough memory to perform the action

#define CKERR_OUTOFMEMORY							-6

// The function  is not yet implemented

#define CKERR_NOTIMPLEMENTED						-7

// There was an attempt to remove something not present

#define CKERR_NOTFOUND								-11

// There is no level currently created

#define CKERR_NOLEVEL								-13

// 

#define CKERR_CANCREATERENDERCONTEXT				-14

// The notification message was not used

#define CKERR_NOTIFICATIONNOTHANDLED				-16

// Attempt to add an item that was already present 

#define CKERR_ALREADYPRESENT						-17

// the render context is not valid

#define CKERR_INVALIDRENDERCONTEXT					-18

// the render context is not activated for rendering

#define CKERR_RENDERCONTEXTINACTIVE					-19

// there was no plugins to load this kind of file

#define CKERR_NOLOADPLUGINS							-20

// there was no plugins to save this kind of file

#define CKERR_NOSAVEPLUGINS							-21

// attempt to load an invalid file

#define CKERR_INVALIDFILE							-22

// attempt to load with an invalid plugin

#define CKERR_INVALIDPLUGIN							-23

// attempt use an object that wasnt initialized

#define  CKERR_NOTINITIALIZED						-24

// attempt use a message type that wasn't registred

#define  CKERR_INVALIDMESSAGE						-25

// attempt use an invalid prototype

#define	CKERR_INVALIDPROTOTYPE			            -28

// No dll file found in the parse directory

#define CKERR_NODLLFOUND							-29

// this dll has already been registred 

#define CKERR_ALREADYREGISTREDDLL					-30

// this dll does not contain information to create the prototype

#define CKERR_INVALIDDLL							-31

// Invalid Object (attempt to Get an object from an invalid ID)

#define CKERR_INVALIDOBJECT							-34

// Invalid window was provided as console window 

#define CKERR_INVALIDCONDSOLEWINDOW					-35

// Invalid kinematic chain ( end and start effector may not be part of the same hierarchy )

#define CKERR_INVALIDKINEMATICCHAIN					-36 


// Keyboard not attached or not working properly

#define CKERR_NOKEYBOARD							-37 

// Mouse not attached or not working properly

#define CKERR_NOMOUSE								-38 

// Joystick not attached or not working properly

#define CKERR_NOJOYSTICK							-39 

// Try to link imcompatible Parameters

#define CKERR_INCOMPATIBLEPARAMETERS				-40

// There is no render engine dll 

#define CKERR_NORENDERENGINE						-44	

// There is no current level (use CKSetCurrentLevel )

#define CKERR_NOCURRENTLEVEL						-45	

// Sound Management has been disabled

#define CKERR_SOUNDDISABLED							-46	

// DirectInput Management has been disabled

#define CKERR_DINPUTDISABLED						-47	

// Guid is already in use or invalid 

#define CKERR_INVALIDGUID							-48	

// There was no more free space on disk when trying to save a file

#define CKERR_NOTENOUGHDISKPLACE					-49	

// Impossible to write to file (write-protection ?)

#define CKERR_CANTWRITETOFILE						-50	

// The behavior cannnot be added to this entity 

#define CKERR_BEHAVIORADDDENIEDBYCB					-51	

// The behavior cannnot be added to this entity 

#define CKERR_INCOMPATIBLECLASSID					-52	

// A manager was registered more than once

#define CKERR_MANAGERALREADYEXISTS					-53	

// CKprocess or TimeManager process while CK is paused will fail

#define CKERR_PAUSED								-54	

// Some plugins were missing whileloading a file

#define CKERR_PLUGINSMISSING						-55	

// Virtools version too old to load this file

#define CKERR_OBSOLETEVIRTOOLS						-56	

// CRC Error while loading file

#define CKERR_FILECRCERROR							-57

// A Render context is already in Fullscreen Mode

#define CKERR_ALREADYFULLSCREEN						-58

// Operation was cancelled by user

#define CKERR_CANCELLED								-59


// there were no animation key at the given index

#define CKERR_NOANIMATIONKEY						-121

// attemp to acces an animation key with an invalid index

#define CKERR_INVALIDINDEX							-122

// the animation is invalid (no entity associated or zero length)

#define CKERR_INVALIDANIMATION						-123


#endif 