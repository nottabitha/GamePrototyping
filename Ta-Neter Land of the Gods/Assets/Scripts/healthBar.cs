﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{

    private Transform bar;
    // Start is called before the first frame update
    private void Start()
    {
        bar = transform.Find("Bar");
    }

    // Update is called once per frame
    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
