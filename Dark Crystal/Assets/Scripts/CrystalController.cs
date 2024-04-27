using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    DarkCrystal crystalStats;

    public float spawnCoolDownTimer = -1;

    [SerializeField]
    GameObject lightSuckerPrefab, flyerPrefab, tankPrefab;

    public bool overrideSpawning = false;
    [Range(1,4)]
    public int overideSpawnAmount;
    [Range(1, 100)]
    public float overrideSpawnTimer;

    [SerializeField] public Mob mob;


    // Start is called before the first frame update
    void Start()
    {
        crystalStats = GetComponent<DarkCrystal>();
    }

    // Update is called once per frame
    void Update()
    {
        
        crystalStats.InLight = CheckInLight();

        if (spawnCoolDownTimer > 0)
        {
            spawnCoolDownTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);

        if (playerDistance <= crystalStats.DangerRange) { crystalStats.InDanger = true; } else { crystalStats.InDanger = false; }
        if (playerDistance > crystalStats.DangerRange && playerDistance < crystalStats.ThreatRange) 
        { crystalStats.IsThreatened = true; } else { crystalStats.IsThreatened = false; }

        if (!crystalStats.InLight)
        {
            bool isLightNear = false;

            Collider2D lightCollider = Physics2D.OverlapCircle(transform.position, crystalStats.DangerRange, LayerMask.GetMask("Light"));
            if (lightCollider != null)
            {
                isLightNear = true;
            }

            SpawnTesting(isLightNear, player, playerDistance);
        }
    }

    bool CheckInLight()
    {
        
        Collider2D collision = Physics2D.OverlapCircle(transform.position, .3f, LayerMask.GetMask("Light"));
        if (collision != null)
        {
            crystalStats.AfflictingLight = collision.gameObject;
            return true;
        }
        return false;
    }

    void SpawnTesting(bool isLightNear, GameObject player, float playerDistance)
    {
        /*
         * 
         * Spawn Logic:
         * First Test if it can afford to spawn anything
         *  if no recharge
         * Second Test if there are any lights close to it
         *  if yes then spawn lightsuckers, if it can afford
         * Third Test if the enemy is within close range
         *  if yes then spawn tank, if it can afford
         * Fourth Test if there are any enemies at a distance to it
         *  if yes then spawn flyers, if it can afford
         * Fifth Test if there are any enemies to the crystal (ie the player) around it
         *  if not spawn a random unit, that it can afford
         * 
         */

        //Test if Spawn is on cooldown
        if (spawnCoolDownTimer >= 0) { return; }
        
        //Test if the Crystal is recharging energy
        if(crystalStats.IsRecharging) 
        { 
            Debug.Log("is Recharging Energy."); 
            return; 
        }

        if (overrideSpawning)
        {
            OverridedSpawning();
            return;
        }

        //Test if the crystal has the energy to spawn if not recharge it's energy
        if (crystalStats.SpawnEnergy < 10) { Debug.Log("No Energy Must Recharge."); StartCoroutine(crystalStats.RechargeEnergy()); return; }


        //Test if the Player is near the Crystal if yes spawn tanks, if cannot afford spawn light suckers
        if (playerDistance <= crystalStats.DangerRange)
        {
            if (crystalStats.CanSpendEnergy(tankPrefab.GetComponent<TankLogic>().spawnCost))
            {
                int spawnAmount = Mathf.RoundToInt(Random.Range(tankPrefab.GetComponent<TankLogic>().spawnQuantityMin, tankPrefab.GetComponent<TankLogic>().spawnQuantityMax));
                spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                SpawnEnemies(tankPrefab, spawnAmount);
            } else if (crystalStats.CanSpendEnergy(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnCost))
            {
                int spawnAmount = Mathf.RoundToInt(Random.Range(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMin, lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMax));
                spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                SpawnEnemies(lightSuckerPrefab, spawnAmount);
            }
            return;
        }

        //Test if there is a Light near the Crystal if yes spawned light suckers
        if (isLightNear)
        {
            if (crystalStats.CanSpendEnergy(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnCost))
            {
                int spawnAmount = Mathf.RoundToInt(Random.Range(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMin, lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMax));
                spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                SpawnEnemies(lightSuckerPrefab, spawnAmount);
            }
            return;
        }

        //Test if the Player is far from the Crystal if yes spawn flyers, if cannot afford spawn light suckers
        if (playerDistance <= crystalStats.ThreatRange)
        {
            if (crystalStats.CanSpendEnergy(flyerPrefab.GetComponent<FlyerLogic>().spawnCost))
            {
                int spawnAmount = Mathf.RoundToInt(Random.Range(flyerPrefab.GetComponent<FlyerLogic>().spawnQuantityMin, flyerPrefab.GetComponent<FlyerLogic>().spawnQuantityMax));
                spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                SpawnEnemies(flyerPrefab, spawnAmount);
            }
            else if (crystalStats.CanSpendEnergy(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnCost))
            {
                int spawnAmount = Mathf.RoundToInt(Random.Range(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMin, lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMax));
                spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                SpawnEnemies(lightSuckerPrefab, spawnAmount);
            }
            return;
        }

        //If player is farther then threat range do not spawn 
        if (playerDistance > crystalStats.ThreatRange) { return; }

        //If not stopped by anyother tests spawn a random mob it can afford
        int randomSpawnCase = Mathf.RoundToInt(Random.Range(1, 3));
        switch (randomSpawnCase)
        {
            case 1:
                if (crystalStats.CanSpendEnergy(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnCost))
                {
                    int spawnAmount = Mathf.RoundToInt(Random.Range(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMin, lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMax));
                    spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                    SpawnEnemies(lightSuckerPrefab, spawnAmount);
                }
                break;
            case 2:
                if (crystalStats.CanSpendEnergy(flyerPrefab.GetComponent<FlyerLogic>().spawnCost))
                {
                    int spawnAmount = Mathf.RoundToInt(Random.Range(flyerPrefab.GetComponent<FlyerLogic>().spawnQuantityMin, flyerPrefab.GetComponent<FlyerLogic>().spawnQuantityMax));
                    spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                    SpawnEnemies(flyerPrefab, spawnAmount);
                }
                else if (crystalStats.CanSpendEnergy(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnCost))
                {
                    int spawnAmount = Mathf.RoundToInt(Random.Range(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMin, lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMax));
                    spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                    SpawnEnemies(lightSuckerPrefab, spawnAmount);
                }
                break;
            case 3:
                if (crystalStats.CanSpendEnergy(tankPrefab.GetComponent<TankLogic>().spawnCost))
                {
                    int spawnAmount = Mathf.RoundToInt(Random.Range(tankPrefab.GetComponent<TankLogic>().spawnQuantityMin, tankPrefab.GetComponent<TankLogic>().spawnQuantityMax));
                    spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                    SpawnEnemies(tankPrefab, spawnAmount);
                }
                else
                {
                    int secondaryRandomSpawnCase = Mathf.RoundToInt(Random.Range(1, 2));
                    switch (secondaryRandomSpawnCase)
                    {
                        case 1:
                            if (crystalStats.CanSpendEnergy(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnCost))
                            {
                                int spawnAmount = Mathf.RoundToInt(Random.Range(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMin, lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMax));
                                spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                                SpawnEnemies(lightSuckerPrefab, spawnAmount);
                            }
                            break;
                        case 2:
                            if (crystalStats.CanSpendEnergy(flyerPrefab.GetComponent<FlyerLogic>().spawnCost))
                            {
                                int spawnAmount = Mathf.RoundToInt(Random.Range(flyerPrefab.GetComponent<FlyerLogic>().spawnQuantityMin, flyerPrefab.GetComponent<FlyerLogic>().spawnQuantityMax));
                                spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                                SpawnEnemies(flyerPrefab, spawnAmount);
                            }
                            else if (crystalStats.CanSpendEnergy(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnCost))
                            {
                                int spawnAmount = Mathf.RoundToInt(Random.Range(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMin, lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnQuantityMax));
                                spawnCoolDownTimer = crystalStats.SpawnCooldownTime;
                                SpawnEnemies(lightSuckerPrefab, spawnAmount);
                            }
                            break;
                    }
                }
                break;
        }
    }

    void SpawnEnemies(GameObject enemyPrefab, int spawnAmount)
    {
        List<Vector3> spawnableLocations = new List<Vector3>();

        for (int x = -crystalStats.SpawnRadius; x <= crystalStats.SpawnRadius; x++)
        {
            for (int y = -crystalStats.SpawnRadius; y <= crystalStats.SpawnRadius; y++)
            {
                Vector2 testLocation = new Vector2(x, y) + (Vector2)transform.position;
                if (Vector2.Distance(testLocation, transform.position) <= crystalStats.SpawnRadius)
                {
                    //makes circle grid /\ /\ /\

                    Collider2D nodeCheck = Physics2D.OverlapCircle(testLocation, .1f);
                    if (nodeCheck != null)
                    {
                        if (nodeCheck.tag == "Floor")
                        {
                            spawnableLocations.Add(testLocation);
                        }
                    } else
                    {
                        spawnableLocations.Add(testLocation);
                    }
                }
            }
        }

        if (spawnableLocations.Count >= spawnAmount)
        {
            for (int spawned = 0; spawned < spawnAmount; spawned++)
            {
                int spawnLocation = Mathf.RoundToInt(Random.Range(0, spawnableLocations.Count));
                GameObject freshSpawnedEnemy = Instantiate(enemyPrefab, spawnableLocations[spawnLocation], Quaternion.identity);
                freshSpawnedEnemy.GetComponent<BaseEnemyController>().homeCrystal = this.gameObject;
                spawnableLocations.Remove(spawnableLocations[spawnLocation]);
            }
        } else
        {
            for (int spawned = 0; spawned <= spawnableLocations.Count; spawned++)
            {
                int spawnLocation = Mathf.RoundToInt(Random.Range(0, spawnableLocations.Count - 1));
                GameObject freshSpawnedEnemy = Instantiate(enemyPrefab, spawnableLocations[spawnLocation], Quaternion.identity);
                freshSpawnedEnemy.GetComponent<BaseEnemyController>().homeCrystal = this.gameObject;
                spawnableLocations.Remove(spawnableLocations[spawnLocation]);
            }
        }

        
    }

    void OverridedSpawning()
    {
        switch (mob)
        {
            case Mob.LightSucker:
                if (crystalStats.CanSpendEnergy(lightSuckerPrefab.GetComponent<LightSuckerLogic>().spawnCost))
                {
                    spawnCoolDownTimer = overrideSpawnTimer;
                    SpawnEnemies(lightSuckerPrefab, overideSpawnAmount);
                }
                else
                {
                    StartCoroutine(crystalStats.RechargeEnergy());
                }
                break;
            case Mob.Flyer:
                if (crystalStats.CanSpendEnergy(flyerPrefab.GetComponent<FlyerLogic>().spawnCost))
                {
                    spawnCoolDownTimer = overrideSpawnTimer;
                    SpawnEnemies(flyerPrefab, overideSpawnAmount);
                }
                else
                {
                    StartCoroutine(crystalStats.RechargeEnergy());
                }
                break;
            case Mob.Tank:
                if (crystalStats.CanSpendEnergy(tankPrefab.GetComponent<TankLogic>().spawnCost))
                {
                    spawnCoolDownTimer = overrideSpawnTimer;
                    SpawnEnemies(tankPrefab, overideSpawnAmount);
                }
                else
                {
                    StartCoroutine(crystalStats.RechargeEnergy());
                }
                break;
        }
    }
}

public enum Mob { LightSucker, Flyer, Tank }
