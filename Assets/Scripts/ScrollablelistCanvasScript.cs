using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ScrollablelistCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI CurrentTaskNameText;
    public TextMeshProUGUI CurrentTaskDescText;
    public ScreenManagerObject ScreenManagerObject;
    public Button GoDoQuestButton2;//başla butonu
    

    private void Start()
    {
        //For test purposes
        //PlayerPrefs.DeleteKey("userQuestsDone");
        //PlayerPrefs.DeleteKey("TaskFinalCount");
        
        GoDoQuestButton2.onClick.AddListener(() => GoDoQuestButtonfunction());

        /*Test
        PlayerPrefs.DeleteKey("userQuestsDone");
        PlayerPrefs.DeleteKey("TaskFinalCount");
        PlayerPrefs.DeleteKey("Title");
        PlayerPrefs.DeleteKey("Description");
        PlayerPrefs.DeleteKey("Latitude");
        PlayerPrefs.DeleteKey("Longitude");
        PlayerPrefs.DeleteKey("questNumber");
        */
        

        if (!PlayerPrefs.HasKey("TaskFinalCount"))
        {
            // If not, set an initial value
            int currentInt=PlayerPrefs.GetInt("TaskFinalCount");
            PlayerPrefs.SetInt("TaskFinalCount", 0);
            PlayerPrefs.Save();
            ScreenManagerObject.PanelHelpHowWorks.SetActive(true);
        }
        ScreenManagerObject.TextTaskFinalCount.text = PlayerPrefs.GetInt("TaskFinalCount").ToString();

        if (!PlayerPrefs.HasKey("Title"))
        {
            // If not, set an initial value
            /*
            int currentInt=PlayerPrefs.GetInt("TaskFinalCount");
            PlayerPrefs.SetInt("TaskFinalCount", 0);
            PlayerPrefs.Save();
            */

            CurrentTaskNameText.text = "Bir Görev Seç";
            CurrentTaskDescText.text = "Listeden bir görev seçerek başla.";

        }else{
            //
            string userQuestsDone = PlayerPrefs.GetString("userQuestsDone");
            string title = PlayerPrefs.GetString("Title");
            string description = PlayerPrefs.GetString("Description");

            // Set the texts
            CurrentTaskNameText.text = title;
            CurrentTaskDescText.text = description;
            
        }
        
    }
    private void GoDoQuestButtonfunction(){

        if (PlayerPrefs.HasKey("Title"))
        {
            ScreenManagerObject.QuestDetailsCanvas.SetActive(true);
            ScreenManagerObject.ScrollablelistCanvas.SetActive(false); 
        }
               

    }
}
