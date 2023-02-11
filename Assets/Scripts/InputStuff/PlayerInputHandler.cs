using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Playermove move;
    private Vector2 direction;

    void Update(){
        // move.direction = direction;
        //move.moveCharacter();
    }

    void Awake(){
        move = GetComponent<Playermove>();
    }
    public void OnMove(InputAction.CallbackContext ctx) => move.direction = ctx.ReadValue<Vector2>();

    public void OnJump(){
        move.StartJump();
    }

    public void OnDash(){
        StartCoroutine(move.Dash());
    }
}
