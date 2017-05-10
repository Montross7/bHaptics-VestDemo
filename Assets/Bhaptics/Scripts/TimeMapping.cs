using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class TimeMapping : MonoBehaviour
{
    [SerializeField] private Text text;
    private LinearMapping linearMapping;
	void Start ()
	{
	    linearMapping = GetComponent<LinearMapping>();
	}
	
	// Update is called once per frame
	void Update () {
        
	    text.text = "Delay\n" + GetDelayMillis() + "ms";

	}

    public int GetDelayMillis()
    {
        // 0 ~ 200
        return (int) (linearMapping.value * 200) / 20 * 20;
    }
}
