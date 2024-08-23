using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ballance2.Utils;
using static Ballance2.Game.LevelEditor.LevelDynamicLoader;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorSubmitControl : MonoBehaviour
  {
    public Button ButtonSubmit;
    public Image ImageInto;
    public Image ImageLogo;
    public TMP_Text ChooseImageCount;
    public TMP_InputField InputFieldName;
    public TMP_InputField InputFieldDesc;
    public TMP_InputField InputFieldChange;

    public Sprite DefaultImage;

    private LevelDynamicAssembe currentLevel = null;
    private int currentLogoImageIndex = 0;
    private int currentIntoImageIndex = 0;
    private List<Sprite> currentImages = new List<Sprite>();
    private List<string> currentImagePaths = new List<string>();

    public void Show(LevelDynamicAssembe levelDynamicAssembe)
    {
      gameObject.SetActive(true);
      currentLevel = levelDynamicAssembe;
      currentLogoImageIndex = 0;
      currentIntoImageIndex = 0;
      StartCoroutine(LoadScreenShorts());
      InputFieldName.text = currentLevel.LevelInfo.name;
      InputFieldDesc.text = currentLevel.LevelInfo.introduction;
    }
    public void Hide()
    {
      gameObject.SetActive(false);
      currentLevel = null;
    }

    private IEnumerator LoadScreenShorts()
    {
      foreach (Sprite sprite in currentImages)
        Destroy(sprite);
      currentImages.Clear();
      currentImagePaths.Clear();

      var dirName = currentLevel.LevelDirPath + "/screenshot";
      if (Directory.Exists(dirName))
      {
        var dirInfo = new DirectoryInfo(dirName);
        var files = dirInfo.GetFiles("*.png", SearchOption.TopDirectoryOnly);
        foreach (var item in files)
        {
          var result = new EnumeratorResultPacker<Texture2D>();
          yield return TextureUtils.LoadTexture2dFromFile("file:///" + item.FullName, result);
          if (result.Success)
          {
            currentImages.Add(TextureUtils.CreateSpriteFromTexture(result.Result));
            currentImagePaths.Add(item.FullName);
          }
        }
      }
      ShowImage();
    }
    private void ShowImage()
    {
      ImageInto.sprite = currentImages.Count > 0 ? currentImages[currentIntoImageIndex] : DefaultImage;
      ImageLogo.sprite = currentImages.Count > 0 ? currentImages[currentLogoImageIndex] : DefaultImage;
      ChooseImageCount.text = $"{currentIntoImageIndex + 1}/{currentImages.Count}";
    }

    public void PrevImage()
    {
      if (currentIntoImageIndex > 0)
        currentIntoImageIndex--;
      else
        currentIntoImageIndex = currentImages.Count - 1;
      ShowImage();
    }
    public void NextImage()
    {
      if (currentIntoImageIndex < currentImages.Count - 1)
        currentIntoImageIndex++;
      else
        currentIntoImageIndex = 0;
      ShowImage();
    }
    public void PrevLogoImage()
    {
      if (currentLogoImageIndex > 0)
        currentLogoImageIndex--;
      else
        currentLogoImageIndex = currentImages.Count - 1;
      ShowImage();
    }
    public void NextLogoImage()
    {
      if (currentLogoImageIndex < currentImages.Count - 1)
        currentLogoImageIndex++;
      else
        currentLogoImageIndex = 0;
      ShowImage();
    }
    public void ShowSteamLicense()
    {
      //TODO: ShowSteamLicense
    }
    public void ShowFolder()
    {
      var dirName = currentLevel.LevelDirPath + "/screenshot";
      if (Directory.Exists(dirName))
        Application.OpenURL(dirName);
    }
    public void Submit()
    {
      if (currentLevel != null)
        StartCoroutine(_Submit());
    }

    private IEnumerator _Submit()
    {
      var ui = LevelEditorManager.Instance.LevelEditorUIControl;
      ui.ShowLoading();

      var result = new LevelDynamicLoaderResult();
      yield return StartCoroutine(LevelDynamicLoader.Instance.PackLevel(
        result, 
        currentLevel,
        currentLogoImageIndex < currentImagePaths.Count ? currentImagePaths[currentLogoImageIndex] : null,
        currentIntoImageIndex < currentImagePaths.Count ? currentImagePaths[currentIntoImageIndex] : null
      ));

      if (!result.Success)
      {
        ui.HideLoading();
        ui.Alert("I18N:core.editor.messages.PackeLevelFailed", result.Error);
        yield break;
      }

      //TODO: update to steam

      ui.HideLoading();
    }
  }
}