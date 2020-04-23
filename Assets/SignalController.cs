using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SignalController : MonoBehaviour
{
    public Sprite enabledSignal;
    public Sprite signal;
    public AudioSource signalSound;

    private Transform left;
    private Transform right;

    private TrafficSystemVehiclePlayer.SignalType currentType;


    public void updateType(TrafficSystemVehiclePlayer.SignalType type)
    {
        if (type == currentType)
        {
            return;
        }
        currentType = type;

        blink();
    }

    
    Transform blink_object;
    private Transform unblink_object;
    private RepeatExecutor blinkExecutor;

    private void blink()
    {
    
        if (currentType == TrafficSystemVehiclePlayer.SignalType.Left)
        {
            blink_object = left;
            unblink_object = right;
            signalSound.Play();
        }
        else if (currentType == TrafficSystemVehiclePlayer.SignalType.Right)
        {
            blink_object = right;
            unblink_object = left;
            signalSound.Play();
        }
        else
        {
            blink_object = null;
            signalSound.Stop();
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        left = transform.Find("left");
        right = transform.Find("right");
        signalSound.Stop();
        blinkExecutor = new RepeatExecutor((i =>
        {
            if (!blink_object)
            {
                left.GetComponent<Image>().sprite = signal;
                right.GetComponent<Image>().sprite = signal;
                return;
            }

            unblink_object.GetComponent<Image>().sprite = signal;
            if (i % 2 == 0)
            {
                blink_object.GetComponent<Image>().sprite = enabledSignal;
            }
            else
            {
                blink_object.GetComponent<Image>().sprite = signal;
            }
        }), 0.3f);
        StartCoroutine(blinkExecutor.getEnumerator());
    }

    // Update is called once per frame
    void Update()
    {
    }
}