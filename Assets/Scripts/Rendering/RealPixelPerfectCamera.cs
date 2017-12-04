using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RealPixelPerfectCamera : MonoBehaviour {

	[SerializeField] int _pixelHeight = 180;
	[SerializeField] Color _edgeColor = Color.black;
	[SerializeField] Shader _copyShader;
	[SerializeField] Shader _singleColorShader;

	private Camera _thisCamera;
	private Camera _pixelCamera;
	private RenderTexture _renderTexture;
	private Material _copyMaterial;
	private Material _singleColorMaterial;
	private int _pixelsWidth;	
	private Vector2 _copyScale;
	private Vector2 _offset;
	
	private void Awake() {

		_copyShader = Shader.Find("Custom/SimpleCopy");
		_singleColorShader = Shader.Find("Custom/SingleColor");
	}

	private void Start() {

		if (!Application.isPlaying) {
			return;
		}

		_copyMaterial = new Material(_copyShader);
		_singleColorMaterial = new Material(_singleColorShader);
		_singleColorMaterial.SetColor("_Color", _edgeColor);

		_thisCamera = GetComponent<Camera>();
		
		var pixelCameraGO = new GameObject("PixelCamera");
		pixelCameraGO.transform.parent = transform;
		_pixelCamera = pixelCameraGO.AddComponent<Camera>();
		_pixelCamera.CopyFrom(_thisCamera);
		_pixelCamera.enabled = false;

		var _pixelWidth = Mathf.RoundToInt(_pixelHeight * 16.0f / 9.0f);
		var _renderTexture = new RenderTexture(_pixelWidth, _pixelHeight, 24, RenderTextureFormat.ARGB32);
		_renderTexture.filterMode = FilterMode.Point;
		_renderTexture.wrapMode = TextureWrapMode.Clamp;
		_pixelCamera.targetTexture = _renderTexture;

		// This camera doesn't render anything.
		_thisCamera.cullingMask = 0;

		var w = 0;
		var h = 0;
		while (w + _pixelWidth <= _thisCamera.pixelWidth && h + _pixelHeight <= _thisCamera.pixelHeight) {
			
			w += _pixelWidth;
			h += _pixelHeight;
		}			

		_copyScale = new Vector2((float)w / _thisCamera.pixelWidth, (float)h / _thisCamera.pixelHeight);
		_offset = new Vector2(0.5f * (1.0f - _copyScale.x), 0.5f * (1.0f - _copyScale.y));		
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst) {

		if (Application.isPlaying) {
			_pixelCamera.Render();
			Graphics.SetRenderTarget(dst);	
			if (_copyScale.x < 1.0f || _copyScale.y < 1.0f) {
				DrawFullscreenQuad(_singleColorMaterial);
			}
			DrawTexture(_pixelCamera.targetTexture, _offset.x, _offset.y, _copyScale.x, _copyScale.y, _copyMaterial, 0.0f, 0.0f, 1.0f, 1.0f);
		}
		else {
			Graphics.Blit(src, dst);
		}
	}

    private void DrawTexture(Texture texture, float x, float y, float w, float h, Material mat, float sx = 0.0f, float sy = 0.0f, float sw = 1.0f, float sh = 1.0f) {

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.mainTexture = texture;
        mat.SetPass(0);

        GL.Begin(GL.QUADS);
        GL.TexCoord2(sx, sy);
        GL.Vertex3(x, y, 0.0f);
        GL.TexCoord2(sx + sw, sy);
        GL.Vertex3(x + w, y, 0.0f);
        GL.TexCoord2(sx + sw, sy + sw);
        GL.Vertex3(x + w, y + h, 0.0f);
        GL.TexCoord2(sx, sy + sw);
        GL.Vertex3(x, y + h, 0.0f);
        GL.End();

        GL.PopMatrix();
    }	

	private void DrawFullscreenQuad(Material mat) {

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);
        GL.Vertex3(0.0f, 0.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 0.0f);
        GL.Vertex3(1.0f, 1.0f, 0.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f);
        GL.End();

        GL.PopMatrix();
	}
}
