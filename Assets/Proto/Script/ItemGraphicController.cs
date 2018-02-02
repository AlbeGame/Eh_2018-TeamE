﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemGraphicController : MonoBehaviour {

    public Material NormalMat;
    public Material HighlightedMat;
    public Material PressedMat;
    List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    private void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
    }

    public void PaintNormal()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = NormalMat;
        }
    }
    public void PaintHighlight()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = HighlightedMat;
        }
    }
    public void PaintPressed()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = PressedMat;
        }
    }
}
