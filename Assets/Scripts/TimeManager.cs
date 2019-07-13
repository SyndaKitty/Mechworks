using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public FloatVariable TickLength;
    public TickInfo TickInfo;
    float timeAccumulation;
    void Awake()
    {
        TickInfo.InterpolatedTime = 0;
        TickInfo.CurrentTick = 0;
    }

    void Update()
    {
        timeAccumulation += Time.deltaTime;

        // If we've ticked
        if (timeAccumulation >= TickLength.Value)
        {
            TickInfo.Ticking = true;

            timeAccumulation %= TickLength.Value;
            TickInfo.CurrentTick ++;
        }
        else
        {
            TickInfo.Ticking = false;
        }

        TickInfo.InterpolatedTime = timeAccumulation / TickLength.Value;
    }
}