---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GL
---@field public TRIANGLES number 
---@field public TRIANGLE_STRIP number 
---@field public QUADS number 
---@field public LINES number 
---@field public LINE_STRIP number 
---@field public wireframe boolean 
---@field public sRGBWrite boolean 
---@field public invertCulling boolean 
---@field public modelview Matrix4x4 
local GL={ }
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return void 
function GL.Vertex3(x, y, z) end
---
---@public
---@param v Vector3 
---@return void 
function GL.Vertex(v) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return void 
function GL.TexCoord3(x, y, z) end
---
---@public
---@param v Vector3 
---@return void 
function GL.TexCoord(v) end
---
---@public
---@param x number 
---@param y number 
---@return void 
function GL.TexCoord2(x, y) end
---
---@public
---@param unit number 
---@param x number 
---@param y number 
---@param z number 
---@return void 
function GL.MultiTexCoord3(unit, x, y, z) end
---
---@public
---@param unit number 
---@param v Vector3 
---@return void 
function GL.MultiTexCoord(unit, v) end
---
---@public
---@param unit number 
---@param x number 
---@param y number 
---@return void 
function GL.MultiTexCoord2(unit, x, y) end
---
---@public
---@param c Color 
---@return void 
function GL.Color(c) end
---
---@public
---@return void 
function GL.Flush() end
---
---@public
---@return void 
function GL.RenderTargetBarrier() end
---
---@public
---@param m Matrix4x4 
---@return void 
function GL.MultMatrix(m) end
---
---@public
---@param eventID number 
---@return void 
function GL.IssuePluginEvent(eventID) end
---
---@public
---@param revertBackFaces boolean 
---@return void 
function GL.SetRevertBackfacing(revertBackFaces) end
---
---@public
---@return void 
function GL.PushMatrix() end
---
---@public
---@return void 
function GL.PopMatrix() end
---
---@public
---@return void 
function GL.LoadIdentity() end
---
---@public
---@return void 
function GL.LoadOrtho() end
---
---@public
---@return void 
function GL.LoadPixelMatrix() end
---
---@public
---@param mat Matrix4x4 
---@return void 
function GL.LoadProjectionMatrix(mat) end
---
---@public
---@return void 
function GL.InvalidateState() end
---
---@public
---@param proj Matrix4x4 
---@param renderIntoTexture boolean 
---@return Matrix4x4 
function GL.GetGPUProjectionMatrix(proj, renderIntoTexture) end
---
---@public
---@param left number 
---@param right number 
---@param bottom number 
---@param top number 
---@return void 
function GL.LoadPixelMatrix(left, right, bottom, top) end
---
---@public
---@param callback IntPtr 
---@param eventID number 
---@return void 
function GL.IssuePluginEvent(callback, eventID) end
---
---@public
---@param mode number 
---@return void 
function GL.Begin(mode) end
---
---@public
---@return void 
function GL.End() end
---
---@public
---@param clearDepth boolean 
---@param clearColor boolean 
---@param backgroundColor Color 
---@param depth number 
---@return void 
function GL.Clear(clearDepth, clearColor, backgroundColor, depth) end
---
---@public
---@param clearDepth boolean 
---@param clearColor boolean 
---@param backgroundColor Color 
---@return void 
function GL.Clear(clearDepth, clearColor, backgroundColor) end
---
---@public
---@param pixelRect Rect 
---@return void 
function GL.Viewport(pixelRect) end
---
---@public
---@param clearDepth boolean 
---@param camera Camera 
---@return void 
function GL.ClearWithSkybox(clearDepth, camera) end
---
UnityEngine.GL = GL