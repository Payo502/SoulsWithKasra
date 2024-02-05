using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    public PlayerManager player;

    // THINK ABOUT GOALS IN STEPS
    // 1. FIND A WAY TO READ THE VALUES OF JOY STICK
    // 2. MOVE CHARACTER ABSED ON THOSE VALUES

    PlayerControls playerControls;

    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 cameraMovementInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("LOCK ON INPUT")]
    [SerializeField] bool lockOn_Input;
    [SerializeField] bool lockOn_Left_Input;
    [SerializeField] bool lockOn_Right_Input;
    private Coroutine lockOnCoroutine;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float vertical_Input;
    public float horizontal_Input;
    public float moveAmount;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool dodge_Input = false;
    [SerializeField] bool sprint_Input = false;
    [SerializeField] bool jump_Input = false;
    [SerializeField] bool switch_Right_Weapon_Input = false;
    [SerializeField] bool switch_Left_Weapon_Input = false;

    [Header("BUMPER INPUTS")]
    [SerializeField] bool RB_Input = false;

    [Header("TRIGGER INPUTS")]
    [SerializeField] bool RT_Input = false;
    [SerializeField] bool Hold_RT_Input = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // WHEN SCENE CHANGES, RUN THIS LOGIC
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;

        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // IF LOADING INTO WORLD SCENE, ENABLE OUR PLAYER CONTROLS
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldIndex())
        {
            instance.enabled = true;


            if (playerControls != null)
            {
                playerControls.Enable();
            }
        }
        // OTHERWISE WE ARE AT THE MENU SO DISABLE PLAYER CONTROLS
        // THIS IS SO OUR PLAYER CANT MOVE IF WE ENTER THINGS LIKE CHARACTER CREATION MENU ETC
        else
        {
            instance.enabled = false;


            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraMovementInput = i.ReadValue<Vector2>();

            // ACTIONS
            playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;

            // BUMPERS
            playerControls.PlayerActions.RB.performed += i => RB_Input = true;

            // TRIGGERS
            playerControls.PlayerActions.RT.performed += i => RT_Input = true;
            playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
            playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;

            // LOCK ON INPUT
            playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
            playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
            playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;

            // HOLDING THE INPUT, SETS THE BOOL TO TRUE
            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            // RELEASING THE INPUT, SETS THE BOOL TO FALSE
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // UNSUBSCRIBE FROM THE EVENT ON DESTROY
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    // IF WE MINIMIZE WINDOW, STOP ADJUSTING INPUTS
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintingInput();
        HandleJumpInput();
        HandleRBInput();
        HandleRTInput();
        HandleHoldRTInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
    }

    // LOCK ON 
    private void HandleLockOnInput()
    {
        // CHECK FOR DEAD TARGET
        if (player.playerNetworkManager.isLockedOn.Value)
        {
            if (player.playerCombatManager.currentTarget == null)
                return;

            if (player.playerCombatManager.currentTarget.isDead.Value)
            {
                player.playerNetworkManager.isLockedOn.Value = false;
            }

            // TRY TO FIND NEW TARGET

            // MAKES SURE COROUTINE IS NOT RUN MORE THAN ONCE
            if (lockOnCoroutine != null)
                StopCoroutine(lockOnCoroutine);

            lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
        }

        if (lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
        {
            lockOn_Input = false;
            PlayerCamera.instance.ClearLockOnTargets();
            player.playerNetworkManager.isLockedOn.Value = false;
            // DISABLE LOCK ON
            return;
        }

        if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
        {
            lockOn_Input = false;

            // IF USING RANGED WEAPON YOU CANT LOCK ON

            // Enable LOCK ON
            PlayerCamera.instance.HandleLocatingLockOnTargets();

            if (PlayerCamera.instance.nearestLockOnTarget != null)
            {
                // SET THE TARGET AS CURRENT TARGET
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }
        }
    }

    private void HandleLockOnSwitchTargetInput()
    {
        if (lockOn_Left_Input)
        {
            lockOn_Left_Input = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                }
            }
        }

        if (lockOn_Right_Input)
        {
            lockOn_Right_Input = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                }
            }
        }
    }

    // MOVEMENT

    private void HandlePlayerMovementInput()
    {
        vertical_Input = movementInput.y;
        horizontal_Input = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(vertical_Input) + Mathf.Abs(horizontal_Input));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        // WHY DO WE PASS 0 IN HORIZONTAL? BECAUSE WE ONLY WANT NON STRAFFING MOVEMENT
        // WE USE HORIZONTAL WHEN WE ARE STRAFING OR LOCKED ON TO A TARGET

        if (player == null)
            return;

        if(moveAmount != 0)
        {
            player.playerNetworkManager.isMoving.Value = true;
        }
        else
        {
            player.playerNetworkManager.isMoving.Value = false;
        }

        // IF WE ARE NOT LOCKED ON, ONLY USE MOVE AMOUNT
        if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameter(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameter(horizontal_Input, vertical_Input, player.playerNetworkManager.isSprinting.Value);
        }
        // IF WE ARE LOCKED ON PASS THE HORIZONTAL MOVEMENT
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraMovementInput.y;
        cameraHorizontalInput = cameraMovementInput.x;
    }

    // ACTION

    private void HandleDodgeInput()
    {
        if (dodge_Input)
        {
            dodge_Input = false;

            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprintingInput()
    {
        if (sprint_Input)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jump_Input)
        {
            jump_Input = false;

            // IF WE HAVE UI WINDOW, RETURN

            player.playerLocomotionManager.AttemptToPerformJump();
        }
    }

    private void HandleRBInput()
    {
        if (RB_Input)
        {
            RB_Input = false;

            // TODO: IF UI WINDOW OPEN RETURN

            player.playerNetworkManager.SetCharacterActionHand(true);

            // TODO: IF TWO HAND, USE ACTION FOR TWO HANDS

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightWeapon.oh_RB_Action, player.playerInventoryManager.currentRightWeapon);

        }
    }

    private void HandleRTInput()
    {
        if (RT_Input)
        {
            RT_Input = false;

            // TODO: IF UI WINDOW OPEN RETURN

            player.playerNetworkManager.SetCharacterActionHand(true);

            // TODO: IF TWO HAND, USE ACTION FOR TWO HANDS

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightWeapon.oh_RT_Action, player.playerInventoryManager.currentRightWeapon);

        }
    }

    private void HandleHoldRTInput()
    {
        if (player.isPerformingAction)
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
            }
        }
    }

    private void HandleSwitchRightWeaponInput()
    {
        if (switch_Right_Weapon_Input)
        {
            switch_Right_Weapon_Input = false;
            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLeftWeaponInput()
    {
        if (switch_Left_Weapon_Input)
        {
            switch_Left_Weapon_Input = false;
            player.playerEquipmentManager.SwitchLeftWeapon();
        }
    }
}
