using System;
using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;

using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isRunning = true;

    public float ElapsedTime => elapsedTime;

    //3 minaa max time
    private float maxTime = 180f;

    private float d;

    public MummonLiikutinOhjain mummo;
    [SerializeField] private Transform goal;
    private float maxDistance;

    [SerializeField] private GameObject enterNameCanvas;

    private void Start()
    {
        maxDistance = Vector3.Distance(mummo.transform.position, goal.position);
        Debug.Log("madistance"+maxDistance);
    }

    private void Update()
    {

        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }

        if( elapsedTime >= maxTime)
        {
            EndLevel();
        }

        float currentDistance = mummo.CalculateDistance(goal);
        Debug.Log("curren dstance"+currentDistance);
        Debug.Log("elapsed time"+elapsedTime);
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public int GetScore()
    {
        float currentDistance = mummo.CalculateDistance(goal);

        float distanceScore = (1f - (currentDistance / maxDistance)) * 100f;
        distanceScore = Mathf.Max(0f, distanceScore); 

        float timeScore = (1f - (elapsedTime / maxTime)) * 100f;
        timeScore = Mathf.Max(0f, timeScore);

        float totalScore = (distanceScore + timeScore) * 10;
        
        return Mathf.RoundToInt(totalScore);
    }


    private void EndLevel()
    {
        isRunning = false;
        int score = GetScore();
        PlayerPrefs.SetInt("lastScore", score);
        PlayerPrefs.Save();

        enterNameCanvas.SetActive(true);

    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        UpdateTimerUI();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

}
