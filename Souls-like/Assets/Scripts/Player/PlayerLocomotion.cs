using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerLocomotion : MonoBehaviour
    {
        public bool isRolling;
        public float rollTimer;
        CameraHandler cameraHandler;
        PlayerManager playerManager;
        PlayerStats playerStats;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float walkingSpeed = 1;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 45;

        [Header("Stamina Costs")]
        [SerializeField]
        int rollStaminaCost = 23;
        int backstepStaminaCost = 23;
        int sprintStaminaCost = 1;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        void Start()
        {
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation(float delta)
        {
            if (!playerManager.isInDialogue)
            {
                if (animatorHandler.canRotate)
                {
                    if (inputHandler.lockOnFlag)
                    {
                        if (inputHandler.sprintFlag || inputHandler.rollFlag)
                        {
                            Vector3 targetDirection = Vector3.zero;
                            targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                            targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                            targetDirection.Normalize();
                            targetDirection.y = 0;

                            if (targetDirection == Vector3.zero)
                            {
                                targetDirection = transform.forward;
                            }

                            Quaternion tr = Quaternion.LookRotation(targetDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                            transform.rotation = targetRotation;
                        }
                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();
                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }
                    }
                    else
                    {
                        Vector3 targetDir = Vector3.zero;
                        float moveOverride = inputHandler.moveAmount;

                        targetDir = cameraObject.forward * inputHandler.vertical;
                        targetDir += cameraObject.right * inputHandler.horizontal;

                        targetDir.Normalize();
                        targetDir.y = 0;

                        if (targetDir == Vector3.zero)
                            targetDir = myTransform.forward;

                        float rs = rotationSpeed;

                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                        myTransform.rotation = targetRotation;
                    }
                }

            }
        }

        public void HandleMovement(float delta)
        {
            if (!playerManager.isInDialogue)
            {
                movementSpeed = 3;
                walkingSpeed = 4;
                sprintSpeed = 7;
            }
            else
            {
                movementSpeed = 0;
                walkingSpeed = 0;
                sprintSpeed = 0;
            }

            if (inputHandler.rollFlag)
                return;

            if (playerManager.isInteracting)
                return;

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
               // playerStats.TakeStaminaDamage(sprintStaminaCost);
            }
            else
            {
                if(inputHandler.moveAmount < 0.5)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (!playerManager.isInDialogue)
            {
                if (animatorHandler.anim.GetBool("isInteracting"))
                    return;

                if (playerStats.currentStamina <= 0)
                    return;

                if (inputHandler.rollFlag)
                {
                    moveDirection += cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;


                    if (inputHandler.moveAmount > 0)
                    {
                        animatorHandler.PlayTargetAnimation("Rolling", true);
                        moveDirection.y = 0;
                        Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = rollRotation;
                        playerStats.TakeStaminaDamage(rollStaminaCost);
                        isRolling = true;
                        rollTimer = 0.3f;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Backstep", true);
                        playerStats.TakeStaminaDamage(backstepStaminaCost);
                    }
                }

                if (rollTimer > 0)
                {
                    rollTimer -= Time.deltaTime;
                }
                else
                {
                    rollTimer = 0;
                    isRolling = false;
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin, myTransform.forward, out hit, 4.5f))
            {
                moveDirection = Vector3.zero;
            }

            if(playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed / 1.4f);
                rigidbody.AddForce(moveDirection * fallingSpeed / 9f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if(playerManager.isInAir)
                {
                    if(inAirTimer > 0.1f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("landing", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if(playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if(playerManager.isInAir == false)
                {
                    if(playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if(playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }

            if(playerManager.isGrounded)
            {
                if(playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
                return;

            if (playerStats.currentStamina <= 0)
                return;

            if(inputHandler.jump_input)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Jump", true);
                    moveDirection.y = 0;
                    playerStats.TakeStaminaDamage(rollStaminaCost);
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                }
            }
        }

        #endregion
    }
}