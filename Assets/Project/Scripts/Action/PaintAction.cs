using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintAction : AbstractAction {
	
	/****************
	 *  PaintPoint  *
	 ****************/

	private class PaintPoint
	{
		// Properties
		public Vector3 pos;

		// Reference
		public GameObject instance;

		// Contructor
		public PaintPoint(Vector3 position){
			pos = position;
			instance = null;
		}
	}
	
	/****************
	 *  References  *
	 ****************/
	
	private GameObject painterPrefab;
	private ArtPaintingMaterial mat;
	private bool isDrawing;
	private List<PaintPoint> listOfPoints;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public PaintAction(MainManager manager, ArtPaintingMaterial matRef) : base(manager){
		mat = matRef;
		isDrawing = false;
		listOfPoints = new List<PaintPoint> ();
		painterPrefab = manager.painterPrefab;
		Debug.Log (mat.category);
		ParticleRenderer particleRenderer = (ParticleRenderer) painterPrefab.renderer;
		particleRenderer.material = mat.material;
	}
	
	/******************
	 *      Draw      *
	 ******************/

	public void StartDrawing(){
		isDrawing = true;
	}

	public bool IsDrawing(){
		return isDrawing;
	}

	public void Draw(Vector3 paintPos){
		if (!isDrawing)
			Debug.LogError ("Can't draw until StartDrawing method wasn't called.");
		else {
			PaintPoint point = new PaintPoint(paintPos);
			listOfPoints.Add(point);
			DrawPaintPoint(point);
		}
	}

	public void CancelDrawing(){
		foreach(PaintPoint point in listOfPoints)
			ErasePaintPoint(point);
		listOfPoints.Clear ();
	}

	public void EndDrawing(){
		isDrawing = false;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void Do(){
		if (!isDrawing)
			foreach(PaintPoint point in listOfPoints)
				DrawPaintPoint(point);
	}
	
	public override void Undo(){
		if (isDrawing)
			Debug.LogError ("Impossible to undo PaintAction while it is drawing.");
		else
			foreach(PaintPoint point in listOfPoints)
				ErasePaintPoint(point);
	}
	
	/******************
	 *  Tool Methods  *
	 ******************/

	private void DrawPaintPoint(PaintPoint point){
		if (point.instance != null)
			Debug.LogError ("Impossible to draw PaintPoint : it's already drawn.");
		else {
			point.instance = manager.PaintOnCanvas (painterPrefab, point.pos, Quaternion.identity);
		}
	}
	
	private void ErasePaintPoint(PaintPoint point){
		if (point.instance == null)
			Debug.LogError ("Impossible to erase PaintPoint : it's not instanciated.");
		else {
			Object.Destroy(point.instance);
			point.instance = null;
		}
	}
	
}
