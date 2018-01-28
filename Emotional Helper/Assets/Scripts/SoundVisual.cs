using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVisual : MonoBehaviour {

    private const int SAMPLE_SIZE = 1024;

   // public AudioManager audioManager;

    public float rmsValue;
    public float dbValue;
    public float pitchValue;
    public float visualModifier = 50.0f;
    public float smoothSpeed = 10.0f;
    public float maxVisualScale = 15.0f;
    public float keepPercentage = 0.2f;
    public int bgCompence = 18;

    public float bacgroundIntensity;
    public Material bacgroundMaterial;
    public Color minColor;
    public Color maxColor;

    private AudioSource source;
    private float[] samples;
    private float[] spectrum;
    private float sampleRate;

    private Transform[] visualList;
    private float[] visualScale;
    private int amountOfVisual = 64;
	
    // Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        samples = new float[SAMPLE_SIZE];
        spectrum = new float[SAMPLE_SIZE];
        sampleRate = AudioSettings.outputSampleRate;

        SpawnCircle();
	}
	

    void SpawnCircle()
    {
        visualScale = new float[amountOfVisual];
        visualList = new Transform[amountOfVisual];

        Vector3 center = Vector3.zero;
        float radius = 10.0f;

        for (int i = 0; i < amountOfVisual; i++)
        {
            float angle = i*1.0f / amountOfVisual;
            angle = angle * Mathf.PI * 2;

            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;
            
            Vector3 pos = new Vector3(x, y, 0);
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere) as GameObject;
            go.transform.position = pos;
            go.transform.rotation = Quaternion.LookRotation(Vector3.forward, pos);

            visualList[i] = go.transform;
        }

    }
	// Update is called once per frame
	void Update () {
        AnalyzeSound();
        UpdateVisual();
        UpdateBackground();
    }
    void UpdateVisual()
    {
        int visualIndex = 0;
        int spectrumIndex = 0;
        int averageSize = (int)((SAMPLE_SIZE * keepPercentage) / amountOfVisual);

        while (visualIndex < amountOfVisual)
        {
            int j = 0;
            float sum = 0;
            while (j < averageSize)
            {
                sum += spectrum[spectrumIndex];
                spectrumIndex++;
                j++;
            }

            float scaleY = sum / averageSize * visualModifier;
            visualScale[visualIndex] -= Time.deltaTime * smoothSpeed;

            if (visualScale[visualIndex] < scaleY)
            {
                visualScale[visualIndex] = scaleY/2;
            }   

            if (visualScale[visualIndex] > maxVisualScale)
            {
                visualScale[visualIndex] = maxVisualScale;
            }

            visualList[visualIndex].localScale = Vector3.one + Vector3.up * visualScale[visualIndex];
            visualIndex++;
        }
    }
    void UpdateBackground()
    {
        bacgroundIntensity -= Time.deltaTime * 0.4f;
        if (bacgroundIntensity< dbValue / bgCompence)
        {
            bacgroundIntensity = dbValue / bgCompence;
        }

        bacgroundMaterial.color = Color.Lerp(minColor, maxColor, bacgroundIntensity);
    }
    void AnalyzeSound()
    {
        source.GetOutputData(samples,0);

        //Get the RMS
        float sum = 0;

        for (int i = 0; i< SAMPLE_SIZE; ++i) 
        {
            sum += samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum/SAMPLE_SIZE);

        //Get the dbValue
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

        //Get sound spectrum
        source.GetSpectrumData(spectrum,0,FFTWindow.BlackmanHarris);
    }

    //void SpawnLines()
    //{
    //    visualScale = new float[amountOfVisual];
    //    visualList = new Transform[amountOfVisual];

    //    for (int i = 0; i < amountOfVisual; i++)
    //    {
    //        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
    //        visualList[i] = go.transform;
    //        visualList[i].position = Vector3.right * i;
    //    }
    //}
}
