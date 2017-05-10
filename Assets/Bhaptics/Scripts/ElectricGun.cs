using Assets.Bhaptics.Scripts;
using DigitalRuby.LightningBolt;
using Tactosy.Common;
using Tactosy.Unity;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class ElectricGun : MonoBehaviour
{
    private TactosyPlayer _tactosyPlayer;

    public ShootSound shootSound;
    public LightningBoltScript LightningBolt;
    public Transform gunBarrelEnd;

    void Start()
    {
        _tactosyPlayer = FindObjectOfType<Manager_Tactosy>().TactosyPlayer;
//        LightningBolt.ChaosFactor = 0.3f;
        LightningBolt.ManualMode = true;
    }

    private void HandAttachedUpdate(Hand hand)
    {
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            LightningBolt.ManualMode = false;
            Shoot();
        }
        
        if (hand.controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            ShootEnd();
            LightningBolt.ManualMode = true;
        }

        if (hand.controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (!shootSound.IsPlaying())
            {
                shootSound.Shoot();
            }

            if (!_tactosyPlayer.IsPlaying("Electricgun"))
            {
                _tactosyPlayer.SendSignal("Electricgun");
            }

            Ray shootRay = new Ray(gunBarrelEnd.position, gunBarrelEnd.forward);
            
            RaycastHit[] damagedEnemy = Physics.SphereCastAll(shootRay.origin, 1f, shootRay.direction, 10f, -1);
            LightningBolt.EndPosition = shootRay.origin + shootRay.direction * 10f;
            Debug.DrawLine(LightningBolt.StartObject.transform.position, LightningBolt.EndPosition, Color.red);

            if (damagedEnemy.Length > 0)
            {
                foreach (RaycastHit shootHit in damagedEnemy)
                {
                    bool isSelf = TactosyTransform.IsSelf(shootHit.collider.gameObject);

                    if (shootHit.collider.gameObject.tag == "Head")
                    {
                        Debug.Log("head");
                        if (!_tactosyPlayer.IsPlaying("Electric_Head"))
                        {
                            _tactosyPlayer.SendSignal("Electric_Head");
                        }
                        if (!isSelf)
                        {
                            LightningBolt.EndPosition = shootHit.collider.gameObject.transform.position;
                        }
                    }
                    else if (shootHit.collider.gameObject.tag == "Body")
                    {
                        Debug.Log("body");
                        if (!_tactosyPlayer.IsPlaying("Electric_VestFront"))
                        {
                            _tactosyPlayer.SendSignal("Electric_VestFront");
                        }
                        if (!_tactosyPlayer.IsPlaying("Electric_VestBack"))
                        {
                            _tactosyPlayer.SendSignal("Electric_VestBack");
                        }
                        if (!isSelf)
                        {
                            LightningBolt.EndPosition = shootHit.collider.gameObject.transform.position;
                        }
                    }
                }
            }
            else
            {
                LightningBolt.EndPosition = shootRay.origin + shootRay.direction * 10f;
            }
        }
    }

    private void Shoot()
    {
        shootSound.Shoot();
        _tactosyPlayer.SendSignal("Electricgun");
    }

    private void ShootEnd()
    {
        _tactosyPlayer.TurnOff();
        shootSound.ShootEnd();
    }
}