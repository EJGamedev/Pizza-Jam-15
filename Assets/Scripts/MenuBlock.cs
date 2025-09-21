using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBlock : MonoBehaviour
{


    [Header("Level Loader Settings")]
    public bool loadLevel;
    public string levelToLoad;

    [Header("Menu Navigation Settings")]
    public bool rotateCamera;
    public Transform newAngle;
    public Transform cam;
    public float rotationSpeed = 0.1f;

    [Header("Audio Settings")]
    public bool musicSettings;
    public float musicIncrement = 0.01f;

    [Header("MISC")]
    public TMP_Text blockText;


    //Things you cant see
    bool startrotation;
    float timeCount;
    static Transform targetAngle;

    // Start is called before the first frame update
    void Start()
    {   

        if (musicSettings && blockText != null)
        {
            blockText.text = "Music Volume\n" + (PlayerPrefs.GetFloat("MusicVolume", 0.5f) * 100).ToString("0");
            return;
        }

        if(loadLevel && blockText != null)
        {
            if(PlayerPrefs.GetFloat(levelToLoad) < 1.1f)
            {
                blockText.text = levelToLoad + ": N/A";
            }
            else
            {
                blockText.text = levelToLoad + ": " + PlayerPrefs.GetFloat(levelToLoad).ToString("N3")+"s";            
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetAngle != newAngle) { startrotation = false; timeCount = 0f; }

        if (startrotation)
        {

            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, newAngle.rotation, timeCount * rotationSpeed);
            cam.transform.position = Vector3.Lerp(cam.transform.position, newAngle.position, timeCount * rotationSpeed);
            timeCount = timeCount + Time.deltaTime;
            if (cam.transform.rotation == newAngle.rotation && cam.transform.position == newAngle.position)
            {
                startrotation = false;
                timeCount = 0;
            }
        }


    }

    public void interact()
    {
        if (loadLevel)
        {
            SceneManager.LoadScene(levelToLoad);
        }

        if (rotateCamera)
        {
            timeCount = 0;
            startrotation = true;
            targetAngle = newAngle;
        }



        if (musicSettings)
        {
            if (musicIncrement > 0 && PlayerPrefs.GetFloat("MusicVolume", 0.5f) + musicIncrement > 1) { return; }
            else
            if (musicIncrement < 0 && PlayerPrefs.GetFloat("MusicVolume", 0.5f) + musicIncrement < 0) { return; }

            PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0.5f) + musicIncrement);

            if (blockText != null)//this updates the blocktext
            {
                blockText.text = "Music Volume\n" + (PlayerPrefs.GetFloat("MusicVolume", 0.5f) * 100).ToString("0");
            }
            PlayerPrefs.Save();
            return;
        }
    }
}
