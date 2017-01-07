using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMorphStar : Star {
	
	// Update is called once per frame
	public new void Update () {
        base.Update();
        this.baseStarColor = Color.HSVToRGB((Time.fixedTime / 10f) % 1, 0.5f + Mathf.PingPong(Time.fixedTime / 3, 0.5f), 1);
	}
}
