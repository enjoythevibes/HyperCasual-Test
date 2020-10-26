using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enjoythevibes.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput = default;

        [SerializeField]
        private Transform targetTransform = default;
        private Rigidbody targetRigidbody;

        [SerializeField]
        private Vector2 xAngleLimit = new Vector3(80f, 10f);
        [SerializeField]
        private Vector2 zAngleLimit = new Vector3(50f, -50f);

        private Vector3 defaultUpVector;
        private Vector3 globalRightVector;
        private Quaternion rotateVectorTo; 

        [SerializeField]
        private float zoom = 19f;
        public Vector3 currentVelocity; // Сделать приватной
        public float smoothTime = 0.15f;

        public Vector3 GravityVector { private set; get; }

        public Vector3 defaultRotation;

        private CameraState currentState;

        public float RotateAngleX { private set; get; }
        public float RotateAngleZ { private set; get; }

        private void Awake() 
        {
            defaultUpVector = transform.up;
            globalRightVector = Functions.Flat(transform.right);
            rotateVectorTo = Quaternion.FromToRotation(defaultUpVector, Vector3.up);
            defaultRotation = transform.rotation.eulerAngles;
            targetRigidbody = targetTransform.GetComponent<Rigidbody>();
        }

        private void Update() 
        {
            var xRotateAngle = 0f;
            var zRotateAngle = 0f;
            if (currentState == CameraState.SetToDefault)
            {
                xRotateAngle = (defaultRotation.x - transform.eulerAngles.x) * Time.deltaTime * 50f;
                zRotateAngle = (defaultRotation.z - Functions.ToSignedAngle(transform.eulerAngles.z)) * Time.deltaTime * 50f;
            }
            else
            if (currentState == CameraState.Control)
            {
                xRotateAngle = Functions.ClampAngle((transform.eulerAngles.x - playerInput.Value.x * Time.deltaTime * 25f), xAngleLimit.y, xAngleLimit.x) - transform.eulerAngles.x;
                zRotateAngle = Functions.ClampAngle((transform.eulerAngles.z + playerInput.Value.z * Time.deltaTime * 25f), zAngleLimit.y, zAngleLimit.x) - transform.eulerAngles.z;
            }
            RotateAngleX = xRotateAngle;
            RotateAngleZ = zRotateAngle;

            transform.RotateAround(targetTransform.position, globalRightVector, xRotateAngle);
            transform.RotateAround(targetTransform.position, transform.forward, zRotateAngle);

            var dotProductUpVector = (/* 1f -  */Vector3.Dot(-transform.up, -defaultUpVector));
            var gravityVector = -transform.up * dotProductUpVector;
            gravityVector = rotateVectorTo * gravityVector;
            GravityVector = Vector3.ClampMagnitude(gravityVector * 100f, 1f);
        }
        public bool lockFollow;
        private void LateUpdate()
        {
            if (lockFollow) return;
            transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position - transform.forward * zoom, ref currentVelocity, smoothTime);
        }

        public void SetToControlState()
        {
            currentState = CameraState.Control;
        }

        public void SetRotateToDefaultState()
        {
            currentState = CameraState.SetToDefault;
        }

        public void SetLockState()
        {
            currentState = CameraState.Lock;
        }
    }
}