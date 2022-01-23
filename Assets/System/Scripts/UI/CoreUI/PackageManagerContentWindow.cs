using System.Text;
using Ballance2;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.InputManager;
using Ballance2.Services.Pool;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.UI;

public class PackageManagerContentWindow : MonoBehaviour
{
  public Text TextPackageName;
  public Text TextPackageVersion;
  public Text TextPackageMoreInfos;
  public GameObject LinkVisitSite;
  public GameObject LinkVisitDoc;
  public GameObject LinkVisiAuthor;
  public Image ImagePackageLogo;
  public InputField InputPackageName;
  public InputField InputFieldVersion;
  public InputField InputFieldAuthor;
  public InputField InputFieldUpdateTime;
  public Text TextExplain;
  public RectTransform PanelDepends;
  public GameObject ItemDependsPrefab;

  private GamePackage currentPackage;
  private GamePackageManager gamePackageManager;
  private GameObjectPool itemDependsPrefabPool;

  private void Start() {
    gamePackageManager = GameSystem.GetSystemService("GamePackageManager") as GamePackageManager;
    itemDependsPrefabPool = new GameObjectPool("ItemDependsPrefabPool", ItemDependsPrefab, 8, 128, PanelDepends);

    EventTriggerListener.Get(LinkVisitSite).onClick = (go) => {
      Application.OpenURL(currentPackage.BaseInfo.Link);
    };
    EventTriggerListener.Get(LinkVisitDoc).onClick = (go) => {
      Application.OpenURL(currentPackage.BaseInfo.DocLink);
    };
    EventTriggerListener.Get(LinkVisiAuthor).onClick = (go) => {
      Application.OpenURL(currentPackage.BaseInfo.AuthorLink);
    };
  }
  private void OnDestroy() {
    itemDependsPrefabPool.Destroy();
  }

  public delegate void GoPackageDelegate(string name);

  public GoPackageDelegate OnGoPackage;

  private void GoPackage(string name) {
    OnGoPackage?.Invoke(name);
  }

  public void SetPackageInfo(GamePackage package) {
    currentPackage = package;
    TextPackageName.text = package.BaseInfo.Name;
    TextPackageVersion.text = package.BaseInfo.VersionName + " (" + package.PackageVersion.ToString() + ")";
    TextPackageMoreInfos.text = package.BaseInfo.Introduction;
    LinkVisitSite.SetActive(!StringUtils.isNullOrEmpty(package.BaseInfo.Link));
    LinkVisiAuthor.SetActive(!StringUtils.isNullOrEmpty(package.BaseInfo.AuthorLink));
    LinkVisitDoc.SetActive(!StringUtils.isNullOrEmpty(package.BaseInfo.DocLink));
    ImagePackageLogo.sprite = package.BaseInfo.LogoTexture;
    InputPackageName.text = package.PackageName;
    InputFieldVersion.text = package.PackageVersion.ToString();
    InputFieldAuthor.text = package.BaseInfo.Author;
    InputFieldUpdateTime.text = package.UpdateTime.ToShortDateString();
    TextExplain.text = package.BaseInfo.Description;

    for (int i = 0; i < PanelDepends.childCount; i++)
      itemDependsPrefabPool.ReturnObjectToPool("ItemDependsPrefabPool", PanelDepends.GetChild(i).gameObject);

    foreach(var p in package.BaseInfo.Dependencies) {
      var go = itemDependsPrefabPool.NextAvailableObject();
      var text = go.transform.Find("Text").GetComponent<Text>();
      var button = go.transform.Find("Button").GetComponent<Button>();

      var pack = gamePackageManager.FindRegisteredPackage(p.Name);
      if(pack != null) {

        StringBuilder sb = new StringBuilder();
        switch(pack.Status) {
          case GamePackageStatus.NotLoad: 
          case GamePackageStatus.Loading: 
          case GamePackageStatus.Registing: 
          case GamePackageStatus.Registered:
            sb.Append("<color=#121a2a>");
            sb.Append(p.Name);
            sb.Append("</color>");
            break;
          case GamePackageStatus.LoadFailed: 
            sb.Append("<color=#f47920>");
            sb.Append(p.Name);
            sb.Append("</color>");
            break;
          case GamePackageStatus.LoadSuccess:
            sb.Append(p.Name);
            break;
        }

        sb.Append(" >= ");
        sb.Append(p.MinVersion);
        sb.Append(" ");
        if(!p.MustLoad)
          sb.Append(" (可选)");

        text.text = sb.ToString();
        button.gameObject.SetActive(true);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => GoPackage(p.Name));
      } else {
        text.text = "<color=#ed1941>" + p.Name + "</color>";
        button.gameObject.SetActive(false);
        button.onClick.RemoveAllListeners();
      }
    }
  }
}
