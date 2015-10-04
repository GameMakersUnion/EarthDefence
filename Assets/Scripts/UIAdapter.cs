using UnityEngine;
using System.Collections;

/**
 *  This class exists as a layer of abstraction between the GameManager class and the
 *  Canvas' UI's Button's Script's component OnClick() 
 *  This way, only the ButtonManager methods are listed in the Unity Editor, which now requires an 
 *  explicit decision whether or not to include an adapter to them.
 */
public class UIAdapter : MonoBehaviour {

    ButtonManager bm; 

    void Start()
    {
        GameObject gm = GameObject.Find("GameManager");
        if (gm == null) { Debug.LogWarning("Game Manager not found! Cannot issue UI commands!"); return; }

        bm = gm.GetComponent<ButtonManager>();
        if (bm == null) { Debug.LogWarning("Button Manager script not found! Cannot issue UI commands!"); return; }
    }


    bool leftDown;
    bool rightDown;

    public void OnClickLeft(int status)
    {
        switch (status)
        {//0=Off 1=On
            case 0: leftDown = false; break;
            case 1: leftDown = true; break;
        }
    }
    public void OnClickRight(int status)
    {
        switch (status)
        {
            case 0: rightDown = false; break;
            case 1: rightDown = true; break;
        }
    }


    public void Update()
    {
        if (leftDown)
        {
            if (bm == null) { Debug.LogWarning("Button Manager not found!"); return; }
            bm.LeftPressed();
        }
        if (rightDown)
        {
            if (bm == null) { Debug.LogWarning("Button Manager not found!"); return; }
            bm.RightPressed();
        }
    }


    /*
    public void OnClickLeft()
    {
        if (bm == null) { Debug.LogWarning("Button Manager not found!"); return; }
        bm.LeftPressed();
    }
    public void OnClickRight()
    {
        if (bm == null) { Debug.LogWarning("Button Manager not found!"); return; }
        bm.RightPressed();
    }*/

}
