using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Slots/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Bank")] 
    public float defaultBankAmount = 100;
    
    [Header("Game Speed")]
    public float spinTime = 2f;
    public float spinFinishTime = .5f;
    public float timeBetweenReels = .05f;

    [Header("Progressives")] 
    public int freeSpinsAwarded = 6;
}
