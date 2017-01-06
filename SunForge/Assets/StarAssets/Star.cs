using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A UObj extension that creates all of the needed properties for a star
//This means class, surface activity, flares, light, etc.
//TODO: Variable stars, calculation of mass, rotation, temperature, radiation, luminosity, magnitude, structure, designation, etc. 
public class Star : MonoBehaviour
{
    public double temperatureKelvin = 0;
    public double radiusKm = 6371;
    public double massKg = 6e24;
    public Vector3 rotationRates = new Vector3(0, -1, 0);

    //TODO
    public double rotationRate = 0.1;//Measured in radians per second

    public double GetArea()
    {
        return 4d * System.Math.PI * GetRadius() * GetRadius();
    }

    public double GetRadius()
    {
        return radiusKm;
    }

    public double GetDiameter()
    {
        return GetRadius() * 2;
    }

    public double GetVolume()
    {
        return 4d / 3d * System.Math.PI * GetRadius() * GetRadius() * GetRadius();
    }

    public double GetDensity()
    {
        return GetMass() / GetVolume();
    }

    public double GetMass()
    {
        return massKg;
    }

    public double GetTemperature()
    {
        return temperatureKelvin;
    }

    public GameObject[] coronaStrips;

    public void Update()
    {
        //TODO move this to start
        GetComponent<Renderer>().material.SetColor("_StarColor", GetColor());
        GetComponent<Renderer>().material.SetVector("_StarCenter", new Vector4(transform.position.x, transform.position.y, transform.position.z, 49));
        GetComponent<Renderer>().material.SetVector("_RotRate", new Vector4(rotationRates.x, rotationRates.y, rotationRates.z, 0));

        foreach (GameObject coronaStrip in coronaStrips)
        {
            coronaStrip.GetComponent<Renderer>().material.SetColor("_StarColor", GetColor());
            coronaStrip.GetComponent<Renderer>().material.SetVector("_StarCenter", new Vector4(transform.position.x, transform.position.y, transform.position.z, 49));
        }

        GetComponentInChildren<Light>().color = GetColor();
    }

    public string GetStarClass()
    {
        if (GetTemperature() < 3700)
            return "M";
        if (GetTemperature() < 5200)
            return "K";
        if (GetTemperature() < 6000)
            return "G";
        if (GetTemperature() < 7500)
            return "F";
        if (GetTemperature() < 10000)
            return "A";
        if (GetTemperature() < 30000)
            return "B";
        //Temperature > 30000
        return "O";
    }

    private static readonly float[] temperatureLookup = {
        500000, //Made up entry for blending
        113017,
        56701,
        33605,
        22695,
        16954,
        13674,
        11677,
        10395,
        9531,
        8917,
        8455,
        8084,
        7767,
        7483,
        7218,
        6967,
        6728,
        6500,
        6285,
        6082,
        5895,
        5722,
        5563,
        5418,
        5286,
        5164,
        5052,
        4948,
        4849,
        4755,
        4664,
        4576,
        4489,
        4405,
        4322,
        4241,
        4159,
        4076,
        3989,
        3892,
        3779,
        3640,
        3463,
        3234,
        2942,
        2579,
        2150,
        1675,
        1195,
        0 //Made up entry for blending
};

    private static readonly string[] colorLookup = {
        "0000ff",   //Made up color for blending
        "9bb2ff",
        "9eb5ff",
        "a3b9ff",
        "aabfff",
        "b2c5ff",
        "bbccff",
        "c4d2ff",
        "ccd8ff",
        "d3ddff",
        "dae2ff",
        "dfe5ff",
        "e4e9ff",
        "e9ecff",
        "eeefff",
        "f3f2ff",
        "f8f6ff",
        "fef9ff",
        "fff9fb",
        "fff7f5",
        "fff5ef",
        "fff3ea",
        "fff1e5",
        "ffefe0",
        "ffeddb",
        "ffebd6",
        "ffe9d2",
        "ffe8ce",
        "ffe6ca",
        "ffe5c6",
        "ffe3c3",
        "ffe2bf",
        "ffe0bb",
        "ffdfb8",
        "ffddb4",
        "ffdbb0",
        "ffdaad",
        "ffd8a9",
        "ffd6a5",
        "ffd5a1",
        "ffd29c",
        "ffd096",
        "ffcc8f",
        "ffc885",
        "ffc178",
        "ffb765",
        "ffa94b",
        "ff9523",
        "ff7b00",
        "ff5200",
        "ff0000"//Made up color for blending
    };


    private static readonly float[] bMinusVLookup = {
        -.45f,  //Made up for blending
        -.40f,
        -.35f,
        -.30f,
        -.25f,
        -.20f,
        -.15f,
        -.10f,
        -.05f,
        0f,
        .05f,
        .10f,
        .15f,
        .20f,
        .25f,
        .30f,
        .35f,
        .40f,
        .45f,
        .50f,
        .55f,
        .60f,
        .65f,
        .70f,
        .75f,
        .80f,
        .85f,
        .90f,
        .95f,
        1.00f,
        1.05f,
        1.10f,
        1.15f,
        1.20f,
        1.25f,
        1.30f,
        1.35f,
        1.40f,
        1.45f,
        1.50f,
        1.55f,
        1.60f,
        1.65f,
        1.70f,
        1.75f,
        1.80f,
        1.85f,
        1.90f,
        1.95f,
        2.00f,
        2.05f   //Made up for blending
    };

    public static bool StarLookupTablesTest()
    {
        Debug.Log(bMinusVLookup.Length + ", " + temperatureLookup.Length + ", " + colorLookup.Length);
        //Test lengths
        if (bMinusVLookup.Length != temperatureLookup.Length || temperatureLookup.Length != colorLookup.Length)
            return false;
        //Test that temperature and b-v are linear
        for (int i = 0; i < bMinusVLookup.Length - 1; i++)
        {
            if (bMinusVLookup[i] >= bMinusVLookup[i + 1])
                return false;
            if (temperatureLookup[i] <= temperatureLookup[i + 1])
                return false;
        }
        return true;
    }

    //Approximates a color based off of temperature data
    //TODO can be improved
    //These are just approximations from a lookup table, 
    // there is no fast and accurate equation as far as I'm aware
    // besides doing the actual physics equations
    public Color GetColor()
    {
        /*
        http://www.vendian.org/mncharity/dir3/starcolor/details.html
         */
        string colorCode = "";
        //First find if it's out of bounds, and if so set the appropriate color
        if (GetTemperature() >= temperatureLookup[0])
        {
            colorCode = colorLookup[0];
        }
        else if (GetTemperature() <= temperatureLookup[temperatureLookup.Length - 1])
        {
            colorCode = colorLookup[colorLookup.Length - 1];
        }
        else
        {
            //It's in bounds, so find the closest two color/temperature pairs and do a linear interpolation
            //Or the exact color if it matches perfectly
            //This can be sped up later on TODO
            int max = 0;
            int min = temperatureLookup.Length - 1;
            for (int i = 0; i < temperatureLookup.Length; i++)
            {
                //Handle exact match
                if (temperatureLookup[i] == GetTemperature())
                {
                    //colorCode = colorLookup[i];
                    //break;
                }
                if (temperatureLookup[i] > GetTemperature() && temperatureLookup[i] < temperatureLookup[max])
                {
                    max = i;
                }
                else if (temperatureLookup[i] < GetTemperature() && temperatureLookup[i] > temperatureLookup[min])
                {
                    min = i;
                }
            }

            Color interpolatedColor = Color.Lerp(
                hexCodeToColor(colorLookup[min]),
                hexCodeToColor(colorLookup[max]),
                (float)(GetTemperature() - temperatureLookup[min]) / (float)(temperatureLookup[max] - temperatureLookup[min]));

            //Interpolate
            return interpolatedColor;


        }


        return hexCodeToColor(colorCode);
    }

    private Color hexCodeToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color(r / 255f, g / 255f, b / 255f);
    }
}
