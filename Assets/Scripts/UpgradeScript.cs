using UnityEngine;
using System.Collections;

public class UpgradeScript : MonoBehaviour {

    private RectTransform myt;
    private float initialY;
    bool isOpen;


	// Use this for initialization
	void Start () {
        myt = GetComponent<RectTransform>();
        initialY = myt.anchoredPosition.y;
        isOpen = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isOpen)
        {
            if (myt.anchoredPosition.y < 0)
                myt.anchoredPosition = new Vector2(0.0f, myt.anchoredPosition.y + 1080 * Time.deltaTime);
        }
        else if (myt.anchoredPosition.y > initialY)
            myt.anchoredPosition = new Vector2(0.0f, myt.anchoredPosition.y - 1080 * Time.deltaTime);
        else
        {
            myt.anchoredPosition = new Vector2(0.0f, initialY);
        }
    }

    public void Toggle()
    {
        isOpen = !isOpen;
    }
}
