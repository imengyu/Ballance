using System.Collections;
using Ballance.Sys.Debug;
using Ballance2.Sys;
using Ballance2.Sys.UI;
using Ballance2.Utils;
using RuntimeGizmos;
using RuntimeSceneGizmo;
using SLua;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DebugCamera.cs
* 
* 用途：
* 调试摄像机主逻辑控制类。
*
* 作者：
* mengyu
*
*/

namespace Ballance2
{
    [CustomLuaClass]
    public class DebugCamera : MonoBehaviour
    {
        public SceneGizmoRenderer SceneGizmoRenderer;
        public TransformGizmo TransformGizmo;
        public DebugCameraTool Tool = DebugCameraTool.Drag;
        public bool EnableDebug = true;
        public bool Wireframe = true;
        public bool Audio = true;
        public bool Fog = true;
        public bool SkyBox = true;
        public static DebugCamera Instance;

        [DoNotToLua]
        public Texture2D cursorDragEyeTexture;
        [DoNotToLua]
        public Texture2D cursorDragEyeFreeTexture;
        [DoNotToLua]
        public Texture2D cursorDragHandTexture;
        private AudioListener AudioListener;

        public GameObject GameGrid;
        [HideInInspector]
        public GameDebugTools GameDebugTools;
        [HideInInspector]
        public GameDebugHierarchy GameDebugHierarchy;
        [HideInInspector]
        public Window GameDebugInspectorWindow;
        [HideInInspector]
        public Window GameDebugHierarchyWindow;
        public DebugControl DebugControl;

        public enum DebugCameraTool {
            Drag,
            TransformGizmo
        }
       
        private bool _EnableDebug  = false;
        private bool _Wireframe  = false;
        private bool _Audio = true;
        private bool _Fog = true;
        private bool _SkyBox = true;
        
        private void UpdateEnableDebug() {
            _EnableDebug = EnableDebug;
            if(GameManager.Instance != null)
                GameManager.Instance.SetGameBaseCameraVisible(EnableDebug);
            SceneGizmoRenderer.gameObject.SetActive(_EnableDebug);
            GameGrid.SetActive(_EnableDebug);
            TransformGizmo.enabled = _EnableDebug;
        }
        private void UpdateWireframe() {
            _Wireframe = Wireframe;
            mainCam.clearFlags = _Wireframe ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
        }
        private void UpdateAudio() {
            _Audio = Audio;
            AudioListener.enabled = _Audio;
        }
        private void UpdateFog() {
            _Fog = Fog;
            RenderSettings.fog = _Fog;
        }
        private void UpdateSkyBox() {
            _SkyBox = SkyBox;
            if(!_Wireframe)
                mainCam.clearFlags = _SkyBox ? CameraClearFlags.Skybox : CameraClearFlags.SolidColor ;
        }

        public float cameraSpeed = 4f;
        public float m_moveSensitivityX = 1f;
        public float m_moveSensitivityY = 1f;
        public float m_moveSensitivityZoom = 1f;
        //Roate camera
        public float m_sensitivityX = 10f;
        public float m_sensitivityY = 10f;
        // 水平方向的 镜头转向
        public float m_minimumX = -360f;
        public float m_maximumX = 360f;
        // 垂直方向的 镜头转向 (这里给个限度 最大仰角为45°)
        public float m_minimumY = -45f;
        public float m_maximumY = 45f;

        private float m_rotationY = 0f;
		private Vector3 prevMousePos = Vector3.zero;
		private Vector3 moveCamTargetPos = Vector3.zero;
		private Vector3 moveCamTargetLookPos = Vector3.zero;
		private bool moveCam = false;
        private Vector3 currentVelocity = Vector3.zero;
		private Camera mainCam;

		private void Awake()
		{
            mainCam = GetComponent<Camera>();
		}
        private void Start()
        {
            Instance = this;
            AudioListener = GetComponent<AudioListener>();
            TransformGizmo.onStartSelect = (t) => {
                if(GameDebugTools != null)
                    GameDebugTools.InspectObject(t);
            };
        }
		private void Update()
		{
            if(Wireframe != _Wireframe) UpdateWireframe();
            if(Audio != _Audio) UpdateAudio();
            if(Fog != _Fog) UpdateFog();
            if(SkyBox != _SkyBox) UpdateSkyBox();

            if(_EnableDebug) {
                //在GUI上拖动除外
                if (!EventSystem.current.IsPointerOverGameObject() && GUIUtility.hotControl == 0 && !TransformGizmo.isTransforming)
                {
                    bool mouseLeftMoveCamera = Tool == DebugCameraTool.Drag;

                    //左键(或者中键)平移
                    //===============================

                    if((mouseLeftMoveCamera && Input.GetMouseButton(0)) ||  Input.GetMouseButton(2))
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

                        if(GameDebugTools != null)
                            GameDebugTools.SetFreeDragging(true);
                        Cursor.SetCursor(cursorDragEyeFreeTexture, Vector2.zero, CursorMode.Auto);
                    }
                    else {
                        if(GameDebugTools != null)
                            GameDebugTools.SetFreeDragging(false);
                    }
                    if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)) {
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    }
#endif



                    //在右键按下时
                    if(Input.GetMouseButton(1)) {
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
                            else if (cameraSpeed < 50)
                                cameraSpeed -= 10f;
                            else if (cameraSpeed < 100)
                                cameraSpeed -= 50f;
                            else cameraSpeed -= 100f;

                            Log.D("CameraSpeed", cameraSpeed.ToString());
                        }
                        //减少移动速度
                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            if (cameraSpeed < 1)
                                cameraSpeed += 0.01f;
                            else if (cameraSpeed < 5)
                                cameraSpeed += 0.2f;
                            else if (cameraSpeed < 10)
                                cameraSpeed += 2f;
                            else if (cameraSpeed < 50)
                                cameraSpeed += 10f;
                            else if (cameraSpeed < 100)
                                cameraSpeed += 50f;
                            else cameraSpeed += 100f;

                            Log.D("CameraSpeed", cameraSpeed.ToString());
                        }
                    }
                    else {
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
        }
        private void LateUpdate()
        {
            if (moveCam)
            {
                transform.position = Vector3.SmoothDamp(transform.position, moveCamTargetPos, ref currentVelocity, 0.2f);
                transform.LookAt(moveCamTargetLookPos);
            }
        }
        
        [DoNotToLua]
        public void UpdateEnableDebugZ() {
            if(EnableDebug != _EnableDebug) UpdateEnableDebug();
        }
        public void AddObjectToTransformGizmo(Transform t) {
            TransformGizmo.AddTarget(t);
        }
        public void MoveCameraToTransform(Transform t) {
            moveCam = true;
            moveCamTargetPos = new Vector3(t.position.x - 5 * t.localScale.x, transform.position.y, 5 * t.position.z - t.localScale.z);
            moveCamTargetLookPos = t.position;
            StartCoroutine(MoveCameraToTransformLate());
        }
        private IEnumerator MoveCameraToTransformLate() {
            yield return new WaitForSeconds(1.2f);
            moveCam = false;
        }

        public void PrepareWindow() {
            DebugControl.PrepareWindow();
        }

        private void OnPreRender() {
            if(_Wireframe)
                GL.wireframe = true;
        }
        private void OnPostRender() {
            if(_Wireframe)
                GL.wireframe = false;
        }
            
    }
}
