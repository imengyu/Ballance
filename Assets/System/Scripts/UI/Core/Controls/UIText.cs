using TMPro;
using UnityEngine;
using Ballance2.Utils;

namespace Ballance2.UI.Core.Controls
{ 
  [RequireComponent(typeof(TMP_Text))]
  [AddComponentMenu("Ballance/UI/Controls/Text")]
  public class UIText : MonoBehaviour 
  {
    public GetAndTempVar<I18NController> i18n;
    public GetAndTempVar<TMP_Text> tmp;

    public UIText() 
    {
      i18n = new GetComponentAndTempVar<I18NController>(this);
      tmp = new GetComponentAndTempVar<TMP_Text>(this);
    }

    public string localizedText {
      get {
        return i18n.Get().localizedText;
      }
    }
    public string i18nResourceKey 
    {
      get { return i18n.Get().ResourceKey; }
      set {
        i18n.Get().ResourceKey = value;
        i18n.Get().UpdateLocalization();
      }
    }
    public string text 
    {
      get { return tmp.Get().text; }
      set {
        var i18N = i18n.Get();
        if (value.StartsWith(I18NController.AUTO_I18N_PREFIX))
        {
          i18N.ResourceKey = value.Substring(I18NController.AUTO_I18N_PREFIX.Length);
          i18N.DefaultString = "";
          i18N.UpdateLocalization();
        } 
        else 
        {
          tmp.Get().text = value;
          i18N?.NoLocalization();
        }
      }
    } 
    public Color color 
    {
      get { return tmp.Get().color; }
      set {
        tmp.Get().color = value;
      }
    } 
  
  }
}