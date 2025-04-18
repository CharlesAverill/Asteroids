using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Label : MonoBehaviour
{
    public string name;
    public Transform target;
    public Vector3 offset = new Vector3(-488, -309, 0) / 2;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = name;
    }

    public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
        screenPoint.z = 0;
     
        Vector2 screenPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
        {
            return screenPos;
        }
     
        return screenPoint;
    }


    // Update is called once per frame
    void Update()
    {
        if (!target || target.GetComponent<Asteroid>().isDying) {
            Destroy(gameObject);
        }

        ((RectTransform)transform).anchoredPosition = WorldToScreenSpace(target.transform.position, Camera.main, (RectTransform)transform) + offset;
    }
}
