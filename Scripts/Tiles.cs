using UnityEngine;
using System.Collections;

public class Tiles : MonoBehaviour {
	public int mapSizeX = 0;
	public int mapSizeY = 0;
	public int tileSizeX = 0;
	public int tileSizeY = 0;
	public int yMax = 0;
	public int yMin = 0;
	public int xMax = 0;
	public int xMin = 0;
	public int tileNum = 32;
	public Rect[,] rectArr = new Rect[0,0];
	private int i = 0;
	private int j = 0;
	private Vector2 position;
	// Use this for initialization
	void Start () {
		mapSizeY = GetComponent<SpriteRenderer>().sprite.texture.height;
		mapSizeX = GetComponent<SpriteRenderer>().sprite.texture.width;
		tileSizeX = mapSizeX/tileNum;
		tileSizeY = mapSizeY/tileNum;
		yMax = mapSizeY;
		for (i = 0; i <= tileNum; i++) {
			for (j = 0; j <= tileNum; j++) {
				rectArr[j,i] = new Rect(yMax,xMin,tileSizeX/tileNum, tileSizeY/tileNum);
				xMin += tileSizeX;
			}
			yMax -= tileSizeY;
		}
		position = GetComponent<RectTransform>().position;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (mapSizeY);
		//Debug.Log (mapSizeX);


	}
	void OnGUI() {
	}
}
