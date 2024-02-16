using UnityEngine;
using ProefExamen.Framework.Gameplay.Settings;

public class Note : MonoBehaviour
{
    [SerializeField]
    private float _timeStamp = 0;

    [SerializeField]
    private int _laneID = -1;

    [SerializeField]
    private int _levelID = -1;

    [SerializeField]
    private Vector2 _initialPosition;

    [SerializeField]
    private Vector2 _targetPosition;

    [SerializeField]
    private float _lerpAlpha = 0;

    public float LerpAlpha
    {
        get => _lerpAlpha;
    }

    public void SetNoteValues(Vector2 initialPosition, Vector2 targetPosition, int laneID, int levelID, float timeStamp)
    {
        _initialPosition = initialPosition;
        _targetPosition = targetPosition;
        _laneID = laneID;
        _levelID = levelID;
        _timeStamp = timeStamp;
    }

    public void Update()
    {
        if (Settings.paused || _laneID == -1) return;
        else if ((_timeStamp + (Settings.travelTime * .5) < Settings.time))
            Destroy(gameObject);

        _lerpAlpha = Settings.ReturnNoteLerpAlpha(_timeStamp);

        transform.position = Vector3.Lerp(_initialPosition, _targetPosition, _lerpAlpha);
    }
}
