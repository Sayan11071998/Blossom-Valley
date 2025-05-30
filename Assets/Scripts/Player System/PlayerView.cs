using BlossomValley.GameStrings;
using UnityEngine;
using BlossomValley.InventorySystem;
using BlossomValley.LandSystem;
using BlossomValley.TimeSystem;

namespace BlossomValley.PlayerSystem
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Transform interactorTransform;

        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float gravity = 9.81f;

        private CharacterController controller;
        private Animator animator;
        private PlayerController playerController;

        public PlayerModel PlayerModel => playerController.PlayerModel;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            PlayerModel playerModel = new PlayerModel(walkSpeed, runSpeed, gravity);
            playerController = new PlayerController(playerModel, this);

            playerModel.MoneyChanged += OnMoneyChanged;
        }

        private void Update()
        {
            HandleMovementInput();
            HandleInteractionInput();
            HandleRaycastSelection();
            HandleCheatShortcuts(); // For testing, remove later
        }

        private void HandleMovementInput()
        {
            float horizontal = Input.GetAxisRaw(GameString.Horizontal);
            float vertical = Input.GetAxisRaw(GameString.Vertical);
            bool isSprinting = Input.GetButton(GameString.Sprint);

            playerController.HandleMovement(horizontal, vertical, isSprinting);
        }

        private void HandleInteractionInput()
        {
            if (Input.GetButtonDown(GameString.InteractFire_1))
                playerController.Interact();

            if (Input.GetButtonDown(GameString.InteractFire_2))
                playerController.ItemInteract();

            if (Input.GetButtonDown(GameString.InteractFire_3))
                playerController.ItemKeep();
        }

        private void HandleRaycastSelection()
        {
            if (Physics.Raycast(interactorTransform.position, Vector3.down, out RaycastHit hit, 1))
            {
                Collider other = hit.collider;
                if (other.CompareTag(GameString.Land))
                {
                    LandView land = other.GetComponent<LandView>();
                    playerController.SelectLand(land);
                }
                else if (other.CompareTag(GameString.Item))
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
        }

        // Delete Before Final Build
        private void HandleCheatShortcuts()
        {
            if (Input.GetKey(KeyCode.RightBracket))
                TimeManager.Instance.Tick();

            if (Input.GetKeyDown(KeyCode.R))
                UIManager.Instance.ToggleRelationshipPanel();
        }
        //

        public void Move(Vector3 velocity, Vector3 direction, bool isSprinting)
        {
            controller.Move(velocity);

            if (direction.magnitude >= 0.1f)
                transform.rotation = Quaternion.LookRotation(direction);

            animator.SetFloat(GameString.SpeedAnimationFloat, direction.magnitude);
            animator.SetBool(GameString.RunAminationBool, isSprinting);
        }

        public bool IsGrounded() => controller.isGrounded;

        private void OnMoneyChanged() => UIManager.Instance.RenderPlayerStats();
    }
}