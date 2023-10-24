using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDetailsCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private void Start()
    {
        // fetch the data from PlayerPrefs
        string title = PlayerPrefs.GetString("Title");
        string description = PlayerPrefs.GetString("Description");

        // Set the texts 
        titleText.text = title;
        descriptionText.text = description;
    }
    void Update()
    {
        string title = PlayerPrefs.GetString("Title");
        string description = PlayerPrefs.GetString("Description");

        // Set the texts
        
        titleText.text = title;
        descriptionText.text = description;
    }
}
