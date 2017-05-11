using UnityEngine;
using Valve.VR.InteractionSystem;
using Tactosy.Common;
using Tactosy.Unity;

public class Sword : MonoBehaviour
{
    private TactosyPlayer _tactosyPlayer;
    private Vector3 lastPosition;
    bool isLeftHand;

    // Use this for initialization
    void Start()
    {
        isLeftHand = false;
        _tactosyPlayer = FindObjectOfType<Manager_Tactosy>().TactosyPlayer;
        lastPosition = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 vel = (gameObject.transform.position - lastPosition) / Time.deltaTime;
        lastPosition = gameObject.transform.position;
        float velocity = vel.magnitude;
        if(velocity > 3.5f)
        {
            if (isLeftHand)
            {
                _tactosyPlayer.SendSignal("SwordLeft");
            }
            else
            {
                _tactosyPlayer.SendSignal("SwordRight");
            }
        }
    }

    void HandAttachedUpdate(Hand hand)
    {
        if (hand.GuessCurrentHandType() == Hand.HandType.Left)
        {
            isLeftHand = true;
        }
        else
        {
            isLeftHand = false;
        }
    }
}