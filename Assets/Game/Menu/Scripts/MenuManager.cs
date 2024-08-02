using Ballance2.Package;
using Ballance2.Services;
using Ballance2.UI.Core;

namespace Ballance2.Menu
{
  //初始化基础菜单
  public class MenuManager
  {
    public static GameUIMessageCenter MessageCenter;

    public static void Init()
    {
      var sytemPackage = GamePackage.GetSystemPackage();
      var GameUIManager = GameManager.GetSystemService<GameUIManager>();

      GameUIManager.RegisterUIPrefab("PageTransparent", GameUIPrefabType.Page, sytemPackage.GetPrefabAsset("GameUIPageBallanceTransparent"));
      GameUIManager.RegisterUIPrefab("PageTransparentInGame", GameUIPrefabType.Page, sytemPackage.GetPrefabAsset("GameUIPageBallanceTransparentInGame"));
      GameUIManager.RegisterUIPrefab("PageWide", GameUIPrefabType.Page, sytemPackage.GetPrefabAsset("GameUIPageBallanceWide"));
      GameUIManager.RegisterUIPrefab("PageCommon", GameUIPrefabType.Page, sytemPackage.GetPrefabAsset("GameUIPageBallanceCommon"));
      GameUIManager.RegisterUIPrefab("PageCommonInGame", GameUIPrefabType.Page, sytemPackage.GetPrefabAsset("GameUIPageBallanceCommonInGame"));
      GameUIManager.RegisterUIPrefab("PageFull", GameUIPrefabType.Page, sytemPackage.GetPrefabAsset("GameUIPageFull"));

      MessageCenter = GameUIManager.CreateUIMessageCenter("GameUIGlobalMessageCenter");
      MessageCenter.SubscribeEvent("BtnBackClick", () => GameUIManager.BackPreviusPage());

      MenuLevelUIManager.Create();
      SettingsUIControl.Create();
      GamePlayUIManager.Create();
    }
    public static void Destroy() {
      var GameUIManager = GameManager.GetSystemService<GameUIManager>();
      GameUIManager.DestroyUIMessageCenter("GameUIGlobalMessageCenter");
    }
  }
}