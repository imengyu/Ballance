using Ballance2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorObjectScenseIcon : MonoBehaviour 
  {
    public LevelDynamicModel BindModel = null;
    public Canvas BindCanvas;
    public Camera BindCamera;
    public RectTransform Sectors;
    public GameObject SectorTextPrefab;
    public TMP_Text NameText;
    public RectTransform Name;
    public Image Icon;

    public Sprite SpriteError;
    public Sprite SpriteModel;
    public Sprite SpriteNone;

    public float HideTextDistance = 0;
    public float HideIconDistance = 0;
    public float distance = 0;
    public float MinDistance = 1;

    private void Update() 
    {
      if (BindCamera != null)
      {
        transform.LookAt(BindCamera.transform);
        var distance = Vector3.Distance(transform.position, BindCamera.transform.position);
        if (this.distance != distance)
        {
          this.distance = distance;
          if (HideTextDistance == 0)
          {
            Name.gameObject.SetActive(false);
            Sectors.gameObject.SetActive(false);
          }
          else
          {
            Name.gameObject.SetActive(distance < HideTextDistance && distance > MinDistance);
            Sectors.gameObject.SetActive(distance < HideTextDistance && distance > MinDistance);
          }

          if (HideIconDistance == 0)
            Icon.gameObject.SetActive(false);
          else
            Icon.gameObject.SetActive(distance < HideIconDistance && distance > MinDistance);
        }
      }
    }

    public void UpdateBindModel()
    {
      if (BindModel == null)
        return;

      if (BindModel.ModulRef != null)
      {
        transform.localPosition = new Vector3(0, 7, 0);
        BindCanvas.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        HideTextDistance = 100;
        HideIconDistance = 500;
      }
      else if (BindModel.IsSubObj)
      {
        transform.localPosition = new Vector3(10, 5, 0);
        BindCanvas.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
        HideTextDistance = 40;
        HideIconDistance = 70;
      }
      else
      {
        transform.localPosition = new Vector3(0, 10, 0);
        BindCanvas.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        HideTextDistance = 50;
        HideIconDistance = 0;
      }

      NameText.text = BindModel.Name;

      if (BindModel.IsError)
        Icon.sprite = SpriteError;
      else if (BindModel.ModulRef != null)
        Icon.sprite = BindModel.AssetRef.ScenseGizmePreviewImage == null ?  SpriteModel : BindModel.AssetRef.ScenseGizmePreviewImage;
      else 
      {
        Icon.sprite = SpriteNone;
        HideIconDistance = 0;
      }

      CreateSectorDisplay();
    }

    private int currentDisplaySectorCount = 0;
    private void CreateSectorDisplay()
    {
      Sectors.DestroyAllChildren();
      foreach (var item in BindModel.ActiveSectors)
      {
        var go = CloneUtils.CloneNewObjectWithParent(SectorTextPrefab, Sectors);
        var image = go.GetComponent<Image>();
        var text = go.transform.Find("Text").GetComponent<TMP_Text>();
        image.color = LevelEditorManager.Instance.LevelEditorUIControl.GetSectorColor(item);
        text.text = item.ToString();
        go.SetActive(true);
      }
      currentDisplaySectorCount = BindModel.ActiveSectors.Count;
    }
    public void UpdateSectorDisplay()
    {
      if (currentDisplaySectorCount != Sectors.transform.childCount)
        CreateSectorDisplay();
      else
      {
        for (int i = 0; i < Sectors.transform.childCount; i++)
        {
          var go = Sectors.transform.GetChild(i);
          var image = go.GetComponent<Image>();
          var text = go.transform.Find("Text").GetComponent<TMP_Text>();
          image.color = LevelEditorManager.Instance.LevelEditorUIControl.GetSectorColor(BindModel.ActiveSectors[i]);
          text.text = BindModel.ActiveSectors[i].ToString();
        }
      }
    }
  
  }
}