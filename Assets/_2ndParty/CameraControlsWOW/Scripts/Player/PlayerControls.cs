using UnityEngine;

namespace BaseCameraControlsWOW
{

    // TODO: bug - when starting strafing, character slightly moves forward

    /* Player Controls
    * @obs This script has variables that are changed by "CameraController" script in scene
    */
    public class PlayerControls : MonoBehaviour
    {

        // > Refs
        public Controls controls;

        [Tooltip("Object nested in Player, at feet/ground level, with Z pointing forward and Y up")]
        [SerializeField] Transform groundDirection, fallDirection;
        [Tooltip("Attach CameraRig here or another scene camera")]
        public CameraController mainCam;
        CharacterController controller;

        // > Status/Inputs
        Vector2 inputs;
        bool isRunning = true, isJumping = false, jumpInput = false;
        [HideInInspector] public bool steer, autoRun; // Set by another script

        // > Speed/Acceleration
        public float baseSpeed = 1f, runSpeedMultiplier = 4f, rotateSpeed = 2f;
        public float gravity = -9.8f, terminalVelocity = -25f, jumpSpeed = 10f, jumpHeight = 2f;
        Vector3 velocity, jumpDirection;
        [HideInInspector] public float rotation;
        float currentSpeed = 0f, velocityY = 0f;

        // > Direction
        [HideInInspector] public Vector2 inputNormalized = new Vector2(0, 0);
        Vector3 forwardDirection;
        [SerializeField] float slopeAngle, forwardAngle;
        Ray groundRay;
        RaycastHit groundHit;
        float slopeMult = 1f;

        // > Constants
        private float groundRayMax = 1f;

        void Start()
        {
            if (!this.mainCam) Debug.LogWarning("'mainCam' not set in 'PlayerControls'");
            this.controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            GetInputs();
            GroundDirection();
            Locomotion();
        }

        void Locomotion()
        {
            // > Inputs
            this.inputNormalized = inputs.normalized;
            if (this.jumpInput && this.controller.isGrounded && this.slopeAngle <= controller.slopeLimit) Jump();

            // > Speed Calc
            this.currentSpeed = this.isRunning ? (this.baseSpeed * this.runSpeedMultiplier) : this.baseSpeed;
            if (inputNormalized.y < 0f) this.currentSpeed = this.currentSpeed / 2;
            this.currentSpeed *= this.slopeMult;

            // > Rotation
            Vector3 rotation = transform.eulerAngles + new Vector3(0f, this.rotation * this.rotateSpeed, 0f);
            transform.eulerAngles = rotation;

            // > Movement
            if (!controller.isGrounded && this.velocityY > this.terminalVelocity) this.velocityY += this.gravity * Time.deltaTime;
            else if (controller.isGrounded && this.slopeAngle > controller.slopeLimit) this.velocityY = Mathf.Lerp(this.velocityY, this.terminalVelocity * this.slopeMult, 0.25f);

            if (!this.isJumping) this.velocity = (this.groundDirection.forward * this.inputNormalized.magnitude) * this.currentSpeed + this.fallDirection.up * this.velocityY;
            else this.velocity = this.jumpDirection * this.jumpSpeed + Vector3.up * this.velocityY;

            controller.Move(this.velocity * Time.deltaTime);

            if (controller.isGrounded)
            {
                this.isJumping = false;
                this.velocityY = 0;
            }
        }

        /**
        * Calculate "grounDirection" to climb ramps and "fallDirection" to slide down slopes
        */
        void GroundDirection()
        {
            // > Setting control/movement direction
            this.forwardDirection = transform.position;
            if (this.inputNormalized.magnitude > 0) this.forwardDirection += transform.forward * this.inputNormalized.y + transform.right * this.inputNormalized.x;
            else this.forwardDirection += transform.forward;
            this.groundDirection.LookAt(this.forwardDirection);
            this.fallDirection.rotation = transform.rotation;

            // > Correct direction on down slope
            this.slopeMult = 1f;
            this.groundRay.origin = transform.position + Vector3.up * 0.05f;
            this.groundRay.direction = Vector3.down;
            if (Physics.Raycast(this.groundRay, out groundHit, this.groundRayMax))
            {
                this.slopeAngle = Vector3.Angle(Vector3.up, this.groundHit.normal);
                this.forwardAngle = Vector3.Angle(this.groundDirection.forward, this.groundHit.normal) - 90f;
                // Climbable
                if (this.forwardAngle < 0 && this.slopeAngle <= controller.slopeLimit)
                {
                    this.slopeMult = 1 / Mathf.Cos(this.forwardAngle * Mathf.Deg2Rad);
                    this.groundDirection.eulerAngles += new Vector3(-this.forwardAngle, 0, 0);
                }
                // Too steep
                else if (this.slopeAngle > controller.slopeLimit)
                {
                    this.slopeMult = 1 / Mathf.Cos((90 - this.slopeAngle) * Mathf.Deg2Rad);
                    Vector3 groundCross = Vector3.Cross(this.groundHit.normal, Vector3.up).normalized;
                    Vector3 slopeVector = Vector3.Cross(groundCross, this.groundHit.normal).normalized;
                    this.fallDirection.rotation = Quaternion.LookRotation(this.groundHit.normal, slopeVector);
                }
            }
            // Freefall
            else this.slopeAngle = 90;
        }


        void Jump()
        {
            this.isJumping = true;
            this.jumpDirection = (transform.forward * this.inputs.y + transform.right * this.inputs.x).normalized;
            this.jumpSpeed = this.currentSpeed;

            this.velocityY = Mathf.Sqrt(Mathf.Abs(gravity * this.jumpHeight));
        }

        void GetInputs()
        {
            // Toggle Input
            if (controls.walkRun.IsDownBind()) this.isRunning = !this.isRunning;
            if (controls.autoRun.IsDownBind()) this.autoRun = !this.autoRun;
            this.jumpInput = controls.jump.IsPressBind();

            // Move Input
            this.inputs.y = AxisInputValue(controls.forwards.IsPressBind(), controls.backwards.IsPressBind());
            if (this.inputs.y != 0 && !this.mainCam.autoRunReset) this.autoRun = false;
            if (this.autoRun) this.inputs.y += 1;
            this.inputs.x = AxisInputValue(controls.strageRight.IsPressBind(), controls.strafeLeft.IsPressBind());
            if (this.steer)
            {
                this.inputs.x = AxisInputValue(controls.rotateRight.IsPressBind(), controls.rotateLeft.IsPressBind());
                this.inputs.x = Mathf.Clamp(this.inputs.x, -1, 1);
            }

            // Rotate Input
            if (this.steer) this.rotation = Input.GetAxis("Mouse X") * this.mainCam.cameraSpeed;
            else this.rotation = AxisInputValue(controls.rotateRight.IsPressBind(), controls.rotateLeft.IsPressBind());
        }

        /* Gives -1(neg is pressed), 0(both or none are pressed) or 1 (pos is pressed) */
        float AxisInputValue(bool pos, bool neg)
        {
            float value = 0;
            if (pos) value += 1;
            if (neg) value -= 1;
            return value;
        }
    }
}