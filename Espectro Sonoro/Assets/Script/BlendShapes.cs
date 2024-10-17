using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class BlendShapes : MonoBehaviour
{
public SkinnedMeshRenderer skinnedMeshRenderer;
    public AudioSource audioSource;
    public int lowFreqIndex = 0;
    public int midFreqIndex = 1;
    public int highFreqIndex = 2;
    public float lowIntensityMultiplier = 120f;
    public float midIntensityMultiplier = 80f;
    public float highIntensityMultiplier = 40f;
    public bool debugSpectrum = false;

    public Button playButton;    
    public Button pauseButton;   
    public Button restartButton; 
    public Button quitButton;

    private float[] spectrumData = new float[256];

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        playButton.onClick.AddListener(PlayMusic);
        pauseButton.onClick.AddListener(PauseMusic);
        restartButton.onClick.AddListener(RestartMusic);
        quitButton.onClick.AddListener(QuitGame); 

        audioSource.Stop();
        ResetBlendShapes();
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

            float lowFrequencyValue = GetFrequencyValue(lowFreqIndex);
            float midFrequencyValue = GetFrequencyValue(midFreqIndex);
            float highFrequencyValue = GetFrequencyValue(highFreqIndex);

            skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Clamp(lowFrequencyValue * lowIntensityMultiplier, 0, 100));
            skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Clamp(midFrequencyValue * midIntensityMultiplier, 0, 100));
            skinnedMeshRenderer.SetBlendShapeWeight(2, Mathf.Clamp(highFrequencyValue * highIntensityMultiplier, 0, 100));

            if (debugSpectrum)
            {
                DrawSpectrumDebug();
            }
        }
    }

    float GetFrequencyValue(int index)
    {
        if (index < 0 || index >= spectrumData.Length)
        {
            return 0f;
        }
        return spectrumData[index];
    }

    void DrawSpectrumDebug()
    {
        for (int i = 1; i < spectrumData.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrumData[i] + 10, 0), new Vector3(i, spectrumData[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrumData[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrumData[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrumData[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrumData[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrumData[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrumData[i]), 3), Color.blue);
        }
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void RestartMusic()
    {
        if (audioSource.isPlaying || audioSource.time > 0)  
        {
            audioSource.Stop();
            audioSource.time = 0;  
            ResetBlendShapes();
            audioSource.Play();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Sair do jogo.");
        Application.Quit();  
    }

    private void ResetBlendShapes()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(2, 0);
    }
}