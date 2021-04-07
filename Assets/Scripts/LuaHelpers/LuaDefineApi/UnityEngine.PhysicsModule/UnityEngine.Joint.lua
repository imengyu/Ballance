---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Joint : Component
---@field public connectedBody Rigidbody 
---@field public connectedArticulationBody ArticulationBody 
---@field public axis Vector3 
---@field public anchor Vector3 
---@field public connectedAnchor Vector3 
---@field public autoConfigureConnectedAnchor boolean 
---@field public breakForce number 
---@field public breakTorque number 
---@field public enableCollision boolean 
---@field public enablePreprocessing boolean 
---@field public massScale number 
---@field public connectedMassScale number 
---@field public currentForce Vector3 
---@field public currentTorque Vector3 
local Joint={ }
---
UnityEngine.Joint = Joint