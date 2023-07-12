using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenDrag : OnScreenControl,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    [InputControl(layout = "Delta")]
    [SerializeField] string m_ControlPath;
    [SerializeField] float sencitivity = 2;

    protected override string controlPathInternal { get => m_ControlPath; set => m_ControlPath = value; }

    Vector2 prevPos;
    Vector2 change;
    Vector2 Value;
    float dpi;

    private void Start()
    {
        dpi = Screen.dpi;
    }
    private void Update()
    {

        Value = (change / dpi) * sencitivity;
        SendValueToControl(Value);
    }

    public void OnDrag(PointerEventData eventData)
    {
        change = prevPos - eventData.position;
        prevPos = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        prevPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        change = Vector2.zero;
    }
}
