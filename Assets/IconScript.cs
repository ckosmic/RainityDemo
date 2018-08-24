using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Diagnostics;

public class IconScript : MonoBehaviour {

	public string filePath;

	public TextMeshPro nameText;
	public GameObject bubble;
	public ProgramIcon programIcon;
	public ContextMenu contextMenu;

	public Texture2D cursor_pointer;

	public string newName;

	[System.NonSerialized]
	public bool mouseOver = false;

	public string programName;

	Vector3 screenPoint;
	Vector3 offset;

	// Use this for initialization
	void Start() {
		programName = Path.GetFileNameWithoutExtension(filePath);
		string myName = PlayerPrefs.GetString(programName + "_config_name", programName);
		SetBubbleText(myName);
	}

	// Update is called once per frame
	void Update() {
		if (mouseOver) {
			bubble.transform.localPosition = Vector3.Lerp(bubble.transform.localPosition, new Vector3(0, 0.81f, -1), Time.deltaTime * 8);
			bubble.transform.localScale = Vector3.Lerp(bubble.transform.localScale, Vector3.one*100, Time.deltaTime * 10);
		} else {
			bubble.transform.localPosition = Vector3.Lerp(bubble.transform.localPosition, new Vector3(0, 0, -1), Time.deltaTime * 8);
			bubble.transform.localScale = Vector3.Lerp(bubble.transform.localScale, Vector3.zero, Time.deltaTime * 10);
		}

		if (newName.Length > 0) {
			PlayerPrefs.SetString(programName + "_config_name", newName);
			SetBubbleText(newName);
			newName = "";
			SaveConfig();
		}
	}

	void OnMouseDown() {
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag() {
		if (!Manager.iconsLocked) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			if (curPosition != transform.position) mouseOver = false;
			curPosition.z = 0;
			transform.position = curPosition;
		}
	}

	void OnMouseEnter() {
		mouseOver = true;
		ScrollBG.mouseOverIcon = true;
		Cursor.SetCursor(cursor_pointer, Vector2.zero, CursorMode.Auto);
	}

	void OnMouseOver() {
		if (Input.GetMouseButtonDown(1)) {
			AddNew.editingIcon = GetComponent<IconScript>();
			contextMenu.activeMenu = 0;
			contextMenu.GoToMousePosition();
		}
	}

	void OnMouseExit() {
		ScrollBG.mouseOverIcon = false;
		mouseOver = false;
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

	void OnMouseUp() {
		SaveConfig();
	}

	void OnMouseUpAsButton() {
		if (File.Exists(filePath) && mouseOver) {
			Process proc = new Process();
			proc.StartInfo.FileName = filePath;
			proc.Start();
			SaveConfig();
		}
	}

	void SetBubbleText(string myName) {
		SkinnedMeshRenderer bubbleRenderer = bubble.GetComponent<SkinnedMeshRenderer>();
		nameText.text = myName;
		bubbleRenderer.SetBlendShapeWeight(0, (nameText.preferredWidth - 12) * 12);
	}

	public void LoadProgramIcon() {
		programIcon.LoadIcon();
	}

	public void SaveConfig() {
		if (!Directory.Exists(Application.dataPath + "/StreamingAssets/config")) Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/config");
		string output = "";
		output += filePath + ";\n";
		output += transform.position.x + ";\n";
		output += transform.position.y + ";\n";

		File.WriteAllText(Application.dataPath + "/StreamingAssets/config/config_" + programName + ".txt", output);
	}
}
