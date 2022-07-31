using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using System.Numerics;
using DSPLib;


public class SongController : MonoBehaviour {

	float[] realTimeSpectrum;
	SpectralFluxAnalyzer realTimeSpectralFluxAnalyzer;

	AudioSource audioSource;
	[SerializeField] private SOEvent musicBeat;

	[SerializeField] private bool isRoundRunning;

	[SerializeField]
	[Range(0f, 100f)]
	private float thresholdMultiplier;

	[SerializeField]
	[Range(0, 100)]
	private int thresholdWindowSize;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();

		// Process audio as it plays
		realTimeSpectrum = new float[1024];
		realTimeSpectralFluxAnalyzer = new SpectralFluxAnalyzer();
		realTimeSpectralFluxAnalyzer.SetMusicBeatEvent(musicBeat);

	}

	void Update() {
		// Real-time
		if (isRoundRunning) {
			audioSource.GetSpectrumData (realTimeSpectrum, 0, FFTWindow.BlackmanHarris);
			realTimeSpectralFluxAnalyzer.analyzeSpectrum (realTimeSpectrum, audioSource.time);
		}

		realTimeSpectralFluxAnalyzer.SetThresholdMultiplier (thresholdMultiplier);
		realTimeSpectralFluxAnalyzer.SetThresholdWindowSize (thresholdWindowSize);
	}

	public void OnRoundStart()
    {
		audioSource.Play();
		isRoundRunning = true;
	}
	
	public void OnMusicBeat()
    {
		Debug.Log("Beat");
    }

	public void OnRoundEnd()
    {
		audioSource.Stop();
		isRoundRunning = false;
	}
}