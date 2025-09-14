using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotCellController : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    public SlotCellOption cellOption;

    /*public int shownID
    {
         (cellOption == null) ? -1 : cellOption.uniqueID; 
    }*/

    public void AssignCellOption(SlotCellOption argNewOption)
    {
        icon.sprite = argNewOption.image;
        cellOption = argNewOption;
    }
}
