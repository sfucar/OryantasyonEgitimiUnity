using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenManagerObject : MonoBehaviour
{
    public GameObject QuestDetailsCanvas;
    public GameObject ScrollablelistCanvas;
    public GameObject PanelAfterFinishing;
    public GameObject PanelQuestDetails;//pusula ekranı
    public GameObject PanelQuestTakenPopup;
    public GameObject PanelHelpHowWorks;

    public TMP_Text TextTaskFinalCount;
    public TMP_Text CurrentTaskNameText;
    public TMP_Text CurrentTaskDescText;
    public TMP_Text PanelQuestTakenPopupTextMessage;
    public GameObject TextHorizontalAccuracyWarning;
    public int refreshVariable; 

    //these are for moving the image that..
    public GameObject ImageWarningBackground;
    public Button GoDoQuestButton2;
    public RectTransform imageRectTransform;
    public float startX = -129.4f;
    public float endX = 112f;


    public void SetTextTaskFinalCount(string StringValue){
        TextTaskFinalCount.text=StringValue;
    }

    void start(){
        refreshVariable=0;
        StartCoroutine(MoveImageX(startX));
    }



    public IEnumerator MoveImageX(float startX){ 
    ImageWarningBackground.SetActive(true);       
    float elapsedTime = 0f;
    float duration = 0.7f;
    
    // 
    while (elapsedTime < duration)
    {
        // 
        float t = elapsedTime / duration;
        float newX = Mathf.Lerp(startX, endX, t);
        // Update the position
        imageRectTransform.anchoredPosition = new Vector2(newX, imageRectTransform.anchoredPosition.y);
        // Increment the elapsed time... Last frame
        elapsedTime += Time.deltaTime;

        yield return null;
    }
    imageRectTransform.anchoredPosition = new Vector2(endX, imageRectTransform.anchoredPosition.y);
    ImageWarningBackground.SetActive(false);
    }

    public void GoDoQuestButtonfunction(){
        
        
        if (PlayerPrefs.HasKey("Title"))
        {
            QuestDetailsCanvas.SetActive(true);
            ScrollablelistCanvas.SetActive(false); 
        }
        else if(!PlayerPrefs.HasKey("Title")){
            StartCoroutine(MoveImageX(startX));
        }

        

    }



}
