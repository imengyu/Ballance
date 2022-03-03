using System.Collections;
using System.Collections.Generic;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2
{
  public class PackageManagerTrustUIControl : MonoBehaviour
  {
    public Button ButtonAllow;
    public Button ButtonDisallow;
    public Button ButtonCheckInfo;
    public Button ButtonDisallowAndDelete;
    public Text TextTitle;

    private GamePackageManager gamePackageManager;
    private GameUIManager gameUIManager;
    private GameUIPage page;

    private string currentPackageName = "";
    private string currentPackageTitle = "";

    void Start()
    {
      gamePackageManager = GameSystem.GetSystemService("GamePackageManager") as GamePackageManager;
      gameUIManager = GameSystem.GetSystemService("GameUIManager") as GameUIManager;

      ButtonAllow.onClick.AddListener(() => gamePackageManager.TrustPackageDialogFinish(true, false));
      ButtonDisallow.onClick.AddListener(() => gamePackageManager.TrustPackageDialogFinish(false, false));
      ButtonDisallowAndDelete.onClick.AddListener(() => gamePackageManager.TrustPackageDialogFinish(false, true));
      ButtonCheckInfo.onClick.AddListener(() => {
        Dictionary<string, string> options = new Dictionary<string, string>();
        options.Add("PackageName", currentPackageName);
        gameUIManager.GoPageWithOptions("PagePackageManagerInfo", options);
      });

      page = gameUIManager.FindPage("PagePackageManagerTrust");
      page.OnShow = (options) =>
      {
        if (options.ContainsKey("PackageName"))
          currentPackageName = options["PackageName"];
        if (options.ContainsKey("PackageTitle"))
          currentPackageTitle = options["PackageTitle"];
        TextTitle.text = I18N.TrF("core.ui.PackageManagerTrustTitle", currentPackageTitle);
      };
    }
  }
}
