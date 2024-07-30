using Ballance2.Services.I18N;
using TMPro;
using UnityEngine;

namespace Ballance2.UI.Core.Controls
{ 
  /// <summary>
  /// 国际化文本控制器
  /// </summary>
  public class I18NController : MonoBehaviour
  {
    public const string AUTO_I18N_PREFIX = "I18N:";
    
    public string ResourceKey = "";
    public string DefaultString = "";
    public object[] FormatParams = null;
    public TMP_Text ControlText = null;

    private void Awake() 
    {
      if (ControlText == null)
        ControlText = GetComponent<TMP_Text>();
      UpdateLocalization();
    }

    [HideInInspector]
    public string localizedText = "";

    public void NoLocalization() {
      ResourceKey = "";
      localizedText = "";
      DefaultString = "";
    }
    public void UpdateLocalization() {
      if (
        (!string.IsNullOrEmpty(ResourceKey) || !string.IsNullOrEmpty(DefaultString))
        && ControlText != null
      ) {
        localizedText = FormatParams != null ? 
          I18N.TrF(ResourceKey, DefaultString, FormatParams) : 
          I18N.Tr(ResourceKey, DefaultString);
        ControlText.text = localizedText;
      } else {
        localizedText = "";
      }
    }
    public void UpdateParams(object[] param) {
      FormatParams = param;
      UpdateLocalization();
    }

  }
}
