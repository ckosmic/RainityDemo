using Crosstales.FB;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNew : MonoBehaviour {

	public string iconPath;

	public GameObject iconPrefab;
	public ContextMenu contextMenu;

	public Text pathText;
	public InputField field;

	public static IconScript editingIcon;

	public void ClearFields() {
		iconPath = "";
		pathText.text = "No file chosen";
		field.text = "";
	}

	public void FillFields() {
		iconPath = editingIcon.filePath;
		pathText.text = editingIcon.filePath;
		field.text = editingIcon.nameText.text;
	}

	public void OpenFileBrowser() {
		TransparentWindow.forceInBack = false;
		string extensions = "";
		string file = FileBrowser.OpenSingleFile("Choose a file", "", extensions);
		SetIconPath(file);
		TransparentWindow.forceInBack = true;
	}

	public void SetIconPath(string path) {
		iconPath = path;
		pathText.text = path;
		if (field.text.Length == 0) {
			string fileName = Path.GetFileNameWithoutExtension(path);
			field.text = fileName;
		}
	}

	public void CreateNewIcon() {
		if (iconPath.Length > 0) {
			GameObject icon = Instantiate(iconPrefab);
			icon.transform.position = Vector3.zero;
			IconScript iconScript = icon.GetComponent<IconScript>();
			iconScript.filePath = iconPath;
			iconScript.contextMenu = contextMenu;
			iconScript.newName = field.text;

			gameObject.SetActive(false);
		}
	}

	public void EditExistingIcon() {
		if (iconPath.Length > 0) {
			if (editingIcon.filePath != iconPath)
				File.Delete(Application.dataPath + "/StreamingAssets/config/config_" + editingIcon.programName + ".txt");
			editingIcon.filePath = iconPath;
			editingIcon.programName = Path.GetFileNameWithoutExtension(iconPath);
			editingIcon.newName = field.text;
			editingIcon.LoadProgramIcon();

			gameObject.SetActive(false);
		}
	}
}
