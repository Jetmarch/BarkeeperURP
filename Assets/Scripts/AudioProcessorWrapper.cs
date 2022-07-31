using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProcessorWrapper : MonoBehaviour
{
    AudioProcessor audioProcessor;

    AudioSource audioSource;

    [SerializeField] private SOEvent OnBeatEvent;
    [SerializeField] private SOEvent OnSpectrumEvent;

    void Start()
    {
        audioProcessor = GetComponent<AudioProcessor>();
        audioSource = GetComponent<AudioSource>();
        audioProcessor.onBeat.AddListener(OnBeat);
        audioProcessor.onSpectrum.AddListener(OnSpectrum);
    }

    public void OnBeat()
    {
        OnBeatEvent.Raise();

        Debug.Log("BEAT!");
    }

    public void OnSpectrum(float[] spectrum)
    {
        Debug.Log(spectrum.Length);
        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }
    }

    public void OnStartRound()
    {
        audioSource.Play();
    }

    public void OnRoundOver()
    {
        audioSource.Stop();
    }

}
