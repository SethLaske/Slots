using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReelController : MonoBehaviour
{
    public float normalMovementSpeed = 1000;
    private float currentMovementSpeed = 1000;
    public float spaceBetweenCells = 100;
    
    public bool isSpinning;

    public int selectedCellIndex = 0;
    
    public Transform reel = null;
    public List<SlotCellController> cells = new List<SlotCellController>();
    public Transform upperLimit = null;
    public Transform lowerLimit = null;
    private SlotCellController lastCell = null;

    private float timeTillStop = 0;

    public SlotCellConfig cellConfig = null;

    public SlotCellOption GetSelectedCellOption()
    {
        if (isSpinning || selectedCellIndex < 0 || selectedCellIndex >= cells.Count)
        {
            return null;
        }

        return cells[selectedCellIndex].cellOption;
    }

    private void Awake()
    {
        lastCell = cells[^1];
        currentMovementSpeed = normalMovementSpeed;
        
        foreach (SlotCellController cellController in cells)
        {
            cellController.AssignCellOption(cellConfig.GetRandomSlotCellWithoutRepeats(cells));
        }
    }

    private void Update()
    {
        OnUpdate(Time.deltaTime);
    }

    public void OnUpdate(float argDelta)
    {
        if (isSpinning == false)
        {
            return;
        }
        
        MoveReel(currentMovementSpeed * argDelta);
    }

    private void MoveReel(float argDistance)
    {
        reel.position += argDistance * Vector3.down;

        foreach (SlotCellController cell in cells)
        {
            if (cell.transform.position.y <= lowerLimit.position.y)
            {
                cell.transform.localPosition = lastCell.transform.localPosition + (spaceBetweenCells * Vector3.up);
                lastCell = cell;
                selectedCellIndex = (selectedCellIndex + 1) % cells.Count;
                cell.AssignCellOption(cellConfig.GetRandomSlotCell());
            }
        }
    }

    public void StartSpinning()
    {
        isSpinning = true;
        currentMovementSpeed = normalMovementSpeed;
    }

    
    public void StopSpinning(float timeTillFinalCell)
    {
        StartCoroutine(StopRoutine(timeTillFinalCell));
    }
    
    private IEnumerator StopRoutine(float totalTime)
    {
        float spinTime = Mathf.Max(0, totalTime - .5f);
        float timer = 0f;
        
        while (timer < spinTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        float yPos = reel.localPosition.y % spaceBetweenCells;
        float yDistanceRemaining = spaceBetweenCells - Mathf.Abs(yPos);

        currentMovementSpeed = yDistanceRemaining / (totalTime - timer);

        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            yield return null;    
        }
        
        isSpinning = false;

        float modulus = Mathf.Abs(reel.localPosition.y % spaceBetweenCells);

        if (modulus < Mathf.Abs(spaceBetweenCells - modulus))
        {
            MoveReel(-1 * modulus);
        }
        else
        {
            MoveReel((spaceBetweenCells - modulus));
        }

        OnSpinningFinished();
    }

    private void OnSpinningFinished()
    {
        ResetAtCurrentPosition();
        currentMovementSpeed = normalMovementSpeed;

        SlotGameController.instance.OnReelStoppedSpinning();
    }

    private void ResetAtCurrentPosition()
    {
        float desiredPosition = reel.localPosition.y % (spaceBetweenCells * cells.Count);
        float change = desiredPosition - reel.localPosition.y;
        reel.localPosition = new Vector3(reel.localPosition.x, desiredPosition, reel.localPosition.z);
        foreach (SlotCellController cell in cells)
        {
            cell.transform.localPosition -= Vector3.up * change;
        }

    }
}
