---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Graphics
---@field public activeColorGamut number 
---@field public activeTier number 
---@field public preserveFramebufferAlpha boolean 
---@field public minOpenGLESVersion number 
---@field public activeColorBuffer RenderBuffer 
---@field public activeDepthBuffer RenderBuffer 
---@field public deviceName string 
---@field public deviceVendor string 
---@field public deviceVersion string 
local Graphics={ }
---
---@public
---@return void 
function Graphics.ClearRandomWriteTargets() end
---
---@public
---@param buffer CommandBuffer 
---@return void 
function Graphics.ExecuteCommandBuffer(buffer) end
---
---@public
---@param buffer CommandBuffer 
---@param queueType number 
---@return void 
function Graphics.ExecuteCommandBufferAsync(buffer, queueType) end
---
---@public
---@param rt RenderTexture 
---@param mipLevel number 
---@param face number 
---@param depthSlice number 
---@return void 
function Graphics.SetRenderTarget(rt, mipLevel, face, depthSlice) end
---
---@public
---@param colorBuffer RenderBuffer 
---@param depthBuffer RenderBuffer 
---@param mipLevel number 
---@param face number 
---@param depthSlice number 
---@return void 
function Graphics.SetRenderTarget(colorBuffer, depthBuffer, mipLevel, face, depthSlice) end
---
---@public
---@param colorBuffers RenderBuffer[] 
---@param depthBuffer RenderBuffer 
---@return void 
function Graphics.SetRenderTarget(colorBuffers, depthBuffer) end
---
---@public
---@param setup RenderTargetSetup 
---@return void 
function Graphics.SetRenderTarget(setup) end
---
---@public
---@param index number 
---@param uav RenderTexture 
---@return void 
function Graphics.SetRandomWriteTarget(index, uav) end
---
---@public
---@param index number 
---@param uav ComputeBuffer 
---@param preserveCounterValue boolean 
---@return void 
function Graphics.SetRandomWriteTarget(index, uav, preserveCounterValue) end
---
---@public
---@param index number 
---@param uav GraphicsBuffer 
---@param preserveCounterValue boolean 
---@return void 
function Graphics.SetRandomWriteTarget(index, uav, preserveCounterValue) end
---
---@public
---@param src Texture 
---@param dst Texture 
---@return void 
function Graphics.CopyTexture(src, dst) end
---
---@public
---@param src Texture 
---@param srcElement number 
---@param dst Texture 
---@param dstElement number 
---@return void 
function Graphics.CopyTexture(src, srcElement, dst, dstElement) end
---
---@public
---@param src Texture 
---@param srcElement number 
---@param srcMip number 
---@param dst Texture 
---@param dstElement number 
---@param dstMip number 
---@return void 
function Graphics.CopyTexture(src, srcElement, srcMip, dst, dstElement, dstMip) end
---
---@public
---@param src Texture 
---@param srcElement number 
---@param srcMip number 
---@param srcX number 
---@param srcY number 
---@param srcWidth number 
---@param srcHeight number 
---@param dst Texture 
---@param dstElement number 
---@param dstMip number 
---@param dstX number 
---@param dstY number 
---@return void 
function Graphics.CopyTexture(src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight, dst, dstElement, dstMip, dstX, dstY) end
---
---@public
---@param src Texture 
---@param dst Texture 
---@return boolean 
function Graphics.ConvertTexture(src, dst) end
---
---@public
---@param src Texture 
---@param srcElement number 
---@param dst Texture 
---@param dstElement number 
---@return boolean 
function Graphics.ConvertTexture(src, srcElement, dst, dstElement) end
---
---@public
---@param stage number 
---@return GraphicsFence 
function Graphics.CreateAsyncGraphicsFence(stage) end
---
---@public
---@return GraphicsFence 
function Graphics.CreateAsyncGraphicsFence() end
---
---@public
---@param fenceType number 
---@param stage number 
---@return GraphicsFence 
function Graphics.CreateGraphicsFence(fenceType, stage) end
---
---@public
---@param fence GraphicsFence 
---@return void 
function Graphics.WaitOnAsyncGraphicsFence(fence) end
---
---@public
---@param fence GraphicsFence 
---@param stage number 
---@return void 
function Graphics.WaitOnAsyncGraphicsFence(fence, stage) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param sourceRect Rect 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@param color Color 
---@param mat Material 
---@param pass number 
---@return void 
function Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat, pass) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param sourceRect Rect 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@param mat Material 
---@param pass number 
---@return void 
function Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, mat, pass) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@param mat Material 
---@param pass number 
---@return void 
function Graphics.DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat, pass) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param mat Material 
---@param pass number 
---@return void 
function Graphics.DrawTexture(screenRect, texture, mat, pass) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param materialIndex number 
---@return void 
function Graphics.DrawMeshNow(mesh, position, rotation, materialIndex) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param materialIndex number 
---@return void 
function Graphics.DrawMeshNow(mesh, matrix, materialIndex) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@return void 
function Graphics.DrawMeshNow(mesh, position, rotation) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@return void 
function Graphics.DrawMeshNow(mesh, matrix) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows boolean 
---@param receiveShadows boolean 
---@param useLightProbes boolean 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, useLightProbes) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param probeAnchor Transform 
---@param useLightProbes boolean 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, useLightProbes) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows boolean 
---@param receiveShadows boolean 
---@param useLightProbes boolean 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, useLightProbes) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param probeAnchor Transform 
---@param lightProbeUsage number 
---@param lightProbeProxyVolume LightProbeProxyVolume 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, lightProbeUsage, lightProbeProxyVolume) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@param count number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@param lightProbeProxyVolume LightProbeProxyVolume 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices List`1 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@param lightProbeProxyVolume LightProbeProxyVolume 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param bounds Bounds 
---@param count number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@param lightProbeProxyVolume LightProbeProxyVolume 
---@return void 
function Graphics.DrawMeshInstancedProcedural(mesh, submeshIndex, material, bounds, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param bounds Bounds 
---@param bufferWithArgs ComputeBuffer 
---@param argsOffset number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@param lightProbeProxyVolume LightProbeProxyVolume 
---@return void 
function Graphics.DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param bounds Bounds 
---@param bufferWithArgs GraphicsBuffer 
---@param argsOffset number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@param lightProbeProxyVolume LightProbeProxyVolume 
---@return void 
function Graphics.DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume) end
---
---@public
---@param topology number 
---@param vertexCount number 
---@param instanceCount number 
---@return void 
function Graphics.DrawProceduralNow(topology, vertexCount, instanceCount) end
---
---@public
---@param topology number 
---@param indexBuffer GraphicsBuffer 
---@param indexCount number 
---@param instanceCount number 
---@return void 
function Graphics.DrawProceduralNow(topology, indexBuffer, indexCount, instanceCount) end
---
---@public
---@param topology number 
---@param bufferWithArgs ComputeBuffer 
---@param argsOffset number 
---@return void 
function Graphics.DrawProceduralIndirectNow(topology, bufferWithArgs, argsOffset) end
---
---@public
---@param topology number 
---@param indexBuffer GraphicsBuffer 
---@param bufferWithArgs ComputeBuffer 
---@param argsOffset number 
---@return void 
function Graphics.DrawProceduralIndirectNow(topology, indexBuffer, bufferWithArgs, argsOffset) end
---
---@public
---@param topology number 
---@param bufferWithArgs GraphicsBuffer 
---@param argsOffset number 
---@return void 
function Graphics.DrawProceduralIndirectNow(topology, bufferWithArgs, argsOffset) end
---
---@public
---@param topology number 
---@param indexBuffer GraphicsBuffer 
---@param bufferWithArgs GraphicsBuffer 
---@param argsOffset number 
---@return void 
function Graphics.DrawProceduralIndirectNow(topology, indexBuffer, bufferWithArgs, argsOffset) end
---
---@public
---@param material Material 
---@param bounds Bounds 
---@param topology number 
---@param vertexCount number 
---@param instanceCount number 
---@param camera Camera 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@return void 
function Graphics.DrawProcedural(material, bounds, topology, vertexCount, instanceCount, camera, properties, castShadows, receiveShadows, layer) end
---
---@public
---@param material Material 
---@param bounds Bounds 
---@param topology number 
---@param indexBuffer GraphicsBuffer 
---@param indexCount number 
---@param instanceCount number 
---@param camera Camera 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@return void 
function Graphics.DrawProcedural(material, bounds, topology, indexBuffer, indexCount, instanceCount, camera, properties, castShadows, receiveShadows, layer) end
---
---@public
---@param material Material 
---@param bounds Bounds 
---@param topology number 
---@param bufferWithArgs ComputeBuffer 
---@param argsOffset number 
---@param camera Camera 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@return void 
function Graphics.DrawProceduralIndirect(material, bounds, topology, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer) end
---
---@public
---@param material Material 
---@param bounds Bounds 
---@param topology number 
---@param indexBuffer GraphicsBuffer 
---@param bufferWithArgs ComputeBuffer 
---@param argsOffset number 
---@param camera Camera 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@return void 
function Graphics.DrawProceduralIndirect(material, bounds, topology, indexBuffer, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@return void 
function Graphics.Blit(source, dest) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@param sourceDepthSlice number 
---@param destDepthSlice number 
---@return void 
function Graphics.Blit(source, dest, sourceDepthSlice, destDepthSlice) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@param scale Vector2 
---@param offset Vector2 
---@return void 
function Graphics.Blit(source, dest, scale, offset) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@param scale Vector2 
---@param offset Vector2 
---@param sourceDepthSlice number 
---@param destDepthSlice number 
---@return void 
function Graphics.Blit(source, dest, scale, offset, sourceDepthSlice, destDepthSlice) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@param mat Material 
---@param pass number 
---@return void 
function Graphics.Blit(source, dest, mat, pass) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@param mat Material 
---@param pass number 
---@param destDepthSlice number 
---@return void 
function Graphics.Blit(source, dest, mat, pass, destDepthSlice) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@param mat Material 
---@return void 
function Graphics.Blit(source, dest, mat) end
---
---@public
---@param source Texture 
---@param mat Material 
---@param pass number 
---@return void 
function Graphics.Blit(source, mat, pass) end
---
---@public
---@param source Texture 
---@param mat Material 
---@param pass number 
---@param destDepthSlice number 
---@return void 
function Graphics.Blit(source, mat, pass, destDepthSlice) end
---
---@public
---@param source Texture 
---@param mat Material 
---@return void 
function Graphics.Blit(source, mat) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@param mat Material 
---@param offsets Vector2[] 
---@return void 
function Graphics.BlitMultiTap(source, dest, mat, offsets) end
---
---@public
---@param source Texture 
---@param dest RenderTexture 
---@param mat Material 
---@param destDepthSlice number 
---@param offsets Vector2[] 
---@return void 
function Graphics.BlitMultiTap(source, dest, mat, destDepthSlice, offsets) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param materialIndex number 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, materialIndex) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@return void 
function Graphics.DrawMesh(mesh, matrix) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param materialIndex number 
---@return void 
function Graphics.DrawMesh(mesh, matrix, materialIndex) end
---
---@public
---@param topology number 
---@param vertexCount number 
---@param instanceCount number 
---@return void 
function Graphics.DrawProcedural(topology, vertexCount, instanceCount) end
---
---@public
---@param topology number 
---@param bufferWithArgs ComputeBuffer 
---@param argsOffset number 
---@return void 
function Graphics.DrawProceduralIndirect(topology, bufferWithArgs, argsOffset) end
---
---@public
---@param stage number 
---@return GPUFence 
function Graphics.CreateGPUFence(stage) end
---
---@public
---@param fence GPUFence 
---@param stage number 
---@return void 
function Graphics.WaitOnGPUFence(fence, stage) end
---
---@public
---@return GPUFence 
function Graphics.CreateGPUFence() end
---
---@public
---@param fence GPUFence 
---@return void 
function Graphics.WaitOnGPUFence(fence) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows boolean 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows boolean 
---@param receiveShadows boolean 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows) end
---
---@public
---@param mesh Mesh 
---@param position Vector3 
---@param rotation Quaternion 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param probeAnchor Transform 
---@return void 
function Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows boolean 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows boolean 
---@param receiveShadows boolean 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param probeAnchor Transform 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param probeAnchor Transform 
---@param useLightProbes boolean 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, useLightProbes) end
---
---@public
---@param mesh Mesh 
---@param matrix Matrix4x4 
---@param material Material 
---@param layer number 
---@param camera Camera 
---@param submeshIndex number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param probeAnchor Transform 
---@param lightProbeUsage number 
---@return void 
function Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, lightProbeUsage) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@param count number 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@param count number 
---@param properties MaterialPropertyBlock 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@param count number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@param count number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@param count number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@param count number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices Matrix4x4[] 
---@param count number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices List`1 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices List`1 
---@param properties MaterialPropertyBlock 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices List`1 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices List`1 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices List`1 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices List`1 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param matrices List`1 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@return void 
function Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param bounds Bounds 
---@param bufferWithArgs ComputeBuffer 
---@param argsOffset number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@return void 
function Graphics.DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage) end
---
---@public
---@param mesh Mesh 
---@param submeshIndex number 
---@param material Material 
---@param bounds Bounds 
---@param bufferWithArgs GraphicsBuffer 
---@param argsOffset number 
---@param properties MaterialPropertyBlock 
---@param castShadows number 
---@param receiveShadows boolean 
---@param layer number 
---@param camera Camera 
---@param lightProbeUsage number 
---@return void 
function Graphics.DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param sourceRect Rect 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@param color Color 
---@param mat Material 
---@return void 
function Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param sourceRect Rect 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@param color Color 
---@return void 
function Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param sourceRect Rect 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@param mat Material 
---@return void 
function Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, mat) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param sourceRect Rect 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@return void 
function Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@param mat Material 
---@return void 
function Graphics.DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param leftBorder number 
---@param rightBorder number 
---@param topBorder number 
---@param bottomBorder number 
---@return void 
function Graphics.DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@param mat Material 
---@return void 
function Graphics.DrawTexture(screenRect, texture, mat) end
---
---@public
---@param screenRect Rect 
---@param texture Texture 
---@return void 
function Graphics.DrawTexture(screenRect, texture) end
---
---@public
---@param rt RenderTexture 
---@return void 
function Graphics.SetRenderTarget(rt) end
---
---@public
---@param rt RenderTexture 
---@param mipLevel number 
---@return void 
function Graphics.SetRenderTarget(rt, mipLevel) end
---
---@public
---@param rt RenderTexture 
---@param mipLevel number 
---@param face number 
---@return void 
function Graphics.SetRenderTarget(rt, mipLevel, face) end
---
---@public
---@param colorBuffer RenderBuffer 
---@param depthBuffer RenderBuffer 
---@return void 
function Graphics.SetRenderTarget(colorBuffer, depthBuffer) end
---
---@public
---@param colorBuffer RenderBuffer 
---@param depthBuffer RenderBuffer 
---@param mipLevel number 
---@return void 
function Graphics.SetRenderTarget(colorBuffer, depthBuffer, mipLevel) end
---
---@public
---@param colorBuffer RenderBuffer 
---@param depthBuffer RenderBuffer 
---@param mipLevel number 
---@param face number 
---@return void 
function Graphics.SetRenderTarget(colorBuffer, depthBuffer, mipLevel, face) end
---
---@public
---@param index number 
---@param uav ComputeBuffer 
---@return void 
function Graphics.SetRandomWriteTarget(index, uav) end
---
---@public
---@param index number 
---@param uav GraphicsBuffer 
---@return void 
function Graphics.SetRandomWriteTarget(index, uav) end
---
UnityEngine.Graphics = Graphics