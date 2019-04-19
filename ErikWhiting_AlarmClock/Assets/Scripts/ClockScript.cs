using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Digital_ClockLib;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    private Digital_ClockLib.Classes.Time Time = new Digital_ClockLib.Classes.Time(1, 50);
    private Digital_ClockLib.Classes.Time Alarm = new Digital_ClockLib.Classes.Time(2, 0);
    private TimeManager TimeManager = new TimeManager();
    private Digital_ClockLib.Classes.Display display = new Digital_ClockLib.Classes.Display();
    private ModeManager ModeManager = new ModeManager();

    public Text HoursUI, MinuteUI, AlarmUI;
    public Button CurrentMinUp, ShowAlarm, SetAlarm, AlarmOff, ShowTime, SnoozeButton;
    public Image Background;

    public string CurrentHour { get; set; }
    public string CurrentMin { get; set; }
    public string AlarmHour { get; set; }
    public string AlarmMin { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ModeManager.SetMode(Mode.SET_TIME);
        CurrentHour = Time.GetHour().ToString();
        if (Time.GetMinute() <= 9)
        {
            CurrentMin = "0" + Time.GetMinute().ToString();
        }
        else
        {
            CurrentMin = Time.GetMinute().ToString();
        }
        
        AlarmHour = Alarm.GetHour().ToString();
        AlarmMin = Alarm.GetMinute().ToString();
        AlarmUI.text = "";

        AlarmOff.onClick.AddListener(TurnAlarmOff);
        ShowAlarm.onClick.AddListener(ShowAlarmFunction);
        ShowTime.onClick.AddListener(ShowTimeFunction);
        CurrentMinUp.onClick.AddListener(MinutePlusOne);
        SetAlarm.onClick.AddListener(SetAlarmFunction);
        SnoozeButton.onClick.AddListener(Snooze);

        StartCoroutine("IncreaseMinute");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowAlarmFunction()
    {
        AlarmUI.text = "Alarm: ";
        HoursUI.text = Alarm.GetHour().ToString() ;
        if (Alarm.GetMinute() <= 9)
        {
            MinuteUI.text = "0" + Alarm.GetMinute().ToString();
        }
        else
        {
            MinuteUI.text = Alarm.GetMinute().ToString();
        }
    }
    void ShowTimeFunction()
    {
        ModeManager.SetMode(Mode.SET_TIME);
        AlarmUI.text = "";
        HoursUI.text = Time.GetHour().ToString();
        if (Time.GetMinute() <= 9)
        {
            MinuteUI.text = "0" + Time.GetMinute().ToString();
        } else
        {
            MinuteUI.text = Time.GetMinute().ToString();
        }
    }

    void SetAlarmFunction()
    {
        ModeManager.SetMode(Mode.SET_ALARM);
        AlarmUI.text = "Alarm: ";
        HoursUI.text = Alarm.GetHour().ToString();
        MinuteUI.text = Alarm.GetMinute().ToString();
    }

    void TurnAlarmOff()
    {
        ModeManager.SetMode(Mode.SET_TIME);
        Background.color = new Color32(0, 0, 0, 255);
    }

    void Snooze()
    {
        TurnAlarmOff();
        for (int i = 1; i <= 10; i++)
        {
            Alarm.IncrementMinute();
        }
    }

    IEnumerator IncreaseMinute()
    {
        for (; ;)
        {
            Time.IncrementMinute();
            AlarmUI.text = "";
            CurrentHour = Time.GetHour().ToString();
            if (Time.GetMinute() <= 9)
            {
                CurrentMin = "0";
                CurrentMin += Time.GetMinute().ToString();
            } else
            {
                CurrentMin = Time.GetMinute().ToString();
            }
            if (Time.Equate(Alarm))
            {
                DoAlarm();
            }

            if (ModeManager.GetMode() != Mode.SET_ALARM)
            {
                HoursUI.text = CurrentHour;
                MinuteUI.text = CurrentMin;
            }
            
            yield return new WaitForSeconds(60f);
        }
    }

    void MinutePlusOne()
    {
        if (ModeManager.GetMode() == Mode.SET_TIME)
        {
            Time.IncrementMinute();
            if (Time.GetMinute() <= 9)
            {
                CurrentMin = "0";
                CurrentMin += Time.GetMinute().ToString();
            }
            else
            {
                CurrentMin = Time.GetMinute().ToString();
            }
            if (Time.Equate(Alarm))
            {
                DoAlarm();
            }

            ShowTimeFunction();
        }
        else if (ModeManager.GetMode() == Mode.SET_ALARM)
        {
            Alarm.IncrementMinute();
            AlarmUI.text = "Alarm: ";
            HoursUI.text = Alarm.GetHour().ToString();
            if (Alarm.GetMinute() <= 9)
            {
                MinuteUI.text = "0" + Alarm.GetMinute().ToString();
            }
            else
            {
                MinuteUI.text = Alarm.GetMinute().ToString();
            }
        }
        

    }

    void DoAlarm()
    {
        ModeManager.SetMode(Mode.ALARM_ON);
        Background.color = new Color32(100, 20, 20, 255);
    }
}
