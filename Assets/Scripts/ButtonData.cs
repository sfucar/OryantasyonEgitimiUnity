using UnityEngine;

[CreateAssetMenu(fileName = "ButtonData", menuName = "ButtonData/Create")]
public class ButtonData : ScriptableObject
{
    public string title;
    public string description;
    public int questNumber;
    
    public float latitude; 
    public float longitude; 
    
}
