using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSortingLayer : MonoBehaviour
{
    private void OnValidate()
    {
        var text_mesh = GetComponent<MeshRenderer>();
        text_mesh.sortingOrder = 2;
    }

    private void Awake()
    {
        var text_mesh = GetComponent<MeshRenderer>();
        text_mesh.sortingOrder = 2;
    }
}
