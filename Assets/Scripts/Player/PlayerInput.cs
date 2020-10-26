using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enjoythevibes.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector2 lastTouchPosition;
        private float xTouchVelocity;
        private float yTouchVelocity;
        private float smoothTime = 0.01f;
        [SerializeField]
        private float sensitivity = 2f;

        public Vector3 Value { private set; get; }

        private void Update()
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                var currentTouchPosition = Input.GetTouch(0).position;
                lastTouchPosition = currentTouchPosition;
            }
            else
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var currentTouchPosition = Input.GetTouch(0).position;
                var deltaPosition = (currentTouchPosition - lastTouchPosition);

                var xAxis = Mathf.SmoothDamp(Value.x, deltaPosition.x * 0.075f * sensitivity, ref xTouchVelocity, smoothTime);
                var yAxis = Mathf.SmoothDamp(Value.z, deltaPosition.y * 0.075f * sensitivity, ref yTouchVelocity, smoothTime);
                Value = new Vector3(yAxis, 0f, xAxis);
                lastTouchPosition = currentTouchPosition;
            }
            else
            if (Input.touchCount == 0)
            {
                Value = default(Vector3);
            }
        }
    }
}