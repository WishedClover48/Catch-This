using UnityEngine;

public class RoundData : MonoBehaviour
{

    private static RoundData _instance;
    public static RoundData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RoundData>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("GameData");
                    _instance = go.AddComponent<RoundData>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }


    public int roundsPassed {  get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            ResetRounds();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    public void ResetRounds() {  roundsPassed = 0; }

    public void PassRound() { roundsPassed++; }

    public void RemoveRound() { roundsPassed--; }

    public void SetRound(int round) { roundsPassed = round; }


}
