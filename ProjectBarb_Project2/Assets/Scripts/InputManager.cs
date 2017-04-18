using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    private Character character;
    private char[] latestKeys = { '0', '0', '0', '0' }; // 0

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("xMove") && Input.GetAxisRaw("xMove") > 0) // D
        {
            pushKey('d');
        }
        if (Input.GetButtonDown("xMove") && Input.GetAxisRaw("xMove") < 0) // A, set the latest key
        {
            pushKey('a');
        }
        if (Input.GetButtonDown("zMove") && Input.GetAxisRaw("zMove") > 0) // W
        {
            pushKey('w');
        }
        if (Input.GetButtonDown("zMove") && Input.GetAxisRaw("zMove") < 0) // S
        {
            pushKey('s');
        }

        checkIfLatest(); // Check if latestKey is valid
                            //Debug.Log(latestKey.ToString());
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        float hMove = Input.GetAxis("xMove"); // Invert stuff here
        float vMove = Input.GetAxis("zMove");
        
        character.move(hMove, vMove);
    }

    private void checkIfLatest() // Runs until the latest key is either nothing or is being pressed
    {
        if (latestKeys[0] == 'd' && !(Input.GetButton("xMove") && Input.GetAxisRaw("xMove") > 0)) // D
        {
            pushKey('0');
            checkIfLatest();
        }
        if (latestKeys[0] == 'a' && !(Input.GetButton("xMove") && Input.GetAxisRaw("xMove") < 0)) // A, set the latest key
        {
            pushKey('0');
            checkIfLatest();
        }
        if (latestKeys[0] == 'w' && !(Input.GetButton("zMove") && Input.GetAxisRaw("zMove") > 0)) // W
        {
            pushKey('0');
            checkIfLatest();
        }
        if (latestKeys[0] == 's' && !(Input.GetButton("zMove") && Input.GetAxisRaw("zMove") < 0)) // S
        {
            pushKey('0');
            checkIfLatest();
        }
    }

    private void pushKey(char val) // Pushes a key into the latestKeys array
    {
        latestKeys[3] = latestKeys[2];
        latestKeys[2] = latestKeys[1];
        latestKeys[1] = latestKeys[0];
        latestKeys[0] = val;
    }
}
