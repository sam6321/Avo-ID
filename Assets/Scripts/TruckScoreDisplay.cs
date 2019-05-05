using UnityEngine;
using UnityEngine.UI;

public class TruckScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private float minRotation = 90;

    [SerializeField]
    private float maxRotation = -90;

    [SerializeField]
    private float lerpSpeed = 3;

    [SerializeField]
    private Image needleImage;

    private float targetRotation = 0;

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = needleImage.transform.eulerAngles;
        float diff = Mathf.Abs(targetRotation - rotation.z);
        rotation.z = Mathf.MoveTowardsAngle(rotation.z, targetRotation, diff * lerpSpeed * Time.deltaTime);
        needleImage.transform.eulerAngles = rotation;
    }

    public void SetDial(float value)
    {
        targetRotation = Mathf.Lerp(minRotation, maxRotation, value);
    }
}
