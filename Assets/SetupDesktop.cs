using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SetupDesktop : MonoBehaviour {

	public GameObject iconPrefab;
	public ContextMenu contextMenu;

	// Use this for initialization
	void Start () {
		DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/StreamingAssets/config");
		FileInfo[] fileInfo = info.GetFiles();
		foreach (FileInfo f in fileInfo) {
			if (f.Extension == ".txt") {
				GameObject icon = Instantiate(iconPrefab);
				object[] iconInfo = ParseFile(f.FullName);
				IconScript iconScript = icon.GetComponent<IconScript>();
				iconScript.filePath = iconInfo[0].ToString();
				iconScript.contextMenu = contextMenu;
				icon.transform.position = new Vector3((float)iconInfo[1], (float)iconInfo[2], 0);
				iconScript.LoadProgramIcon();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public object[] ParseFile(string configPath) {
		string text = File.ReadAllText(configPath);

		char[] separators = { ',', ';', '|' };
		string[] strValues = text.Split(separators);

		List<float> floatValues = new List<float>();
		foreach (string str in strValues) {
			float val = 0;
			if (float.TryParse(str, out val))
				floatValues.Add(val);
		}

		object[] retn = new object[3];
		retn[0] = strValues[0];
		retn[1] = floatValues[0];
		retn[2] = floatValues[1];

		return retn;
	}
}
