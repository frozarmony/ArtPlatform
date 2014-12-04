using UnityEngine;
using System.Collections;

public class ArtPaintingMaterial : ArtMaterial {

	/*************************
	 *      Properties       *
	 *************************/
	
	public Material material;

	public ArtPaintingMaterial(){
		this.category = Category.PaintTexture;
	}
}
