using UnityEngine;
using PhysicsRT;
using UnityEngine.EventSystems;
using System.Collections;

public class Test : MonoBehaviour {

    Camera mainCamera;
    public float cameraSpeed = 4;
    public float shootForce = 100;
    public GameObject ballPrefab = null;
    public int targetFps = 60;

    private void Start() {  
        mainCamera = GetComponent<Camera>();
        Application.targetFrameRate = targetFps;
    }
    private void Update() {
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
            this.gameObject.transform.Translate(new Vector3(0, 0, 2 * cameraSpeed * Time.deltaTime));
        }
        //s键
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            this.gameObject.transform.Translate(new Vector3(0, 0, 2 * -cameraSpeed * Time.deltaTime));
        }
        //a键
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.gameObject.transform.Translate(new Vector3(-(cameraSpeed) * Time.deltaTime, 0, 0));
        }
        //d键
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) )
        {
            this.gameObject.transform.Translate(new Vector3((cameraSpeed) * Time.deltaTime, 0, 0));
        }

        //Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (cameraSpeed < 1)
                cameraSpeed -= 0.01f;
            else if (cameraSpeed < 10)
                cameraSpeed -= 0.1f;
            else if (cameraSpeed < 50)
                cameraSpeed -= 1f;
            else if (cameraSpeed < 100)
                cameraSpeed -= 5f;
            else cameraSpeed -= 10f;

            ShowText("Camera Speed : " + cameraSpeed);
        }
        //Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (cameraSpeed < 1)
                cameraSpeed += 0.01f;
            else if (cameraSpeed < 10)
                cameraSpeed += 0.1f;
            else if (cameraSpeed < 50)
                cameraSpeed += 1f;
            else if (cameraSpeed < 100)
                cameraSpeed += 5f;
            else cameraSpeed += 10f;

            ShowText("Camera Speed : " + cameraSpeed);
        }

        if (!EventSystem.current.IsPointerOverGameObject() && GUIUtility.hotControl == 0)
        {
            //左键按下事件
            //
            if (Input.GetMouseButtonDown(0) && !mouseLeftDowned)
            {
                Shoot(false);
                mouseLeftDowned = true;
            }
            if (Input.GetMouseButtonUp(0) && mouseLeftDowned)
                mouseLeftDowned = false;
        
        }

        if (Input.GetMouseButton(1))
        {
            float m_rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * m_sensitivityX;
            m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY;
            m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);

            transform.localEulerAngles = new Vector3(-m_rotationY, m_rotationX, 0);
        }
    }

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

    // UI Events
    //===============================

    Rect uiBoxRect = new Rect(12, 30, 200, 73);
    int textShowInt = 0;
    string textShow = "";

    private void ShowText(string text, int sec = 3)
    {
        textShow = text;
        textShowInt = sec * 60;
    }
    private void OnGUI()
    {
        GUI.Box(uiBoxRect, "");
        GUI.Label(new Rect(18, 36, 190, 46), "Camera : " + transform.position.ToString() + "\n" +
            transform.rotation.ToString() + "\n" +
             transform.eulerAngles.ToString());
        if (textShowInt > 0)
        {
            GUI.Label(new Rect(200, 15, 300, 30), textShow);
            textShowInt--;
        }
    }

    // Other
    //===============================
    
    bool mouseLeftDowned = false;

    public void Shoot(bool center)
    {
        ShowText("Shoot !");

        //获取鼠标点击位置
        //创建射线;从摄像机发射一条经过鼠标当前位置的射线

        Vector3 mousePos = Input.mousePosition;
        if (center) mousePos = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        
        //从摄像机的位置创建一个带有刚体的球ballPrefab为预制小球
        GameObject go = null;
        go = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        go.transform.rotation = Random.rotation;

        //发射数来的球沿着摄像机到鼠标点击的方向进行移动
        PhysicsBody ball = go.GetComponent<PhysicsBody>();   
        StartCoroutine(LateAddForce(ball, shootForce * ray.direction));
    }

    private IEnumerator LateAddForce(PhysicsBody ball, Vector3 f) {
      yield return new WaitForSeconds(0.1f);
      ball.ApplyForce(f);
    }
}