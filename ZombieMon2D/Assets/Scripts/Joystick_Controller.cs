using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick_Controller : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float maxDistance = 100f;

    private float input;

    public float Input => input;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            float clampedX = Mathf.Clamp(localPoint.x, -maxDistance, maxDistance);

            handle.anchoredPosition = new Vector2(clampedX, 0f);
            input = clampedX / maxDistance;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        input = 0f;
    }
}