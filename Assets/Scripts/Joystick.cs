using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static Joystick instance;

    [Header("Joystick Settings")]
    public RectTransform outerCircle;
    public RectTransform innerCircle;
    public float maxDistance = 100f;

    private Vector2 inputVector;

    private void Awake()
    {
        instance = this;
    }

    public Vector2 InputVector => inputVector;
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(null, outerCircle.position);
        inputVector = (eventData.position - position) / maxDistance;

        inputVector = inputVector.magnitude > 1.0f ? inputVector.normalized : inputVector;

        innerCircle.anchoredPosition = inputVector * maxDistance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            outerCircle.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        outerCircle.anchoredPosition = localPoint;
        innerCircle.anchoredPosition = Vector2.zero;

        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        innerCircle.anchoredPosition = Vector2.zero;
    }
}