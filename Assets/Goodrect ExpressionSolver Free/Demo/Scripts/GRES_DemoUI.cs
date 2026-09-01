using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GRES_DemoUI : MonoBehaviour
{
    public GameObject[] lessons;
    public Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject l in lessons) l.SetActive(false);
    }

    public void ActivateDemo(int l)
    {
        for (int i = 0; i < lessons.Length; i++)
            lessons[i].SetActive(l == i);
    }
}
