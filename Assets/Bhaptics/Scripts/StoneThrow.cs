using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneThrow : MonoBehaviour {
    public GameObject PopEff;
    public GameObject player;
    private SphereCollider col;

	// Use this for initialization
	void Start () {
        col = gameObject.GetComponent<SphereCollider>();
        col.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        // Invoke("EnableCollider", 0.5f);
	}

    private void OnCollisionEnter(Collision collision)
    {
        Invoke("PopEffect", 0.1f);
    }

    private void PopEffect()
    {
        var Pop = Instantiate(PopEff, gameObject.GetComponent<Transform>());
        Pop.transform.LookAt(player.transform);
        Destroy(gameObject);
    }
}
