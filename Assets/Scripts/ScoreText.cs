﻿using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    [SerializeField]
    private float riseDistance = 1.0f;

    [SerializeField]
    private float fadeTime = 1.0f;

    private Text scoreText;

    private Vector3 startPosition;
    private float startTime;

    private Color colour = new Color(0, 0, 0);
    private string text = "";

    public Color Colour
    {
        get => scoreText ? scoreText.color : colour;
        set
        {
            if (scoreText)
            {
                scoreText.color = value;
            }
            else
            {
                colour = value;
            }
        }
    }

    public string Text
    {
        get => scoreText ? scoreText.text : text;
        set
        {
            if(scoreText)
            {
                scoreText.text = value;
            }
            else
            {
                text = value;
            }
        }
    }

    void Start()
    {
        scoreText = GetComponent<UIPopup>().GetPopupComponent<Text>();
        scoreText.text = text;
        scoreText.color = colour;
        startPosition = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        float endTime = startTime + fadeTime;
        if(Time.time >= endTime)
        {
            Destroy(gameObject);
        }
        else
        {
            float factor = Mathf.InverseLerp(startTime, startTime + fadeTime, Time.time);
            float smoothed = Mathf.SmoothStep(0.0f, 1.0f, factor);

            transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.up * riseDistance, smoothed);

            Color color = scoreText.color;
            color.a = 1.0f - factor;
            scoreText.color = color;
        }
    }
}
