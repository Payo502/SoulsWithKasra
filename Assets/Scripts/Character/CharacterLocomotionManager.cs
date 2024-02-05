using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Groung and Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity; // THE FORCE WHICH THE CHARACTER IS PULLED UP OR DOWN
    [SerializeField] protected float groundedYVelocity = -20; // THE FORCE KEEPING THE CHARACTER ON THE GROUND, WHEN ITS GROUNDED
    [SerializeField] protected float fallStartYVelocity = -5; // THE FORCE WHICH THE CHARACTER BEGINS TO FALL
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    [Header("Flags")]
    public bool isRolling = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isGrounded = true;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.characterLocomotionManager.isGrounded)
        {
            // IF NOT JUMPING 
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            // IF WE ARE NOT JUMPING, AND FALLING VELOCITY HASNT BEEN SET
            if (character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);

            yVelocity.y += gravityForce * Time.deltaTime;
        }

        // THERE SHOULD ALWAYS BE SOME FORCE APPLIED TO THE Y VELOCITY
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.characterLocomotionManager.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    // DRAWS OUR GROUND CHECK SPHERE IN SCENE VIEW
    protected void OnDrawGizmosSelected()
    {
        //Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }

    public void EnableCanRotate()
    {
        canRotate = true;
    }

    public void DisableCanRotate()
    {
        canRotate = false;
    }
}
