using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public enum enemyState
{
    IDLE, ALERT, PATROL, FOLLOW, FURY, DIE
}

public enum GameState
{
    GAMEPLAY, DIE
}
public class GameManager : MonoBehaviour
{
    public GameState gameState;

    [Header("Info Player")]
    public Transform player;
    private int gems;

    [Header("UI")]
    public Text txtGem;

    [Header("Drop Item")]
    public GameObject gemPrefab;
    public int percDrop = 25; // valor de chance de dropar


    [Header("Slime IA")]
    public float slimeIdleWaitTime;
    public Transform[] slimeWayPoints;
    public float slimeDistanceToAttack = 2.3f;
    public float slimeAlertTime = 3f;
    public float slimeAttackDelay = 1f;
    public float slimeLookAtSpeed = 1f;

    [Header("Rain manager")]
    public PostProcessVolume postB;
    public ParticleSystem rainParticle;
    private ParticleSystem.EmissionModule rainModule;
    public int rainRateOverTime;
    public int rainIncrement;
    public float rainIncrementDelay;


    

    // Start is called before the first frame update
    void Start()
    {
        rainModule = rainParticle.emission;
        txtGem.text = gems.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOffRain(bool isRain)
    {
        StopCoroutine("RainManager");
        StopCoroutine("PostBManager");
        StartCoroutine("RainManager", isRain);
        StartCoroutine("PostBManager", isRain);
    }

    IEnumerator RainManager(bool isRain)
    {
        switch (isRain)
        {
            case true: //aumenta a chuva

                for(float r = rainModule.rateOverTime.constant; r < rainRateOverTime; r+= rainIncrement)
                {
                    rainModule.rateOverTime = r;
                    yield return new WaitForSeconds(rainIncrementDelay);
                }

                rainModule.rateOverTime = rainRateOverTime;

                break;

            case false: //diminui a chuva

                for (float r = rainModule.rateOverTime.constant; r > 0; r -= rainIncrement)
                {
                    rainModule.rateOverTime = r;
                    yield return new WaitForSeconds(rainIncrementDelay);
                }

                rainModule.rateOverTime = 0;

                break;
        }
    }

    IEnumerator PostBManager(bool isRain)
    {
        switch(isRain)
        {
            case true:

                for(float w = postB.weight; w < 1; w += 1 * Time.deltaTime)
                {
                    postB.weight = w;
                    yield return new WaitForEndOfFrame();
                }

                postB.weight = 1;

                break;

            case false:

                for (float w = postB.weight; w > 0; w -= 1 * Time.deltaTime)
                {
                    postB.weight = w;
                    yield return new WaitForEndOfFrame();
                }

                postB.weight = 0;

                break;
        }
    }

    public void ChangeGameState(GameState newState)
    {
        gameState = newState;
    }

    public void setGem(int amount)
    {
        gems += amount;
        txtGem.text = gems.ToString();
    }

    public bool Perc(int p)
    {
        int temp = Random.Range(0, 100);
        bool retorno = temp <= p ? true : false;
        return retorno;
    }



}
