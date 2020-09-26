using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBlocker : MonoBehaviour
{
    void Start()
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            r.enabled = false;
        }
    }
}
