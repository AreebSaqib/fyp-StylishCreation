using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animationoff : MonoBehaviour
{
    public void animationOFF()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }
}
