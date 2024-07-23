using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float horizontalSpeed = 2f;
    public float maxHorizontalDistance = 3f;

    private Vector2 touchStartPosition;
    private Vector2 touchCurrentPosition;
    private bool isSwiping = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                touchCurrentPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isSwiping = false;
            }
        }
    }

    private void MoveCharacter()
    {
        Vector3 forwardMovement = transform.forward * moveSpeed * Time.fixedDeltaTime;
        Vector3 horizontalMovement = Vector3.zero;

        if (isSwiping)
        {
            float swipeDelta = touchCurrentPosition.x - touchStartPosition.x;
            float horizontalDelta = swipeDelta / Screen.width * horizontalSpeed;
            horizontalMovement = transform.right * horizontalDelta;
            float newXPosition = Mathf.Clamp(rb.position.x + horizontalMovement.x, -maxHorizontalDistance, maxHorizontalDistance);
            horizontalMovement = new Vector3(newXPosition - rb.position.x, 0, 0);
        }

        Vector3 newPosition = rb.position + forwardMovement + horizontalMovement;
        rb.MovePosition(newPosition);
    }
}
