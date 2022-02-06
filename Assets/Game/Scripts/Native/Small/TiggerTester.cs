using UnityEngine;

namespace Ballance2.Game
{
  [LuaApiDescription("Tigger检查脚本")]
  [SLua.CustomLuaClass]
  public class TiggerTester : MonoBehaviour
  {   
    [SLua.CustomLuaClass]
    public delegate void OnTiggerTesterEventCallback(GameObject self, GameObject other);

    public OnTiggerTesterEventCallback onTriggerEnter;
    public OnTiggerTesterEventCallback onTriggerExit;

    private void OnTriggerEnter(Collider other) {
      if(onTriggerEnter != null) 
        onTriggerEnter.Invoke(gameObject, other.gameObject);
    }    
    private void OnTriggerExit(Collider other) {
      if(onTriggerExit != null) 
        onTriggerExit.Invoke(gameObject, other.gameObject);
    }
  }
}