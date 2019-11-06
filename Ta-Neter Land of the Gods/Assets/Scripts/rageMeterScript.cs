using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rageMeterScript : MonoBehaviour
{
    public GameObject bar;
    public float sizeNormalized = 0f;
    // Start is called before the first frame update
    private void Start()
    {
    }

    private void Update()
    {
        SetSize();
    }

    // Update is called once per frame
    public void SetSize()
    {
        bar.transform.localScale = new Vector3(sizeNormalized, 1f);
    }
}
