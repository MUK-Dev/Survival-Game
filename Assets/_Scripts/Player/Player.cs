using UnityEngine;

public class Player : MonoBehaviour
{
    private string IS_WALKING = "IsWalking";

    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask targetMask;

    private CharacterController controller;

    float m_MaxDistance;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        //? Choose the distance the Box can reach to
        m_MaxDistance = 300.0f;
    }

    void Update()
    {
        PlayerMovement();
    }

    void FixedUpdate()
    {
        if (AvailableItems.Instance.IsListActive()) CheckItemsToPickup();
    }

    private void CheckItemsToPickup()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + Vector3.up * 0.3f,
            transform.localScale * 1.5f, transform.position,
                transform.rotation, m_MaxDistance, targetMask);

        AvailableItems.Instance.UpdateItems(hits);
    }

    //? Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //? Draw a cube that extends to where the hit exists
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.3f, transform.localScale * 3);

    }

    private void PlayerMovement()
    {
        Vector3 motion = PlayerInputManager.Instance.GetMovementVector() * runSpeed;
        bool canMove = PlayerInputManager.Instance.CanMove();

        //? Animate the character
        animator.SetBool(IS_WALKING, canMove);

        //? Apply gravity
        motion += Physics.gravity;

        if (canMove)
        {
            //* Move and rotate the player using unity controller
            Vector3 lookRotation = new Vector3(motion.x, 0, motion.z);
            Quaternion targetRotation = Quaternion.LookRotation(lookRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            controller.Move(motion * Time.deltaTime);
        }
        else
        {
            controller.Move(motion * Time.deltaTime);
        }
    }
}
