using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformAttack : BossAttack
{
    [Header("Attack Settings")]
    public List<Platform> platforms;
    private void Awake()
    {
        List<GameObject> platformsObj = GameObject.FindGameObjectsWithTag("Platform").ToList();
        foreach (GameObject platformObj in platformsObj)
        {
            platforms.Add(platformObj.GetComponent<Platform>());
        }
    }
    public override void Start()
    {
        base.Start();
        
        ActionEvent += ExecuteAttack;
    }

    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }
    private void ExecuteAttack()
    {
        FlipRandomPlatform();
    }
    private void FlipRandomPlatform()
    {
        int randomIndex = UnityEngine.Random.Range(0, platforms.Count);

        platforms[randomIndex].FlipPlatform(DelayBeforeAttack);
    }

    private void FlipAllPlatform()
    {
        foreach (Platform platform in platforms)
        {
            platform.FlipPlatform(DelayBeforeAttack);
        }
    }

}