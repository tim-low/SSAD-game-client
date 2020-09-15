using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using SuperSad.Gameplay;

public class CountdownTimer : MonoBehaviour {
    
    [SerializeField]
    private Text timerText;

    [SerializeField]
    private UnityEvent triggerAfterCountdown;

    private bool runTimer = false;
    private float tempDuration = 0f;

    private bool toTriggerEvent = false;

    public void StartTimer(int duration, bool triggerEvent)
    {
        // Initialize timer values
        gameObject.SetActive(true);
        runTimer = true;
        tempDuration = duration;    // value to count down
        toTriggerEvent = triggerEvent;  // set whether to trigger the event or not

        Debug.Log("Start Timer: " + duration + " seconds");
    }

    void Update()
    {
        if (runTimer)  // timer is active 
        {
            // Update countdown timer
            tempDuration -= Time.deltaTime;

            // Update timer string
            timerText.text = ((int)tempDuration).ToString();

            if (tempDuration <= 0f) // time's up
            {
                runTimer = false;

                // Execute effect after countdown
                if (toTriggerEvent)
                    triggerAfterCountdown.Invoke();
            }
        }
    }
}
