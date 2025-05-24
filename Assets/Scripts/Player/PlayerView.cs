using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private PlayerModel playerModel;
    private PlayerController playerController;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerModel = new PlayerModel();
        playerController = new PlayerController(playerModel, this);

        playerModel.MoneyChanged += OnMoneyChanged;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isSprinting = Input.GetButton("Sprint");

        playerController.HandleMovement(horizontal, vertical, isSprinting);

        if (Input.GetButtonDown("Fire1"))
            playerController.Interact();

        if (Input.GetButtonDown("Fire2"))
            playerController.ItemInteract();

        if (Input.GetButtonDown("Fire3"))
            playerController.ItemKeep();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            Collider other = hit.collider;
            if (other.tag == "Land")
            {
                Land land = other.GetComponent<Land>();
                playerController.SelectLand(land);
            }
            else if (other.tag == "Item")
            {
                InteractableObject interactable = other.GetComponent<InteractableObject>();
                playerController.SelectInteractable(interactable);
            }
            else
            {
                playerController.Deselect();
            }
        }
        else
        {
            playerController.Deselect();
        }

        // Game Cheats. Need to Delete Later!!
        if (Input.GetKey(KeyCode.RightBracket))
            TimeManager.Instance.Tick();
        if (Input.GetKeyDown(KeyCode.R))
            UIManager.Instance.ToggleRelationshipPanel();
    }

    public void Move(Vector3 velocity, Vector3 direction, bool isSprinting)
    {
        controller.Move(velocity);

        if (direction.magnitude >= 0.1f)
            transform.rotation = Quaternion.LookRotation(direction);

        animator.SetFloat("Speed", direction.magnitude);
        animator.SetBool("Running", isSprinting);
    }

    public bool IsGrounded() => controller.isGrounded;

    private void OnMoneyChanged() => UIManager.Instance.RenderPlayerStats();

    public PlayerModel GetPlayerModel() => playerModel;
}