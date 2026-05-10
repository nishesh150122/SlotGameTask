using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReelView : MonoBehaviour
{
    public List<SpriteRenderer> symbolRenderers;
    public List<SymbolData> symbolStrip = new List<SymbolData>();
    public float spinSpeed = 15f;
    public float snapDuration = 0.3f;

    public ReelState State { get; private set; } = ReelState.Idle;
    public SymbolData LandedSymbol { get; private set; }
    public event Action OnReelStopped;

    private int currentIndex = 0;
    private Coroutine spinCoroutine;
    private SymbolData targetSymbol;
    public int CurrentIndex => currentIndex;


    public void Initialise(List<SymbolData> strip)
    {
        symbolStrip = strip;
        currentIndex = 0;
        ShowSymbols(currentIndex);
        State = ReelState.Idle;
    }

    public void Spin()
    {
        if (State != ReelState.Idle) return;
        State = ReelState.Spinning;
        spinCoroutine = StartCoroutine(SpinLoop());
    }

    public void Stop(SymbolData target)
    {
        if (State != ReelState.Spinning) return;
        targetSymbol = target;
        State = ReelState.Stopping;
    }

    private IEnumerator SpinLoop()
    {
        float timer = 0f;
        float interval = 1f / spinSpeed;

        while (State == ReelState.Spinning)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer -= interval;
                AdvanceStrip();
            }
            yield return null;
        }

        yield return SnapToTarget();
    }

    private void AdvanceStrip()
    {
        currentIndex = (currentIndex + 1) % symbolStrip.Count;
        ShowSymbols(currentIndex);
    }

    private IEnumerator SnapToTarget()
    {
        int targetIndex = FindNearestIndex(targetSymbol);

        yield return transform
            .DOPunchPosition(Vector3.down * 0.1f, snapDuration, 1, 0.5f)
            .WaitForCompletion();

        currentIndex = targetIndex;
        ShowSymbols(currentIndex);

        LandedSymbol = targetSymbol;
        State = ReelState.Result;
        OnReelStopped?.Invoke();
    }

    private int FindNearestIndex(SymbolData target)
    {
        for (int i = 1; i <= symbolStrip.Count; i++)
        {
            int idx = (currentIndex + i) % symbolStrip.Count;
            if (symbolStrip[idx] == target)
                return idx;
        }
        return currentIndex;
    }

    private void ShowSymbols(int startIndex)
    {
        if (symbolStrip == null || symbolStrip.Count == 0) return;
        for (int i = 0; i < symbolRenderers.Count; i++)
        {
            int idx = (startIndex + i) % symbolStrip.Count;
            symbolRenderers[i].sprite = symbolStrip[idx].sprite;
        }
    }

    public void ResetToIdle()
    {
        State = ReelState.Idle;
        LandedSymbol = null;
    }
}
