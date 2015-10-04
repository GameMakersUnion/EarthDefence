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

    public void OnClickLeft()
    {
        if (bm == null) { Debug.LogWarning("Button Manager not found!"); return; }
        bm.LeftPressed();
    }
    public void OnClickRight()
    {
        if (bm == null) { Debug.LogWarning("Button Manager not found!"); return; }
        bm.RightPressed();
    }

}
