using System.Collections.Generic;
using SubjectNerd.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Window.cs
* 
* 用途：
* 一个上下数值组件
* 
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.Controls
{
  /// <summary>
  /// 一个上下数值组件
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu("Ballance/UI/Controls/Updown")]
  public class Updown : UIBehaviour
  {
    public Button DownBtn;
    public Button UpBtn;
    public UIText TextValue;
    public UIText TextTitle;
    public float MinValue = 0;
    public float MaxValue = 100;
    public float StepValue = 10;
    public string PrependText = "";
    public string AppendText = "";
    public string ToStringFormat = "00";
    public string UnknowOptionString = "未知";
    public bool UseOptions = false;

    [SerializeField, HideInInspector]
    private float _value = 0;
    public float value
    {
      get { return _value; }
      set
      {
        if (_value != value)
        {
          _value = value;
          UpdateValue();
        }
      }
    }
    [SerializeField]
    [Reorderable()]
    public List<string> options = new List<string>();

    public void AddOption(string s)
    {
      options.Add(s);
      SetMaxMinByOptions();
    }
    public void RemoveOption(string s)
    {
      options.Remove(s);
      SetMaxMinByOptions();
    }
    public void RemoveOption(int index)
    {
      options.RemoveAt(index);
      SetMaxMinByOptions();
    }
    public void SetOptions(List<string> options)
    {
      this.options = options;
      UseOptions = true;
      SetMaxMinByOptions();
    }

    private void SetMaxMinByOptions()
    {
      if (UseOptions)
      {
        StepValue = 1;
        MinValue = 0;
        MaxValue = options.Count - 1;
      }
    }

    public Slider.SliderEvent onValueChanged;

    public void UpdateValue()
    {
      if (UseOptions)
      {
        int index = (int)_value;
        string str;
        if (index < 0 || index >= options.Count) str = UnknowOptionString;
        else str = options[index];
        TextValue.text = PrependText + str + AppendText;
      }
      else
      {
        TextValue.text = PrependText + _value.ToString(ToStringFormat) + AppendText;
      }
    }

    protected override void Start()
    {
      base.Start();
      if (DownBtn != null)
        DownBtn.onClick.AddListener(() =>
        {
          value -= StepValue;
          if (value < MinValue) value = MinValue;
          if (onValueChanged != null) onValueChanged.Invoke(value);
        });
      if (UpBtn != null)
        UpBtn.onClick.AddListener(() =>
        {
          value += StepValue;
          if (value > MaxValue) value = MaxValue;
          if (onValueChanged != null) onValueChanged.Invoke(value);
        });
      UpdateValue();
    }
  }
}
