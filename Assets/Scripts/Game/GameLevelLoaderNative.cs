using System.Collections;
using System.IO;
using Ballance2.Sys.Res;
using UnityEngine;
using UnityEngine.Networking;

namespace Ballance2.Game
{

  [SLua.CustomLuaClass]
  public delegate void GameLevelLoaderNativeCallback(GameObject prefab, string jsonString, AssetBundle assetBundle);
  [SLua.CustomLuaClass]
  public delegate void GameLevelLoaderNativeErrCallback(string code, string err);

  [SLua.CustomLuaClass]
  public class GameLevelLoaderNative : MonoBehaviour
  {
    public void LoadLevel(string name, GameLevelLoaderNativeCallback callback, GameLevelLoaderNativeErrCallback errCallback)
    {
      //路径
      string path = GamePathManager.GetLevelRealPath(name);
      if (!File.Exists(path))
      {
        errCallback("FILE_NOT_EXISTS", "文件 " + name + " 不存在");
        return;
      }

      //加载资源包
      StartCoroutine(Loader(path, callback, errCallback));
    }
    public void UnLoadLevel(AssetBundle assetBundle) {
      if(assetBundle) 
        assetBundle.Unload(true);
    }

    private IEnumerator Loader(string path, GameLevelLoaderNativeCallback callback, GameLevelLoaderNativeErrCallback errCallback)
    {
      UnityWebRequest request = UnityWebRequest.Get(path);
      yield return request.SendWebRequest();

      if (request.result == UnityWebRequest.Result.Success)
      {
        AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(request.downloadHandler.data);
        yield return assetBundleCreateRequest;
        var assetBundle = assetBundleCreateRequest.assetBundle;

        if (assetBundle == null)
        {
          errCallback("FAILED_LOAD_ASSETBUNDLE", "错误的关卡，加载 AssetBundle 失败");
          yield break;
        }

        TextAsset LevelJsonTextAsset = assetBundle.LoadAsset<TextAsset>("Level.json");
        if (LevelJsonTextAsset == null || string.IsNullOrEmpty(LevelJsonTextAsset.text))
        {
          errCallback("BAD_LEVEL_JSON", "关卡 Level.json 为空或无效");
          yield break;
        }
        GameObject LevelPrefab = assetBundle.LoadAsset<GameObject>("Level.prefab");
        if (LevelPrefab == null)
        {
          errCallback("BAD_LEVEL", "关卡无效，不存在 Level.prefab ");
          yield break;
        }

        callback(LevelPrefab, LevelJsonTextAsset.text, assetBundle);
      }
      else
      {
        if (request.responseCode == 404)
          errCallback("FILE_NOT_FOUND", "关卡文件未找到");
        else if (request.responseCode == 403)
          errCallback("ACCESS_DENINED", "无权限读取资源包");
        else
          errCallback("REQUEST_ERROR", "请求失败：" + request.responseCode);
        yield break;
      }
    }
  }
}