using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public static class Utilities
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\


        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        #region PublicMethods

        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color color = default(Color), TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortOrder = 1)
        {
            if (color == null)
            {
                color = Color.black;
            }

            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortOrder;
            return textMesh;
        }

        public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
        {
            if (worldCamera.orthographic)
            {
                return GetMouseWorldPositionOrthographic(screenPosition, worldCamera);
            }
            else
            {
                return GetMouseWorldPositionPerspective(screenPosition, worldCamera);
            }
        }
        #endregion


        #region PrivateMethods

        private static Vector3 GetMouseWorldPositionOrthographic(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 vec = worldCamera.ScreenToWorldPoint(screenPosition);
            vec.z = 0f;
            return vec;
        }

        private static Vector3 GetMouseWorldPositionPerspective(Vector3 screenPosition, Camera worldCamera)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
            float distance;
            xy.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }
        #endregion


        //**************************************************\\
        //****************** Properties ********************\\
        //**************************************************\\


    }
}
