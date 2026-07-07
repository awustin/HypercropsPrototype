using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    [Header("12 hour clock")]
    public GameObject uiComponentHour;
    public GameObject uiComponentMinute;
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

            if (_hour == _resetValue)
            {
                _hour = 0;
                _newDay = true;
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
    public int UpdateUIPeriod = 1;
    private int _hour;
    private int _minute;
    private int _resetValue;
    [SerializeField] private string _time;
    private bool _newDay;

    void Start()
    {
        Initialise(0, 0);
    }

    public void Initialise(int hour, int minute)
    {
        Hour = hour;
        Minute = minute;
        _resetValue = 12;
    }

    public void Tick(out bool isDayTick)
    {
        _newDay = false;
        Minute ++;
        isDayTick = _newDay;
        _time = ToString();
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
        uiComponentHour.GetComponent<TMP_Text>().text = Hour.ToString("D2");
        uiComponentMinute.GetComponent<TMP_Text>().text = Minute.ToString("D2");
    }

    public override string ToString()
    {
        return $"{Hour:D2}:{Minute:D2}";
    }

    private bool IsValidHour(int hour)
    {
        return (hour >= 0) && (hour <= _resetValue);
    }

    private bool IsValidMinute(int minute)
    {
        return (minute >= 0) && (minute <= 60);
    }
}
