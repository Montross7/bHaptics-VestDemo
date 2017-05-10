using UnityEngine;
using Valve.VR.InteractionSystem;
using Tactosy.Common;
using Tactosy.Unity;

public class Sword : MonoBehaviour
{
    private Rigidbody rb;
    private TactosyPlayer _tactosyPlayer;
    // Use this for initialization
    void Start()
    {
        _tactosyPlayer = FindObjectOfType<Manager_Tactosy>().TactosyPlayer;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void HandAttachedUpdate(Hand hand)
    {
        double intense = rb.velocity.magnitude;
    }
}
