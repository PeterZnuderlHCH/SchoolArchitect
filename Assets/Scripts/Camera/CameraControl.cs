using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float cameraspeed;
    public float zoomspeed;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Mouse ScrollWheel");
        transform.position += new Vector3(h, v, 0)*cameraspeed * cam.orthographicSize;
        //float size = cam.orthographicSize + z * zoomspeed;
        cam.orthographicSize -= z * zoomspeed;

    }
}
