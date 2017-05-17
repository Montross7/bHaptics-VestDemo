using UnityEngine;
using Valve.VR.InteractionSystem;

public class BodyFollow : MonoBehaviour {

    // Settings for VIVE Tracker
    
    // Distances between the vest and the tracker
    const float dX = 0.0f;
    const float dY = 0.0f;
    const float dZ = 0.0f;

    // Rotations of the vest to the tracker
    const float roll = 0f;
    const float yaw = 180f;
    const float pitch = 90f;

    [SerializeField] private GameObject bodyObject;
    private Transform HMD;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame -> Tracking the tracker
	void Update () {
        // Collect delta rotation and displacement between Tracker and Tacvest
        Vector3 delta_displacement = new Vector3(dX, dY, dZ);
        Quaternion delta_rotation = Quaternion.Euler(roll, yaw, pitch);

        // Get current Tracker pose
        Vector3 tracker_position = SteamVR_Controller.Input(5).transform.pos;
        Quaternion tracker_rotation = SteamVR_Controller.Input(5).transform.rot;

        // Transform current Tracker pose to Tacvest pose
        transform.rotation = tracker_rotation * delta_rotation;
        transform.position = tracker_position + (tracker_rotation * delta_rotation) * delta_displacement; 
        /*
        transform.localPosition = Head.localPosition - (Head.position - (tracker_position + (tracker_rotation * delta_rotation) * delta_displacement));
        */
        /* Original version things
        var direction = player.bodyDirectionGuess;
        var pos =  (player.hmdTransform.position) + new Vector3(0, -0.4f, 0);
        var height = (player.hmdTransform.position.y - 0.15f);
        var roation = new Vector3(0, player.hmdTransform.eulerAngles.y, 0);
        transform.position = pos;
        transform.rotation = Quaternion.Euler(roation);
     */
    }

        /*
    private void Update()
    {
        if (SteamVR_Controller.Input(3) != null)
        {
            SteamVR vr = SteamVR.instance;
            if (vr != null)
            {
                var pose = SteamVR_Controller.Input(3).transform.pos;
                var gamePose = SteamVR_Controller.Input(3).transform.rot;
                var t = new SteamVR_Utils.RigidTransform(gamePose.mDeviceToAbsoluteTracking);
                transform.localPosition = t.pos;
                transform.localRotation = t.rot;
            }
        }
    }*/
}
