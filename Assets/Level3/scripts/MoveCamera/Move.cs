using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AEA
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f; // Player movement speed
        [SerializeField] private Transform _player;  // Reference to the player object
        [SerializeField] private Vector3 _cameraOffset = new Vector3(0, 0, -10); // Camera offset from the player
        [SerializeField] private float _smoothSpeed = 0.125f; // Smoothing factor for camera movement

        // Update is called once per frame
        void Update()
        {
            MovePlayer();
            FollowPlayer();
        }

        // Handle player movement
        private void MovePlayer()
        {
            float moveX = 0;

            if (Input.GetKey(KeyCode.A) && transform.position.x > 0f)
            {
                moveX = -_speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D) && transform.position.x < 36f)
            {
                moveX = _speed * Time.deltaTime;
            }

            transform.Translate(new Vector2(moveX, 0));
        }

        // Camera follows the player with smooth movement
        private void FollowPlayer()
        {
            // Calculate desired camera position (player's position with offset)
            Vector3 desiredPosition = _player.position + _cameraOffset;

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }
}
