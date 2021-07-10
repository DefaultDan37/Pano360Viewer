using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class TextureSwapper : MonoBehaviour
{
    public GameObject[] buttons;

    public UnityEvent TextureSwap;

    // Start is called before the first frame update
    void Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("Button");

        if (( buttons != null) || (buttons.Length > 0))
        {

        }
    }
    
    public void UpdateMaterial(Material m)
    {
        Debug.Log("pressed");

        if (m != null)
        {
            MeshRenderer ms = gameObject.GetComponent<MeshRenderer>();

            if (ms != null)
            {
                ms.material = m;
            }
        }
    }

    //    public class ClickExample : MonoBehaviour
    //{
    //    public Button yourButton;

    //    void Start()
    //    {
    //        Button btn = yourButton.GetComponent<Button>();
    //        btn.onClick.AddListener(TaskOnClick);
    //    }

    //    void TaskOnClick()
    //    {
    //        Debug.Log("You have clicked the button!");
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
