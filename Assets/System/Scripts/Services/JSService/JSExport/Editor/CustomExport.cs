namespace Ballance2.JSService.JSExport
{
  using System.Collections.Generic;

  public class CustomExport
  {
    public static void OnGetAssemblyToGenerateExtensionMethod(out List<string> list)
    {
      list = new List<string> {
        "Assembly-CSharp",
      };
    }

    public static void OnAddCustomClass(JSCodeGen.ExportGenericDelegate add)
    {
      // below lines only used for demostrate how to add custom class to export, can be delete on your app

      add(typeof(System.Func<int>), null);
      add(typeof(System.Action<int, string>), null);
      add(typeof(System.Action<int, Dictionary<int, object>>), null);
      add(typeof(List<int>), "ListInt");
      // .net 4.6 export class not match used class on runtime, so skip it
      //add(typeof(Dictionary<int, string>), "DictIntStr");
      add(typeof(string), "String");
      //Xml
      add(typeof(System.Xml.XmlAttribute), null);
      add(typeof(System.Xml.XmlDocument), null);
      add(typeof(System.Xml.XmlNode), null);
      add(typeof(System.Xml.XmlElement), null);
      add(typeof(System.Xml.XmlReader), null);
      add(typeof(System.Xml.XmlWriter), null);
      add(typeof(System.Xml.XmlReaderSettings), null);
      add(typeof(System.Xml.XmlWriterSettings), null);

      add(typeof(UnityEngine.Input), null);

      // add your custom class here
      // add( type, typename)
      // type is what you want to export
      // typename used for simplify generic type name or rename, like List<int> named to "ListInt", if not a generic type keep typename as null or rename as new type name
    }

    public static void OnAddCustomAssembly(ref List<string> list)
    {
      // add your custom assembly here
      // you can build a dll for 3rd library like ngui titled assembly name "NGUI", put it in Assets folder
      // add its name into list, slua will generate all exported interface automatically for you

      //list.Add("NGUI");
    }

    public static HashSet<string> OnAddCustomNamespace()
    {
      return new HashSet<string>
      {
        //"NLuaTest.Mock"
      };
    }

    // if uselist return a white list, don't check noUseList(black list) again
    public static void OnGetUseList(out List<string> list)
    {
      list = new List<string>
      {
        // "UnityEngine.Font",

      };
    }

    public static List<string> FunctionFilterList = new List<string>()
    {
      "UIWidget.showHandles",
      "UIWidget.showHandlesWithMoveTool",
      "UnityEngine.QualitySettings.get_streamingMipmapsRenderersPerFrame",
      "UnityEngine.QualitySettings.set_streamingMipmapsRenderersPerFrame",
      "UnityEngine.QualitySettings.streamingMipmapsRenderersPerFrame",
      "UnityEngine.Texture.get_imageContentsHash",
      "UnityEngine.Texture.set_imageContentsHash",
      "UnityEngine.Texture.imageContentsHash",
      "UnityEngine.MeshRenderer.scaleInLightmap",
      "UnityEngine.MeshRenderer.receiveGI",
      "UnityEngine.MeshRenderer.stitchLightmapSeams",
      "UnityEngine.UI.DefaultControls.factory",
      "UnityEngine.Light.SetLightDirty",
      "UnityEngine.Light.get_shadowAngle",
      "UnityEngine.Light.set_shadowAngle",
      "UnityEngine.Light.get_shadowRadius",
      "UnityEngine.Light.set_shadowRadius",
      "UnityEngine.Shader.DisableKeyword",
      "UnityEngine.Shader.SetKeyword",
      "UnityEngine.Shader.IsKeywordEnabled",
      "UnityEngine.Shader.EnableKeyword",
      "UnityEngine.Screen.MoveMainWindowTo",
      "UnityEngine.Graphics.RenderPrimitivesIndexedIndirect",
      "UnityEngine.Graphics.RenderPrimitivesIndexed",
      "UnityEngine.Graphics.RenderPrimitives",
      "UnityEngine.Graphics.RenderMesh",
      "UnityEngine.Graphics.RenderPrimitivesIndirect",
      "UnityEngine.Graphics.RenderMeshIndirect",
      "UnityEngine.Graphics.RenderMeshPrimitives",
      "UnityEngine.ComputeShader.EnableKeyword",
      "UnityEngine.ComputeShader.DisableKeyword",
      "UnityEngine.ComputeShader.SetKeyword",
      "UnityEngine.ComputeShader.IsKeywordEnabled",
      "UnityEngine.Device.Screen.MoveMainWindowTo",
      "UnityEngine.Material.SetKeyword",
      "UnityEngine.Material.EnableKeyword",
      "UnityEngine.Material.DisableKeyword",
      "UnityEngine.Material.IsKeywordEnabled",
    };
    // black list if white list not given
    public static void OnGetNoUseList(out List<string> list)
    {
      list = new List<string>
      {
        "System.ReadOnlySpan<System.Char>",
        "ReadOnlySpan<Char>",
        "ReadOnlySpan",
        "HideInInspector",
        "ExecuteInEditMode",
        "AddComponentMenu",
        "ContextMenu",
        "RequireComponent",
        "DisallowMultipleComponent",
        "SerializeField",
        "AssemblyIsEditorAssembly",
        "Attribute",
        "Types",
        "UnitySurrogateSelector",
        "TrackedReference",
        "TypeInferenceRules",
        "FFTWindow",
        "RPC",
        "Network",
        "MasterServer",
        "BitStream",
        "HostData",
        "ConnectionTesterStatus",
        "EventType",
        "EventModifiers",
        "FontStyle",
        "TextAlignment",
        "TextEditor",
        "TextEditorDblClickSnapping",
        "TextGenerator",
        "TextClipping",
        "Gizmos",
        "ADBannerView",
        "ADInterstitialAd",
        "Android",
        "Tizen",
        "jvalue",
        "iPhone",
        "iOS",
        "Windows",
        "GUIStyleState",
        "CalendarIdentifier",
        "CalendarUnit",
        "CalendarUnit",
        "ClusterInput",
        "FullScreenMovieControlMode",
        "FullScreenMovieScalingMode",
        "Handheld",
        "LocalNotification",
        "NotificationServices",
        "RemoteNotificationType",
        "RemoteNotification",
        "SamsungTV",
        "TextureCompressionQuality",
        "TouchScreenKeyboardType",
        "TouchScreenKeyboard",
        "MovieTexture",
        "Terrain",
        "Tree",
        "SplatPrototype",
        "DetailPrototype",
        "DetailRenderMode",
        "MeshSubsetCombineUtility",
        "AOT",
        "Social",
        "Enumerator",
        "SendMouseEvents",
        "Cursor",
        "Flash",
        "ActionScript",
        "OnRequestRebuild",
        "Ping",
        "ShaderVariantCollection",
        "SimpleJson.Reflection",
        "CoroutineTween",
        "GraphicRebuildTracker",
        "Advertisements",
        "UnityEditor",
        "WSA",
        "EventProvider",
        "Apple",
        "ClusterInput",
        "LightingSettings",
        "Motion",
        "UnityEngine.UI.ReflectionMethodsCache",
        "NativeLeakDetection",
        "NativeLeakDetectionMode",
        "WWWAudioExtensions",
        "UnityEngine.Experimental",
        "Unity.Jobs",
        "Unity.Collections",
        "Unity.IO.LowLevel",
        "UnityEngine.AudioSettings",
        "UnityEngine.DrivenRectTransformTracker",
        "UnityEngine.tvOS",
        "UnityEngine.LightProbeGroup",
        "UnityEngine.Playables",
        "UnityEngine.Rendering",
        "UnityEngine.TrailRenderer",
        "UnityEngine.LineRenderer",
        "Unity.Profiling.LowLevel.Unsafe.ProfilerCategoryDescription",
        "Unity.Profiling.LowLevel.Unsafe.ProfilerMarkerData",
        "Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderDescription",
        "Unity.Profiling.LowLevel.Unsafe.ProfilerUnsafeUtility",
        "UnityEngine.PlayerLoop.TimeUpdate.ProfilerStartFrame",
        "UnityEngine.PlayerLoop.Initialization.PlayerUpdateTime",
        "UnityEngine.Assertions.Must.MustExtensions",
        "UnityEngine.PlayerLoop.EarlyUpdate.ProfilerStartFrame",
        "UnityEngine.GUIText",
        "UnityEngine.WWW",
        "UnityEngine.AnimationInfo",
        "UnityEngine.AnimationClipPair",
        "UnityEngine.GUIElement",
        "UnityEngine.GUILayer",
        "UnityEngine.UI.IMask",
        "UnityEngine.UI.BaseVertexEffect",
        "UnityEngine.UI.IVertexModifier",
        "UnityEngine.EventSystems.TouchInputModule",
        "UnityEngine.CacheIndex",
        "ProfilerStartFrame",
        "PlayerUpdateTime",
        "TangoUpdate",
      };
    }
  }
}
