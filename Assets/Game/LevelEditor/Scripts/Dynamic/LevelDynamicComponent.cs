using UnityEditor;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicComponent : MonoBehaviour
  {
    /// <summary>
    /// 更新当前动态物体的形状
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual void UpdateShape()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// 更新编辑器用于选择的MeshCollider
    /// </summary>
    public virtual void UpdateEditorMesh(Mesh mesh)
    {
      var meshCol = transform.parent?.gameObject?.GetComponent<MeshCollider>();
      if (meshCol != null)
        meshCol.sharedMesh = mesh;
    }

    protected virtual void OnUpdateControllers() { }

    private bool enableEdit = false;

    public bool EnableEdit
    {
      get => enableEdit;
      set
      {
        if (enableEdit != value)
        {
          enableEdit = value;
          OnUpdateControllers();
        }
      }
    }
  }
}
