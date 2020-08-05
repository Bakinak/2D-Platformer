using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundClass : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void playSound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        soundEffect soundScript = soundObject.AddComponent<soundEffect>();
        Instantiate(soundObject, transform);
    }

}
