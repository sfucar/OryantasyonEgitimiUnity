using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GPSLocation : MonoBehaviour
{
    public static GPSLocation Instance { set; get; }
    
    public TMP_Text GPSStatus;
    public TMP_Text latitudeValue;
    public TMP_Text longitudeValue;

    public TMP_Text horizontalAccuracyValue;

    public TMP_Text DegreeOfLocation;
    public TMP_Text TrueHeadingDegree;

    public TMP_Text North;
    public TMP_Text South;
    public TMP_Text East;
    public TMP_Text West;

    public GameObject CompusObject;
    public GameObject ButtonCompleteTheTask;
    public Button ButtonOkay;
    public Button BButtonCompleteTheTask;
    public ScreenManagerObject ScreenManagerObject;
    public ButtonPopulator ButtonPopulator;

    string questSuccessText = "10 metreden daha yakın";
    string SuccessLine;
    string FailLine;

    string GPSSuccessText = "GPS Durumu: Aktif";
    bool GPSStatusActive=false;
    bool isHorizontalAccuracyBigger=true;
    
    private int ns=0;
    private int ew=0;
    private bool isTimerRunning = false;
    private float AverageLat=0f;

    private List<int> userQuestsDone = new List<int>();
    
    void Start()
    {
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        StartCoroutine(GPSLoc()); 

        Input.gyro.enabled = true;
        Input.compass.enabled = true;
        ButtonCompleteTheTask.SetActive(false);
        BButtonCompleteTheTask.onClick.AddListener(() => FinishingTheQuest());
        ButtonOkay.onClick.AddListener(() => ButtonOkayFunction());

        if (!PlayerPrefs.HasKey("TaskFinalCount"))
        {
            // If not, set an initial value
            int currentInt=PlayerPrefs.GetInt("TaskFinalCount");
            PlayerPrefs.SetInt("TaskFinalCount", 0);
            PlayerPrefs.Save();
            //
            PlayerPrefs.SetString("userQuestsDone", "");
            PlayerPrefs.Save();


        }
    }
    public float GetLatitudeValue(){
        //return latitudeValue.text;
        return Input.location.lastData.latitude;
    }

    public float GetLongitudeValue(){
        //return longitudeValue.text;
        return Input.location.lastData.longitude;
    }


  
    public void DeactivateTheCompleteButtonInstant(){
        ButtonCompleteTheTask.SetActive(false);
    }

    IEnumerator GPSLoc(){
        //check for location service enabled
        Input.location.Start(0.5f,1f);
            yield return new WaitForSeconds(5);
        if (!Input.location.isEnabledByUser)
        {
            GPSStatus.text = "GPS Durumu: Konum bilgisi alınamadı.";
            yield break;
        }

        //start service
        Input.location.Start();

        //wait until the service start
        int maxWait=20;
        while( Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        //Service didnt start in 30 sec
        if (maxWait < 1)
        {
            GPSStatus.text = "GPS Durumu: Zaman Aşımı";
            yield break;
        }

        //Connection failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "GPS Durumu: Konum bilgisi alınamadı.";
            yield break;
        }else
        {
            //Success
            GPSStatus.text = GPSSuccessText;
            InvokeRepeating("UpdateGPSData", 0.5f,1f);
        }
    }//end of GPSLoc

    
    private void UpdateGPSData(){
        if (Input.location.status == LocationServiceStatus.Running)
        {
            //successfuly gets GPS values and it has been set.
            GPSStatus.text = GPSSuccessText;  
            latitudeValue.text= Input.location.lastData.latitude.ToString();
            longitudeValue.text= Input.location.lastData.longitude.ToString();
            horizontalAccuracyValue.text= Input.location.lastData.horizontalAccuracy.ToString();
            GPSStatusActive=true;
            //timestamp.text= Input.location.lastData.timestamp.ToString();
        }else
        {
            //service is stoped
            GPSStatus.text = "GPS Durumu: GPS durdu.";
            GPSStatusActive=false;
            
        }
    } //end of UpdateGPSData
    void Update()
    {
        Input.compass.enabled = true;
        float magneticHeading = Input.compass.magneticHeading;
        float trueHeading = Input.compass.trueHeading;
        
        //rotation
        Quaternion originalRotation = transform.localRotation;
       
        North.transform.rotation = originalRotation;
        South.transform.rotation = originalRotation;
        West.transform.rotation = originalRotation;
        East.transform.rotation = originalRotation;

        CompusObject.transform.rotation = Quaternion.Euler(0f, 0f, magneticHeading);

        //Setting the texts
        DegreeOfLocation.text = magneticHeading.ToString("F1");
        TrueHeadingDegree.text = trueHeading.ToString("F1");
        horizontalAccuracyValue.text= Input.location.lastData.horizontalAccuracy.ToString();

        
        //Setting Compus

        //north and south
        //North
        if ((Input.location.lastData.latitude - PlayerPrefs.GetFloat("Latitude"))<0){
            //get correct value to other side.
            South.text="Güney";
            //close than 10 meter.
            if(CalculateVerticalDistance(Input.location.lastData.latitude, PlayerPrefs.GetFloat("Latitude"))<10f){
                North.text= questSuccessText + " (" + CalculateVerticalDistance(Input.location.lastData.latitude, PlayerPrefs.GetFloat("Latitude")).ToString("F1")+ " m)";
                //ns true. So quest can be done successfuly
                ns=1;
            }else{
                //not close than 10 meter.
                North.text= CalculateVerticalDistance(Input.location.lastData.latitude, PlayerPrefs.GetFloat("Latitude")).ToString("F1") + "\n"+ " metre bu yönde";
                //ns true
                ns=0;
            }
        }
        //South
        else if ((Input.location.lastData.latitude - PlayerPrefs.GetFloat("Latitude"))>0){
            North.text="Kuzey";
            //close than 10 meter
            if(CalculateVerticalDistance(Input.location.lastData.latitude, PlayerPrefs.GetFloat("Latitude"))<10f){
                South.text= questSuccessText + " (" + CalculateVerticalDistance(Input.location.lastData.latitude, PlayerPrefs.GetFloat("Latitude")).ToString("F1")+ " m)";
                //ns true. So quest can be done successfuly
                ns=1;
            }else{
                //not close than 10 meter
                
                South.text= CalculateVerticalDistance(Input.location.lastData.latitude, PlayerPrefs.GetFloat("Latitude")).ToString("F1") + " metre bu yönde";
                //ns true
                ns=0;
            }
            
        }
        //TEST AREA
        //South.text= questSuccessText + " (" + CalculateVerticalDistance(Input.location.lastData.latitude, PlayerPrefs.GetFloat("Latitude")).ToString("F1")+ " m)";
        //South.text= CalculateVerticalDistance(Input.location.lastData.latitude, PlayerPrefs.GetFloat("Latitude")).ToString("F1") + " metre bu yönde";
        
        //East and West
        //East
        if ((Input.location.lastData.longitude - PlayerPrefs.GetFloat("Longitude"))<0){
            West.text="Batı";
            if(CalculateHorizontalDistance(Input.location.lastData.longitude, PlayerPrefs.GetFloat("Longitude"))<10f){
                East.text = questSuccessText + " (" + CalculateHorizontalDistance(Input.location.lastData.longitude, PlayerPrefs.GetFloat("Longitude")).ToString("F1")+ " m)";
                //ew means EastWest. ew true. So quest can be done successfuly
                ew=1;
            }else{
                ew=0;
                East.text= CalculateHorizontalDistance(Input.location.lastData.longitude, PlayerPrefs.GetFloat("Longitude")).ToString("F1") + " metre bu yönde";
            }
        }
        //West
        else if ((Input.location.lastData.longitude - PlayerPrefs.GetFloat("Longitude"))>0){
            East.text="Doğu";
            if(CalculateHorizontalDistance(Input.location.lastData.longitude, PlayerPrefs.GetFloat("Longitude"))<10f){
                West.text = questSuccessText + " (" + CalculateHorizontalDistance(Input.location.lastData.longitude, PlayerPrefs.GetFloat("Longitude")).ToString("F1")+ " m)";
                ew=1; //ew true. So quest can be done successfuly
            }else{
                ew=0;
                West.text= CalculateHorizontalDistance(Input.location.lastData.longitude, PlayerPrefs.GetFloat("Longitude")).ToString("F1") + " metre bu yönde";
            }
        }
        //
        //
        //

        
        //The task/quest completed or not.
        //everthing fine, so activate the complete button.


        if(CheckEverythingIsFineToCompleteTheTask()){
            ScreenManagerObject.TextHorizontalAccuracyWarning.SetActive(false);
            ButtonCompleteTheTask.SetActive(true);
        }else if(CheckEverythingIsFineToCompleteTheTask()==false){
            ButtonCompleteTheTask.SetActive(false);
            if(isHorizontalAccuracyBigger==true){
                ScreenManagerObject.TextHorizontalAccuracyWarning.SetActive(true);
            }else{
                ScreenManagerObject.TextHorizontalAccuracyWarning.SetActive(false);
            }
        }
        
        /*
        if(isTimerRunning==false){        
            if(CheckEverythingIsFineToCompleteTheTask()){
                //activate the complete button
                StartCoroutine(ActivateDeactivateWithDelay(ButtonCompleteTheTask,2f));
            }else{
                //something wrong make sure the complete button deactivated.
                //set ButtonCompleteTheTask.SetActive(false); after 2 seconds.
                if(ButtonCompleteTheTask.activeSelf){
                    //if it's activated you should wait like 2 second before deactivate in anycase
                    StartCoroutine(ActivateDeactivateWithDelay(ButtonCompleteTheTask,2f));
                }else{}
            }
        }else{}
        */

        //
        //
        //


    }//endupdate


    //ButtonActivation without delay Function
    private bool CheckEverythingIsFineToCompleteTheTask(){
        //gps must be working
        //horizontalAccuracy must be in correct range
        //ns(nort-south distance) and ew(east west distance) must be 1(means it is closer then x meter).
        if(GPSStatusActive==true && !float.IsNaN(Input.location.lastData.horizontalAccuracy) && 1f<Input.location.lastData.horizontalAccuracy && Input.location.lastData.horizontalAccuracy<=10f && ns==1 && ew==1){
            isHorizontalAccuracyBigger=false;
            return true;
            
        }else{
            if(Input.location.lastData.horizontalAccuracy>10f){
                isHorizontalAccuracyBigger=true;
            }else{
                isHorizontalAccuracyBigger=false;
            }
            return false;
        }
    }

    //ButtonActivation with delay Function
    private IEnumerator ActivateDeactivateWithDelay(GameObject gameObject,float delayTime){
        //if timer is up, u cant use an other timer.
        if(isTimerRunning==false){
            isTimerRunning=true;
            yield return new WaitForSeconds(delayTime);
            //When the complete button active and  it shoudnt be active
            if(gameObject.activeSelf && CheckEverythingIsFineToCompleteTheTask()==false){
                gameObject.SetActive(false);
            }
            //when the complete button deactive and it should be active
            else if(gameObject.activeSelf == false && CheckEverythingIsFineToCompleteTheTask()==true){
                gameObject.SetActive(true);
            }else{}
            isTimerRunning = false;
        }
        
    }



    //after clicking görevi ver 
    private void FinishingTheQuest(){
  
        ScreenManagerObject.PanelAfterFinishing.SetActive(true);
        ButtonCompleteTheTask.SetActive(false);
        
    }
    private void ButtonOkayFunction(){ //Hepsi tamam. Set everything..


        string userQuestsDone = PlayerPrefs.GetString("userQuestsDone");
        int questNumber=PlayerPrefs.GetInt("questNumber");
        
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

        if(!Array.Exists(numbers, number => number == questNumber)){
        //add to them into string
            userQuestsDone=userQuestsDone+  " " + questNumber;
            PlayerPrefs.SetString("userQuestsDone", userQuestsDone);
            PlayerPrefs.Save();
        }


        //set TextTaskFinalCount
        int currentInt=PlayerPrefs.GetInt("TaskFinalCount");
        currentInt++;
        PlayerPrefs.SetInt("TaskFinalCount", currentInt);
        PlayerPrefs.Save();
        //ScreenManagerObject.SetTextTaskFinalCount(PlayerPrefs.GetInt("TaskFinalCount").ToString);
        ScreenManagerObject.TextTaskFinalCount.text = PlayerPrefs.GetInt("TaskFinalCount").ToString();
        ScreenManagerObject.CurrentTaskNameText.text = "Bir Görev Seç";

        
        //ScreenManagerObject.CurrentTaskDescText.text = userQuestsDone  + "RefreshVariable: " + ScreenManagerObject.refreshVariable;
        
        ScreenManagerObject.CurrentTaskDescText.text = "Listeden bir görev seçerek başla.";
        //Delete all prefabs to get new ones.
        PlayerPrefs.DeleteKey("Title");
        PlayerPrefs.DeleteKey("Description");
        PlayerPrefs.DeleteKey("Latitude");
        PlayerPrefs.DeleteKey("Longitude");
        PlayerPrefs.DeleteKey("questNumber");
        
        
        //Set the screens
        ScreenManagerObject.QuestDetailsCanvas.SetActive(false);
        ScreenManagerObject.PanelAfterFinishing.SetActive(false);
        ScreenManagerObject.ScrollablelistCanvas.SetActive(true);
        ButtonCompleteTheTask.SetActive(false);

        //buttonlist refresh. 1 means the quest list should be refreshed.
        ScreenManagerObject.refreshVariable=1;
        //ButtonPopulator ButtonPopulator = GetComponent<ButtonPopulator>();
        //ButtonPopulator.Once=0;
    }

    //These functions are only for calculating the distance
    float CalculateVerticalDistance(float lat1, float lat2)
    {
        float AverageLat = Mathf.Abs(lat2 + lat1)/2;
        float metersPerDegree = 111132.92f - 559.82f * Mathf.Cos(2f * AverageLat) +
                                   1.175f * Mathf.Cos(4f * AverageLat) - 0.0023f * Mathf.Cos(6f * AverageLat);
        float distance = Mathf.Abs(lat2 - lat1) * metersPerDegree;
        return distance;
    }
    float CalculateHorizontalDistance(float long1, float long2)
    { 
        float equatorialCircumference = 40008000f;
        float distance = Mathf.Abs(long2 - long1) * (equatorialCircumference / 360f)* Mathf.Cos(Mathf.Deg2Rad * AverageLat);
        return distance;
    }

}//end of GPSLocation