using UnityEngine;

public class DragWindowCntrl : MonoBehaviour
{
    private RectTransform window;
    //delta drag
    private Vector2 delta;

    private void Awake()
    {
        window = (RectTransform)transform;
    }

    public void BeginDrag()
    {
        delta = Input.mousePosition - window.position;
    }
    public void Drag()
    {
        Vector2 newPos = (Vector2)Input.mousePosition - delta;
        Vector2 Transform = new Vector2(window.rect.width * transform.root.lossyScale.x, window.rect.height * transform.root.lossyScale.y);
        Vector2 OffsetMin, OffsetMax;
        OffsetMin.x = newPos.x - window.pivot.x * Transform.x;
        OffsetMin.y = newPos.y - window.pivot.y * Transform.y;
        OffsetMax.x = newPos.x + (1 - window.pivot.x) * Transform.x;
        OffsetMax.y = newPos.y + (1 - window.pivot.y) * Transform.y;
        if (OffsetMin.x < 0)
        {
            newPos.x = window.pivot.x * Transform.x;
        }
        else if (OffsetMax.x > Screen.width)
        {
            newPos.x = Screen.width - (1 - window.pivot.x) * Transform.x;
        }
        if (OffsetMin.y < 0)
        {
            newPos.y = window.pivot.y * Transform.y;
        }
        else if (OffsetMax.y > Screen.height)
        {
            newPos.y = Screen.height - (1 - window.pivot.y) * Transform.y;
        }
        window.position = newPos;
    }
}
