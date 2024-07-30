﻿using Ballance2.UI.Core.Controls;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameGlobalErrorUI.cs
* 
* 用途：
* 全局错误UI逻辑控制
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.CoreUI
{
  public class GameGlobalErrorUI : MonoBehaviour
  {
    public UIText TextGameErrorTitle = null;
    public UIText TextGameErrorHelp = null;
    public UIText TextGameErrorText = null;
    public Button ButtonShowErrorContent = null;
    public Button ButtonHideErrorContent = null;
    public GameObject ScrollViewErrorContent = null;
    public Button ButtonCloseGame = null;
    public Button ButtonRestartGame = null;
    public Button ButtonDebugger = null;
    public Button ButtonContinue = null;

    private void Start()
    {
      ButtonShowErrorContent.onClick.AddListener(() =>
      {
        ButtonShowErrorContent.gameObject.SetActive(false);
        ButtonHideErrorContent.gameObject.SetActive(true);
        TextGameErrorHelp.gameObject.SetActive(false);
        ScrollViewErrorContent.gameObject.SetActive(true);
      });
      ButtonHideErrorContent.onClick.AddListener(() =>
      {
        ButtonShowErrorContent.gameObject.SetActive(true);
        ButtonHideErrorContent.gameObject.SetActive(false);
        TextGameErrorHelp.gameObject.SetActive(true);
        ScrollViewErrorContent.gameObject.SetActive(false);
      });
      ButtonCloseGame.onClick.AddListener(() =>
      {
        GameSystem.QuitPlayer();
      });
      ButtonDebugger.onClick.AddListener(() =>
      {
        PlayerPrefs.SetInt("core.DebugMode", 1);
        GameSystem.QuitPlayer();
      });
      ButtonContinue.onClick.AddListener(() =>
      {
        gameObject.SetActive(false);
      });
    }

    public void ShowErrorUI(string err)
    {
      var GameGlobalMask = GameObject.Find("GameGlobalMask");
      if(GameGlobalMask != null) 
        GameGlobalMask.SetActive(false);
      gameObject.SetActive(true);
      TextGameErrorText.text = err;
    }
  }
}
