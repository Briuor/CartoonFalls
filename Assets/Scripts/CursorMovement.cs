using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorMovement : MonoBehaviour
{

    public float speed;
    public Vector2 MoveAxis;
    // void Awake() {
        // inputAction = new DefaultInputActions()
    // }

    void Update()
    {
        float x = 0, y = 0;
        if(Keyboard.current.dKey.isPressed) {
            x += 1;
        }
        else if(Keyboard.current.aKey.isPressed) {
            x -= 1;
        }
        else if(Keyboard.current.wKey.isPressed) {
            y += 1;
        }
        else if(Keyboard.current.sKey.isPressed) {
            y -= 1;
        }


        transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

        Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x),
            Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y),
            transform.position.z);


    }
}
