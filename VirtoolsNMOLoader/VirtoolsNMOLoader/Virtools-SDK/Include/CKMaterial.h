/*************************************************************************/
/*	File : CKMaterial.h													 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKMATERIAL_H

#define CKMATERIAL_H "$Id:$"

#include "CKBeObject.h" 

#ifndef NO_SHADER
class CKShader;
class CKShaderTechnique;

/*********************************************************************************
Name: Material Shader
Summary: CKShader used by a CKMaterial and its parameters.

Remarks:
	The material effect structure hold information about what shader (CKShader) is used on the
material. It also store which technique is used and the parameters, specific to the material 
that are given to the effect.
See also: CKMaterial::SetMaterialShader,CKShader,
********************************************************************************/
class	CKMaterialShader
{
public:
	CKShader*					m_Shader;					// The shader used for the rendering.
	int							m_CompID;					// Last compilationID (used internally at runtime).
	XString						m_TechniqueName;			// Technique wanted technique
	CKBOOL						m_FindNextValidTechnique;	// If try to find the next valid technique from the one selected.
	int							m_TechIndex;				// The real used technique
	int							m_MaterialClass;			// Not Yet Used : Where does it go in the pipeline : Additive, Transparent(sorted), Solid.
	XArray<CKParameterLocal*>	m_Params;					// Parameters for the effect.
	VxMatrix					m_ShadowViewProj;           // Special, for shadow mapping using channels : map from world
	                                                        // to shadow map space
	void MakeParamsUpToDate();
	CKBOOL SetTechniqueByName(const CKSTRING techName, CKBOOL findNextValidTechnique = FALSE);
};

#endif

/***********************************************************************
{filename:CKMaterial}
Summary: Color and texture settings for objects.

Remarks:
	+The CKMaterial class provides functions to set color settings for faces of an object, a sprite3D or a 2DEntity.
	+It specified the way faces will be rendered (eg. Transparency, Texture to use, Blend modes,
	Texture filter modes, Fill and shade modes, etc..)

	+The default diffuse color is (0.7,0.7,0.7)
	+The default ambient color is (0.3,0.3,0.3,1.0)
	+The default specular color is (0.5,0.5,0.5) but specular power is 0 (specular is disabled)
	+The default emissive color is (0,0,0)
	+The material is single sided.
	+There is no texture .
	+Default filter is set to VXTEXTUREFILTER_LINEAR. (Bilinear filtering)
	+Default blend factors are (VXBLEND_ONE,VXBLEND_ZERO) and blending is disabled.
	+Default shade mode is VXSHADE_GOURAUD.
	+Default fill mode is VXFILL_SOLID.
	+Alpha Testing is Off.
	+ZBuffer writing is enabled.
	+Its class id is CKCID_MATERIAL



See also: Using Materials,CKTexture,CKMesh,CKSprite3D,CK2dEntity
***********************************************************************/
class CKMaterial : public CKBeObject {
public :

/*************************************************
Summary: Gets the specular highlight power.
Return Value: Specular power.
Remarks:
	+ A specular power of 0 disables specular highlights calculation.

See also: SetPower,GetSpecular
*************************************************/
virtual 	float	GetPower() = 0;
/*************************************************
Summary: Sets the specular highlight power.
Arguments:
	Value: Specular power.
Remarks:
+Specular power controls the size and appearance of specular highlights on the material.
+Larger values will create sharper highlights.
+A specular power of 0 disables specular highlights calculation.

See Also:GetPower,GetSpeculat
*************************************************/
virtual 	void	SetPower(float Value) = 0;

/*************************************************
Summary: Gets the ambient color of the material.
Return Value: A reference to the VxColor that contains the material ambient color

See also: SetAmbient,SetDiffuse, SetSpecular, SetEmissive
*************************************************/
virtual		const	VxColor& GetAmbient() = 0;
/*************************************************
Summary: Sets the ambient color of the material
Arguments:
	Color : A reference a VxColor that contains the material ambient color
Remarks:

See also:SetDiffuse, SetSpecular, SetEmissive
*************************************************/
virtual 	void	SetAmbient(const VxColor& Color) = 0;

/*************************************************
Summary: Gets the diffuse color of the material
Return value: A reference to a VxColor that contains the material diffuse color

see also: SetAmbient,SetDiffuse, SetSpecular, SetEmissive
*************************************************/
virtual		const	VxColor& GetDiffuse() = 0;
/*************************************************
Summary: Sets the diffuse color of the material
Arguments:
	Color : A reference to a VxColor that contains the material diffuse color
Remarks:

See also:SetAmbient,GetDiffuse, SetSpecular, SetEmissive
*************************************************/
virtual 	void	SetDiffuse(const VxColor& Color) = 0;

/*************************************************
Summary: Sets the specular color of the material.
Return value: A reference to a VxColor that contains the material specular color

see also: SetAmbient,SetDiffuse, SetSpecular, SetEmissive,SetPower
*************************************************/
virtual		const	VxColor& GetSpecular() = 0;
/*************************************************
Summary: Sets the specular color of the material
Arguments:
	Color : A reference to a VxColor that contains the material specular color
Remarks:

see also: SetAmbient,SetDiffuse, GetSpecular, SetEmissive
*************************************************/
virtual 	void	SetSpecular(const VxColor& Color) = 0;

/*************************************************
Summary: Sets the emissive color of the material
Return value: A reference to a VxColor that contains the material emissive color

see also: SetAmbient,SetDiffuse, GetSpecular, GetEmissive
*************************************************/
virtual		const	VxColor& GetEmissive() = 0;
/*************************************************
Summary: Sets the emissive color of the material
Arguments:
	Color : A reference to a VxColor that contains the material emissive color
Remarks:

See also:SetAmbient, SetDiffuse, SetSpecular
*************************************************/
virtual 	void	SetEmissive(const VxColor& Color) = 0;

//--------------------------------------------------------------
//  TEXTURE 

/*************************************************
Summary:  Gets the texture of the material
Return value: Pointer to CKTexture

See also: SetTexture,SetTextureBlendMode
*************************************************/
virtual	CKTexture*		GetTexture(int TexIndex = 0) = 0;


/*************************************************
Summary: Sets the texture to the material

Arguments: 
	Tex: A Pointer to the texture to use.
See also: GetTexture,SetTextureBlendMode
*************************************************/
virtual	void			SetTexture(CKTexture * Tex) = 0;

//--------------------------------------------------------------
//  TEXTURE BLEND MODE

/*************************************************
Summary: Sets texture blend mode to the material

Arguments:
	BlendMode: Texture blend mode
Remarks:
	+ The texture blend mode specifies the way the texture and face color are mixed.
See also: GetTextureBlendMode,GetTexture
*************************************************/
virtual void	SetTextureBlendMode(VXTEXTURE_BLENDMODE BlendMode) = 0;

/*************************************************
Summary: Gets texture blend mode of the material.
Return value: Texture blend mode

See also: SetTextureBlendMode,GetTexture
*************************************************/
virtual VXTEXTURE_BLENDMODE	GetTextureBlendMode() = 0;

//--------------------------------------------------------------
//  TEXTURE FILTER MODE 

/*************************************************
Summary: Sets the texture filter mode when the texture is minified.

Arguments:
	FilterMode: Texture filter mode.
See also: GetTextureMinMode,GetTextureMagMode,GetTexture
*************************************************/
virtual 	void	SetTextureMinMode(VXTEXTURE_FILTERMODE FilterMode) = 0;

/*************************************************
Summary: Gets the texture filter mode when the texture is minified.
Return Value: Texture filter mode.

See also: SetTextureMinMode,GetTextureMagMode
*************************************************/
virtual 	VXTEXTURE_FILTERMODE	GetTextureMinMode() = 0;

/*************************************************
Summary: Sets the texture filter mode when the texture is magnified.

Arguments:
	FilterMode: Texture filter mode.
See also: GetTextureMinMode,GetTextureMagMode,GetTexture
*************************************************/
virtual 	void	SetTextureMagMode(VXTEXTURE_FILTERMODE FilterMode) = 0;

/*************************************************
Summary: Sets the texture filter mode when the texture is magnified.
Return Value: Texture filter mode.

See also: SetTextureMinMode,SetTextureMagMode,GetTexture
*************************************************/
virtual 	VXTEXTURE_FILTERMODE	GetTextureMagMode() = 0;

//--------------------------------------------------------------
//  TEXTURE ADDRES MODE 

/*************************************************
Summary: Sets the texture Address mode

Arguments:
	Mode: Texture address mode.
Remarks:
	+ The address mode controls how the texture coordinates outside the range 0..1 
	will be interpreted.
See also: GetTextureAddressMode,VXTEXTURE_ADDRESSMODE,GetTexture
*************************************************/
virtual 	void	SetTextureAddressMode(VXTEXTURE_ADDRESSMODE Mode) = 0;

/*************************************************
Summary: Gets the texture Address mode.
Return value: Texture Address mode

See also: SetTextureAddressMode,VXTEXTURE_ADDRESSMODE
*************************************************/
virtual 	VXTEXTURE_ADDRESSMODE	GetTextureAddressMode() = 0;

//--------------------------------------------------------------
//  TEXTURE BORDER COLOR

/*************************************************
Summary: Sets texture border color.

Arguments:
	color: Border color
Remarks:
	+ The border color is used when the texture address mode is VXTEXTURE_ADDRESSBORDER.
	+ The border color features is rarely supported by current video cards.
See also: GetTextureBorderColor,SetTextureAddressMode
*************************************************/
virtual 	void		SetTextureBorderColor(CKDWORD Color) = 0;

/*************************************************
Summary: Gets texture color to the material border.

Return value: Texture Border color
Remarks:
	+ The border color is used when the texture address mode is VXTEXTURE_ADDRESSBORDER.
	+ The border color features is rarely supported by current video cards.
See also: SetTextureBorderColor,SetTextureAddressMode
*************************************************/
virtual 	CKDWORD		GetTextureBorderColor() = 0;

//--------------------------------------------------------------
//  BLEND FACTORS 
  
/*************************************************
Summary: Sets source blend factor.

Arguments:
	BlendMode: Source blend factor
Remarks:
	+ See VXBLEND_MODE documentation for more details on the blend modes.
See also: GetSourceBlend, VXBLEND_MODE
*************************************************/
virtual	void SetSourceBlend(VXBLEND_MODE BlendMode) = 0;
  
/*************************************************
Summary: Sets destination blend factor.

Arguments:
	BlendMode: Destination blend factor
Remarks:
	+ See VXBLEND_MODE documentation for more details on the blend modes.
See also: GetDestBlend, VXBLEND_MODE
*************************************************/
virtual	void SetDestBlend(VXBLEND_MODE BlendMode) = 0;
 
/*************************************************
Summary: Gets source blend factor.

Return Value: Source blend factor
Remarks:
	+ See VXBLEND_MODE documentation for more details on the blend modes.
See also: SetSourceBlend, VXBLEND_MODE
*************************************************/
virtual	VXBLEND_MODE	GetSourceBlend() = 0;
  
/*************************************************
Summary: Gets destination blend factor.

Return value: Destination blend factor
Remarks:
	+ See VXBLEND_MODE documentation for more details on the blend modes.
See also: SetDestBlend, VXBLEND_MODE
*************************************************/
virtual	VXBLEND_MODE	GetDestBlend() = 0;

//------------------------------------------------------
//  TWO SIDED MAT  

/*************************************************
Summary: Checks whether the material is both sided or not

Return Value: TRUE, if it is two sided
See also: SetTwoSided
*************************************************/ 
virtual 	CKBOOL	IsTwoSided() = 0;

/*************************************************
Summary: Sets the material in both sided mode.

Arguments:
	TwoSided: TRUE to set the material to both sided, FALSE otherwise.
See also: IsTwoSided
*************************************************/ 
virtual 	void	SetTwoSided(CKBOOL TwoSided) = 0;

//------------------------------------------------------
//  TWO SIDED LIGHTING

/*************************************************
Summary: Checks whether this material should use 2 sided lighting. This feature will have effect with an OpenGL rasterizer only.

Return Value: TRUE, if material uses 2 sided lighting
See also: SetTwoSidedLighting
*************************************************/ 
virtual 	CKBOOL	IsLightingTwoSided() = 0;

/*************************************************
Summary: Sets the material in both sided lighting mode. This feature will have effect with an OpenGL rasterizer only.

Arguments:
TwoSided: TRUE to set the material lighting should be both sided, FALSE otherwise.
See also: IsLightingTwoSided
*************************************************/ 
virtual 	void	SetTwoSidedLighting(CKBOOL TwoSided) = 0;

//--------------------------------------------------------------
//  Write in ZBuffer

/*************************************************
Summary: Checks whether writing in ZBuffer is enabled.

Return value: TRUE, if it is enabled.
See also: EnableZWrite 
*************************************************/ 
virtual 	CKBOOL	ZWriteEnabled() = 0;

/*************************************************
Summary: Enables writing in Zbuffer

Arguments:
	ZWrite: TRUE to enable writing in the ZBuffer.
See also: ZWriteEnabled 
*************************************************/ 
virtual 	void	EnableZWrite(CKBOOL ZWrite=TRUE) = 0;

//--------------------------------------------------------------
//  Alpha blending

/*************************************************
Summary: Checks whether alpha blending is enabled or not.

Return Value: TRUE, if it enables.
Remarks:
	+ See VXBLEND_MODE documentation for more details on the blend modes.
See also: EnableAlphaBlend,SetSourceBlend
*************************************************/
virtual 	CKBOOL	AlphaBlendEnabled() = 0;

/*************************************************
Summary: Enables or disables blending.

Arguments:
	Blend: TRUE to enable blending, FALSE otherwise.
Remarks:
	+ See VXBLEND_MODE documentation for more details on the blend modes.
See also: AlphaBlendEnabled,Set
*************************************************/
virtual 	void	EnableAlphaBlend(CKBOOL Blend=TRUE) = 0;

//--------------------------------------------------------------
//  Z comparison func

/*************************************************
Summary: Gets the Z comparison function.

Return Value: Z Comparison function.
See also: SetZFunc, VXCMPFUNC
*************************************************/ 
virtual 	VXCMPFUNC	GetZFunc() = 0;

/*************************************************
Summary: Sets the Z comparison function.

Arguments:
	ZFunc: Z comparison function
Remarks:
+When rendering a primitive a pixel will be written to 
the screen only if the Z comparison function returns TRUE.
+The default value is VXCMP_LESSEQUAL where to be written a fragment
must have a Z value inferior at what is already on the screen (that 
is must be closer to the viewpoint).
See also: GetZFunc, VXCMPFUNC
*************************************************/ 
virtual 	void		SetZFunc(VXCMPFUNC ZFunc=VXCMP_LESSEQUAL) = 0;

//--------------------------------------------------------------
//  Texture pespective correction

/*************************************************
Summary: Checks whether texture perspective correction is enabled.

Return value: TRUE, if it is enabled, FALSE otherwise.
See also: EnablePerpectiveCorrection
*************************************************/ 
virtual 	CKBOOL		PerspectiveCorrectionEnabled() = 0;

/*************************************************
Summary: Enables texture perspective correction.

Arguments:
	Perspective: TRUE to enable Texture perspective correction.
See also: EnablePerpectiveCorrection
*************************************************/ 
virtual 	void		EnablePerpectiveCorrection(CKBOOL Perspective=TRUE) = 0;

//--------------------------------------------------------------
//  FILL MODE   

/*************************************************
Summary: Sets the fill mode.

See also: GetFillMode
*************************************************/ 
virtual 	void		SetFillMode(VXFILL_MODE FillMode) = 0;

/*************************************************
Summary: Gets the fill mode.
Return Value: Fill mode

See also: SetFillMode
*************************************************/ 
virtual 	VXFILL_MODE	GetFillMode() = 0;

//--------------------------------------------------------------
// SHADE MODE 

/*************************************************
Summary: Sets the shade mode.

See also: VXSHADE_MODE, GetShadeMode
*************************************************/ 
virtual 	void		 SetShadeMode(VXSHADE_MODE ShadeMode) = 0;

/*************************************************
Summary: Gets the shade mode.

Return value: Shade mode.
See also: VXSHADE_MODE, SetShadeMode
*************************************************/ 
virtual 	VXSHADE_MODE GetShadeMode() = 0;

//--------------------------------------------------------------
// CURRENT MATERIAL  

/*************************************************
Summary: Sets the current material to use for lighting when drawing primitives.

Arguments:
	context: A pointer to the CKRenderContext on which this material should be set as current.
	Lit: TRUE to set the color settings of the material or FALSE if they will not be used (when rendering prelitted primitives for example)	
	TextureStage: For multi-texture monopass rendering index of the texture stage on which this material should be set. Most of the time this value does not need to be set.
Return Value:
	TRUE if successful.

Remarks:
When setting a material as current the lighting engine will use its parameters (Diffuse Color ,etc.)
for lighting calculation. But this function also sets the render state according to the material properties:

+If the material has a texture it is set as current texture.
+If material is two-sided  the culling is disabled : SetState(VXRENDERSTATE_CULLMODE,VXCULL_NONE) otherwise it is set to counter-clockwise SetState(VXRENDERSTATE_CULLMODE, VXCULL_CCW).
+The Texture Blend Mode is set : SetState(VXRENDERSTATE_TEXTUREMAPBLEND,TextureBlendMode);
+The Shade Mode is set : SetState(VXRENDERSTATE_SHADEMODE,ShadeMode) unless the render context has a global shade mode in which case this is the one used. 
+The Filtering Mode is set :SetState(VXRENDERSTATE_TEXTUREMAG,TextureMagMode) and SetState(VXRENDERSTATE_TEXTUREMIN,TextureMinMode)
+Texture Border Color and Address Mode (Wrap,Clamp,etc.) are set : 	Dev->SetState(VXRENDERSTATE_BORDERCOLOR,BorderColor ) and 
Dev->SetState(VXRENDERSTATE_TEXTUREADDRESS,TextureAddressMode )
+If Blending is enabled the blending is enabled through SetState(VXRENDERSTATE_BLENDENABLE, ...) 
Z buffer writing is disabled according to hte current flag SetState(VXRENDERSTATE_ZWRITEENABLE, ...)
and the Blending Factors are set SetState(VXRENDERSTATE_SRCBLEND, SrcBlendMode) and SetState(VXRENDERSTATE_DESTBLEND, DestBlendMode). 
+Alpha testing is set,SetState(VXRENDERSTATE_ALPHATESTENABLE,AlphaTestEnabled),SetState(VXRENDERSTATE_ALPHAFUNC, AlphaFunc),SetState(VXRENDERSTATE_ALPHAREF, AlphaRef);

See also:CKRenderContext::SetCurrentMaterial,CKTexture::SetAsCurrent,CKRenderContext::SetState
*************************************************/
virtual 	CKBOOL	SetAsCurrent(CKRenderContext*,BOOL Lit=TRUE,int TextureStage = 0) = 0;


/*************************************************
Summary: Checks whether this material has transparency.

Remarks:
	+ This method is used by the engine to identify objects that should be rendered last (transparent objects).
Return Value:
	TRUE if the blending is enabled and the destination blend mode is different from VXBLEND_ZERO.
See also: SetDestBlend,EnableAlphaBlend
*************************************************/ 
virtual		CKBOOL	IsAlphaTransparent() = 0;

//--------------------------------------------------------------
//  Alpha comparison func

/*************************************************
Summary: Checks whether the alpha test is enabled

Return Value: TRUE, if it is enabled, FALSE otherwise
See also: EnableAlphaTest,GetAlphaFunc,GetAlphaRef
*************************************************/ 
virtual 	CKBOOL		AlphaTestEnabled() = 0;

/*************************************************
Summary: Enables or disables alpha testing.

See also: AlphaTestEnabled, SetAlphaFunc,SetAlphaRef
*************************************************/ 
virtual 	void		EnableAlphaTest(CKBOOL Enable = TRUE) = 0;

/*************************************************
Summary: Gets alpha comparision function.

Return value: Alpha comparision function
See also: VXCMPFUNC, SetAlphaFunc,EnableAlphaTest,SetAlphaRef
*************************************************/ 
virtual 	VXCMPFUNC	GetAlphaFunc() = 0;

/*************************************************
Summary: Sets alpha comparison function.

Arguments:
	AlphaFunc: Alpha comparison function.
See also: VXCMPFUNC,SetAlphaRef,GetAlphaFunc,EnableAlphaTest
*************************************************/ 
virtual 	void		SetAlphaFunc(VXCMPFUNC AlphaFunc=VXCMP_ALWAYS) = 0;

//--------------------------------------------------------------
//  Alpha Referential Value

/*************************************************
Summary: Gets alpha referential value.

Return Value: Alpha referential value.
See also: SetAlphaRef,SetAlphaFunc,EnableAlphaTest
*************************************************/ 
virtual 	CKBYTE	GetAlphaRef() = 0;

/*************************************************
Summary: Sets alpha referential value for alpha testing.

Arguments:
	AlphaRef: Alpha referential value (0..255)
See also: GetAlphaRef,SetAlphaFunc,EnableAlphaTest
*************************************************/ 
virtual 	void		SetAlphaRef(CKBYTE AlphaRef=0) = 0;


/*************************************************
Summary: Sets a special effect on a material.

Remarks:
	Some additional effects were added on a CKMaterial to support new graphics features such 
	as bump mapping and cube maps. When en effect is set it may override default features 
See also: VxEffectDescription,GetEffectParameter,CKRenderManager::GetEffectDescription
*************************************************/ 
virtual 	void SetEffect(VX_EFFECT Effect) = 0;

virtual 	VX_EFFECT GetEffect() = 0;

/*************************************************
Summary: Sets the texture to the material

Arguments: 
	  TexIndex: Index of the texture to set (0..3)
	  Tex: A Pointer to the texture to use.
Remarks:
	For devices that support 	
See also: GetTexture,SetTextureBlendMode
*************************************************/
virtual	void			SetTexture(int TexIndex,CKTexture * Tex) = 0;


/*************************************************
Summary: Gets the effect parameter

Return Value:
	A CKParameter that is used to setup the effect.
Remarks:
When an effect is set on a material it can create a parameter
that will be used as a settings for this effect. For example the Bump Map effect
expose a "Bump power" parameter that store a float that 
See also: GetEffect,VxEffectDescription,CKRenderManager::GetEffectDescription
*************************************************/
virtual	CKParameter*	GetEffectParameter() = 0;

CKMaterial(CKContext *Context,CKSTRING name=NULL)  : CKBeObject(Context,name) {}	

#ifndef NO_SHADER
/*************************************************
Summary: Gets the material shader (DirectX 9 Effect Extension)
Arguments:
	LinkParams: The first time it is used for rendering a material shader parameters must be linked
	in case the effect was recompiled so this parameter must be set to TRUE. Other calls to GetMaterialShader in the same frame 
	can be made with LinParams set to FALSE.
Return Value:
	The current CKMaterialShader set on the material.
Remarks:
	When a Material Effect is set, it uses a CKShader and a CKShaderTechnique that is used 
as pipeline configuration for rendering the mesh. If supported by the current rasterizer (DX9 for example)
the effect completly overrides the settings of the material.al.
See Also: SetMaterialShader,DestroyCKMaterialShader
*************************************************/
virtual	CKMaterialShader*	GetMaterialShader(bool LinkParams = FALSE) = 0;

/*************************************************
Summary: Applies a shader to the material (DirectX 9 Effect Extension)
Argument:
	shader:The CKShader to apply to the material (can be null to remove a shader)
Remarks:
	The SetShader function won't perform any action if the material has already 
	be assigned with the same shader.
*************************************************/
virtual void	SetShader( CKShader* shader ) = 0;
virtual void	SetShaderByName( char* iName ) = 0;
virtual void	SetShaderParamValue( char* iParamName, void* iBuffer ) = 0;
virtual void	SetShaderParamObject( char* iParamName, CKObject* iObj ) = 0;
virtual void	GetShaderParamValue( char* iParamName, void* oBuffer ) = 0;
virtual CKObject*	GetShaderParamObject( char* iParamName ) = 0;

/*************************************************
Summary: Sets the material shader (DirectX 9 Effect Extension)
Argument:
	effect:The CKMaterialShader to use or null to disable usage of material effects..
Remarks:
	When a Material Effect is set, it uses a CKShader and a CKShaderTechnique that is used 
as pipeline configuration for rendering the mesh. If supported by the current rasterizer (DX9 for example)
the effect completly overrides the settings of the material.
See Also: DestroyCKMaterialShader,GetMaterialShader
*************************************************/
virtual void	SetMaterialShader(CKMaterialShader*	shader) = 0;

/*************************************************
Summary: Destroy a CKMaterialShader, and set the input pointer (mfx) to NULL
Remarks:
You must call this function in order to destroy a CKMaterialShader (attached 
or not to the material).
When a Material Effect is set, it uses a CKShader and a CKShaderTechnique that is used 
as pipeline configuration for rendering the mesh. If supported by the current rasterizer (DX9 for example)
the effect completly overrides the settings of the material.
See Also: SetMaterialShader,GetMaterialShader
*************************************************/
virtual void DestroyCKMaterialShader( CKMaterialShader*& mfx ) = 0;
#endif

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
static CKMaterial* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_MATERIAL)?(CKMaterial*)iO:NULL;
}
};

#endif
