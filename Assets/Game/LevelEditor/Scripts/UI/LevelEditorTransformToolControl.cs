using System;
using System.Collections.Generic;
using System.Linq;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Battlehub.RTCommon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorTransformToolControl : MonoBehaviour 
  {
    public UIButtonActiveState ButtonToolMove;
    public UIButtonActiveState ButtonToolRotation;
    public UIButtonActiveState ButtonToolScale;
    public Image HandlePositionImage;
    public Image HandleRotationImage;
    public TMP_Text HandlePositionText;
    public TMP_Text HandleRotationText;

    public Sprite PovitRotLocal;
    public Sprite PovitRotGrobal;
    public Sprite PovitCenter;
    public Sprite PovitPovit;

    private IRTE m_editor;
    private ResourcePreviewUtility m_resourcePreview;
    private IRuntimeSelection m_selection;

    public Action<LevelDynamicModel[]> OnSelect;
    public Action<LevelDynamicModel[]> OnDelete;

    private void Start()
    {
      m_editor = IOC.Resolve<IRTE>();
      m_editor.Tools.ToolChanged += OnToolChanged;
      m_editor.Tools.PivotModeChanged += OnPivotModeChanged;
      m_editor.Tools.PivotRotationChanged += OnPivotRotationChanged;
      m_selection = IOC.Resolve<IRTE>().Selection;
      m_selection.SelectionChanged += OnSelectionChanged;
      m_resourcePreview = gameObject.AddComponent<ResourcePreviewUtility>();
      IOC.Register<IResourcePreviewUtility>(m_resourcePreview);
    }
    private void OnDestroy() {
      if (m_selection != null)
        m_selection.SelectionChanged -= OnSelectionChanged;
      IOC.Unregister<IResourcePreviewUtility>(m_resourcePreview);
    }

    public void DoDeleteSeletedObjects()
    {
      ExposeToEditor[] exposed = m_editor.Selection.gameObjects
        .Where(o => o != null)
        .Select(o => o.GetComponent<ExposeToEditor>())
        .Where(o => o != null && o.CanDelete)
        .ToArray();

      OnDelete.Invoke(m_editor.Selection.gameObjects
        .Where(o => o != null)
        .Select(o => o.GetComponent<LevelEditorObjectSelectionData>())
        .Where(o => o != null && o.LevelDynamicModel.AssetRef.CanDelete)
        .Select(o => o.LevelDynamicModel)
        .ToArray());

      m_editor.Undo.BeginRecord();
      m_editor.Selection.objects = null;
      m_editor.Undo.DestroyObjects(exposed);
      m_editor.Undo.EndRecord();
    }
    public void ClearSelection()
    {
      m_editor.Selection.objects = new UnityEngine.Object[0];
    }

    private List<UnityEngine.Object> lastSelectObjects = new List<UnityEngine.Object>();
    private void OnSelectionChanged(UnityEngine.Object[] unselectedObjects)
    {
      if (unselectedObjects != null)
        foreach (var item in unselectedObjects)
          lastSelectObjects.Remove(item);
      if (m_selection.objects != null)
        lastSelectObjects.AddRange(m_selection.objects);
      OnSelect?.Invoke(lastSelectObjects
        .Where(o => o != null)
        .Select(o => ((GameObject)o).GetComponent<LevelEditorObjectSelectionData>())
        .Where(o => o != null)
        .Select(o => o.LevelDynamicModel)
        .ToArray());
    }

    private void OnToolChanged()
    {
      ButtonToolMove.SetActive(false);
      ButtonToolRotation.SetActive(false);
      ButtonToolScale.SetActive(false);

      RuntimeTool tool = m_editor.Tools.Current;
      switch(tool)
      {
        case RuntimeTool.Move:
          ButtonToolMove.SetActive(true);
          break;
        case RuntimeTool.Rotate:
          ButtonToolRotation.SetActive(true);
          break;
        case RuntimeTool.Scale:
          ButtonToolScale.SetActive(true);
          break;
        case RuntimeTool.Rect:
        case RuntimeTool.None:
          break;
      }
    }
    private void OnPivotModeChanged()
    {
      switch(m_editor.Tools.PivotMode)
      {
        case RuntimePivotMode.Pivot: HandlePositionImage.sprite = PovitPovit; break;
        case RuntimePivotMode.Center: HandlePositionImage.sprite = PovitCenter; break;
      }
      HandlePositionText.text = m_editor.Tools.PivotMode.ToString();
    }
    private void OnPivotRotationChanged()
    {
      switch(m_editor.Tools.PivotRotation)
      {
        case RuntimePivotRotation.Global: HandleRotationImage.sprite = PovitRotGrobal; break;
        case RuntimePivotRotation.Local: HandleRotationImage.sprite = PovitRotLocal; break;
      }
      HandleRotationText.text = m_editor.Tools.PivotRotation.ToString();
    }

    public void ToolMove()
    {
      m_editor.Tools.Current = RuntimeTool.Move;
    }
    public void ToolRotate()
    {
      m_editor.Tools.Current = RuntimeTool.Rotate;
    }
    public void ToolScale()
    {
      m_editor.Tools.Current = RuntimeTool.Scale;
    }
    public void ToolHandlePosition()
    {
      m_editor.Tools.PivotMode = m_editor.Tools.PivotMode == RuntimePivotMode.Center ? RuntimePivotMode.Pivot : RuntimePivotMode.Center;
    }
    public void ToolHandleRotation()
    {
      m_editor.Tools.PivotRotation = m_editor.Tools.PivotRotation == RuntimePivotRotation.Global ? RuntimePivotRotation.Local : RuntimePivotRotation.Global;
    }
  }
}