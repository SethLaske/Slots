using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotCellController : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [HideInInspector]
    public SlotCellOption cellOption;
    public GameObject freezeIcon;
    
    /*public int shownID
    {
         (cellOption == null) ? -1 : cellOption.uniqueID; 
    }*/

    public void AssignCellOption(SlotCellOption argNewOption)
    {
        icon.sprite = argNewOption.image;
        cellOption = argNewOption;

        if (argNewOption is PrizeCell)
        {
            freezeIcon.SetActive(true);
        }
        else
        {
            freezeIcon.SetActive(false);
        }
    }

    public void SetFreezeVisible(bool argVisible)
    {
        freezeIcon.SetActive(argVisible);
    }
}
