using Tactosy.Common;
using Tactosy.Unity;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Pistol : MonoBehaviour
{
    private TactosyPlayer _tactosyPlayer;
    void Start()
    {
        _tactosyPlayer = FindObjectOfType<Manager_Tactosy>().TactosyPlayer;
    }

    public ShootSound shootSound;
    public FireEffect fireEffect;

    private void HandAttachedUpdate(Hand hand)
    {
        bool isLeftHand = false;
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (hand.GuessCurrentHandType() == Hand.HandType.Left)
            {
                isLeftHand = true;
            }
            else
            {
                isLeftHand = false;
            }
            if (isLeftHand)
            {
                _tactosyPlayer.SendSignal("PistolLeftFire");
            }
            else
            {
                _tactosyPlayer.SendSignal("PistolRightFire");
            }
            Shoot();
        }
    }

    private void Shoot()
    {
        shootSound.Shoot();
        fireEffect.Fire();
    }
}