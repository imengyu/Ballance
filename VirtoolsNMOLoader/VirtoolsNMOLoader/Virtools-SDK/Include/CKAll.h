/*************************************************************************/
/*	File : CKAll.h														 */
/*	Main Header file : Includes all necessary files for Virtools SDK	 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKALL_H 

#define CKALL_H

// Defines And Global Functions
#include "CKDefines2.h"
#include "CKGlobals.h"
#include "CKBaseManager.h"
#include "CKContext.h"
#include "CKInterfaceObjectManager.h"

// Managers    
#include "CKVariableManager.h"
#include "CKParameterManager.h"
#include "CKTimeManager.h"
#include "CKMessageManager.h"
#include "CKRenderManager.h"
#include "CKBehaviorManager.h"
#include "CKAttributeManager.h"
#include "CKPluginManager.h"
#include "CKPathManager.h"

// External Managers
#include "CKFloorManager.h"
#include "CKGridManager.h"
#include "CKInterfaceManager.h"
#include "CKSoundManager.h"
#include "CKMidiManager.h"
#include "CKInputManager.h"
#include "CKCollisionManager.h"

// Misc

#include "CKShader.h"


#include "CKMaterial.h"
#include "CKTexture.h"
#include "CKRenderContext.h"
#include "CKSynchroObject.h"
// Parameters
#include "CKParameter.h"
#include "CKParameterIn.h"
#include "CKParameterOut.h"
#include "CKParameterLocal.h"
#include "CKParameterOperation.h"
#include "CKParameterVariable.h"
// Behaviors
#include "CKBehaviorIO.h"
#include "CKBehaviorLink.h"
#include "CKBehaviorPrototype.h"
#include "CKBehavior.h"
#include "CKMessage.h"
#include "CKObjectDeclaration.h"
// Level/Scene/place
#include "CKLevel.h"
#include "CKPlace.h"
#include "CKGroup.h"
#include "CKScene.h"
// Save/load
#include "CKStateChunk.h"
#include "CKFile.h"
// Sound
#include "CKSound.h"
#include "CKWaveSound.h"
#include "CKMidiSound.h"
#include "CKSoundReader.h"
// Curves
#include "CK2dCurve.h"
#include "CK2dCurvePoint.h"
#include "CKCurve.h"
#include "CKCurvePoint.h"
// Character and Animation
#include "CKAnimation.h"
#include "CKKeyedAnimation.h"
#include "CKObjectAnimation.h"
#include "CKKinematicChain.h"
#include "CKCharacter.h"
// Base Objects
#include "CKObject.h"
#include "CKSceneObject.h"
#include "CKRenderObject.h"
#include "CKBeObject.h"
#include "CKDependencies.h"
// 2d Objects
#include "CK2dEntity.h"
#include "CKSprite.h"
#include "CKSpriteText.h"
// 3d Objects
#include "CKMesh.h"
#include "CKPatchMesh.h"
#include "CK3dEntity.h"
#include "CKCamera.h"
#include "CKTargetCamera.h"
#include "CKSprite3D.h"
#include "CKLight.h"
#include "CKTargetLight.h"
#include "CK3dObject.h"
#include "CKBodyPart.h"
// Containers
#include "CKDataArray.h"
#include "CKDebugContext.h"

#ifdef VIRTOOLS_RUNTIME_VERSION
	#ifdef CKBEHAVIOR_VARIABLEINPUTS
		#undef CKBEHAVIOR_VARIABLEINPUTS	
	#endif

	#ifdef CKBEHAVIOR_VARIABLEOUTPUTS
		#undef CKBEHAVIOR_VARIABLEOUTPUTS	
	#endif

	#ifdef CKBEHAVIOR_VARIABLEPARAMETERINPUTS
		#undef CKBEHAVIOR_VARIABLEPARAMETERINPUTS	
	#endif
	
	#ifdef CKBEHAVIOR_VARIABLEPARAMETERINPUTS
		#undef CKBEHAVIOR_VARIABLEPARAMETERINPUTS	
	#endif
	
	#define	CKBEHAVIOR_VARIABLEINPUTS			((CK_BEHAVIOR_FLAGS)0)	
	#define	CKBEHAVIOR_VARIABLEOUTPUTS			((CK_BEHAVIOR_FLAGS)0)	
	#define	CKBEHAVIOR_VARIABLEPARAMETERINPUTS	((CK_BEHAVIOR_FLAGS)0)	
	#define	CKBEHAVIOR_VARIABLEPARAMETERINPUTS	((CK_BEHAVIOR_FLAGS)0)	

#endif

#endif


