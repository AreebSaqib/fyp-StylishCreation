using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Slicing_VS_Object_off : MonoBehaviour
{
    public AudioSource CollideSound;
    public void ObjectOff()
    {
        gameObject.SetActive(false);
    }
    public void PlayCollide()
    {
        CollideSound.Play();
    }
}
