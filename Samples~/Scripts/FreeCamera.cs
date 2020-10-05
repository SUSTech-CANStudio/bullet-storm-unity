using UnityEngine;

// ReSharper disable Unity.InefficientPropertyAccess

namespace CANStudio.BulletStorm.Samples.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class FreeCamera : MonoBehaviour
    {
        [Range(1, 10)]
        public float cameraSpeed = 1;
        [Range(0, 5)]
        public float mouseSensitive = 2;
        
        private void Update()
        {
            var distance = Time.deltaTime * cameraSpeed;
            var view = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitive;
            var move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * distance;
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            var up = Input.GetKey(KeyCode.LeftShift);
            var down = Input.GetKey(KeyCode.LeftControl);
            
            cameraSpeed = Mathf.Clamp(cameraSpeed + scroll, 1, 10);
            transform.position += transform.forward * move.y + transform.right * move.x;
            transform.eulerAngles += new Vector3(-view.y, view.x);
            if (up && !down) transform.position += Vector3.up * distance;
            if (down && !up) transform.position += Vector3.down * distance;
        }
    }
}