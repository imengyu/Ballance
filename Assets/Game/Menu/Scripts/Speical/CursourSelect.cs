using System;
using Ballance2.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class CursourSelect : MonoBehaviour 
  {
    public InputAction Move;
    public InputAction Pos;
    public InputAction Select;

    public RectTransform Arrow;
    public float MaxMoveSpeed = 10f;

    private RectTransform rectTransform;
    private Vector2 realSize;

    private void Start() {
      Move.performed += (e) => {
        UpdateMoveSpeed(e.ReadValue<Vector2>());
      };
      Move.canceled += (e) => {
        UpdateMoveSpeed(Vector2.zero);
      };
      Pos.performed += (e) => {
        UpdatePos(e.ReadValue<Vector2>());
      };
      Select.performed += (e) => {
        if (e.ReadValueAsButton())
          onSelect?.Invoke(currentPosition);
      };
      
      rectTransform = transform as RectTransform;
      UpdatePos(new Vector2(Screen.width / 2, Screen.height / 2));
    }

    public Action<Vector2> onSelect;
    public Action<Vector2> onRecast;

    private Vector2 currentSpeed = Vector2.zero;
    private Vector2 currentPosition = Vector2.zero;

    private void UpdateMoveSpeed(Vector2 pos) 
    {
      currentSpeed = pos;
    }
    private void UpdatePos(Vector2 pos) 
    {
      if (pos.x < 0) pos.x = 0;
      if (pos.y < 0) pos.y = 0;
      if (pos.x > Screen.width) pos.x = Screen.width;
      if (pos.y > Screen.height) pos.y = Screen.height;
      currentPosition = pos;
      Arrow.anchoredPosition = new Vector2(
        currentPosition.x / Screen.width * rectTransform.rect.size.x,
        currentPosition.y / Screen.height * rectTransform.rect.size.y
      );
      onRecast?.Invoke(currentPosition);
    }

    private void Update() 
    {
      if (currentSpeed.x != 0 || currentSpeed.y != 0)
      UpdatePos(new Vector2(
        currentPosition.x + currentSpeed.x * MaxMoveSpeed,
        currentPosition.y + currentSpeed.y * MaxMoveSpeed
      ));
    }

    private void OnEnable() 
    {
      GameTimer.Delay(0.8f, () => {
        Select.Enable();
        Pos.Enable();
        Move.Enable();
      });
    }
    private void OnDisable() 
    {
      Select.Disable();
      Pos.Disable();
      Move.Disable();
    }
  }
}