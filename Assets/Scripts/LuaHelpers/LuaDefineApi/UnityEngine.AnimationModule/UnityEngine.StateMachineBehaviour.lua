---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StateMachineBehaviour : ScriptableObject
local StateMachineBehaviour={ }
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@return void 
function StateMachineBehaviour:OnStateEnter(animator, stateInfo, layerIndex) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@return void 
function StateMachineBehaviour:OnStateUpdate(animator, stateInfo, layerIndex) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@return void 
function StateMachineBehaviour:OnStateExit(animator, stateInfo, layerIndex) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@return void 
function StateMachineBehaviour:OnStateMove(animator, stateInfo, layerIndex) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@return void 
function StateMachineBehaviour:OnStateIK(animator, stateInfo, layerIndex) end
---
---@public
---@param animator Animator 
---@param stateMachinePathHash number 
---@return void 
function StateMachineBehaviour:OnStateMachineEnter(animator, stateMachinePathHash) end
---
---@public
---@param animator Animator 
---@param stateMachinePathHash number 
---@return void 
function StateMachineBehaviour:OnStateMachineExit(animator, stateMachinePathHash) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@param controller AnimatorControllerPlayable 
---@return void 
function StateMachineBehaviour:OnStateEnter(animator, stateInfo, layerIndex, controller) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@param controller AnimatorControllerPlayable 
---@return void 
function StateMachineBehaviour:OnStateUpdate(animator, stateInfo, layerIndex, controller) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@param controller AnimatorControllerPlayable 
---@return void 
function StateMachineBehaviour:OnStateExit(animator, stateInfo, layerIndex, controller) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@param controller AnimatorControllerPlayable 
---@return void 
function StateMachineBehaviour:OnStateMove(animator, stateInfo, layerIndex, controller) end
---
---@public
---@param animator Animator 
---@param stateInfo AnimatorStateInfo 
---@param layerIndex number 
---@param controller AnimatorControllerPlayable 
---@return void 
function StateMachineBehaviour:OnStateIK(animator, stateInfo, layerIndex, controller) end
---
---@public
---@param animator Animator 
---@param stateMachinePathHash number 
---@param controller AnimatorControllerPlayable 
---@return void 
function StateMachineBehaviour:OnStateMachineEnter(animator, stateMachinePathHash, controller) end
---
---@public
---@param animator Animator 
---@param stateMachinePathHash number 
---@param controller AnimatorControllerPlayable 
---@return void 
function StateMachineBehaviour:OnStateMachineExit(animator, stateMachinePathHash, controller) end
---
UnityEngine.StateMachineBehaviour = StateMachineBehaviour