using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Wertet Eingaben aus, die die Spielfigur steuern sollen
und leitet sie an das zu steuernde Figurenscript weiter.
*/
public class PlayerInputController : MonoBehaviour
{
    public Player player;
    private void Update()
    {
        if (Time.timeScale < 1f) { // Controller in der Pause deaktivieren
            return;
        }
        if (Input.GetAxisRaw("Horizontal") > 0f)
            player.change.x = 1;

        else if (Input.GetAxisRaw("Horizontal") < 0f)
            player.change.x = -1;

        else if (Input.GetAxisRaw("Vertical") > 0f)
            player.change.y = 1;

        else if (Input.GetAxisRaw("Vertical") < 0f)
            player.change.y = -1;
        /*
        else if (Input.GetAxisRaw("Fire1") > 0f)
            player.performAction();
        */     
    }
}
