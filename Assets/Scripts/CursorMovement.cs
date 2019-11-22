using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorMovement : MonoBehaviour
{

    public float speed;
    public Vector2 moveAxis;
    private float xAxis = 0f; 
    private float yAxis = 0f;

    void Update()
    {
        
        transform.position += new Vector3(xAxis, yAxis, 0) * Time.deltaTime * speed;

        Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x),
            Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y),
            transform.position.z);
    }

    public void OnNavigate(InputValue value){
        moveAxis = value.Get<Vector2>();
        xAxis = moveAxis.x;
        yAxis = moveAxis.y;
    }
}
