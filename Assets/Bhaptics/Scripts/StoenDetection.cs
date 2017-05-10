using System.Collections;
using System.Collections.Generic;
using Assets.Bhaptics.Scripts;
using Tactosy.Common;
using Tactosy.Unity;
using UnityEngine;

public class StoenDetection : MonoBehaviour {
    public GameObject PopEff;
    private TactosyPlayer _tactosyPlayer;
    private TimeMapping timeMapping;

    void Start () {
        _tactosyPlayer = FindObjectOfType<Manager_Tactosy>().TactosyPlayer;
        timeMapping = FindObjectOfType<TimeMapping>();
    }
	

    void OnCollisionEnter(Collision other)
    {
        Process(other);

    }

    private void Process(Collision other)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        Instantiate(PopEff, GetComponent<Transform>());

        Destroy(gameObject, 0.1f);

        if (TactosyTransform.IsSelf(other.gameObject))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Head")
        {
            Debug.Log("stay : " + other.gameObject);
            byte[] bytes = TactosyTransform.ConvertHeadToTactosyPoint(other.gameObject.transform, other.contacts[0].point, 80);
            _tactosyPlayer.SendSignal("head", PositionType.Head, bytes, 200);
        }
        else if (other.gameObject.tag == "Body")
        {
            bool isFrontFirst = false;
            Vector3 localPoint = other.gameObject.transform.InverseTransformPoint(other.contacts[0].point);
            Vector3 localDir = localPoint.normalized;
            float fwdDot = Vector3.Dot(localDir, Vector3.forward);
            
            Point point;
            if (TactosyTransform.IsSelf(other.gameObject))
            {
                point = TactosyTransform.ConvertToSelfTactosyPoint(other.gameObject.transform, other.contacts[0].point, 100);
                if (fwdDot > 0)
                {
                    isFrontFirst = true;
                }
            }
            else
            {
                point = TactosyTransform.ConvertToTactosyPoint(other.gameObject.transform, other.contacts[0].point, 100);
                if (fwdDot < 0)
                {
                    isFrontFirst = true;
                }
            }
            List<Point> list = new List<Point>();
            list.Add(point);
            List<Point> backList = new List<Point>();
            backList.Add(new Point(point.X, 1f - point.Y, point.Intensity));
            StartCoroutine(PlayFeedback(timeMapping.GetDelayMillis(), isFrontFirst, list, backList));
//            _tactosyPlayer.SendSignal("body", PositionType.VestFront, list, 100);
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
