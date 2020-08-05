using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSoundEffect : soundClass
{
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        playSound(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
