using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageOrder51 : MonoBehaviour
{
    public TextMeshProUGUI instruction1;
    public TextMeshProUGUI instruction2;
    public TextMeshProUGUI instruction3;
    public int flag = 1;

    // Start is called before the first frame update
    void Start()
    {
        instruction2.enabled = false;
        instruction3.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && flag == 1)
        {
            instruction1.enabled = false;

            flag = 2;
        }

        if (flag == 2)
        {
            instruction2.enabled = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                instruction2.enabled = false;
                flag = 3;
            }
        }

        if (flag == 3)
        {
            instruction3.enabled = true;
        }
    }
}
