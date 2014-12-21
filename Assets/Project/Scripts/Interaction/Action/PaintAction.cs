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
		public float size;

		// Reference
		public GameObject instance;
		public ParticleSystem.Particle[] particles;

		// Contructor
		public PaintPoint(Vector3 position, float sizeOfPoint){
			pos = position;
			size = sizeOfPoint;
			instance = null;
			particles = null;
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

	public void Draw(Vector3 paintPos, float size){
		if (!isDrawing)
			Debug.LogError ("Can't draw until StartDrawing method wasn't called.");
		else {
			PaintPoint point = new PaintPoint(paintPos, size);
			if(DrawPaintPoint(point))
				listOfPoints.Add(point);
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

	private bool DrawPaintPoint(PaintPoint point){
		if (point.instance != null)
			Debug.LogError ("Impossible to draw PaintPoint : it's already drawn.");
		else {
			point.instance = manager.PaintOnCanvas (painterPrefab, point.pos, Quaternion.identity);

			if(point.particles == null){
				if(!manager.DrawPainter(point.instance.particleSystem, mat.material, point.size, 3f, 0.3f)){
					Object.Destroy(point.instance);
					point.instance = null;
					return false;
				}
				else{
					point.instance.particleSystem.GetParticles(point.particles);
				}
			}
			else{
				point.instance.particleSystem.SetParticles(point.particles, point.particles.Length);
				point.instance.renderer.material = mat.material;
			}
		}
		return true;
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
