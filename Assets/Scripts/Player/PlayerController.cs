using System;
using System.Collections;
using System.Collections.Generic;
using enjoythevibes.Levels;
using UnityEngine;

namespace enjoythevibes.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Level defaultLevel = default;
        private Level currentLevel;
        private PlayerInput playerInput;

        [SerializeField]
        private PlayerCameraController playerCameraController = default;

        private Rigidbody rb;

        private bool isGround;
        private float flyTime;
        private LayerMask rampLayer;

        private bool setCameraToDefault;
        private PlayerState currentState;

        private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        private void Awake() 
        {
            playerInput = GetComponent<PlayerInput>();
            rb = GetComponent<Rigidbody>();
            rampLayer = LayerMask.GetMask("Ramp");
            currentLevel = defaultLevel;
        }

        private void Start() 
        {
            currentLevel.Enable();    
        }

        private void FixedUpdate() 
        {   
            if (currentState != PlayerState.Control) return;
            if (Physics.Raycast(transform.position, Vector3.down, (transform.localScale.x / 2f) + 0.1f, rampLayer))
            {
                isGround = true;
                flyTime = 0f;
            }
            else
            {
                isGround = false;
                flyTime += Time.fixedDeltaTime;
            }

            if (flyTime > 1f)
            {
                Respawn();
                flyTime = 0f;
                return;
            }
            rb.AddForce(playerCameraController.GravityVector * 100f, ForceMode.Acceleration);
            if (!isGround)
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 50f);
        }

        // public IEnumerator DoTranslateToNextLevel(Level nextLevel)
        // {
        //     var timer = 0f;
        //     // var velocity = Vector3.Project(rb.velocity, Vector3.down);
        //     var velocity = Vector3.down * 50f;
        //     var setCameraToDefault = false;
        //     var levelEnabled = false;
        //     rb.interpolation = RigidbodyInterpolation.None;
        //     rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        //     rb.isKinematic = true;
        //     var smoothCameraTime = playerCameraController.smoothTime;
        //     while (true)
        //     {
        //         // if (smoothCameraTime > 0.0001f)
        //         // {
        //         //     smoothCameraTime -= Time.deltaTime * 0.3f;
        //         //     if (smoothCameraTime <= 0f) smoothCameraTime = 0.0001f;
        //         // } 
        //         // playerCameraController.smoothTime = smoothCameraTime;
        //         var nextPosition = transform.position + (velocity * Time.deltaTime);
        //         if (Physics.Raycast(transform.position, Vector3.down, (transform.localScale.x / 2f) + 0.1f + (velocity * Time.deltaTime).magnitude, rampLayer))
        //         {
        //             rb.isKinematic = false;
        //             rb.interpolation = RigidbodyInterpolation.Interpolate;
        //             rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //             rb.velocity = velocity;
        //             rb.angularVelocity = Vector3.zero;
        //             currentState = PlayerState.Control;
        //             playerCameraController.smoothTime = 0.15f;
        //             yield return new WaitForFixedUpdate();
        //             playerCameraController.SetToControlState();
        //             break;
        //         }
        //         timer += Time.deltaTime;
        //         transform.position = nextPosition;
        //         if (timer > 0.3f)
        //         {
        //             if (!levelEnabled && setCameraToDefault)
        //             {
        //                 if (Mathf.Abs(playerCameraController.RotateAngleX) < 0.1f && Mathf.Abs(playerCameraController.RotateAngleZ) < 0.1f)
        //                 {
        //                     currentLevel.Disable();
        //                     nextLevel.Enable();
        //                     currentLevel = nextLevel;
        //                     levelEnabled = true;
        //                 }              
        //             }
        //             if (!setCameraToDefault)
        //             {
        //                 playerCameraController.SetRotateToDefaultState();
        //                 setCameraToDefault = true;
        //             }
        //         }
        //         yield return null;
        //     }
        //     yield break;
        // }

        public void TranslateToNextLevel(Level nextLevel, Vector3 centralPosition) // EndSubLevel Translate ToNext
        {
            currentState = PlayerState.TranslateToNext;
            playerCameraController.SetLockState();
            transform.position = centralPosition;
            StartCoroutine(TranslateToNextLevelWork(nextLevel));
        }

        // private IEnumerator WaitAndShift(Vector3 spawnPosition, float seconds = 0.4f)
        // {
        //     yield return null;
        //     yield return new WaitForSeconds(seconds);
        //     var shift = spawnPosition - transform.position;
        //     transform.position = spawnPosition;
        //     playerCameraController.transform.position += shift;
        //     yield break;
        // }

        // private IEnumerator FallDisableLevel()
        // {
        //     yield return new WaitForSeconds(0.3f);
        //     currentLevel.Disable();
        // }

        
        private IEnumerator TranslateToNextLevelWork(Level nextLevel)
        {
            var cameraSmoothTime = playerCameraController.smoothTime;
            while (cameraSmoothTime > 0.001f)
            {
                cameraSmoothTime -= Time.deltaTime * 0.3f;
                if (cameraSmoothTime < 0.001f) cameraSmoothTime = 0.001f;
                playerCameraController.smoothTime = cameraSmoothTime;
                yield return null;
            }
            // yield return new WaitForSeconds(0.3f);
            // Debug.Break();
            yield return new WaitForEndOfFrame();
            var velocity = rb.velocity;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;
            playerCameraController.SetRotateToDefaultState();    
            yield return null;   

            // yield return waitForFixedUpdate;     
            // while (Vector3.Dot(Vector3.down, rb.velocity.normalized) < 0.999f)
            // while (Vector3.Dot(Vector3.down, velocity.normalized) < 0.999f)
            while (Mathf.Abs(playerCameraController.RotateAngleX) > 0.001f || Mathf.Abs(playerCameraController.RotateAngleZ) > 0.001f)
            {
                velocity += playerCameraController.GravityVector * 100f;
                velocity = Vector3.ClampMagnitude(velocity, 50f);
                transform.position += velocity * Time.deltaTime;
                // rb.AddForce(playerCameraController.GravityVector * 100f, ForceMode.Acceleration);
                // rb.velocity = Vector3.ClampMagnitude(rb.velocity, 50f);
                yield return null;
                // yield return waitForFixedUpdate;
            }
            velocity = Vector3.down * 50f;
            // rb.interpolation = RigidbodyInterpolation.None;
            // rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            // rb.isKinematic = true;
            // playerCameraController.SetRotateToDefaultState();    
            // Debug.Break();
            currentLevel.Disable();
            nextLevel.Enable();
            currentLevel = nextLevel;
            // yield return waitForFixedUpdate;
            var shift = nextLevel.SpawnPosition - transform.position;
            transform.position += shift;
            playerCameraController.transform.position += shift;
            while (true)
            {
                var nextPosition = transform.position + (velocity * Time.deltaTime);
                if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, (transform.localScale.x / 2f) + 0.1f + (velocity * Time.deltaTime).magnitude, rampLayer))
                {
                    transform.position = transform.position + Vector3.down * (hitInfo.distance - (transform.localScale.x / 2f) + 0.1f);
                    rb.isKinematic = false;
                    rb.interpolation = RigidbodyInterpolation.Interpolate;
                    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    rb.velocity = velocity;
                    rb.angularVelocity = Vector3.zero;
                    currentState = PlayerState.Control;
                    playerCameraController.SetToControlState();
                    yield return waitForFixedUpdate;
                    playerCameraController.smoothTime = 0.15f;
                    break;
                }
                transform.position = nextPosition;
                yield return null;
            }
            yield break;
        }

        public void Respawn()
        {
            currentState = PlayerState.TranslateToNext;
            playerCameraController.SetLockState();
            StartCoroutine(TranslateToNextLevelWork(defaultLevel));
            // playerCameraController.SetRotateToDefaultState();
            // playerCameraController.transform.rotation = Quaternion.Euler(playerCameraController.defaultRotation);
            // StartCoroutine(DoTranslateToNextLevel(defaultLevel));
            // StartCoroutine(WaitAndShift(defaultLevel.SpawnPosition));
            // StartCoroutine(FallDisableLevel());
            // StartCoroutine(FallWait());
        }

        // public void TranslateToNextLevel(Level nextLevel, Vector3 centralPosition)
        // {
        //     // currentState = PlayerState.TranslateToNext;
        //     // playerCameraController.SetLockState();
        //     // transform.position = centralPosition;
        //     // StartCoroutine(DoTranslateToNextLevel(nextLevel));
        //     // StartCoroutine(WaitAndShift(nextLevel.SpawnPosition));
        // }
    }
}