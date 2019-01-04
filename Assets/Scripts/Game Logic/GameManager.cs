﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;

    // the score canvas display
    public Text scoreText;

    // wait for new round
    public bool roundPause = false;

    // game stopped because of menu
    public bool gameStop = false;

    // timer over
    public bool timerOver = false;

    // game over
    public bool gameOver = false;

    // list of shots' accuracy
    private List<float> shots = new List<float>();

	// list of shots' accuracy (average) per round
	private List<float> shotsPerRound = new List<float>();

	// list of ducks' lifespan
	private List<float> ducks = new List<float>();

	// list of ducks' lifespan (average) per round
	private List<float> ducksPerRound = new List<float>();

    // current score
    private int score = 0;

    // Awake is always called before any Start functions
    void Awake()
    {
        // check if instance already exists
        if (instance == null)
        {
            // if not, set instance to this
            instance = this;
        }
        // if instance already exists and it's not this:
        else if (instance != this)
        {
            // then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    public void LogShot(float acc, float lifespan)
    {
        // add new shot accuracy
        shots.Add(acc);
		ducks.Add(lifespan);
    }

    // Increase score if red ducks are shot
    public void IncreaseScore(int increment)
    {
        // increase  score
        score += increment;

        // show score on Canvas
        scoreText.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }

	public void EndRound()
	{
		// pause the round
		roundPause = true;

		// calculate rounds accuracy and store it
		float shotsMean = 0;
		foreach (float acc in shots)
		{
			shotsMean += acc;
		}
		shotsMean = shotsMean / shots.Count;
		shotsPerRound.Add(shotsMean);

		float ducksMean = 0;
		int nValidDucks = 0;
		foreach (float lifespan in ducks)
		{
			if (lifespan > 0)
			{
				ducksMean += lifespan;
				nValidDucks++;
			}			
		}
		ducksMean = ducksMean / nValidDucks;
		ducksPerRound.Add(ducksMean);

		// prepare next ite
		shots.Clear();
		ducks.Clear();

		Debug.Log("Round: #" + shotsPerRound.Count + ", Accuracy: " + (shotsMean * 100).ToString("F2") + "%, Reaction Time: " + ducksMean + "s.");
	}
}
