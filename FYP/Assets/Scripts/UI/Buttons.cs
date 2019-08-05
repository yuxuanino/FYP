using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buttons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject textShade;

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        //Debug.Log("Cursor Entering" + level + "GameObject");

        if (this.tag == "StartButton")
        {
            textShade.SetActive(true);
            //transform.localScale += new Vector3(0.2f, 0.2f, 0);
        }

        if (this.tag == "OtherButton")
        {
            textShade.SetActive(true);
            //transform.localScale += new Vector3(0.1f, 0.1f, 0);
        }
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        //Debug.Log("Cursor Exiting" + level + "GameObject");
        //All these just sets them back to default when cursor exits object
        if (this.tag == "LevelSelection")
        {
            //print(levelDesignHolder.name);
            //levelDesignHolder.SetActive(false);
            transform.localScale += new Vector3(-0.15f, -0.15f, 0);

            //isShowingLevelDesign = false;
            textShade.SetActive(false);
        }

        if (this.tag == "StartButton")
        {
            textShade.SetActive(false);
            //transform.localScale += new Vector3(-0.2f, -0.2f, 0);
        }

        if (this.tag == "OtherButton")
        {
            textShade.SetActive(false);
            //transform.localScale += new Vector3(-0.1f, -0.1f, 0);
        }
    }

}
