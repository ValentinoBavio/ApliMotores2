using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour, IMovement, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Joystick_Controller joystick;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private float direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (joystick == null)
        {
            Debug.LogError("Joystick no está asignado en el inspector");
            return;
        }

        direction = joystick.Input;

        if (direction > 0)
            sprite.flipX = false;
        else if (direction < 0)
            sprite.flipX = true;
    }

    private void FixedUpdate()
    {
        Move(direction);
    }

    public void Move(float direction)
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

  
}
