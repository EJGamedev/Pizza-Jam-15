using UnityEngine;

public class MenuScript : MonoBehaviour
{

    float m1;
    bool hasClicked;



    // Update is called once per frame
    void Update()
    {
        m1 = Input.GetAxis("Fire1");
        //Debug.Log(PlayerPrefs.GetInt("camAngle", -40));
        if (m1 > 0 && !hasClicked)
        {
            mouseClick();
            hasClicked = true;
        }

        if (m1 <= 0)
        {
            hasClicked = false;
        }
    }

    void mouseClick()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Construct a ray from the current mouse coordinates

        RaycastHit hit;
        //Ray ray = cam.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out hit))
        {
            MenuBlock target = hit.collider.gameObject.GetComponent<MenuBlock>();

            if (target != null)
            {
                target.interact();
            }
        }
    }
}