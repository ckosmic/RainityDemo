using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour {

	public Canvas myCanvas;
	public int activeMenu;

	public GameObject addNewWindow;
	public GameObject editWindow;

	// Use this for initialization
	void Start () {
		HideMenu();
	}

	public void GoToMousePosition() {
		transform.GetChild(activeMenu).gameObject.SetActive(true);
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		transform.position = myCanvas.transform.TransformPoint(pos);
		GetComponent<Image>().color = Color.white;
	}

	public void HideMenu() {
		StartCoroutine("HideMenuDelay");
	}

	public void OpenAddNew() {
		addNewWindow.SetActive(true);
		addNewWindow.GetComponent<AddNew>().ClearFields();
	}

	public void OpenEditIcon() {
		editWindow.SetActive(true);
		editWindow.GetComponent<AddNew>().FillFields();
	}

	public void DeleteIcon() {
		File.Delete(Application.dataPath + "/StreamingAssets/config/config_" + AddNew.editingIcon.programName + ".txt");
		Destroy(AddNew.editingIcon.gameObject);
	}

	IEnumerator HideMenuDelay() {
		yield return new WaitForSeconds(0.1f);
		transform.GetChild(activeMenu).gameObject.SetActive(false);
		GetComponent<Image>().color = Color.clear;
		transform.position = new Vector2(Screen.width + 1000, Screen.height + 1000);
	}
}
