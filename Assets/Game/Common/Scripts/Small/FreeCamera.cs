using System.Collections;
using RuntimeGizmos;
using RuntimeSceneGizmo;
using UnityEngine;
using UnityEngine.EventSystems;
using static Ballance2.Services.GameManager;

/*
 * Copyright (c) 2022 mengyu
 * 
 * 模块名：     
 * FreeCamera.cs
 * 
 * 用途：
 * 自由摄像机脚本，包含了方向键移动，鼠标左键拖动，右键旋转视图，滚轮缩放功能。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  /// <summary>
  /// 自由摄像机脚本，包含了方向键移动，鼠标左键拖动，右键旋转视图，滚轮缩放功能。
  /// </summary>
  public class FreeCamera : MonoBehaviour
  {
    [Tooltip("是否启用线框渲染")]
    /// <summary>
    /// 是否启用线框渲染
    /// </summary>
    public bool Wireframe = false;
    [Tooltip("是否启用当前摄像机上的 AudioListener")]
    /// <summary>
    /// 是否启用当前摄像机上的 AudioListener
    /// </summary>
    public bool Audio = true;
    [Tooltip("是否启用雾渲染")]
    /// <summary>
    /// 是否启用雾渲染
    /// </summary>
    public bool Fog = true;
    [Tooltip("是否启用天空盒渲染")]
    /// <summary>
    /// 是否启用天空盒渲染
    /// </summary>
    public bool SkyBox = true;
    [Tooltip("获取当前摄像机的一个实例。")]
    /// <summary>
    /// 获取当前摄像机的一个实例。
    /// </summary>
    public static FreeCamera Instance;
    [Tooltip("摄像机速度更改事件")]
    /// <summary>
    /// 摄像机速度更改事件
    /// </summary>
    public VoidDelegate onCamSpeedChanged;

    [Tooltip("眼睛鼠标")]
    public Texture2D cursorDragEyeTexture;
    [Tooltip("拖动时的眼睛鼠标")]
    public Texture2D cursorDragEyeFreeTexture;
    [Tooltip("拖动时的鼠标")]
    public Texture2D cursorDragHandTexture;
    private AudioListener AudioListener;

    private bool _Wireframe = false;
    private bool _Audio = true;
    private bool _Fog = true;
    private bool _SkyBox = true;

    private void UpdateWireframe()
    {
      _Wireframe = Wireframe;
      mainCam.clearFlags = _Wireframe ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
    }
    private void UpdateAudio()
    {
      _Audio = Audio;
      AudioListener.enabled = _Audio;
    }
    private void UpdateFog()
    {
      _Fog = Fog;
      RenderSettings.fog = _Fog;
    }
    private void UpdateSkyBox()
    {
      _SkyBox = SkyBox;
      if (!_Wireframe)
        mainCam.clearFlags = _SkyBox ? CameraClearFlags.Skybox : CameraClearFlags.SolidColor;
    }

    [Tooltip("摄像机键盘移动速度")]
    /// <summary>
    /// 摄像机键盘移动速度
    /// </summary>
    public float cameraSpeed = 4f;
    [Tooltip("摄像机鼠标水平移动速度")]
    /// <summary>
    /// 摄像机鼠标水平移动速度
    /// </summary>
    public float m_moveSensitivityX = 1f;
    [Tooltip("摄像机鼠标垂直移动速度")]
    /// <summary>
    /// 摄像机鼠标垂直移动速度
    /// </summary>
    public float m_moveSensitivityY = 1f;
    [Tooltip("摄像机缩放速度")]
    /// <summary>
    /// 摄像机缩放速度
    /// </summary>
    public float m_moveSensitivityZoom = 1f;
    //Roate camera
    [Tooltip("摄像机水平移动速度")]
    /// <summary>
    /// 摄像机水平移动速度
    /// </summary>
    public float m_sensitivityX = 10f;
    [Tooltip("摄像机垂直移动速度")]
    /// <summary>
    /// 摄像机垂直移动速度
    /// </summary>
    public float m_sensitivityY = 10f;
    // 水平方向的 镜头转向
    [Tooltip("摄像机转向最小角度")]
    /// <summary>
    /// 摄像机转向最小角度
    /// </summary>
    public float m_minimumX = -360f;
    [Tooltip("摄像机转向最大角度")]
    /// <summary>
    /// 摄像机转向最大角度
    /// </summary>
    public float m_maximumX = 360f;
    // 垂直方向的 镜头转向 (这里给个限度 最大仰角为45°)
    [Tooltip("镜头转向垂直最小仰角")]
    /// <summary>
    /// 镜头转向垂直最小仰角
    /// </summary>
    public float m_minimumY = -45f;
    [Tooltip("镜头转向垂直最大仰角")]
    /// <summary>
    /// 镜头转向垂直最大仰角
    /// </summary>
    public float m_maximumY = 45f;

    private float m_rotationY = 0f;
    private Vector3 prevMousePos = Vector3.zero;
    private Vector3 moveCamTargetPos = Vector3.zero;
    private Vector3 moveCamTargetLookPos = Vector3.zero;
    private bool moveCam = false;
    private Vector3 currentVelocity = Vector3.zero;
    private Camera mainCam;
#if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
    private int lastDownTouchId = 0;
#endif

    private void Awake()
    {
      mainCam = GetComponent<Camera>();
    }
    private void Start()
    {
      Instance = this;
      AudioListener = GetComponent<AudioListener>();
    }
    private void Update()
    {
      if (Wireframe != _Wireframe) UpdateWireframe();
      if (Audio != _Audio) UpdateAudio();
      if (Fog != _Fog) UpdateFog();
      if (SkyBox != _SkyBox) UpdateSkyBox();


      //在GUI上拖动除外
      if (!EventSystem.current.IsPointerOverGameObject() && GUIUtility.hotControl == 0)
      {
        const bool mouseLeftMoveCamera = true;

        //左键(或者中键)平移
        //===============================

        if ((mouseLeftMoveCamera && Input.GetMouseButton(0)) || Input.GetMouseButton(2))
        {
          Vector3 p0 = transform.position;
          Vector3 p01 = p0 - transform.right * Input.GetAxisRaw("Mouse X") * m_moveSensitivityX * Time.timeScale;
          transform.position = p01 - transform.up * Input.GetAxisRaw("Mouse Y") * m_moveSensitivityY * Time.timeScale;

          Cursor.SetCursor(cursorDragHandTexture, Vector2.zero, CursorMode.Auto);
        }

        //右键旋转
        //===============================

#if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
        //移动端是两个手指旋转
        else if (Input.touchCount > 0 && lastDownTouchId >= 0)
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == lastDownTouchId && Input.touches[i].phase == TouchPhase.Moved)
                {
                    float m_rotationX = transform.localEulerAngles.y + Input.touches[i].deltaPosition.x * Time.deltaTime * m_sensitivityX;
                    m_rotationY += Input.touches[i].deltaPosition.y * Time.deltaTime * m_sensitivityY;
                    m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);
                    transform.localEulerAngles = new Vector3(-m_rotationY, m_rotationX, 0);
                    break;
                }
            }
        }
        else lastDownTouchId = 0;
#else
        else if (Input.GetMouseButton(1))
        {
          float m_rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * m_sensitivityX;
          m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY;
          m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);

          transform.localEulerAngles = new Vector3(-m_rotationY, m_rotationX, 0);

          Cursor.SetCursor(cursorDragEyeFreeTexture, Vector2.zero, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
          Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
#endif  
        //在右键按下时
        if (Input.GetMouseButton(1))
        {
          //空格键抬升高度
          if (Input.GetKey(KeyCode.Q))
          {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime, Space.Self);
          }
          if (Input.GetKey(KeyCode.E))
          {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime, Space.Self);
          }
          //w键
          if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
          {
            this.gameObject.transform.Translate(new Vector3(0, 0, cameraSpeed * Time.deltaTime));
          }
          //s键
          if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
          {
            this.gameObject.transform.Translate(new Vector3(0, 0, -cameraSpeed * Time.deltaTime));
          }
          //a键
          if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
          {
            this.gameObject.transform.Translate(new Vector3(-(cameraSpeed) * Time.deltaTime, 0, 0));
          }
          //d键
          if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
          {
            this.gameObject.transform.Translate(new Vector3((cameraSpeed) * Time.deltaTime, 0, 0));
          }

          //增加移动速度
          if (Input.GetAxis("Mouse ScrollWheel") < 0)
          {
            if (cameraSpeed < 1)
              cameraSpeed -= 0.01f;
            else if (cameraSpeed < 5)
              cameraSpeed -= 0.2f;
            else if (cameraSpeed < 10)
              cameraSpeed -= 2f;
            else if (cameraSpeed < 150)
              cameraSpeed -= 10f;
            else if (cameraSpeed < 500)
              cameraSpeed -= 50f;
            else 
              cameraSpeed -= 100f;

            onCamSpeedChanged?.Invoke();
          }
          //减少移动速度
          if (Input.GetAxis("Mouse ScrollWheel") > 0)
          {
            if (cameraSpeed < 1)
              cameraSpeed += 0.01f;
            else if (cameraSpeed < 10)
              cameraSpeed += 0.2f;
            else if (cameraSpeed < 20)
              cameraSpeed += 2f;
            else if (cameraSpeed < 150)
              cameraSpeed += 10f;
            else if (cameraSpeed < 500)
              cameraSpeed += 50f;
            else cameraSpeed += 100f;

            onCamSpeedChanged?.Invoke();
          }
        }
        else
        {
          //（缩小）后退
          if (Input.GetAxis("Mouse ScrollWheel") < 0)
          {
            this.gameObject.transform.Translate(new Vector3(0, 0, cameraSpeed * -m_moveSensitivityZoom * Time.deltaTime));
          }
          //（放大）前进
          if (Input.GetAxis("Mouse ScrollWheel") > 0)
          {
            this.gameObject.transform.Translate(new Vector3(0, 0, cameraSpeed * m_moveSensitivityZoom * Time.deltaTime));
          }

        }
      }
      
    }
    private void LateUpdate()
    {
      if (moveCam)
      {
        transform.position = Vector3.SmoothDamp(transform.position, moveCamTargetPos, ref currentVelocity, 0.2f);
        transform.LookAt(moveCamTargetLookPos);
      }
    }

    private void OnPreRender()
    {
      if (_Wireframe)
        GL.wireframe = true;
    }
    private void OnPostRender()
    {
      if (_Wireframe)
        GL.wireframe = false;
    }

  }
}