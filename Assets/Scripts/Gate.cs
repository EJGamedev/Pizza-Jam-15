using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    [Tooltip("drag the camera here when setting up level, or don't, it should find it on it's own too")]
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        if(audioManager == null)
        {
            audioManager = GameObject.Find("Main Camera").GetComponent<AudioManager>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Phrase>() != null)
        {
            audioManager.addPhrase(other.GetComponent<Phrase>());
        }
    }
}
