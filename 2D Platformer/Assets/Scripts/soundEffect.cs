using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffect : MonoBehaviour
{
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        clip = GetComponent<AudioSource>().clip;
        Destroy(gameObject, clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Add method where it sets the volume based on some overall game options, like master volume, audio volume, music volume, so on.
}
