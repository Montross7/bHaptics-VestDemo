using Tactosy.Common;
using UnityEngine;

namespace Assets.Bhaptics.Scripts
{
    public static class TactosyTransform
    {
        // 0, 0.6   0.7, 1
        public static Point ConvertToTactosyPoint(Transform tr, Vector3 vec, int intensity)
        {
            var xLocalScale = tr.localScale.x;
            var xPos = tr.position.x;
            var yLocalScale = tr.localScale.y;
            var yPos = tr.position.y;

            float y = (-xPos + xLocalScale / 2f + vec.x) / xLocalScale;
            float x = (-yPos + xLocalScale / 2f + vec.y) / yLocalScale;
            
            return new Point(x, y, intensity);
        }

        // 0, 0.6   0.7, 1
        public static Point ConvertToSelfTactosyPoint(Transform tr, Vector3 vec, int intensity)
        {
            Vector3 rotation = -(new Vector3(0, tr.eulerAngles.y, 0));
            var rotatePoint = RotatePointAroundPivot(vec, tr.position, rotation);

            var xLocalScale = tr.localScale.x;
            var xPos = tr.position.x;
            var yLocalScale = tr.localScale.y;
            var yPos = tr.position.y;

            float x = (-xPos + xLocalScale / 2f + rotatePoint.x) / xLocalScale;
            float y = (-yPos + xLocalScale / 2f + vec.y) / yLocalScale;

            // real x, y

            var convertedX = y;
            var convertedY = x;
            // converted

            return new Point(convertedX, convertedY, intensity);
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
            var dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }

        public static bool IsSelf(GameObject go)
        {
            var parent = go.transform.parent;
            if (parent != null)
            {
                if (parent.gameObject.name == "BodyFollow")
                {
                    return true;
                }

                var pParent = parent.parent;
                if (pParent != null)
                {
                    if (pParent.gameObject.name == "VRCamera")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static byte[] ConvertHeadToTactosyPoint(Transform tr, Vector3 vec, int intensity)
        {
            Vector3 rotation = -(new Vector3(0, tr.eulerAngles.y, 0));
            var rotatePoint = RotatePointAroundPivot(vec, tr.position, rotation);

            var xLocalScale = tr.localScale.x;
            var xPos = tr.position.x;
            var yLocalScale = tr.localScale.y;
            var yPos = tr.position.y;

            float x = (-xPos + xLocalScale / 2f + rotatePoint.x) / xLocalScale;
            float y = 1f - (-yPos + xLocalScale / 2f + vec.y) / yLocalScale;

            byte[] bytes;
            // Right to Left (0 to 8)
            if (Mathf.Pow(x - .5f, 2f) + Mathf.Pow(y - .5f, 2f) < 0.09)
            {
                // Center
                bytes = new byte[]
                {
                    80, 80, 80, 80, 80,
                    80, 80, 80, 80, 80,
                    80, 80, 80, 80, 80,
                    80, 80, 80, 80, 80
                };
            }
            else
            {
                if (y < 0.2f) // Top
                {
                    bytes = new byte[]
                    {
                        0, 0, 0, 80, 80,
                        80, 80, 0, 0, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0
                    };
                } else if (y >= 0.8f) // Bottom
                {
                    bytes = new byte[]
                    {
                        80, 80, 0, 0, 0,
                        0, 0, 80, 80, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0
                    };
                }
                else if (x < 0.5f)
                {
                    // Left
                    bytes = new byte[]
                    {
                        0, 0, 0, 0, 80,
                        80, 80, 80, 80, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0
                    };
                }
                else
                {
                    // Right
                    bytes = new byte[]
                    {
                        80, 80, 80, 80, 80,
                        0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0
                    };
                }
            }

            return bytes;
        }
    }
}
