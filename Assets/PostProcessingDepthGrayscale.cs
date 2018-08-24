using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessingDepthGrayscale : MonoBehaviour {

	public Material mat;

	// Use this for initialization
	void Start () {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit(source, destination, mat);
	}
}
