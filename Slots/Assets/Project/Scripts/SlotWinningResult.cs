using System.Collections.Generic;

public abstract class SlotWinningResult
{
    public SlotCellOption winningOption = null;
    public float payout = 1;
    public List<SlotReelController> winningReels = new List<SlotReelController>();

    public void SetHighlightVisibility(bool argIsVisible)
    {
        foreach (SlotReelController reel in winningReels)
        {
            reel.SetHighlightVisibility(argIsVisible);
        }
    }

    public abstract string GetDescription();
}

public class SlotWinningLineResult : SlotWinningResult
{
    public SlotWinningLineResult(NumberCell argOption, List<SlotReelController> argWinningReels, float argBetAmount)
    {
        winningOption = argOption;
        winningReels = argWinningReels;
        payout = argBetAmount * argOption.GetPayoutMultiplier(argWinningReels.Count);
    }

    public override string GetDescription()
    {
        return $"{winningReels.Count}x of {winningOption.uniqueID} pays {payout}";
    }
}

public class SlotWinningPrizeResult : SlotWinningResult
{
    public SlotWinningPrizeResult(PrizeCell argOption, SlotReelController winningReel, float argBetAmount)
    {
        winningOption = argOption;
        winningReels = new List<SlotReelController>(){winningReel};
        payout = argBetAmount * argOption.multiplier;
    }

    public override string GetDescription()
    {
        return $"{winningOption.uniqueID} pays {payout}";
    }
}
