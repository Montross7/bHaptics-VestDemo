using System.Collections;
using System.Collections.Generic;
using Assets.Bhaptics.Scripts;
using Tactosy.Common;
using Tactosy.Unity;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    [SerializeField] private Transform gunBarrelEnd;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private LineRenderer gunLine;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private int tactsoyFeedbackIntensity = 100;
    [SerializeField] private int tactsoyFeedbackDuration = 200;
    [SerializeField] private int tactosyPointCount = 0;
    private float range = 30f;

    private TactosyPlayer _tactosyPlayer;
    private TimeMapping timeMapping;

    void Start()
    {
        gunLine.startWidth = 0.01f;
        gunLine.endWidth = 0.02f;
        gunLine.enabled = true;

        _tactosyPlayer = FindObjectOfType<Manager_Tactosy>().TactosyPlayer;
        timeMapping = FindObjectOfType<TimeMapping>();

    }

    void Update()
    {
        Ray shootRay = new Ray(gunBarrelEnd.transform.position, gunBarrelEnd.transform.forward);
        RaycastHit shootHit;
        gunLine.SetPosition(0, gunBarrelEnd.transform.position);

        if (Physics.Raycast(shootRay, out shootHit, range, -1))
        {
            gunLine.SetPosition(1, shootHit.point);

        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    public void Fire()
    {
        Ray shootRay = new Ray(gunBarrelEnd.transform.position, gunBarrelEnd.transform.forward);
        RaycastHit shootHit;

        if (Physics.Raycast(shootRay, out shootHit, range, -1))
        {
            var go = shootHit.collider.gameObject;
            bool isSelf = TactosyTransform.IsSelf(shootHit.collider.gameObject);
            if (!isSelf)
            {
                var effectIstance = Instantiate(hitEffect,
                    shootHit.point, Quaternion.LookRotation(transform.forward)) as GameObject;
                Destroy(effectIstance, 2f);
            }

            if (shootHit.collider.gameObject.tag == "Body")
            {
                bool isFrontFirst = false;
                Vector3 localPoint = shootHit.transform.InverseTransformPoint(shootHit.point);
                Vector3 localDir = localPoint.normalized;
                float fwdDot = Vector3.Dot(localDir, Vector3.forward);
                Debug.Log(localDir + ", " + fwdDot);


                List<Point> list = new List<Point>();
                List<Point> backList = new List<Point>();

                Point point;
                if (isSelf)
                {
                    point =
                        TactosyTransform.ConvertToSelfTactosyPoint(go.transform, shootHit.point, tactsoyFeedbackIntensity);

                    if (fwdDot > 0)
                    {
                        isFrontFirst = true;
                    }
                }
                else
                {
                    point =
                        TactosyTransform.ConvertToTactosyPoint(go.transform, shootHit.point, tactsoyFeedbackIntensity);

                    if (fwdDot < 0)
                    {
                        isFrontFirst = true;
                    }
                }

                list.Add(point);
                backList.Add(new Point(point.X, 1f-point.Y, point.Intensity));

                for (int i = 0; i < tactosyPointCount; i++)
                {
                    float rand = Random.Range(0, 0.5f);
                    var xx = Mathf.Max(0, Mathf.Min(1f, rand + point.X));
                    var yy = Mathf.Max(0, Mathf.Min(1f, rand + point.Y));
                    list.Add(new Point(xx, yy, tactsoyFeedbackIntensity));
                    backList.Add(new Point(xx, 1f-yy, tactsoyFeedbackIntensity));
                }

                StartCoroutine(PlayFeedback(timeMapping.GetDelayMillis(), isFrontFirst, list, backList));
            } else if (shootHit.collider.gameObject.tag == "Head")
            {
                byte[] bytes = TactosyTransform.ConvertHeadToTactosyPoint(go.transform, shootHit.point, tactsoyFeedbackIntensity);
                _tactosyPlayer.SendSignal("head", PositionType.Head, bytes, tactsoyFeedbackDuration);
            }
        }
        particleSystem.Stop();
        particleSystem.Play();

        if (impactEffect != null)
        {
            var effectIstance = Instantiate(impactEffect, gunBarrelEnd.position,
                Quaternion.LookRotation(-gunBarrelEnd.transform.forward));

            Destroy(effectIstance, 2f);
        }
    }

    IEnumerator PlayFeedback(int delayMillis, bool isFrontFirst, List<Point> frontList, List<Point> backList)
    {
        if (isFrontFirst)
        {
            _tactosyPlayer.SendSignal("key", PositionType.VestFront, frontList, 100);

            if (delayMillis > 0)
            {
                yield return new WaitForSeconds((float)delayMillis * 0.001f);
                _tactosyPlayer.SendSignal("key1", PositionType.VestBack, backList, 100);
            } 

        }
        else
        {
            _tactosyPlayer.SendSignal("key", PositionType.VestBack, backList, 100);
            if (delayMillis > 0)
            {
                yield return new WaitForSeconds((float)delayMillis * 0.001f);
                _tactosyPlayer.SendSignal("key1", PositionType.VestFront, backList, 100);
            }
            
        }
    }
}
