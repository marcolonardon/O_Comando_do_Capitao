using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamMove
{
    public class CamMovement : MonoBehaviour
    {
        Vector3 cameraPosition;

        [Header("CameraSettings")]
        public float cameraSpeed;

        void Start() 
        {
            cameraPosition = this.transform.position;
        }

        void Update()
        {
            if(Input.GetKey(KeyCode.D))
            {
                cameraPosition.x += cameraSpeed/50;
            }

            if(Input.GetKey(KeyCode.A))
            {
                cameraPosition.x -= cameraSpeed/50;
            }

            this.transform.position = cameraPosition;
        }
    }

}


