using System.Collections;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    public ReelView[] reels;
    public SymbolDatabase symbolDatabase;
    public float reelStopDelay = 0.4f;

    private WinChecker winChecker;
    private bool isSpinning = false;
    public UIManager uiManager;

    private void Awake()
    {
        winChecker = GetComponent<WinChecker>();

        foreach (var reel in reels)
            reel.Initialise(symbolDatabase.symbols);
    }

    public void OnSpinPressed()
    {
        if (isSpinning) return;
        StartCoroutine(SpinRoutine());
    }

    public void OnStopPressed()
    {
        if (!isSpinning) return;
        StopAllCoroutines();
        StartCoroutine(StopAllReels());
    }

    private IEnumerator SpinRoutine()
    {
        uiManager.OnSpinStarted();
        isSpinning = true;
        foreach (var reel in reels)
            reel.Spin();
        yield return new WaitForSeconds(1.5f);
        yield return StopAllReels();
    }

    private IEnumerator StopAllReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            SymbolData target = symbolDatabase.GetRandomSymbol();
            reels[i].Stop(target);
            yield return new WaitForSeconds(reelStopDelay);
        }

        yield return new WaitUntil(() => reels[reels.Length - 1].State == ReelState.Result);

        int rows = reels[0].symbolRenderers.Count;
        int cols = reels.Length;
        SymbolData[,] grid = new SymbolData[rows, cols];

        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                int idx = (reels[c].CurrentIndex + r) % reels[c].symbolStrip.Count;
                grid[r, c] = reels[c].symbolStrip[idx];
            }
        }

        winChecker.Evaluate(grid);

        foreach (var reel in reels)
            reel.ResetToIdle();

        uiManager.OnSpinEnded();
        isSpinning = false;
    }
}