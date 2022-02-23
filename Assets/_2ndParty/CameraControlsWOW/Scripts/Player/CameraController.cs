using UnityEngine;

namespace BaseCameraControlsWOW
{

    /* Camera Controller
    * @obs This script finds "PlayerControls" script in scene and changes it state actively
    */
    public class CameraController : MonoBehaviour
    {
        // > Input
        KeyCode leftMouse = KeyCode.Mouse0, rightMouse = KeyCode.Mouse1, middleMouse = KeyCode.Mouse2;

        // > Cam
        public float cameraHeight = 1.75f, cameraMaxDistance = 25f, cameraZoomSpeed = 5f;
        float cameraMaxTilt = 90;
        [Range(0, 4)] public float cameraSpeed = 2f;
        float currentPan, currentTilt = 10f, currentDistance = 5f;
        [HideInInspector]
        public bool autoRunReset;

        // > Smoothing
        float panAngle, panOffset;
        float rotationXCushion = 3f, rotationXSpeed = 0.1f;
        float yRotMin = 0f, yRotMax = 20f, rotationYSpeed = 0.1f;
        bool camXAdjust, camYAdjust;

        // > Collision
        public bool collisionDebug;
        public float collisionCushion = 0.35f, clipCushion = 1.75f;
        public int rayGridX = 9, rayGridY = 5;
        float adjustedDistance;
        public LayerMask collisionMask;
        Ray camRay;
        RaycastHit camRayHit;
        Vector3[] camClip, clipDirection, playerClip, rayColOrigin = new Vector3[0], rayColPoint;
        bool[] rayColHit;

        // > States
        [Range(0.1f, 2.0f)]
        public float cameraAdjustSpeed = 1f;
        /* Camera main states
        * @param CameraNone Camera is locked with player movement and rotation
        * @param CameraRotate Camera rotates freely around player
        * @param CameraSteer Camera rotates and changes player rotation to be aligned with it. Player strafes instead of rotating.
        * @param CameraRun Same as CameraSteer but player is auto-running
        */
        public enum CameraState { CameraNone, CameraRotate, CameraSteer, CameraRun }
        CameraState cameraState = CameraState.CameraNone;
        /* Camera behaviours on the absence of active steering controls
        * @param OnlyWhileMoving Resets rotation to align with player when player runs
        * @param OnlyHorizontalWhileMoving Resets only y-rotation to align with player when player runs
        * @param AlwaysAdjust Always resets rotation to align with player
        * @param NeverAdjust Always maintains angle offset
        */
        public enum CameraMoveState { OnlyWhileMoving, OnlyHorizontalWhileMoving, AlwaysAdjust, NeverAdjust }
        public CameraMoveState cameraMoveState = CameraMoveState.OnlyWhileMoving;

        // > Refs
        [SerializeField] private PlayerControls player = null;
        public Transform tilt = null;
        Camera mainCam = null;

        // * Lifecycles

        void Awake()
        {
            transform.SetParent(null);
        }

        void Start()
        {
            if (!this.player)
            {
                Debug.LogWarning("'player' not set in 'CameraController', thus 'PlayerControls' fetched in scene.");
                this.player = FindObjectOfType<PlayerControls>();
            }
            if (!mainCam)
            {
                Debug.LogWarning("'mainCam' not set in 'CameraController', thus using 'Camera.main'.");
                this.mainCam = Camera.main;
            }

            transform.position = this.player.transform.position + Vector3.up * this.cameraHeight;
            transform.rotation = this.player.transform.rotation;
            this.tilt.eulerAngles = new Vector3(this.currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);

            this.mainCam.transform.position += this.tilt.forward * -this.currentDistance;
        }

        void Update()
        {
            // No button pressed
            if (!Input.GetKey(leftMouse) && !Input.GetKey(rightMouse) && !Input.GetKey(middleMouse)) this.cameraState = CameraState.CameraNone;
            // Left button pressed
            else if (Input.GetKey(leftMouse) && !Input.GetKey(rightMouse) && !Input.GetKey(middleMouse)) this.cameraState = CameraState.CameraRotate;
            // Right button pressed
            else if (!Input.GetKey(leftMouse) && Input.GetKey(rightMouse) && !Input.GetKey(middleMouse)) this.cameraState = CameraState.CameraSteer;
            // Left and Right both pressed
            else if (Input.GetKey(leftMouse) && Input.GetKey(rightMouse)) this.cameraState = CameraState.CameraRun;

            if (this.rayGridX * this.rayGridY != this.rayColOrigin.Length) CameraClipInfo();
            CameraCollision();
            CameraInputs();
        }

        void LateUpdate()
        {
            this.panAngle = Vector3.SignedAngle(transform.forward, this.player.transform.forward, Vector3.up);

            switch (this.cameraMoveState)
            {
                case CameraMoveState.OnlyWhileMoving:
                    if (this.player.inputNormalized.magnitude > 0 || this.player.rotation != 0)
                    {
                        CameraHorizontalAdjust();
                        CameraVerticalAdjust();
                    }
                    break;
                case CameraMoveState.OnlyHorizontalWhileMoving:
                    if (this.player.inputNormalized.magnitude > 0 || this.player.rotation != 0) CameraHorizontalAdjust();
                    break;
                case CameraMoveState.AlwaysAdjust:
                    CameraHorizontalAdjust();
                    CameraVerticalAdjust();
                    break;
                case CameraMoveState.NeverAdjust:
                    CameraNeverAdjust();
                    break;
            }

            CameraTransforms();
        }

        // * Methods

        void CameraClipInfo()
        {
            this.camClip = new Vector3[4];

            this.mainCam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), this.mainCam.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, this.camClip);

            this.clipDirection = new Vector3[4];
            this.playerClip = new Vector3[4];

            int rays = this.rayGridX * this.rayGridY;
            this.rayColOrigin = new Vector3[rays];
            this.rayColPoint = new Vector3[rays];
            this.rayColHit = new bool[rays];
        }

        /** */
        void CameraCollision()
        {
            float camDistance = this.currentDistance + this.collisionCushion;

            // > Get Clipping points
            for (int i = 0; i < this.camClip.Length; i++)
            {
                Vector3 clipPoint = this.mainCam.transform.up * this.camClip[i].y + this.mainCam.transform.right * this.camClip[i].x;
                clipPoint *= this.clipCushion;
                clipPoint += this.mainCam.transform.forward * this.camClip[i].z;
                clipPoint += transform.position - (this.tilt.forward * this.cameraMaxDistance);

                Vector3 playerPoint = this.mainCam.transform.up * this.camClip[i].y + this.mainCam.transform.right * this.camClip[i].x;
                playerPoint += transform.position;

                this.clipDirection[i] = (clipPoint - playerPoint).normalized;
                this.playerClip[i] = playerPoint;
            }

            // > Build Multiple Rays
            int currentRay = 0;
            bool isColliding = false;
            float rayX = this.rayGridX - 1;
            float rayY = this.rayGridY - 1;

            for (int x = 0; x < this.rayGridX; x++)
            {
                Vector3 CU_Point = Vector3.Lerp(this.clipDirection[1], this.clipDirection[2], x / rayX);
                Vector3 CL_Point = Vector3.Lerp(this.clipDirection[0], this.clipDirection[3], x / rayX);

                Vector3 PU_Point = Vector3.Lerp(this.playerClip[1], this.playerClip[2], x / rayX);
                Vector3 PL_Point = Vector3.Lerp(this.playerClip[0], this.playerClip[3], x / rayX);

                for (int y = 0; y < this.rayGridY; y++)
                {
                    this.camRay.origin = Vector3.Lerp(PU_Point, PL_Point, y / rayY);
                    this.camRay.direction = Vector3.Lerp(CU_Point, CL_Point, y / rayY);
                    this.rayColOrigin[currentRay] = this.camRay.origin;

                    if (Physics.Raycast(this.camRay, out this.camRayHit, camDistance, this.collisionMask))
                    {
                        isColliding = true;
                        this.rayColHit[currentRay] = true;
                        this.rayColPoint[currentRay] = this.camRayHit.point;

                        Debug.DrawLine(this.camRay.origin, this.camRayHit.point, Color.cyan);
                        Debug.DrawLine(this.camRayHit.point, this.camRay.origin + this.camRay.direction * camDistance, Color.red);
                    }
                    else
                    {
                        Debug.DrawLine(this.camRay.origin, this.camRay.origin + this.camRay.direction * camDistance, Color.cyan);
                    }

                    currentRay++;
                }
            }

            // > Collision
            if (isColliding)
            {
                currentRay = 0;
                float minRayDistance = float.MaxValue;

                for (int i = 0; i < this.rayColHit.Length; i++)
                {
                    if (this.rayColHit[i])
                    {
                        float colDistance = Vector3.Distance(this.rayColOrigin[i], this.rayColPoint[i]);
                        if (colDistance < minRayDistance)
                        {
                            minRayDistance = colDistance;
                            currentRay = i;
                        }
                    }
                }

                Vector3 clipCenter = transform.position - (this.tilt.forward * this.currentDistance);
                this.adjustedDistance = Vector3.Dot(-this.mainCam.transform.forward, clipCenter - this.rayColPoint[currentRay]);
                this.adjustedDistance = this.currentDistance - (this.adjustedDistance + this.collisionCushion);
                this.adjustedDistance = Mathf.Clamp(this.adjustedDistance, 0, this.cameraMaxDistance);
            }
            else
            {
                this.adjustedDistance = this.currentDistance;
            }
        }

        void CameraInputs()
        {
            if (this.cameraState != CameraState.CameraNone)
            {

                if (!this.camYAdjust && (this.cameraMoveState == CameraMoveState.AlwaysAdjust || this.cameraMoveState == CameraMoveState.OnlyWhileMoving))
                {
                    this.camYAdjust = true;
                }
                if (this.cameraState == CameraState.CameraRotate)
                {
                    if (this.cameraMoveState != CameraMoveState.NeverAdjust) this.camXAdjust = true;
                    this.player.steer = false;
                    this.currentPan += Input.GetAxis("Mouse X") * this.cameraSpeed;
                }
                else if (this.cameraState == CameraState.CameraSteer || this.cameraState == CameraState.CameraRun)
                {
                    if (!this.player.steer)
                    {
                        this.player.transform.eulerAngles = new Vector3(this.player.transform.eulerAngles.x, transform.eulerAngles.y, this.player.transform.eulerAngles.z);
                        this.player.steer = true;
                    }
                }

                this.currentTilt -= Input.GetAxis("Mouse Y") * this.cameraSpeed;
                this.currentTilt = Mathf.Clamp(this.currentTilt, -this.cameraMaxTilt, this.cameraMaxTilt);
            }
            else
            {
                this.player.steer = false;
            }

            this.currentDistance -= Input.GetAxis("Mouse ScrollWheel") * this.cameraZoomSpeed;
            this.currentDistance = Mathf.Clamp(this.currentDistance, 0, this.cameraMaxDistance);
        }

        void CameraHorizontalAdjust()
        {
            if (this.cameraState != CameraState.CameraRotate)
            {
                if (this.camXAdjust)
                {
                    this.rotationXSpeed += Time.deltaTime * this.cameraAdjustSpeed;
                    if (Mathf.Abs(this.panAngle) > this.rotationXCushion) this.currentPan = Mathf.Lerp(this.currentPan, this.currentPan + this.panAngle, this.rotationXSpeed);
                    else this.camXAdjust = false;
                }
                else
                {
                    this.rotationXSpeed = 0;
                    this.currentPan = this.player.transform.eulerAngles.y;
                }
            }
        }

        void CameraVerticalAdjust()
        {
            if (this.cameraState == CameraState.CameraNone)
            {
                if (this.camYAdjust)
                {
                    this.rotationYSpeed += (Time.deltaTime / 2) * this.cameraAdjustSpeed;
                    if (this.currentTilt >= this.yRotMax || this.currentTilt <= this.yRotMin) this.currentTilt = Mathf.Lerp(this.currentTilt, this.yRotMax / 2, this.rotationYSpeed);
                    else this.camYAdjust = false;
                }
                else this.rotationYSpeed = 0;
            }
        }

        void CameraNeverAdjust()
        {
            switch (this.cameraState)
            {
                case CameraState.CameraRun:
                case CameraState.CameraSteer:
                    this.panOffset = 0;
                    this.currentPan = this.player.transform.eulerAngles.y;
                    break;
                case CameraState.CameraNone:
                    this.currentPan = this.player.transform.eulerAngles.y - this.panOffset;
                    break;
                case CameraState.CameraRotate:
                    this.panOffset = this.panAngle;
                    break;
            }
        }

        void CameraTransforms()
        {
            // switch(this.cameraState) {
            //     case CameraState.CameraNone:
            //     case CameraState.CameraRun:
            //     case CameraState.CameraSteer:
            //         this.currentPan = this.player.transform.eulerAngles.y;
            //         break;
            // }

            // if(this.cameraState == CameraState.CameraNone) this.currentTilt = 10f;

            if (this.cameraState == CameraState.CameraRun)
            {
                this.player.autoRun = true;
                this.autoRunReset = true;
            }
            else if (this.autoRunReset)
            {
                this.player.autoRun = false;
                this.autoRunReset = false;
            }

            transform.position = this.player.transform.position + Vector3.up * this.cameraHeight;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, this.currentPan, transform.eulerAngles.z);
            this.tilt.eulerAngles = new Vector3(this.currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);

            this.mainCam.transform.position = transform.position + this.tilt.forward * -this.adjustedDistance;
        }
    }
}