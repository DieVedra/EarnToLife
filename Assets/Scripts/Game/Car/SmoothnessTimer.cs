using Cysharp.Threading.Tasks;
using UnityEngine;

public class SmoothnessTimer
{
    public float Value { get; private set; }

    public bool TimerOn { get; private set; }
    public async void StartTimer(float number)
    {
        if (number == 1f || number == -1f)
        {
            TimerOn = true;
            Value = 0f;
            while (TimerOn == true)
            {
                if (Mathf.Abs(Value) < 1f)
                {
                    Value += Time.fixedDeltaTime * number;
                }
                else
                {
                    TimerOn = false;
                    Value = number;
                }
                await UniTask.Yield();
            }
        }
    }

    public void StopTimer()
    {
        TimerOn = false;
    }
}