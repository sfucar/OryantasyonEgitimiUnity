using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class ButtonPopulator : MonoBehaviour
{
    public Button buttonPrefab; // Button prefab
    public ButtonData[] buttonDataArray; //Button data objects
    public float buttonSpacing = 10f; // Space
    public GameObject panel;
    public GameObject OuterPanelForChoosingQuest; 
    public GameObject ScrollableList;
    public GameObject PanelQuestList;
    public GameObject PanelCurrentQuest;
    public Button closeButton; 
    public Button choosingButton;
    public Button OkayButtonForPanelQuestTakenPopup;
    public GameObject GoDoQuestButton;//başla butonu
    public Button GoDoQuestButton2;//başla butonu
    public TextMeshProUGUI titleText; 
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI CurrentTaskNameText;
    public TextMeshProUGUI CurrentTaskDescText;
    public ScreenManagerObject ScreenManagerObject;
    public float startX = -129.4f; //for moving the image

  
    private void Start()
    {
        string userQuestsDone = PlayerPrefs.GetString("userQuestsDone");
        int questNumber = 0;

        float currentButtonY = 0f; // Current button position Y

        //QuestList
        foreach (ButtonData buttonData in buttonDataArray)
        {
            // a new button from the prefab
            Button buttonInstance = Instantiate(buttonPrefab, transform);

            // get the RectTransform component
            RectTransform buttonTransform = buttonInstance.GetComponent<RectTransform>();

            // Set the position of the button
            buttonTransform.anchoredPosition = new Vector2(0f, currentButtonY);

            // get the TextMeshProUGUI component
            TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();

            questNumber = buttonData.questNumber;


            string[] arrayUserQuestsDone = userQuestsDone.Split(' ');
        //int[] numbers = Array.ConvertAll(arrayUserQuestsDone, int.Parse);
            int[] numbers = new int[arrayUserQuestsDone.Length];

            for (int i = 0; i < arrayUserQuestsDone.Length; i++)
            {
                if (int.TryParse(arrayUserQuestsDone[i], out int number))
                {
                    numbers[i] = number;
                }
                else
                {

                }
            }
            Image[] buttonImages = buttonInstance.GetComponentsInChildren<Image>();
            if(Array.Exists(numbers, number => number == questNumber)){
                foreach (Image img in buttonImages)
                {
                    // Check if it is the image we want to activate
                    if (img.name == "IconNot")
                    {
                        img.gameObject.SetActive(false); // Activate the image
                    }
                    else if(img.name == "IconCheck")
                    {
                        img.gameObject.SetActive(true); // Deactivate other images
                    }
                }
            }else{
                foreach (Image img in buttonImages)
                {
                    // Check the image
                    if (img.name == "IconNot")
                    {
                        img.gameObject.SetActive(true);
                    }
                    else if(img.name == "IconCheck")
                    {
                        img.gameObject.SetActive(false);
                    }
                }
            }


            // Set the title text
            buttonText.text = buttonData.title;

            // Update the Y position for the next button
            currentButtonY -= buttonTransform.rect.height + buttonSpacing;

            buttonInstance.onClick.AddListener(() => OpenPanelWithText(buttonData));
        }
        
        // add a click listener to the close button
        closeButton.onClick.AddListener(() => ClosePanel());
        GoDoQuestButton2.onClick.AddListener(() => GoDoQuestButtonfunction());
        OkayButtonForPanelQuestTakenPopup.onClick.AddListener(() => PanelQuestTakenPopup());
    }

    private void Update(){ //Amaç görev listesini yenilemek.
       
    if(ScreenManagerObject.refreshVariable==1){

            foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        string userQuestsDone = PlayerPrefs.GetString("userQuestsDone");
        int questNumber = 0;

        float currentButtonY = 0f; 
        foreach (ButtonData buttonData in buttonDataArray)
        {
        
            Button buttonInstance = Instantiate(buttonPrefab, transform);

            
            RectTransform buttonTransform = buttonInstance.GetComponent<RectTransform>();

            
            buttonTransform.anchoredPosition = new Vector2(0f, currentButtonY);

            
            TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();
            questNumber = buttonData.questNumber;


            string[] arrayUserQuestsDone = userQuestsDone.Split(' ');
            int[] numbers = new int[arrayUserQuestsDone.Length];

            for (int i = 0; i < arrayUserQuestsDone.Length; i++)
            {
                if (int.TryParse(arrayUserQuestsDone[i], out int number))
                {
                    numbers[i] = number;
                }
                else
                {

                }
            }
            Image[] buttonImages = buttonInstance.GetComponentsInChildren<Image>();
            if(Array.Exists(numbers, number => number == questNumber)){
                foreach (Image img in buttonImages)
                {
                    // 
                    if (img.name == "IconNot")
                    {
                        img.gameObject.SetActive(false); // 
                    }
                    else if(img.name == "IconCheck")
                    {
                        img.gameObject.SetActive(true); //
                    }
                }
            }else{
                foreach (Image img in buttonImages)
                {
                    //
                    if (img.name == "IconNot")
                    {
                        img.gameObject.SetActive(true); //
                    }
                    else if(img.name == "IconCheck")
                    {
                        img.gameObject.SetActive(false); // 
                    }
                }
            }

            
            buttonText.text = buttonData.title;

        
            currentButtonY -= buttonTransform.rect.height + buttonSpacing;
            //Yeniden listener ekle
            buttonInstance.onClick.AddListener(() => OpenPanelWithText(buttonData));
        }

    }
    //You should go back to 0. We have done with refreshing.
    ScreenManagerObject.refreshVariable=0;
        
    }

    public void QuestDoneChangeImgChecked (int questNumber) {
        
        foreach (ButtonData buttonData in buttonDataArray)
        {
            Button buttonInstance = Instantiate(buttonPrefab, transform);
            if(buttonData.questNumber==questNumber)
            // 
            {
                Image[] buttonImages = buttonInstance.GetComponentsInChildren<Image>();
                foreach (Image img in buttonImages)
                {
                    //
                    if (img.name == "IconNot")
                    {
                        img.gameObject.SetActive(false); //
                    }
                    else if(img.name == "IconCheck")
                    {
                        img.gameObject.SetActive(true); // 
                    }
                    else{

                    }
                }
                
            }
        }
    }
    
    public void RefreshQuestList () {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        string userQuestsDone = PlayerPrefs.GetString("userQuestsDone");
        int questNumber = 0;

        float currentButtonY = 0f; // 
        foreach (ButtonData buttonData in buttonDataArray)
        {
            Button buttonInstance = Instantiate(buttonPrefab, transform);
            RectTransform buttonTransform = buttonInstance.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition = new Vector2(0f, currentButtonY);
            TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();

            questNumber = buttonData.questNumber;

            string[] arrayUserQuestsDone = userQuestsDone.Split(' ');
            int[] numbers = new int[arrayUserQuestsDone.Length];

            for (int i = 0; i < arrayUserQuestsDone.Length; i++)
            {
                if (int.TryParse(arrayUserQuestsDone[i], out int number))
                {
                    numbers[i] = number;
                }
                else
                {

                }
            }
            Image[] buttonImages = buttonInstance.GetComponentsInChildren<Image>();
            if(Array.Exists(numbers, number => number == questNumber)){
                foreach (Image img in buttonImages)
                {
                    if (img.name == "IconNot")
                    {
                        img.gameObject.SetActive(false);
                    }
                    else if(img.name == "IconCheck")
                    {
                        img.gameObject.SetActive(true);
                    }
                }
            }else{
                foreach (Image img in buttonImages)
                {
                    if (img.name == "IconNot")
                    {
                        img.gameObject.SetActive(true);
                    }
                    else if(img.name == "IconCheck")
                    {
                        img.gameObject.SetActive(false);
                    }
                }
            }

            buttonText.text = buttonData.title;

            currentButtonY -= buttonTransform.rect.height + buttonSpacing;
        }
    }

    

    private void OpenPanelWithText(ButtonData buttonData)
    {

        // Set the texts
        titleText.text = buttonData.title;
        descriptionText.text = buttonData.description;

        //security
        GoDoQuestButton.SetActive(false);

        // Activate the panel if it is inactive
        if (!OuterPanelForChoosingQuest.activeSelf)
        {
            OuterPanelForChoosingQuest.SetActive(true);
        }
        ScrollableList.SetActive(false);
        //PanelQuestList.SetActive(false);
        choosingButton.onClick.AddListener(() => ChoosingQuest(buttonData));
    }

    private void ClosePanel()
    {
        //Deactivate the panel
        ScrollableList.SetActive(true);
        //original outerpanelforchoosingqeust
        OuterPanelForChoosingQuest.SetActive(false);
        GoDoQuestButton.SetActive(true);
    }
    
    //Choosing quest from the quest list
    private void ChoosingQuest(ButtonData buttonData)
    {
        // Deactivate the panel
        PlayerPrefs.DeleteKey("Title");
        PlayerPrefs.DeleteKey("Description");
        PlayerPrefs.DeleteKey("Latitude");
        PlayerPrefs.DeleteKey("Longitude");
        PlayerPrefs.DeleteKey("questNumber");

        PlayerPrefs.SetString("Title", buttonData.title);
        PlayerPrefs.SetString("Description", buttonData.description); 
        PlayerPrefs.SetFloat("Latitude", buttonData.latitude);
        PlayerPrefs.SetFloat("Longitude", buttonData.longitude);
        PlayerPrefs.SetInt("questNumber", buttonData.questNumber);

        CurrentTaskNameText.text = buttonData.title;
        CurrentTaskDescText.text = buttonData.description;
        
        OuterPanelForChoosingQuest.SetActive(false);
        ScreenManagerObject.PanelQuestTakenPopup.SetActive(true);
        ScreenManagerObject.PanelQuestTakenPopupTextMessage.text= buttonData.title + " görevi başarıyla seçildi.";
        
        
    }
    //func. for OkayButtonForPanelQuestTakenPopup
    private void PanelQuestTakenPopup()
    {
        // Deactivate the panel
        ScreenManagerObject.PanelQuestTakenPopup.SetActive(false);
        ScrollableList.SetActive(true);
        PanelQuestList.SetActive(false);
        PanelCurrentQuest.SetActive(true);
        GoDoQuestButton.SetActive(true);
        
        
    }

    private void GoDoQuestButtonfunction(){

        if (PlayerPrefs.HasKey("Title"))
        {
            ScreenManagerObject.QuestDetailsCanvas.SetActive(true);
            ScreenManagerObject.ScrollablelistCanvas.SetActive(false); 
        }
        else if(!PlayerPrefs.HasKey("Title")){
            //StartCoroutine(ScreenManagerObject.MoveImageX(startX));
        }
        else{
           //ScreenManagerObject.MoveImageX(startX);
        }
               

    }

    


}//end of button populator class
