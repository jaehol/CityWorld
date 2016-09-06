using UnityEngine;
using System.Collections;

public class GoogleMap : MonoBehaviour
{
	public enum MapType
	{
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}
	public bool loadOnStart = true;
	public bool autoLocateCenter = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 14;
	public MapType mapType;
	public int size = 2000;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;

	void Start() {
		if(loadOnStart) Refresh();	
	}

	public void Refresh() {
		if(autoLocateCenter && (markers.Length == 0 && paths.Length == 0)) {
			Debug.LogError("Auto Center will only work if paths or markers are used.");	
		}
		StartCoroutine(_Refresh());
	}

	IEnumerator _Refresh ()
	{
		// Start service before querying location
		Input.location.Start();
		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
			yield break;
		}
		else
		{
			      //  centerLocation.latitude = 34.0224f;
			      //  centerLocation.longitude = -118.2851f;
			centerLocation.latitude = Input.location.lastData.latitude;
			centerLocation.longitude = Input.location.lastData.longitude;
			// Access granted and location value could be retrieved
			//            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
		}

		var url = "http://maps.googleapis.com/maps/api/staticmap";
		var qs = "";
		//		if (!autoLocateCenter) {
		//			if (centerLocation.address != "")
		//				qs += "center=" + HTTP.URL.Encode (centerLocation.address);
		//			else {
		qs += "center=" + HTTP.URL.Encode (string.Format ("{0},{1}", centerLocation.latitude, centerLocation.longitude));               
		//            }

		qs += "&zoom=" + zoom.ToString ();
		//		}
		qs += "&size=" + HTTP.URL.Encode (string.Format ("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=" + mapType.ToString ().ToLower ();
		qs += "&key=" + "AIzaSyCTvLKERhJZtbvHc2xRBQaJwXQDMbblPxI";
		var usingSensor = false;
		#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
		#endif
		qs += "&sensor=" + (usingSensor ? "true" : "false");

		foreach (var i in markers) {
			qs += "&markers=" + string.Format ("size:{0}|color:{1}|label:{2}", i.size.ToString ().ToLower (), i.color, i.label);
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + HTTP.URL.Encode (loc.address);
				else
					qs += "|" + HTTP.URL.Encode (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}

		foreach (var i in paths) {
			qs += "&path=" + string.Format ("weight:{0}|color:{1}", i.weight, i.color);
			if(i.fill) qs += "|fillcolor:" + i.fillColor;
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + HTTP.URL.Encode (loc.address);
				else
					qs += "|" + HTTP.URL.Encode (string.Format ("{0},{1}", loc.latitude, loc.longitude));
			}
		}


		var req = new HTTP.Request ("GET", url + "?" + qs, true);
		req.Send ();
		while (!req.isDone)
			yield return null;
		if (req.exception == null) {
			var tex = new Texture2D (size, size);
			tex.LoadImage (req.response.Bytes);
			Renderer renderer = GetComponent<Renderer>();
			renderer.material.mainTexture = tex;
		}
	}

	void OnApplicationQuit()
	{
		print("End");
		Input.location.Stop();

	}
}

public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;

}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;	
}