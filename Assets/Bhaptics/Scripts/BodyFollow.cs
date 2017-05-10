using UnityEngine;
using Valve.VR.InteractionSystem;

public class BodyFollow : MonoBehaviour {

    private Player player;
    [SerializeField] private GameObject bodyObject;

    private Vector3 startRotation;
	// Use this for initialization
	void Start () {
        player = Player.instance;
        startRotation = player.hmdTransform.rotation.eulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
        var direction = player.bodyDirectionGuess;
        var pos =  (player.hmdTransform.position) + new Vector3(0, -0.4f, 0);
        var height = (player.hmdTransform.position.y - 0.15f);
        var roation = new Vector3(0, player.hmdTransform.eulerAngles.y, 0);
        transform.position = pos;
        transform.rotation = Quaternion.Euler(roation);
	}
}
