using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    Vector2 pos;
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float jumpSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        pos = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(pos.x * speed * Time.deltaTime, 0, pos.y * speed * Time.deltaTime);
    }
}
