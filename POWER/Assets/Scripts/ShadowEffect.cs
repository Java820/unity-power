using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShadowEffect : MonoBehaviour
{
    public Vector3 offset = new Vector3(-0.1f,-0.1f);
    public Material material;
    GameObject shadow;
    SpriteRenderer renderer;
    SpriteRenderer sr;

    void Start()
    {
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;

        shadow.transform.localPosition = offset;
        shadow.transform.localRotation = Quaternion.identity;

        renderer = GetComponent<SpriteRenderer>();
        sr = shadow.AddComponent<SpriteRenderer>();
        sr.material = material;

    }

    void LateUpdate()
    {
        shadow.transform.localPosition = offset;
        sr.sprite = renderer.sprite;
        sr.sortingLayerName = renderer.sortingLayerName;
        sr.sortingOrder = renderer.sortingOrder - 1;


    }
}
