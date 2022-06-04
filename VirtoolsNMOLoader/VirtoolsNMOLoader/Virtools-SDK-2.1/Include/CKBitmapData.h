/*************************************************************************/
/*	File : CKBitmapData.h												 */
/*	Author :  Romain Sididris											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKBITMAPDATA_H
#define CKBITMAPDATA_H "$Id:$"

#include "VxDefines.h"
#include "CKMovieReader.h"

#define CKBITMAPDATA_INVALID         1
#define CKBITMAPDATA_TRANSPARENT     2
#define CKBITMAPDATA_FORCERESTORE    4
#define CKBITMAPDATA_CLAMPUPTODATE   8
#define CKBITMAPDATA_CUBEMAP         16
#define CKBITMAPDATA_FREEVIDEOMEMORY 32
#define CKBITMAPDATA_DYNAMIC         64

class CKBitmapSlot
{
public:
    DWORD *m_DataBuffer; // Image Data
    XString m_FileName;	 // Image Filename
public:
    CKBitmapSlot()
    {
        m_DataBuffer = NULL;
    }

    void Allocate(int Width, int Height, int iBpp)
    {
        Flush();
        m_DataBuffer = (DWORD *)VxNewAligned(Width * Height * iBpp / 8, 16);
    }
    void Free()
    {
        Flush();
        m_FileName = "";
    }

    void Resize(VxImageDescEx &Src, VxImageDescEx &Dst)
    {
        DWORD *NewBuffer = (DWORD *)VxNewAligned(Dst.Width * Dst.Height * sizeof(DWORD), 16);
        if (m_DataBuffer)
        {
            Src.Image = (BYTE *)m_DataBuffer;
            Dst.Image = (BYTE *)NewBuffer;
            VxResizeImage32(Src, Dst);
            Flush();
        }
        else
        {
            DWORD *ptr = NewBuffer;
            DWORD size = Dst.Width * Dst.Height;
            for (DWORD i = 0; i < size; i++, ptr++)
                *ptr = 0xFF000000;
        }
        m_DataBuffer = NewBuffer;
    }

    void Flush()
    {
        VxDeleteAligned(m_DataBuffer);
        m_DataBuffer = NULL;
    }

    ~CKBitmapSlot()
    {
        Flush();
    }
};

class CKMovieInfo
{
public:
    XString m_MovieFileName;
    CKMovieReader *m_MovieReader;
    int m_MovieCurrentSlot;
    CKMovieInfo(const XString &FileName);
    ~CKMovieInfo();
};

/************************************************************
{filename:CKBitmapData}
Summary: Base class for bitmaps management


Remarks:

+ CKBitmapData is used as a base class for CKTexture and is used as an aggregate in CKSprite class since they share the same functionnalities.

+ This class provides the base methods for loading/saving bitmaps or movies.

+ Surface Data can be retrieve with the GetPixel or LockSurfacePtr function

+ Information is also given about pixel format but in the current implementation images are always stored in 32 bit per pixel.

+ These bitmaps can be flagged as transparent using a specific transparency color.

+  The class also provides methods to set the way bitmaps should be store inside compositions files.

+ The image and movie loading functions are not documented in this base class but in the CKTexture and CKSprite documentation: CKTexture::LoadImage and CKSprite::LoadImage

See also: CKTexture,CKSprite
************************************************************/
class CKBitmapData
{
public:
    //----------------------------------------------
    // Bitmap creation

    /*******************************************************
    Summary: Creates an empty image.

    Arguments:
        Width: width in pixel of the bitmap to create
        Height: height in pixel of the bitmap to create
        BPP: Bit per pixel of the bitmap
        Slot: If there a multiple images, index of the image slot to create.
        ImagePointer : You can provide a image pointer that will be used instead of allocating it in CKBitmap Data.
        The pointer must NOT be deleted afterwards and will be deleted by CKBitmapData when the slot is deleted.
        If the system caching mode is CKBITMAP_PROCEDURAL (See SetSystemCaching) the image pointer must be a 32 bit ARGB image allocated with the correct Width and Height.
        Otherwise if the system caching mode is CKBITMAP_VIDEOSHADOW or CKBITMAP_DISCARD the image data must be in the same format than video memory pixel format.
    Return Value: TRUE if successful, FALSE otherwise.
    Remarks:
    + The bitmap data is initialized to black with full alpha (all colors to 0xFF000000)
    + The name for the created slot is set to ""
    + If there was already some slots created but with a different width or height
    the method returns FALSE.
    See Also:SaveImage,
    *******************************************************/
    CKBOOL CreateImage(int Width, int Height, int BPP = 32, int Slot = 0, void *ImagePointer = NULL);
    /************************************************
    Summary: Saves an image slot to a file.
    Arguments:
        Name: Name of the file to save.
        Slot: In a multi-images bitmap,index of the image slot to save.
        UseFormat: If set force usage of the save format specified in SetSaveFormat, otherwise use extension given in Name. Default : FALSE
    Return Value: TRUE if successful, FALSE otherwise.
    Remarks:
        + The image format depends on the image readers installed. The default available
        readers support BMP,TGA,JPG,PNG and GIF format

    See also: CreateImage,CKBitmapReader
    ************************************************/
    CKBOOL SaveImage(CKSTRING Name, int Slot = 0, CKBOOL CKUseFormat = FALSE);
    /************************************************
    Summary: Saves an image alpha channel slot to a file.

    Return Value:
        TRUE if successful.
    Arguments:
        Name: Name of the file to save.
        Slot: In a multi-images bitmap,index of the image slot to save.
    Remarks:
        + The image format depends on the image readers installed. The default available
        readers support BMP,TGA,JPG,PNG and GIF format
    See also: CreateImage,CKBitmapReader
    ************************************************/
    CKBOOL SaveImageAlpha(CKSTRING Name, int Slot = 0);

    //-------------------------------------------------------------
    // Movie Loading

    /*************************************************
    Summary: Returns the name of the movie file used by this bitmap.

    Return Value:
        A pointer to the name of the file which was used to load this bitmap or NULL if this bitmap is not a movie.
    See also: CKTexture::LoadMovie,CKSprite::LoadMovie,GetMovieReader
    *************************************************/
    CKSTRING GetMovieFileName();

    /*************************************************
    Summary: Returns the movie reader used to decompress the current movie.
    Return value: Pointer to CKMovieReader
    Remarks:
        + This method returns a movie reader if one is present,NULL otherwise.

    See also: CKTexture::LoadMovie,CKSprite::LoadMovie,GetMovieFileName,CKMovieReader
    *************************************************/
    CKMovieReader *GetMovieReader()
    {
        return m_MovieInfo ? m_MovieInfo->m_MovieReader : NULL;
    }

    //-------------------------------------------------------------
    // SURFACE PTR ACCES

    /*************************************************
    Summary: Returns a pointer to the image surface buffer.
    Arguments:
        Slot: In a multi-images texture, index of the image slot to get surface pointer of. -1 means the current active slot.
    Return Value: A valid pointer to the texture buffer or NULL if failed.
    Remarks:
        + When dealing with textures or sprites the return value is a pointer on the system memory copy of the texture.
        If any changes are made (write access) to the image surface, you must either call CKTexture::Restore which
        immediatly copies the texture back in video memory or ReleaseSurfacePtr() which flags this bitmap
        as to be reloaded before it is used next time.

    See also: ReleaseSurfacePtr,SetPixel,GetPixel
    *************************************************/
    CKBYTE *LockSurfacePtr(int Slot = -1);

    /*************************************************
    Summary: Marks a slot as modified.

    Return Value:
        TRUE if successful.
    Arguments:
        Slot: In a multi-images bitmap, number of the image slot to mark as invalid.
    Remarks:
        + When changes are made to the bitmap data (using LockSurfacePtr or SetPixel)
        this method marks the changed slot so that they can reloaded in video memory
        when necessary (for textures and sprites).
    See also: LockSurfacePtr,SetPixel
    *************************************************/
    CKBOOL ReleaseSurfacePtr(int Slot = -1);

    /*************************************************
    Summary: Flush a slot system memory

    Arguments:
        Slot: In a multi-images bitmap, number of the image slot to mark as invalid.
    Remarks:
        This method frees the memory allocated in system memory for a given bitmap slot.
    See also: LockSurfacePtr,SetPixel
    *************************************************/
    CKBOOL FlushSurfacePtr(int Slot = -1);
    //-------------------------------------------------------------
    // Bitmap filenames information

    /*************************************************
    Summary: Returns the name of the file used to load a bitmap slot.
    Arguments:
        slot: image slot index
    Return value: A pointer to the name of the file which was used to load this bitmap slot

    See also: SetSlotFileName
    *************************************************/
    CKSTRING GetSlotFileName(int Slot);

    /*************************************************
    Summary: Sets the name of the file used to load a bitmap slot.
    Arguments:
        Slot: image slot index
        Filename: image slot file name
    Return Value: TRUE if successful, FALSE otherwise.

    See also: GetSlotFileName
    *************************************************/
    CKBOOL SetSlotFileName(int Slot, CKSTRING Filename);

    //--------------------------------------------------------------
    // Bitmap storage information

    /*************************************************
    Summary: Gets the image width
    Return value: Image width

    See Also: GetBytesPerLine,GetHeight,GetBitsPerPixel
    *************************************************/
    int GetWidth()
    {
        return m_Width;
    }

    /*************************************************
    Summary: Gets the image height
    Return Value: Image height

    See Also: GetBytesPerLine,GetWidth,GetBitsPerPixel
    *************************************************/
    int GetHeight()
    {
        return m_Height;
    }

    /*************************************************
    Summary: Gets Image format description
    Return Value:
        TRUE if successful.
    Arguments:
        desc: A VxImageDescEx that will contain the image format description.
    Remarks:
        + The desc parameter will be filled with the size,pitch,bpp and mask information
        for the bitmap.

    See also: VxImageDescEx,GetWidth,GetHeight,GetBitsPerPixel
    *************************************************/
    CKBOOL GetImageDesc(VxImageDescEx &oDesc);

    //-------------------------------------------------------------
    // Image slot count

    /************************************************
    Summary: Returns the number of slot (images) in this bitmap.

    Return Value: Number of images.
    See also: SetSlotCount,GetCurrentSlot,SetCurrentSlot
    ************************************************/
    int GetSlotCount();
    /************************************************
    Summary: Sets the number of slot (images) in this bitmap.

    Arguments:
        Count: Image slots  to allocate.
    Return Value: TRUE if successful, FALSE otherwise.
    See also: GetSlotCount,GetCurrentSlot,SetCurrentSlot
    ************************************************/
    CKBOOL SetSlotCount(int Count);
    /************************************************
    Summary: Sets the current active image.

    Arguments:
        Slot: Image slot index.
    Return Value: TRUE if successful, FALSE otherwise.
    See also: GetSlotCount,SetSlotCount,GetCurrentSlot
    ************************************************/
    CKBOOL SetCurrentSlot(int Slot);
    /************************************************
    Summary: Returns current slot index.

    Return Value: Current image slot index.
    See Also:GetSlotCount,SetSlotCount,SetCurrentSlot
    ************************************************/
    int GetCurrentSlot();
    /************************************************
    Summary: Removes an image.

    Return Value:
        TRUE if successful, FALSE if the slot does not exist or the bitmap is a movie.
    Arguments:
        Slot: Index of the image to remove.
    See also: GetSlotCount,GetCurrentSlot,SetCurrentSlot
    ************************************************/
    CKBOOL ReleaseSlot(int Slot);
    /************************************************
    Summary: Deletes all the images.

    Return Value:
        TRUE if successful.
    See also: GetSlotCount,GetCurrentSlot,SetCurrentSlot
    ************************************************/
    CKBOOL ReleaseAllSlots();

    //-------------------------------------------------------------
    // ACCES TO SYSTEM MEMORY SURFACE

    /*************************************************
    Summary: Sets the color of a pixel.

    Return Value:
        TRUE if successful.
    Arguments:
        x: X position of the pixel to set the color of.
        y: Y position of the pixel to set the color of.
        col: A Dword ARGB color to set
        slot: Index of the slot in which the pixel should be set or -1 for the current slot.
    Remarks:
    + There is no check on the validity of x or y parameters so its the
    user responsability.
    + Sets the color of a pixel in the copy of the texture in system memory.
    + If this is used on a texture changes will only be visible after using CKTexture::Restore()
    function to force the texture to re-load from
    system memory.
    See Also:LockSurfacePtr,GetPixel,ReleaseSurfacePtr
    *************************************************/
    CKBOOL SetPixel(int x, int y, CKDWORD col, int slot = -1);
    /*************************************************
    Summary: Gets the color of a pixel.
    Arguments:
        x: X position of the pixel to set the color of.
        y: Y position of the pixel to set the color of.
        slot: Index of the slot in which the pixel should be get or -1 for the current slot.
    Return Value: Color of the pixel (32 bit ARGB)
    Remarks:
        + There is no check on the validity of x or y parameter so its the
        user responsability.

    See Also:LockSurfacePtr,SetPixel
    *************************************************/
    CKDWORD GetPixel(int x, int y, int slot = -1);

    //-------------------------------------------------------------
    // TRANSPARENCY

    /************************************************
    Summary: Returns the transparent color.
    Return Value:
        Color: A 32 bit ARGB transparent color.
    Remarks:
        + 0x00000000 (black) is the default transparent color.

    See also: SetTranparentColor,SetTransparent
    ************************************************/
    CKDWORD GetTransparentColor() { return m_TransColor; }
    /************************************************
    Summary: Sets the transparent color.
    Arguments:
        Color: A 32 bit ARGB transparent color.
    Remarks:
    + 0x00000000 (black) is the default transparent color.
    + Setting on the transparency and a transparent color automatically
    updates the alpha channel so that pixel with the transparent color have
    a 0 alpha value.

    See also: GetTranparentColor,SetTransparent
    ************************************************/
    void SetTransparentColor(CKDWORD Color);
    /************************************************
    Summary: Enables or disables the color key transparency.

    Arguments:
        Transparency: TRUE activates transparency, FALSE disables it.
    Remarks:
        + 0x00000000 (black) is the default transparent color.
        + Setting on the transparency and a transparent color automatically
        updates the alpha channel so that pixel with the transparent color have
        a 0 alpha value.
    See also: IsTransparent,SetTranparentColor
    ************************************************/
    void SetTransparent(CKBOOL Transparency);
    /************************************************
    Summary: Returns whether color keyed transparency is enabled.

    Return Value:
        TRUE if successful.
    Arguments:
        Transparency: TRUE activates transparency, FALSE disables it.
    Return Value:
        TRUE if color keying is enabled.
    See also: IsTransparent
    ************************************************/
    CKBOOL IsTransparent() { return m_BitmapFlags & CKBITMAPDATA_TRANSPARENT; }

    //-------- Save format

    CK_BITMAP_SAVEOPTIONS GetSaveOptions()
    {
        return m_SaveOptions;
    }

    /*************************************************
    Summary: Sets the system memory caching option.
    Arguments:
        iOptions: System Caching Options.
    Remarks:
    The system memory cahcing option specify whether a copy of the image must
    be kept for textures and sprites and in which format this copy should be kept...

    See Also: SetSaveFormat,CK_BITMAP_SYSTEMCACHING
    *************************************************/
    void SetSystemCaching(CK_BITMAP_SYSTEMCACHING iOptions);

    CK_BITMAP_SYSTEMCACHING GetSystemCaching()
    {
        return m_SystemCaching;
    }

    /*************************************************
    Summary: Sets the saving options.
    Arguments:
        Options: Save Options.
    Remarks:
        + When saving a composition textures or sprites can be kept as reference
        to external files or converted to a given format and saved inside the composition file.
        The CK_BITMAP_SAVEOPTIONS enumeration exposes the available options.

    See Also: SetSaveFormat,CK_BITMAP_SAVEOPTIONS
    *************************************************/
    void SetSaveOptions(CK_BITMAP_SAVEOPTIONS Options) { m_SaveOptions = Options; }

    CKBitmapProperties *GetSaveFormat()
    {
        return m_SaveProperties;
    }
    /*************************************************
    Summary: Sets the saving format.
    Arguments:
        format: A CKBitmapProperties that contain the format in which the bitmap should be saved.
    Remarks:
    + If the save options have been set to CKTEXTURE_IMAGEFORMAT you can specify a
    format in which the bitmap will be converted before being saved inside the composition file.
    + The CKBitmapProperties structure contains the CKGUID of a BitmapReader that is to be
    used plus some additionnal settings specific to each format.

    See Also: SetSaveOptions,CKBitmapProperties,CKBitmapReader
    *************************************************/
    void SetSaveFormat(CKBitmapProperties *format);

    /*************************************************
    Summary: Sets pick threshold value.
    Arguments:
        pt: Pick threshold value to be set.
    Remarks:
        + The pick threshold is used when picking object with
        transparent textures.
        + It is the minimum value for alpha component
        below which picking is not valid.So this value is supposed to be in the range 0..255
        and the default value 0 means the picking is always valid.
        + But if a value >0 is used and the texture use transparency (some pixels of the bitmap will have
        alpha component of 0) an object will not be picked on its transparent part.

    See Also: CKRenderContext::Pick
    *************************************************/
    void SetPickThreshold(int pt)
    {
        m_PickThreshold = pt;
    }

    int GetPickThreshold()
    {
        return m_PickThreshold;
    }

    /*************************************************
    Summary: Setup the bitmap to store a cubemap
    Return Value:
        TRUE if the bitmap is set to store a cubemap.
    Arguments:
        CubeMap: TRUE if bitmap is to hold a cube map.
    Remarks:
        + If Cubemap is TRUE , 6 slots are created to store the 6 faces of a cube map.

    *************************************************/
    void SetCubeMap(CKBOOL CubeMap)
    {
        if (CubeMap)
        {
            SetSlotCount(6);
            m_BitmapFlags |= CKBITMAPDATA_CUBEMAP;
        }
        else
        {
            m_BitmapFlags &= ~CKBITMAPDATA_CUBEMAP;
        }
    }

    CKBOOL IsCubeMap()
    {
        return m_BitmapFlags & CKBITMAPDATA_CUBEMAP;
    }

    /*************************************************
    Summary: tells if the bitmap can be restored from the original file
    Return Value:
        TRUE if it can.

    *************************************************/
    CKBOOL HasOriginalFile();

    /************************************************
    Summary: Sets the desired surface pixel format in video memory.

    Arguments:
        pf: A VX_PIXELFORMAT giving the desired pixel format.
    Remarks:
        + As its name indicates theis method only sets a "desired"
        video memory pixel format. The render engine will try to use
        this format is possible otherwise should find the nearest appropriate pixel
        format.
        + Newly created textures use the desired format given in CKRenderManager::SetDesiredTexturesVideoFormat
    See Also:GetVideoPixelFormat,GetDesiredVideoFormat,CKRenderManager::SetDesiredTexturesVideoFormat
    ************************************************/
    void SetDesiredVideoFormat(VX_PIXELFORMAT pf);

    /************************************************
    Summary: Returns the desired pixel format in video memory.

    Return Value:
        Desired pixel format for this texture in video memory.
    See Also:GetVideoPixelFormat,SetDesiredVideoFormat,CKRenderManager::SetDesiredTexturesVideoFormat
    ************************************************/
    VX_PIXELFORMAT GetDesiredVideoFormat()
    {
        return m_DesiredVideoFormat;
    }

    /*******************************************************
    Summary: Resizes all slots.

    Arguments:
        Width: new width in pixel
        Height: new height in pixel
    Return Value: TRUE if successful, FALSE otherwise.
    Remarks:
        + This method resize every images to the given size.
    See also:CreateImage
    *******************************************************/
    CKBOOL ResizeImages(int Width, int Height);

    /************************************************
    Summary: Sets a hint to indicate the bitmap is changed frequently.

    Remarks:
        + If the bitmap content is changed very frequently or to generate
        dynamic video sprites / textures you  should set this hint.
        + On recent graphics card this hint can be taken into account to generate
        efficient dynamic textures.
        + This hint is not saved it can be used on textures and sprite created on the fly.
        + If the Texture or sprite is already in video memory you will have to
        reload it in video memory using FreeVideoMemory/SystemToVideoMemory
    See Also:
    ************************************************/
    void SetDynamicHint(CKBOOL Dynamic)
    {
        if (Dynamic)
        {
            m_BitmapFlags |= CKBITMAPDATA_DYNAMIC;
        }
        else
        {
            m_BitmapFlags &= ~CKBITMAPDATA_DYNAMIC;
        }
    }

    CKBOOL GetDynamicHint()
    {
        return m_BitmapFlags & CKBITMAPDATA_DYNAMIC;
    }

//-------------------------------------------------------------------
// Internal functions
#ifdef DOCJETDUMMY // Docjet secret macro
#else

    CKBOOL ToRestore()
    {
        return m_BitmapFlags & CKBITMAPDATA_FORCERESTORE;
    }
    CKBOOL ReloadSlot(int Slot = 0, VxImageDescEx *iRealFormat = NULL);
    CKBOOL LoadSlotImage(const XString &Name, int Slot = 0);
    CKBOOL LoadMovieFile(const XString &Name);

    CKMovieInfo *CreateMovieInfo(const XString &s, CKMovieProperties **mp);
    void SetMovieInfo(CKMovieInfo *mi);

    CKBitmapData(CKContext *iContext);
    ~CKBitmapData();

public:
    CKContext *m_CKContext;
    CKMovieInfo *m_MovieInfo;
    XArray<CKBitmapSlot *> m_Slots;
    int m_Width;
    int m_Height;
    int m_CurrentSlot;
    int m_PickThreshold;
    DWORD m_BitmapFlags;
    DWORD m_TransColor;

    // TODO : Useless when in player mode
    CKBitmapProperties *m_SaveProperties;
    CK_BITMAP_SAVEOPTIONS m_SaveOptions;
    CK_BITMAP_SYSTEMCACHING m_SystemCaching;
    VX_PIXELFORMAT m_DesiredVideoFormat;
    VX_PIXELFORMAT m_RealVideoFormat;

    void SetAlphaForTransparentColor(const VxImageDescEx &desc);
    void SetBorderColorForClamp(const VxImageDescEx &desc);
    CKBOOL SetSlotImage(int Slot, void *buffer, VxImageDescEx &bdesc);
    CKBOOL DumpToChunk(CKStateChunk *chnk, CKFile *f, DWORD Identifiers[4]);
    CKBOOL ReadFromChunk(CKStateChunk *chnk, CKFile *f, DWORD Identifiers[5]);

#endif // Docjet secret macro
};

#endif