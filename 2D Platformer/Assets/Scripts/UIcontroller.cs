using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{

    public GameObject blackScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Tutorial about this: https://turbofuture.com/graphic-design-video/How-to-Fade-to-Black-in-Unity
    public IEnumerator fadeToBlack(bool fadeToBlack = true, int fadeSpeed = 5)
    {
        Color objectColor = blackScreen.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while(blackScreen.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackScreen.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            //Call respawning function in my game manager?
            GameObject.Find("Manager").GetComponent<myGameManager>().playerDeath();
        }
        else //Fade from black
        {
            while (blackScreen.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackScreen.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }

    }

}
