using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Material mat;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        color = mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        color.a = Mathf.Sin(Time.time) / 2f + 0.5f;
        mat.color = color;
    }
}
