using System.Collections.Generic;
using Ballance2.Base;
using Ballance2.Game.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class HighscoreUIControl : GameSingletonBehavior<HighscoreUIControl>  {
    public TMP_Text TextLevelName;
    public RectTransform ContentView;
    public InputAction PrevPage;
    public InputAction NextPage;

    private int _CurrentIndex = 0;
    private List<TextItemData> _TextItem = new List<TextItemData>();
    private List<string> LevelNames;

    private class TextItemData {
      public TMP_Text textName;
      public TMP_Text testScore;
    }

    private void Start() {
      _CurrentIndex = 0;
      _TextItem.Clear();
      PrevPage.performed += (c) => Prev();
      NextPage.performed += (c) => Next();
      for (int i = 1; i <= 10; i++)
      {
         _TextItem.Add(new TextItemData {
          textName = ContentView.Find($"ItemHighscore{i}/TextValue").GetComponent<TMP_Text>(),
          testScore = ContentView.Find($"ItemHighscore{i}/TextScoreValue").GetComponent<TMP_Text>()
        });
      }
    }
    private void OnEnable() {
      PrevPage.Enable();
      NextPage.Enable();
    }
    private void OnDisable() {
      PrevPage.Disable();
      NextPage.Disable();
    }

    public void LoadLevelData(int index) {
      LevelNames = HighscoreManager.Instance.GetLevelNames();
      LoadLevelData(LevelNames[index]);
    }
    public void LoadLevelData(string name) {
      LevelNames = HighscoreManager.Instance.GetLevelNames();

      var data = HighscoreManager.Instance.GetData(name);
      if (data != null) {
        _CurrentIndex = LevelNames.IndexOf(name);

        for (int i = 0; i < 10; i++) {
          var item = data[i];
          if (item != null) {
            _TextItem[i].textName.text = item.name;
            _TextItem[i].testScore.text = item.score.ToString();
          } else {
            _TextItem[i].textName.text = "";
            _TextItem[i].testScore.text = "";
          }
        }    

        TextLevelName.text = name;
      } 
      else 
      {
        _CurrentIndex = 0;
        for (int i = 0; i < 10; i++) {
          _TextItem[i].textName.text = "";
          _TextItem[i].testScore.text = "";
        }  
        TextLevelName.text = $"{name}Fail";
      }
    }

    public void Next() {
      if (_CurrentIndex < LevelNames.Count - 1)
        _CurrentIndex++;
      else
        _CurrentIndex = 0;
      LoadLevelData(_CurrentIndex);
    }
    public void Prev() {
      if( _CurrentIndex > 0)
        _CurrentIndex--;
      else
        _CurrentIndex = LevelNames.Count - 1;
      LoadLevelData(_CurrentIndex);
    }
  }
}