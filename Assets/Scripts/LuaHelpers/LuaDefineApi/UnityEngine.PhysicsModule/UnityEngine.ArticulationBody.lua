---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ArticulationBody : Behaviour
---@field public jointType number 
---@field public anchorPosition Vector3 
---@field public parentAnchorPosition Vector3 
---@field public anchorRotation Quaternion 
---@field public parentAnchorRotation Quaternion 
---@field public isRoot boolean 
---@field public linearLockX number 
---@field public linearLockY number 
---@field public linearLockZ number 
---@field public swingYLock number 
---@field public swingZLock number 
---@field public twistLock number 
---@field public xDrive ArticulationDrive 
---@field public yDrive ArticulationDrive 
---@field public zDrive ArticulationDrive 
---@field public immovable boolean 
---@field public useGravity boolean 
---@field public linearDamping number 
---@field public angularDamping number 
---@field public jointFriction number 
---@field public velocity Vector3 
---@field public angularVelocity Vector3 
---@field public mass number 
---@field public centerOfMass Vector3 
---@field public worldCenterOfMass Vector3 
---@field public inertiaTensor Vector3 
---@field public inertiaTensorRotation Quaternion 
---@field public sleepThreshold number 
---@field public solverIterations number 
---@field public solverVelocityIterations number 
---@field public maxAngularVelocity number 
---@field public maxLinearVelocity number 
---@field public maxJointVelocity number 
---@field public maxDepenetrationVelocity number 
---@field public jointPosition ArticulationReducedSpace 
---@field public jointVelocity ArticulationReducedSpace 
---@field public jointAcceleration ArticulationReducedSpace 
---@field public jointForce ArticulationReducedSpace 
---@field public dofCount number 
---@field public index number 
local ArticulationBody={ }
---
---@public
---@param force Vector3 
---@return void 
function ArticulationBody:AddForce(force) end
---
---@public
---@param force Vector3 
---@return void 
function ArticulationBody:AddRelativeForce(force) end
---
---@public
---@param torque Vector3 
---@return void 
function ArticulationBody:AddTorque(torque) end
---
---@public
---@param torque Vector3 
---@return void 
function ArticulationBody:AddRelativeTorque(torque) end
---
---@public
---@param force Vector3 
---@param position Vector3 
---@return void 
function ArticulationBody:AddForceAtPosition(force, position) end
---
---@public
---@return void 
function ArticulationBody:ResetCenterOfMass() end
---
---@public
---@return void 
function ArticulationBody:ResetInertiaTensor() end
---
---@public
---@return void 
function ArticulationBody:Sleep() end
---
---@public
---@return boolean 
function ArticulationBody:IsSleeping() end
---
---@public
---@return void 
function ArticulationBody:WakeUp() end
---
---@public
---@param position Vector3 
---@param rotation Quaternion 
---@return void 
function ArticulationBody:TeleportRoot(position, rotation) end
---
---@public
---@param point Vector3 
---@return Vector3 
function ArticulationBody:GetClosestPoint(point) end
---
---@public
---@param relativePoint Vector3 
---@return Vector3 
function ArticulationBody:GetRelativePointVelocity(relativePoint) end
---
---@public
---@param worldPoint Vector3 
---@return Vector3 
function ArticulationBody:GetPointVelocity(worldPoint) end
---
---@public
---@param jacobian ArticulationJacobian& 
---@return number 
function ArticulationBody:GetDenseJacobian(jacobian) end
---
---@public
---@param positions List`1 
---@return number 
function ArticulationBody:GetJointPositions(positions) end
---
---@public
---@param positions List`1 
---@return void 
function ArticulationBody:SetJointPositions(positions) end
---
---@public
---@param velocities List`1 
---@return number 
function ArticulationBody:GetJointVelocities(velocities) end
---
---@public
---@param velocities List`1 
---@return void 
function ArticulationBody:SetJointVelocities(velocities) end
---
---@public
---@param accelerations List`1 
---@return number 
function ArticulationBody:GetJointAccelerations(accelerations) end
---
---@public
---@param accelerations List`1 
---@return void 
function ArticulationBody:SetJointAccelerations(accelerations) end
---
---@public
---@param forces List`1 
---@return number 
function ArticulationBody:GetJointForces(forces) end
---
---@public
---@param forces List`1 
---@return void 
function ArticulationBody:SetJointForces(forces) end
---
---@public
---@param targets List`1 
---@return number 
function ArticulationBody:GetDriveTargets(targets) end
---
---@public
---@param targets List`1 
---@return void 
function ArticulationBody:SetDriveTargets(targets) end
---
---@public
---@param targetVelocities List`1 
---@return number 
function ArticulationBody:GetDriveTargetVelocities(targetVelocities) end
---
---@public
---@param targetVelocities List`1 
---@return void 
function ArticulationBody:SetDriveTargetVelocities(targetVelocities) end
---
---@public
---@param dofStartIndices List`1 
---@return number 
function ArticulationBody:GetDofStartIndices(dofStartIndices) end
---
UnityEngine.ArticulationBody = ArticulationBody