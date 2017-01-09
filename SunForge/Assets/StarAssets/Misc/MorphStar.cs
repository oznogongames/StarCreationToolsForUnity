using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphStar : Star {

    public bool size = false;
    public bool color = false;
    public bool timescale = false;
    public bool resolution = false;
    public bool corona = false;
    public bool rotation = false;

    // Update is called once per frame
    public new void Update () {
        base.Update();
        if(color)
        {
            UpdateColors();
        }
        if(size)
        {
            UpdateScale();
        }
        if(rotation)
        {
            UpdateRotationRate();
        }
        if(timescale)
        {
            UpdateTimeScale();
        }
        if(resolution)
        {
            UpdateResolution();
        }
        if(corona)
        {
            //todo
        }
	}

    public void UpdateScale()
    {
        float scale = (1 + Mathf.Sin(Time.time / 3f)) * 60 + 30;
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void UpdateRotationRate()
    {
        //TODO fix
        float rate_x = Mathf.Pow(Mathf.Sin(Time.time / 5f), 2) * 5;
        float rate_y = 1;
        float rate_z = 1;

        this.rotationRates = new Vector3(rate_x, rate_y, rate_z);
    }

    public void UpdateTimeScale()
    {
        float scale = Mathf.Pow(1 + Mathf.Sin(Time.time / 3), 4);
        this.timeScale = scale;
    }

    public void UpdateResolution()
    {
        float scale = Mathf.Pow(1 + Mathf.Sin(Time.time / 5), 3) * 10 + 0.1f;
        this.resolutionScale = scale;
    }


    public void UpdateColors()
    {
        this.baseStarColor = Color.HSVToRGB((Time.time / 10f) % 1, 0.5f + Mathf.PingPong(Time.time / 3, 0.5f), 1);
    }
}
