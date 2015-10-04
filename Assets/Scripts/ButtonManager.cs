using UnityEngine;
using System.Collections;

/**
 *  This is attached directly to GameManager, issuing BroadcastMessages, thus requiring any 
 *  receiving GameObject scripts to be a child of this GameManager. For example, 
 *  BroadcastMessage("Move", -1) will be received on the Moon class Move(float), because Moon
 *  is a child (eventually) of GameManager.
 */
public class ButtonManager : MonoBehaviour {

    public void LeftPressed()
    {
        BroadcastMessage("Move", -1);
    }

    public void RightPressed()
    {
        BroadcastMessage("Move", 1);
    }
}
