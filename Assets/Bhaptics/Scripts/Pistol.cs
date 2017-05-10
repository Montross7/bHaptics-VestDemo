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
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            _tactosyPlayer.SendSignal("RifleFire");         

            Shoot();
        }
    }

    private void Shoot()
    {
        shootSound.Shoot();
        fireEffect.Fire();
    }
}