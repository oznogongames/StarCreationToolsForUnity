using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeMorphStar : Star {
	
	// Update is called once per frame
	public new void Update () {
        base.Update();
        float scale = Mathf.PingPong(Time.fixedTime * 15, 80) + 60;
        this.transform.localScale = new Vector3(scale, scale, scale);
	}
}
