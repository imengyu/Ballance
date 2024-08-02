using Ballance2.Menu;
using UnityEngine;
  
namespace Ballance2.Game
{
  public class MenuLevelLZSelectControl : MonoBehaviour 
  {
    public CursourSelect CursourSelect;

    private MenuLevelMouseSelect SelectedObject = null;
    private int testTick = 0;
    private Vector2 currentArrowPos = Vector2.zero;
    
    private void Start() {
      CursourSelect.onRecast = (pos) => {
        RecastMouseSelectable(pos);
      };
      CursourSelect.onSelect = (pos) => {
        RecastMouseSelectable(pos);
        if (SelectedObject != null)
          SelectedObject.Select();
      };
    }
    private void Update() {
      if (testTick < 20) {
        testTick++;
      } else {
        testTick = 0;
        Ray ray = Camera.current.ScreenPointToRay(currentArrowPos);
        RaycastHit hit;
        MenuLevelMouseSelect hitObject = null; 

        // 执行射线检测，这里使用LayerMask来过滤只检测特定的层
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
          if (hit.collider != null && hit.collider.gameObject.tag == "IZoneSelect")
            hitObject = hit.collider.gameObject.GetComponent<MenuLevelMouseSelect>();
        }

        if (hitObject != null)
        {
          if (hitObject != SelectedObject)
          {
            if (SelectedObject != null)
            {
              SelectedObject.SetLightState(false);
              SelectedObject = null;
            }
            SelectedObject = hitObject;
            SelectedObject.SetLightState(true);
          }
        }
        else if (SelectedObject != null)
        {
          SelectedObject.SetLightState(false);
          SelectedObject = null;
        }
      }
    }

    private void RecastMouseSelectable(Vector2 screenPost) 
    {
      currentArrowPos = screenPost;
    }
  }
}