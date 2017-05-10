using UnityEngine;
using Valve.VR.InteractionSystem;
using Tactosy.Common;
using Tactosy.Unity;

[RequireComponent(typeof(Interactable))]
public class Stone : MonoBehaviour
{
    private TactosyPlayer _tactosyPlayer;
    [SerializeField] GameObject throwableObject;
    private bool ready = false;
    
    void Start()
    {
        _tactosyPlayer = FindObjectOfType<Manager_Tactosy>().TactosyPlayer;
        ready = true;
    }

    private void HandAttachedUpdate(Hand hand)
    {
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) )
        {
            ControllerButtonHints.HideButtonHint(hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        } else if
            (hand.controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && ready)
        {
            ControllerButtonHints.ShowButtonHint(hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
            transform.GetChild(0).gameObject.SetActive(false);

            var instantiate = Instantiate(throwableObject, transform.position, Quaternion.identity);

            var rb = instantiate.GetComponentInChildren<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 10, ForceMode.VelocityChange);

            if (ready == true && hand.GuessCurrentHandType() == Hand.HandType.Left)
            {
                _tactosyPlayer.SendSignal("SphereThrowLeft");
            }
            else if (ready == true && hand.GuessCurrentHandType() == Hand.HandType.Right)
            {
                _tactosyPlayer.SendSignal("SphereThrowRight");
            }

            Invoke("Show", 0.5f);
            Destroy(instantiate, 3f);

            ready = false;
        }
    }

    private void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        ready = true;

    }
}
