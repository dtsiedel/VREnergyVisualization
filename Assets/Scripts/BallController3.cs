using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BallController3 : MonoBehaviour {

	Dictionary<int, string> buildings;
	GameObject canvas;
	string serverData = "Jordan"; 
	string usageData = "NA";

	// Use this for initialization
	void Start () 
	{
		canvas = GameObject.Find("Canvas");
		StartCoroutine (GetText ());
		StartCoroutine (GetUsagePerSquareFoot ());
	}
		

	// Update is called once per frame
	void Update ()
	{
		string thisBuilding = "Jordan";

		Text[] textValue = canvas.GetComponentsInChildren<Text> ();
		textValue [0].text = "Building: " + thisBuilding + relevantData(thisBuilding) + relevantUsage(thisBuilding);

		if (Input.GetMouseButtonDown (0)) {
			SceneManager.LoadScene("Scene0");
		}
			
	}

	//returns the data from the JSON string in serverData,
	//but only with the data we actually care about
	string relevantData(string thisBuilding)
	{
		JSONObject serverJSON = new JSONObject (serverData);
		JSONObject buildingRelevant = serverJSON[thisBuilding];

		JSONObject DPU = buildingRelevant ["DPU"];
		JSONObject DEU = buildingRelevant ["DEU"];
		JSONObject WU = buildingRelevant ["WU"];

		return "\nDPU: " + DPU.ToString() + " kWH\nDEU: " + DEU.ToString() + " kW\nWU: " + WU.ToString() + " W";
	}

	//extracts the data from the JSON string *usageData* into
	//three strings, to then be put on the end of our data
	string relevantUsage(string thisBuilding)
	{
		JSONObject usageJSON = new JSONObject (usageData);
		JSONObject buildingRelevant = usageJSON [thisBuilding];

		JSONObject DPU = buildingRelevant ["DPU"];
		JSONObject DEU = buildingRelevant ["DEU"];
		JSONObject WU = buildingRelevant ["WU"];

		return "\nDPU/sq ft: " + DPU.ToString() + " \nDEU/sq ft: " + DEU.ToString() + " \nWU/sq ft: " + WU.ToString() + " ";
	}

	//the function that gets text from the server
	IEnumerator GetText() {
		string server = "http://ec2-54-186-143-244.us-west-2.compute.amazonaws.com/json/power_data_current.json";
		using(UnityWebRequest www = UnityWebRequest.Get(server)) {
			yield return www.Send();


			if(www.isError) {
				Application.Quit ();
			}
			else {
				serverData = www.downloadHandler.text;
			}

		}
	}

	//gets the average data from the server,
	//and also returns that value so that it can be
	//put into the summary
	IEnumerator GetUsagePerSquareFoot()
	{
		string server = "http://ec2-54-186-143-244.us-west-2.compute.amazonaws.com/file/per-foot-squared.json";
		using(UnityWebRequest www = UnityWebRequest.Get(server)) {
			yield return www.Send();

			if(www.isError) {
				Application.Quit ();
			}
			else {
				usageData = www.downloadHandler.text;
			}

		}
	}
		

}
