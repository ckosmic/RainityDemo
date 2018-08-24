using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockAndDate : MonoBehaviour {

	public Text clock;
	public Text date;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int hour = DateTime.Now.Hour;
		string am = "AM";
		if (hour > 11) {
			am = "PM";
			hour -= 12;
		}
		if (hour == 0) hour = 12;
		string minute = DateTime.Now.Minute.ToString();
		if (minute.Length == 1) minute = "0" + minute;

		clock.text = hour + ":" + minute + " " + am;

		string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
		string day = DateTime.Now.DayOfWeek.ToString();
		string month = months[DateTime.Now.Month-1];
		int thedate = DateTime.Now.Day;
		date.text = day + ", " + month + " " + thedate;
	}
}
