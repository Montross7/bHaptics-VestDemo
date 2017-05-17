using System.Collections;
using System.Collections.Generic;
using Assets.Bhaptics.Scripts;
using Valve.VR.InteractionSystem;
using Tactosy.Common;
using Tactosy.Unity;
using UnityEngine;

public class SwordDetection : MonoBehaviour
{
    private TactosyPlayer _tactosyPlayer;
    private TimeMapping timeMapping;
    private bool isLeftHand;
    private float velocity;

    void Start()
    {
        _tactosyPlayer = FindObjectOfType<Manager_Tactosy>().TactosyPlayer;
        timeMapping = FindObjectOfType<TimeMapping>();
    }

    void OnTriggerEnter(Collider other)
    {
        Process(other);
    }

    private void Update()
    {
        velocity = FindObjectOfType<Sword>().getVelocity();
    }

    void OnTriggerStay(Collider other)
    {
//        Debug.Log("stay" + other.gameObject.name);
        Process(other);
    }

    private void Process(Collider other)
    {
        RaycastHit hit;
        Debug.DrawLine(transform.parent.position, transform.parent.forward * 100, Color.red);
        if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit))
        {
            if (other.gameObject.tag == "Head")
            {
                Debug.Log("stay : " + other.gameObject);
                byte[] bytes = TactosyTransform.ConvertHeadToTactosyPoint(other.gameObject.transform, hit.point, 80);
                _tactosyPlayer.SendSignal("head", PositionType.Head, bytes, 200);
            }
            else if (other.gameObject.tag == "Body")
            {
                bool isFrontFirst = false;
                Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
                Vector3 localDir = localPoint.normalized;
                float fwdDot = Vector3.Dot(localDir, Vector3.forward);

                Debug.DrawLine(transform.parent.position, transform.parent.forward * 100, Color.red);

                Point point;

                int intenseVelocity = 100;

                if (velocity < 2.0f)
                {
                    intenseVelocity = 0;
                }
                else if (velocity < 8.0f)
                {
                    intenseVelocity = 20 + (int)(10 * velocity);
                }
                else
                {
                    intenseVelocity = 100;
                }

                if (TactosyTransform.IsSelf(other.gameObject))
                {
                    point = TactosyTransform.ConvertToSelfTactosyPoint(other.gameObject.transform, hit.point, intenseVelocity);
                    if (fwdDot > 0)
                    {
                        isFrontFirst = true;
                    }
                }
                else
                {
                    point = TactosyTransform.ConvertToTactosyPoint(other.gameObject.transform, hit.point, intenseVelocity);
                    if (fwdDot < 0)
                    {
                        isFrontFirst = true;
                    }
                }
                List<Point> list = new List<Point>();
                list.Add(point);
                List<Point> backList = new List<Point>();
                backList.Add(new Point(point.X, 1f - point.Y, point.Intensity));
//                _tactosyPlayer.SendSignal("key", PositionType.VestFront, list, 100);
//                if (isFrontFirst)
//                {
                    _tactosyPlayer.SendSignal("key1", PositionType.VestFront, list, 100);
//
//                }
//                else
//                {
                    _tactosyPlayer.SendSignal("key2", PositionType.VestBack, backList, 100);
//
//                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //        Destroy(other.gameObject);
//        Debug.Log("exit : " + other.gameObject);
        Process(other);
    }
}
