using System.Collections;
using System.Collections.Generic;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class PackageManagerTrustUIControl : MonoBehaviour
  {
    public Button ButtonAllow;
    public Button ButtonDisallow;
    public Button ButtonCheckInfo;
    public Button ButtonDisallowAndDelete;
    public TMP_Text TextTitle;

    private GamePackageManager gamePackageManager;
    private GameUIManager gameUIManager;
    private GameUIPage page;

    private string currentPackageName = "";
    private string currentPackageTitle = "";

    void Start()
    {
      gamePackageManager = GameSystem.GetSystemService<GamePackageManager>();
      gameUIManager = GameSystem.GetSystemService<GameUIManager>();

      ButtonAllow.onClick.AddListener(() => gamePackageManager.TrustPackageDialogFinish(true, false));
      ButtonDisallow.onClick.AddListener(() => gamePackageManager.TrustPackageDialogFinish(false, false));
      ButtonDisallowAndDelete.onClick.AddListener(() => gamePackageManager.TrustPackageDialogFinish(false, true));
      ButtonCheckInfo.onClick.AddListener(() => {
        var options = new Dictionary<string, object>
        {
          { "PackageName", currentPackageName }
        };
        gameUIManager.GoPageWithOptions("PagePackageManagerInfo", options);
      });

      page = gameUIManager.FindPage("PagePackageManagerTrust");
      page.OnShow = (options) =>
      {
        if (options.ContainsKey("PackageName"))
          currentPackageName = (string)options["PackageName"];
        if (options.ContainsKey("PackageTitle"))
          currentPackageTitle = (string)options["PackageTitle"];
        TextTitle.text = I18N.TrF("core.ui.PackageManager.TrustTitle", currentPackageTitle);
      };
    }
  }
}
