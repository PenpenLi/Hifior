using System.Collections.Generic;
using UnityEngine;

public class Sequence : MonoBehaviour
{
    private bool bRunning;
    private List<SequenceEvent> Events;
    private int CurrentSequenceIndex;
    public bool IsFinish(int Index)
    {
        return CurrentSequenceIndex > Index;
    }
    public bool IsRunning(int Index)
    {
        return CurrentSequenceIndex == Index;
    }
    public Sequence(params SequenceEvent[] SequencesEvents)
    {
        Events = new List<SequenceEvent>();
        Events.AddRange(SequencesEvents);
        bRunning = false;
    }
    public bool StartSequence()
    {
        if (Events.Count == 0)
        {
            return false;
        }
        bRunning = true;
        return true;
    }
    public void AbortSequence()
    {
        bRunning = false;
    }
    public void Update()
    {

    }
}
