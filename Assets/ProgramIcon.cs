using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System.Diagnostics;
using System.IO;

public class ProgramIcon : MonoBehaviour {

	public IconScript iconScript;
	public Renderer borderRenderer;

	void Start() {
		LoadIcon();
	}

	public void LoadIcon() {
		string filePath = iconScript.filePath;
		string fileName = Path.GetFileNameWithoutExtension(filePath);
		if (!File.Exists(Application.dataPath + "/StreamingAssets/icons/" + fileName + ".png")) {
			UnityEngine.Debug.Log("\"" + fileName + "\" icon doesn't exist. Creating one now...");
			string path = Application.dataPath + "/StreamingAssets/JumboIcon.exe";
			Process proc = new Process();
			proc.StartInfo.WorkingDirectory = Application.dataPath + "/StreamingAssets";
			proc.StartInfo.FileName = "cmd.exe";
			proc.StartInfo.Arguments = "/c JumboIcon.exe \"" + filePath + "\" \"" + Application.dataPath + "/StreamingAssets/\"";
			proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			proc.Start();
			proc.WaitForExit();

			PlayerPrefs.SetString(fileName + "_config_name", fileName);
		}

		StartCoroutine("LoadProgramIcon", Application.dataPath + "/StreamingAssets/icons/" + fileName + ".png");
	}

	IEnumerator LoadProgramIcon(string path) {
		UnityEngine.Debug.Log("file://" + path);
		WWW www = new WWW("file://" + path);
		yield return www;

		Texture2D tex = new Texture2D(www.texture.width, www.texture.height, www.texture.format, false);
		www.LoadImageIntoTexture(tex);
		tex.wrapMode = TextureWrapMode.Clamp;
		GetComponent<Renderer>().material.mainTexture = tex;
		Color32 borderColor = AverageColorFromTexture(tex);
		borderRenderer.material.SetColor("_Color", borderColor);
	}

	Color32 AverageColorFromTexture(Texture2D tex) {
		Color32[] texColors = tex.GetPixels32();

		int total = texColors.Length;

		float r = 0;
		float g = 0;
		float b = 0;

		for (int i = 0; i < total; i++) {
			r += texColors[i].r;
			g += texColors[i].g;
			b += texColors[i].b;
		}

		return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 0);
	}
}