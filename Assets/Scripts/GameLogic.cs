using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [System.Serializable]
    public class ScoreInfo
    {
        public int scorePerCorrectLabel = 1;
        public int bonusScoreForNoIncorrectLabels = 1;
        public int penaltyPerIncorrectLabel = -1;
        public int penaltyPerMissingLabel = -1;
        public int scoreForBinningNonIdealItem = 1;
        public int pentaltyForNonIdealItemHittingFinalWaypoint = -2;
        public int pentalyForBinningIdealItem = -1;
    }

    [SerializeField]
    private Animator truckAnimator;

    [SerializeField]
    private TruckScoreDisplay truckScoreDisplay;

    [SerializeField]
    private Text totalScoreText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject levelFailedUI;
    [SerializeField]
    private Text levelFailedScore;

    [SerializeField]
    private GameObject levelSucceededUI;
    [SerializeField]
    private Text levelSuceededScore;

    [SerializeField]
    private PathNode[] overridePath;

    // When avocados have got the correct labels, they always choose to use these nodes instead of going
    // through the normal path. This just gets rid of completed avocados instead of them cluttering up the conveyors
    public PathNode[] OverridePath { get => overridePath; }

    [SerializeField]
    private ScoreInfo scoreInfo;

    [SerializeField]
    private GameObject scoreTextPopupPrefab;

    [SerializeField]
    private Color scoreTextPopupCorrect;

    [SerializeField]
    private Color scoreTextPopupIncorrect;

    [SerializeField]
    private float scoreTextSpawnDelay = 0.1f;

    private AvoSpawner spawner;

    private int minTruckScore;
    private int maxTruckScore;

    private int level = 1;
    private int score = 0;
    private int truckScore = 0; // Only items that hit the truck add truck score.
    private bool levelFailed = false;
    private bool inEndLevel = false;

    private void Start()
    {
        spawner = GetComponent<AvoSpawner>();

        SetScore(score);
        SetLevel(level);
    }

    public void StartNextLevel()
    {
        spawner.MaxSpawned = Mathf.Clamp((int)Mathf.Ceil(level / 1.5f), 1, 4);
        spawner.MovementSpeed = Mathf.Min(1.0f + 0.1f * level, 2.5f);
        spawner.SpawnRate = Mathf.Max(30.0f - 2.5f * level, 15.0f);
        spawner.ResetLastSpawned();

        minTruckScore = -6 - level;
        maxTruckScore = 6 + level * 2;

        SetScore(0);
        SetTruckScore(0);

        Debug.Log(
            "MaxSpawned " + spawner.MaxSpawned + "\n" +
            "MovementSpeed " + spawner.MovementSpeed + "\n" +
            "SpawnRate " + spawner.SpawnRate + "\n" +
            "MinTruckScore " + minTruckScore + "\n" +
            "MaxTruckScore " + maxTruckScore + "\n"
        );

        spawner.enabled = true;
        levelFailed = false;
        inEndLevel = false;
    }

    void EndLevel(bool failed)
    {
        levelFailed = failed;
        inEndLevel = true;
        spawner.DestroyAllSpawnedItems();
        spawner.enabled = false;
        truckAnimator.SetTrigger("drive");
    }

    public void OnTruckDriveAwayComplete()
    {
        if (levelFailed)
        {
            levelFailedUI.SetActive(true);
            levelFailedScore.text = score.ToString();
        }
        else
        {
            levelSucceededUI.SetActive(true);
            levelSuceededScore.text = score.ToString();
        }
    }

    public void OnNextLevelClicked()
    {
        truckAnimator.SetTrigger("return");
        levelFailedUI.SetActive(false);
        levelSucceededUI.SetActive(false);
    }

    public void OnQuitClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnTruckReturnComplete()
    {
        if (levelFailed)
        {
            SetLevel(level);
        }
        else
        {
            SetLevel(level + 1);
        }

        StartNextLevel();
    }

    public void OnHitBin(PathTraverser traverser, PathNode lastNode, PathNode nextNode)
    {
        if(inEndLevel)
        {
            return;
        }

        Rigidbody rigidbody = traverser.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.isKinematic = false;

        DoScoreOnBin(traverser.gameObject);
    }

    public void OnHitTruck(PathTraverser traverser, PathNode lastNode, PathNode nextNode)
    {
        if(inEndLevel)
        {
            return;
        }

        DoScoreOnTruck(traverser.gameObject);
        Destroy(traverser.gameObject);
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
                scoreInfo.pentalyForBinningIdealItem.ToString() + " avocado binned",
                scoreTextPopupIncorrect
            ));

            SetScore(score + scoreInfo.pentalyForBinningIdealItem);
        }
        else
        {
            // Nice, this should be binned because it's not an avo
            scoreTextSpawns.Add(new ScoreTextSpawnInfo
            (
                binned.transform.position,
                "+" + scoreInfo.scoreForBinningNonIdealItem + " non-avocado binned",
                scoreTextPopupCorrect
            ));

            SetScore(score + scoreInfo.scoreForBinningNonIdealItem);
        }

        StartCoroutine(SpawnScoreTexts(scoreTextSpawns));
    }

    private void DoScoreOnTruck(GameObject trucked)
    {
        List<ScoreTextSpawnInfo> scoreTextSpawns = new List<ScoreTextSpawnInfo>();
        Avocado avo = trucked.GetComponent<Avocado>();
        // Ok lets work out the score delta for this bad boi
        int scoreDelta = 0;
        if (avo)
        {
            // Add score for each correct label
            int correctLabelScore = avo.CorrectLabelCount * scoreInfo.scorePerCorrectLabel;
            if (correctLabelScore != 0)
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
            int incorrectLabelPenalty = avo.IncorrectLabelCount * scoreInfo.penaltyPerIncorrectLabel;
            if (incorrectLabelPenalty != 0)
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
            int missingLabelPenalty = (avo.RequiredLabelCount - avo.CorrectLabelCount) * scoreInfo.penaltyPerMissingLabel;
            if (missingLabelPenalty != 0)
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
            if (avo.CorrectLabelCount == avo.RequiredLabelCount && avo.IncorrectLabelCount == 0)
            {
                scoreTextSpawns.Add(new ScoreTextSpawnInfo
                (
                    trucked.transform.position,
                    "+" + scoreInfo.bonusScoreForNoIncorrectLabels + " perfect!",
                    scoreTextPopupCorrect
                ));

                scoreDelta += scoreInfo.bonusScoreForNoIncorrectLabels;
            }
        }
        else
        {
            // Bad, this isn't an avo, get it out of here
            scoreTextSpawns.Add(new ScoreTextSpawnInfo
            (
                trucked.transform.position,
                scoreInfo.pentaltyForNonIdealItemHittingFinalWaypoint.ToString() + " not binned",
                scoreTextPopupIncorrect
            ));

            scoreDelta = scoreInfo.pentaltyForNonIdealItemHittingFinalWaypoint;
        }

        StartCoroutine(SpawnScoreTexts(scoreTextSpawns));
        SetScore(score + scoreDelta);
        SetTruckScore(truckScore + scoreDelta);

        Debug.Log("Truck Score " + truckScore);

        if (truckScore <= minTruckScore)
        {
            EndLevel(failed: true);
        }
        else if(truckScore >= maxTruckScore)
        {
            EndLevel(failed: false);
        }
        else
        {
            truckAnimator.SetTrigger("shake");
        } 
    }

    private void SetTruckScore(int score)
    {
        truckScore = score;
        float factor = Mathf.InverseLerp(minTruckScore, maxTruckScore, truckScore);
        truckScoreDisplay.SetDial(Mathf.Clamp01(factor));
    }

    private void SetScore(int score)
    {
        this.score = score;
        totalScoreText.text = score.ToString();
    }

    private void SetLevel(int level)
    {
        this.level = level;
        levelText.text = level.ToString();
    }

    private IEnumerator SpawnScoreTexts(IEnumerable<ScoreTextSpawnInfo> spawnInfos)
    {
        foreach(ScoreTextSpawnInfo info in spawnInfos)
        {
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
