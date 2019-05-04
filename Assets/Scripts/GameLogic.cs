using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    private class ScoreTextSpawnInfo
    {
        public Vector3 position;
        public string text;
        public Color colour;

        public ScoreTextSpawnInfo(Vector3 position, string text, Color colour)
        {
            this.position = position;
            this.text = text;
            this.colour = colour;
        }
    }

    [SerializeField]
    private Animator truckAnimator;

    [SerializeField]
    private Text totalScoreText;

    [SerializeField]
    private GameObject scoreTextPopupPrefab;

    [SerializeField]
    private Color scoreTextPopupCorrect;

    [SerializeField]
    private Color scoreTextPopupIncorrect;

    [SerializeField]
    private float scoreTextSpawnDelay = 0.1f;


    [SerializeField]
    private int scorePerCorrectLabel = 1;

    [SerializeField]
    private int bonusScoreForNoIncorrectLabels = 1;

    [SerializeField]
    private int penaltyPerIncorrectLabel = -1;

    [SerializeField]
    private int penaltyPerMissingLabel = -1;

    [SerializeField]
    private int scoreForBinningNonIdealItem = 1;

    [SerializeField]
    private int pentaltyForNonIdealItemHittingFinalWaypoint = -2;

    [SerializeField]
    private int pentalyForBinningIdealItem = -1;

    private int score = 0;

    private void Start()
    {
        SetScore(score);
    }

    public void OnHitBin(PathTraverser traverser, PathNode lastNode, PathNode nextNode)
    {
        Rigidbody rigidbody = traverser.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.isKinematic = false;

        DoScoreOnBin(traverser.gameObject);
    }

    public void OnHitTruck(PathTraverser traverser, PathNode lastNode, PathNode nextNode)
    {
        DoScoreOnTruck(traverser.gameObject);
        Destroy(traverser.gameObject);
        truckAnimator.SetTrigger("shake");
    }

    private void DoScoreOnBin(GameObject binned)
    {
        List<ScoreTextSpawnInfo> scoreTextSpawns = new List<ScoreTextSpawnInfo>();
        Avocado avo = binned.GetComponent<Avocado>();
        if (avo)
        {
            // No you fool don't bin these
            scoreTextSpawns.Add(new ScoreTextSpawnInfo
            (
                binned.transform.position,
                pentalyForBinningIdealItem.ToString() + " avocado binned",
                scoreTextPopupIncorrect
            ));

            SetScore(score + pentalyForBinningIdealItem);
        }
        else
        {
            // Nice, this should be binned because it's not an avo
            scoreTextSpawns.Add(new ScoreTextSpawnInfo
            (
                binned.transform.position,
                "+" + scoreForBinningNonIdealItem + " non-avocado binned",
                scoreTextPopupCorrect
            ));

            SetScore(score + scoreForBinningNonIdealItem);
        }

        StartCoroutine(SpawnScoreTexts(scoreTextSpawns));
    }

    private void DoScoreOnTruck(GameObject trucked)
    {
        List<ScoreTextSpawnInfo> scoreTextSpawns = new List<ScoreTextSpawnInfo>();
        Avocado avo = trucked.GetComponent<Avocado>();
        if (avo)
        {
            // Ok lets work out the score delta for this bad boi
            int scoreDelta = 0;

            // Add score for each correct label
            int correctLabelScore = avo.CorrectLabelCount * scorePerCorrectLabel; 
            if(correctLabelScore != 0)
            {
                scoreTextSpawns.Add(new ScoreTextSpawnInfo
                (
                    trucked.transform.position, 
                    "+" + correctLabelScore + " correct labels", 
                    scoreTextPopupCorrect
                ));

                scoreDelta += correctLabelScore;
            }

            // Remove score for each incorrect
            int incorrectLabelPenalty = avo.IncorrectLabelCount * penaltyPerIncorrectLabel; 
            if(incorrectLabelPenalty != 0)
            {
                scoreTextSpawns.Add(new ScoreTextSpawnInfo
                (
                    trucked.transform.position,
                    incorrectLabelPenalty.ToString() + " incorrect labels",
                    scoreTextPopupIncorrect
                ));

                scoreDelta += incorrectLabelPenalty;
            }

            // Remove score for each label missing
            int missingLabelPenalty = (avo.RequiredLabelCount - avo.CorrectLabelCount) * penaltyPerMissingLabel;
            if(missingLabelPenalty != 0)
            {
                scoreTextSpawns.Add(new ScoreTextSpawnInfo
                (
                    trucked.transform.position,
                    missingLabelPenalty.ToString() + " missing labels",
                    scoreTextPopupIncorrect
                ));

                scoreDelta += missingLabelPenalty;
            }

            // Add in bonus points for getting all labels correct and no errors
            if(avo.CorrectLabelCount == avo.RequiredLabelCount && avo.IncorrectLabelCount == 0)
            {
                scoreTextSpawns.Add(new ScoreTextSpawnInfo
                (
                    trucked.transform.position,
                    "+" + bonusScoreForNoIncorrectLabels +  " perfect!",
                    scoreTextPopupCorrect
                ));

                scoreDelta += bonusScoreForNoIncorrectLabels;
            }

            SetScore(score + scoreDelta);
        }
        else
        {
            // Bad, this isn't an avo, get it out of here
            scoreTextSpawns.Add(new ScoreTextSpawnInfo
            (
                trucked.transform.position,
                pentaltyForNonIdealItemHittingFinalWaypoint.ToString() + " not binned",
                scoreTextPopupIncorrect
            ));

            SetScore(score + pentaltyForNonIdealItemHittingFinalWaypoint);
        }

        StartCoroutine(SpawnScoreTexts(scoreTextSpawns));
    }

    private void SetScore(int score)
    {
        this.score = score;
        totalScoreText.text = score.ToString();
    }

    private IEnumerator SpawnScoreTexts(IEnumerable<ScoreTextSpawnInfo> spawnInfos)
    {
        foreach(ScoreTextSpawnInfo info in spawnInfos)
        {
            Debug.Log("Spawning text");
            SpawnScoreText(info.position, info.text, info.colour);
            yield return new WaitForSeconds(scoreTextSpawnDelay);
        }
    }

    private void SpawnScoreText(Vector3 position, string text, Color colour)
    {
        GameObject scoreTextPopup = Instantiate(scoreTextPopupPrefab, position, Quaternion.identity);
        ScoreText scoreText = scoreTextPopup.GetComponent<ScoreText>();
        scoreText.Text = text;
        scoreText.Colour = colour;
    }
}
