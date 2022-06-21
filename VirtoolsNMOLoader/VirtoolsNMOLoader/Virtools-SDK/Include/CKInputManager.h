/*************************************************************************/
/*	File : CKInputManager.h					 		 					 */
/*	Author : Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKINPUTMANAGER_H

#define CKINPUTMANAGER_H "$Id:$"

#include  "VxMath.h"
#include  "CKdefines.h"
#include  "CKBaseManager.h"


#define NO_KEY			0

#define KEY_PRESSED		1

#define KEY_RELEASED	2


#define KS_IDLE			0

#define KS_PRESSED		1

#define KS_RELEASED		2

/*************************************************************************
{filename:CKInputManager}
Summary: Keyboard,joystick and mouse management.

Remarks: 

+ The input manager is here to provide a centralized access to every available input
devices. The input manager is not implemented in CK but rather as an external manager
that can be replaced.

+ This class defines methods that every implementation of an input manager should
implement so the default Virtools controller behaviors can work.

+ One can override the default input manager provided with Virtools to support 
new features simply by creating a new manager deriving from this class.

See also: CKContext::GetManagerByGuid,
*************************************************************************/
class CKInputManager : public CKBaseManager {

public:

//*****************************************************
// Keyboard Acces

/************************************************
Summary: Enables/Disables keyboard repetition. 

Arguments:
	iEnable: TRUE to enable the keyboard repetition, FALSE to disable it.

See Also:IsKeyboardRepetitionEnabled
************************************************/	
virtual	void	 EnableKeyboardRepetition(CKBOOL iEnable = TRUE) = 0;

/************************************************
Summary: Tests if the keyboard repetition is enabled

Return Value:
	TRUE if the keyboard repetion is enabled, FALSE otherwise.

See Also: EnableKeyboardRepetition
************************************************/	
virtual	CKBOOL	 IsKeyboardRepetitionEnabled() = 0;

// Summary: 
/************************************************
Summary: Returns whether a key is down

Arguments:
	iKey: The key code to watch, in the CKKEYBOARD enumeration.
	oStamp: If the pointer is valid, the function fills it with 
	the time stamp when the key was pressed.
Return Value:
	TRUE if the key is down.

See Also:IsKeyUp, IsKeyToggled
************************************************/	
virtual	CKBOOL	 IsKeyDown(CKDWORD iKey,CKDWORD* oStamp=NULL) = 0;

/************************************************
Summary: Returns whether a key is up

Arguments:
	iKey: The key code to watch, in the CKKEYBOARD enumeration.
Return Value:
	TRUE if the key is up.

See Also:IsKeyDown, IsKeyToggled
************************************************/	
virtual	CKBOOL	 IsKeyUp(CKDWORD iKey) = 0;

/************************************************
Summary: Returns whether a key has been toggled

Arguments:
	iKey: The key code to watch, in the CKKEYBOARD enumeration.
	oStamp: If the pointer is valid, the function fills it with 
	the time stamp when the key was toggled.
Return Value:
	TRUE if the key has been toggled.

See Also:IsKeyDown
************************************************/	
virtual	CKBOOL	 IsKeyToggled(CKDWORD iKey, CKDWORD* oStamp=NULL) = 0;

/************************************************
Summary: Gets the key name 

Arguments:
	iKey: The key code we want the name, in the CKKEYBOARD enumeration.
	oKeyName: XString filled with the key name.

See Also: GetKeyFromName
************************************************/	
virtual	void	 GetKeyName(CKDWORD iKey, XString& oKeyName) = 0;

/************************************************
Summary: Gets a key code from a key name.

Arguments:
	iKeyName: Name of the key code to retreive
Return Value:
	Key code (in the CKKEYBOARD enumeration)

See Also: GetKeyName
************************************************/	
virtual	CKDWORD	 GetKeyFromName(XString& iKeyName) = 0;

/************************************************
Summary: Retreive the current state of the whole keyboard.

Return Value:
	An array of 256 chars representing the state of each key. If 
	the high bit of the byte is set, the key is down.

See Also: IsKeyboardAttached
************************************************/	
virtual	unsigned char* GetKeyboardState() = 0;

/************************************************
Summary: Tests if a keyboard is attached 

Return Value:
	TRUE if a keyboard is attached, FALSE otherwise.

See Also: GetKeyboardState
************************************************/	
virtual	CKBOOL   IsKeyboardAttached() = 0;

/************************************************
Summary: Returns the number of keyboard events in the buffer.

Return Value:
	The number of events in the buffer.
Remarks:
	The buffer contains the keyboard events that happened
	since the last frame.

See Also:GetKeyFromBuffer
************************************************/	
virtual	int		 GetNumberOfKeyInBuffer() = 0;

/************************************************
Summary: Gets a key event from the buffer.

Arguments:
	i: index in the buffer
	oKey: filled with the key code of the event (in the CKKEYBOARD enumeration)
	oTimeStamp: if the pointer is valid, filled with the time stamp of the event.

Return Value:
	KEY_PRESSED if the key was pressed
	KEY_RELEASED if the key was released
	NO_KEY if the index is irrelevant.

See Also:GetNumberOfKeyInBuffer
************************************************/	
virtual	int		 GetKeyFromBuffer(int i, CKDWORD& oKey,CKDWORD* oTimeStamp = NULL) = 0;

//*****************************************************
// Mouse Access

/************************************************
Summary: Test if a mouse button is down

Arguments:
	iButton: Index of the button	
Return Value:
	TRUE if the mouse button is down

See Also:IsMouseClicked
************************************************/	
virtual	CKBOOL	IsMouseButtonDown(CK_MOUSEBUTTON iButton) = 0;

/************************************************
Summary: Test if a mouse button has just been clicked 

Arguments:
	iButton: Index of the button	
Return Value:
	TRUE if the mouse button has been clicked (it was up the 
	previous frame and down now.)

See Also:IsMouseToggled
************************************************/	
virtual	CKBOOL	IsMouseClicked(CK_MOUSEBUTTON iButton) = 0;

/************************************************
Summary: Test if a mouse button has just been toggled.

Arguments:
	iButton: Index of the button	
Return Value:
	TRUE if the mouse button has been released (it was down the 
	previous frame and up now.)

See Also:IsMouseClicked
************************************************/	
virtual	CKBOOL	IsMouseToggled(CK_MOUSEBUTTON iButton) = 0;

/************************************************
Summary: Gets the mouse buttons states.

Arguments:
	oStates: Array of 4 bytes that will be filled with the states 
	of the potential 4 mouse buttons. 
Remarks:
	If a button is down, it will have the mask KS_PRESSED.
	If a button is up, it will have the mask KS_RELEASED.

See Also:IsMouseButtonDown
************************************************/	
virtual void	GetMouseButtonsState(CKBYTE oStates[4])=0;

/************************************************
Summary: Gets the mouse position.

Arguments:
	oPosition: Vx2DVector containing the position of the mouse.
	iAbsolute: TRUE if you want the position in screen coordinates,
	FALSE to have it in window coordinates.

See Also:IsKeyboardRepetitionEnabled
************************************************/	
virtual void	GetMousePosition(Vx2DVector& oPosition ,CKBOOL iAbsolute = TRUE) = 0;

/************************************************
Summary: Gets the relative position of the mouse.

Arguments:
	oPosition: VxVector containing the relative position of the mouse.	
Remarks:
	The z component will contains the wheel relative displacement.

See Also:IsKeyboardRepetitionEnabled
************************************************/	
virtual void	GetMouseRelativePosition(VxVector& oPosition) = 0;

/************************************************
Summary: Tests if a mouse is attached.

Return Value:
	TRUE if a mouse is attached. FALSE otherwise.

See Also:IsKeyboardRepetitionEnabled
************************************************/	
virtual	CKBOOL  IsMouseAttached() = 0;

//*****************************************************
// Joystick Access

/************************************************
Summary: Tests if a joystick is attached

Arguments:
	iJoystick: Index of the joystick to test.
Return Value:
	TRUE if the given joystick is attached, FALSE
	otherwise.

See Also:  GetJoystickPosition
************************************************/	
virtual CKBOOL	IsJoystickAttached(int iJoystick) = 0;

/************************************************
Summary: Gets a joystick position. 

Arguments:
	iJoystick: Index of the joystick to test.
	oPosition: pointer on a VxVector that will be filled
	by the position on the 2 (or 3) joystick axis.

See Also: GetJoystickRotation, GetJoystickSliders
************************************************/	
virtual void	GetJoystickPosition(int iJoystick,VxVector* oPosition) = 0;

/************************************************
Summary: Gets a joystick rotation on its axis (if available).

Arguments:
	iJoystick: Index of the joystick to test.
	oRotation: pointer on a VxVector that will be filled 
	by the angle value on the joystick axis (0, 1, 2 or 3 where its available)

See Also: GetJoystickSliders, GetJoystickPosition
************************************************/	
virtual void	GetJoystickRotation(int iJoystick,VxVector* oRotation) = 0;

/************************************************
Summary: Gets the position of a joystick sliders (if available).

Arguments:
	iJoystick: Index of the joystick to test.
	oPosition: pointer on Vx2DVector that will be filled with
	the 2 sliders position (if available)

See Also: GetJoystickRotation, GetJoystickPosition
************************************************/	
virtual void	GetJoystickSliders(int iJoystick,Vx2DVector* oPosition) = 0;

/************************************************
Summary: Gets a joystick position of its Point of View (if available).

Arguments:
	iJoystick: Index of the joystick to test.
	oAngle: pointer on a float that will be filled with the angle 
	of the Point of View.

See Also: GetJoystickSliders, GetJoystickRotation
************************************************/	
virtual void	GetJoystickPointOfViewAngle(int iJoystick,float* oAngle) = 0;

/************************************************
Summary: Gets the buttons states of a joystick.

Arguments:
	iJoystick: Index of the joystick to test.
Return Value:
	A CKDWORD representing the state of the buttons. If a bit is
	set, the button at its position is pressed.

See Also: GetJoystickPosition, GetJoystickRotation
************************************************/	
virtual CKDWORD	GetJoystickButtonsState(int iJoystick) = 0;

/************************************************
Summary: Tests if a joystick button is down

Arguments:
	iJoystick: Index of the joystick to test.
	iButton: index of the button to test.
Return Value:
	TRUE if the button is pressed, FALSE otherwise.

See Also: GetJoystickButtonsState
************************************************/	
virtual CKBOOL	IsJoystickButtonDown(int iJoystick,int iButton) = 0;

//*****************************************************
// Suspend manager


virtual void Pause(BOOL pause) = 0;

//*****************************************************
// Cursor Access

/************************************************
Summary: Shows/Hides a cursor. 

Arguments:
	iShow: TRUE to show the cursor, FALSE to hide it.

See Also: GetCursorVisibility
************************************************/	
virtual void ShowCursor(BOOL iShow)=0;

/************************************************
Summary: Gets the cursor visibility. 

Return Value:
	TRUE if the cursor is visible, FALSE otherwise.

See Also: ShowCursor
************************************************/	
virtual BOOL GetCursorVisibility()=0;


CKInputManager(CKContext *Context,CKSTRING name):CKBaseManager(Context,INPUT_MANAGER_GUID,name) {}

virtual ~CKInputManager() {}


virtual	VXCURSOR_POINTER		GetSystemCursor() = 0;

virtual	void			SetSystemCursor( VXCURSOR_POINTER cursor ) = 0;

/************************************************
Summary: Returns the number of joystick registered in the input manager.

Return Value:
	The number of joystick registered in the input manager (as an integer).

See Also:  CheckJoystickConnected
************************************************/	
virtual int	 GetJoystickCount()=0;

/************************************************
Summary: (Re)enumerates joysticks connected to the computer to register them into in the input manager.


Remarks:
	This function is called at the input manager creation. It is also
	called on CKPlay. You can call it at runtime to detect new joystick connected
	during application is playing.

See Also: GetJoystickCount
************************************************/	
virtual void CheckJoystickConnected()=0;


};

#endif

