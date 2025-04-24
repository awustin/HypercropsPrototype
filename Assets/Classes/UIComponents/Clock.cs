using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public int Hour
    {
        get { return _hour; }
        set
        {
            if (!IsValidHour(value))
            {
                return;
            }

            _hour = value;

            if (_hour == HoursInDay)
            {
                _hour = 0;
                // New day!
            }
        }
    }
    public int Minute
    {
        get { return _minute; }
        set
        {
            if (!IsValidMinute(value))
            {
                return;
            }

            _minute = value;

            if (_minute == 60)
            {
                _minute = 0;
                Hour ++;
            }

            if (_minute % UpdateUIPeriod == 0)
            {
                UpdateUI();
            }
        }
    }
    public int HoursInDay;
    public int UpdateUIPeriod = 1;
    public GameObject UIClock;
    private int _hour;
    private int _minute;

    void Start()
    {
        Initialise(0, 0);
    }

    public void Initialise(int hour, int minute)
    {
        Hour = hour;
        Minute = minute;
        HoursInDay = 12;
    }

    public void Tick()
    {
        Minute ++;
    }

    public void SetTo(int hour, int minute)
    {
        if (!IsValidHour(hour) || !IsValidMinute(minute))
        {
            Debug.LogWarning("Invalid time!");
            return;
        }

        Hour = hour;
        Minute = minute;
    }

    public void UpdateUI()
    {
        UIClock.transform.Find("HoursValue").gameObject.GetComponent<TMP_Text>().text = Hour.ToString("D2");
        UIClock.transform.Find("MinutesValue").gameObject.GetComponent<TMP_Text>().text = Minute.ToString("D2");
    }

    public override string ToString()
    {
        return $"{Hour:D2}:{Minute:D2}";
    }

    private bool IsValidHour(int hour)
    {
        return (hour >= 0) && (hour <= HoursInDay);
    }

    private bool IsValidMinute(int minute)
    {
        return (minute >= 0) && (minute <= 60);
    }
}
