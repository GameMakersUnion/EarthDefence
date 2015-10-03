using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ToggleUI : MonoBehaviour {

    public bool showCanvas = true;
    public bool showBackground = true;
    public GameObject canvas;
    public GameObject background;

    [ExecuteInEditMode]
    void Start()
    {
        Toggle();
    }

    [ExecuteInEditMode]
    void Update()
    {
        Toggle();
    }

    void Toggle()
    {
        //canvas
        if (canvas == null) { Debug.LogWarning("Canvas not attached, cannot toggle it."); return; }
        if (showCanvas == true)
        {
            print("Enabling Canvas");
            canvas.SetActive(true);
        }
        else if (Application.isPlaying && showCanvas)
        {
            print("Enabling Canvas");
            canvas.SetActive(true);
        }
        else
        {
            print("Hiding Canvas");
            canvas.SetActive(false);
        }

        //background
        if (canvas == null) { Debug.LogWarning("Background not attached, cannot toggle it."); return; }
        if (showBackground == true)
        {
            print("Enabling Background");
            background.SetActive(true);
        }
        else if (Application.isPlaying && showBackground)
        {
            print("Enabling Background");
            background.SetActive(true);
        }
        else
        {
            print("Hiding Background");
            background.SetActive(false);
        }
    }
}
