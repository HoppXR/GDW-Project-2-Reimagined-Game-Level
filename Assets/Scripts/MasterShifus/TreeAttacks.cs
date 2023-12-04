using System.Collections;
using UnityEngine;

public class TreeAttacks : MonoBehaviour
{
    private Rigidbody2D rb;

    Vector3 pickedPosition;

    // Phase 1
    // AirAttack
    public Vector3 lTreeAirAttack = new Vector3(-40f, -10f, 0f);
    public Vector3 rTreeAirAttack = new Vector3(7f, -10f, 0f);
    [SerializeField] private float AirTravelSpeed = 10;
    [SerializeField] private float AirAttackAmount = 10;
    [SerializeField] private float AirAttackDelay = 0.3f;
    [SerializeField] private AudioClip airAttackSound;

    // TreeDropStuff
    [SerializeField] private float TreeDropStuffAmount = 3;
    [SerializeField] private float TreeSpinDropStuffAmount = 10;
    [SerializeField] private AudioClip dropObjectSound;
    [SerializeField] private AudioClip fallSound;


    // TrunkAttack
    [SerializeField] private float trunkMoveSpeed = 150f;
    [SerializeField] private AudioClip trunkAttackSound;

    // Prefabs
    [SerializeField] private Rigidbody2D LeftTreeApplePrefab;
    [SerializeField] private Rigidbody2D LeftTreeSpikePrefab;
    [SerializeField] private Rigidbody2D LeftWormSpikePrefab;
    [SerializeField] private Rigidbody2D RightTreeApplePrefab;
    [SerializeField] private Rigidbody2D RightTreeSpikePrefab;
    [SerializeField] private Rigidbody2D RightWormSpikePrefab;
    [SerializeField] private Rigidbody2D AirFacingRightPrefab;
    [SerializeField] private Rigidbody2D AirFacingLeftPrefab;
    [SerializeField] private Rigidbody2D TrunkPrefab;
    private Rigidbody2D fallingObject;

    private AudioSource audioSource;


    //Phase 2
    [SerializeField] private float P2TreeSpinDropStuffAmount = 15;
    [SerializeField] private float P2TreeDropStuffAmount = 6;
    
    
    [SerializeField] private Rigidbody2D TreePrefab;
    [SerializeField] private Rigidbody2D RootPrefab;
    [SerializeField] private Rigidbody2D DangerPrefab;
    [SerializeField] private Rigidbody2D AngryWormL;
    [SerializeField] private Rigidbody2D AngryWormR;
    [SerializeField] private Rigidbody2D AngrySpikeL;
    [SerializeField] private Rigidbody2D AngrySpikeR;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(P2SpinDropAttack());
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(P2RSideDropStuff());
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(P2LSideDropStuff());
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(RTreeDropStuff());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            TreeRushAttack();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(AOESpikeAttack());
        }
    }*/

    public IEnumerator LAirAttack() // Attack last 3.5 secs
    {
        PlaySound(airAttackSound);

        for (int i = 0; i < AirAttackAmount; i++)
        {
            Rigidbody2D AirProjectiles =
                Instantiate(AirFacingRightPrefab, lTreeAirAttack, Quaternion.Euler(0f, 0f, 0f)).GetComponent<Rigidbody2D>();

            if (AirProjectiles != null)
            {
                float randomAngle = UnityEngine.Random.Range(-30f, 30f);
                Vector2 direction = Quaternion.Euler(0f, 0f, randomAngle) * Vector2.right;
                direction.Normalize();
                AirProjectiles.AddForce(AirTravelSpeed * direction, ForceMode2D.Impulse);
                Destroy(AirProjectiles.gameObject, 5);
                yield return new WaitForSeconds(AirAttackDelay);
            }
        }
    }

    public IEnumerator RAirAttack() // Attack last 3.5 secs
    {
        PlaySound(airAttackSound);

        for (int i = 0; i < AirAttackAmount; i++)
        {
            Rigidbody2D AirProjectiles =
                Instantiate(AirFacingLeftPrefab, rTreeAirAttack, Quaternion.Euler(0f, 0f, 0f)).GetComponent<Rigidbody2D>();

            if (AirProjectiles != null)
            {
                float randomSpreadAngle = UnityEngine.Random.Range(-30f, 30f);
                Vector2 leftDirection = Quaternion.Euler(0f, 0f, randomSpreadAngle) * Vector2.left;
                leftDirection.Normalize();
                AirProjectiles.AddForce(AirTravelSpeed * leftDirection, ForceMode2D.Impulse);
                Destroy(AirProjectiles.gameObject, 5);
                yield return new WaitForSeconds(AirAttackDelay);
            }
        }
    }

    public IEnumerator LTreeDropStuff() // Attack last 5 secs
    {
        PlaySound(dropObjectSound);

        for (int i = 0; i < TreeDropStuffAmount; i++)
        {
            GameObject prefabToSpawn = UnityEngine.Random.Range(0f, 1f) < 0.65f
                ? LeftTreeApplePrefab.gameObject
                : LeftTreeSpikePrefab.gameObject;
            float randomX = UnityEngine.Random.Range(-37f, -32.9f);
            float randomY = UnityEngine.Random.Range(5.5f, 8f);
            float randomX2 = UnityEngine.Random.Range(-26.6f, -20.24f);
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            Vector3 randomPosition2 = new Vector3(randomX2, 9.85f, 0f);
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                pickedPosition = randomPosition;
            }
            else
            {
                pickedPosition = randomPosition2;
            }

            GameObject instantiatedObject = Instantiate(prefabToSpawn, pickedPosition, Quaternion.identity);
            fallingObject = instantiatedObject.GetComponent<Rigidbody2D>();

            PlaySound(dropObjectSound);

            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator RTreeDropStuff() // Attack last 5 secs
    {
        PlaySound(dropObjectSound);

        for (int i = 0; i < TreeDropStuffAmount; i++)
        {
            GameObject prefabToSpawn = UnityEngine.Random.Range(0f, 1f) < 0.65f
                ? RightTreeApplePrefab.gameObject
                : RightTreeSpikePrefab.gameObject;
            float randomX = UnityEngine.Random.Range(0f, 4f);
            float randomY = UnityEngine.Random.Range(5.5f, 8f);
            float randomX2 = UnityEngine.Random.Range(-13f, -6f);
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            Vector3 randomPosition2 = new Vector3(randomX2, 9.85f, 0f);
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                pickedPosition = randomPosition;
            }
            else
            {
                pickedPosition = randomPosition2;
            }

            GameObject instantiatedObject = Instantiate(prefabToSpawn, pickedPosition, Quaternion.identity);
            fallingObject = instantiatedObject.GetComponent<Rigidbody2D>();

            PlaySound(dropObjectSound);

            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator SpinDropAttack() // Attack last 9 secs
    {
        PlaySound(dropObjectSound);

        for (int i = 0; i < TreeSpinDropStuffAmount; i++)
        {
            int leftOrRight = UnityEngine.Random.Range(1, 3);

            GameObject prefabToSpawn;
            if (leftOrRight == 1) // Left
            {
                prefabToSpawn = UnityEngine.Random.Range(0f, 1f) < 0.3f
                    ? LeftTreeApplePrefab.gameObject
                    : LeftWormSpikePrefab.gameObject;
                float randomX = UnityEngine.Random.Range(-37f, -32.9f);
                float randomY = UnityEngine.Random.Range(5.5f, 8f);
                float randomX2 = UnityEngine.Random.Range(-26.6f, -20.24f);
                Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
                Vector3 randomPosition2 = new Vector3(randomX2, 9.85f, 0f);
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    pickedPosition = randomPosition;
                }
                else
                {
                    pickedPosition = randomPosition2;
                }

                GameObject instantiatedObject = Instantiate(prefabToSpawn, pickedPosition, Quaternion.identity);
                fallingObject = instantiatedObject.GetComponent<Rigidbody2D>();
            }

            if (leftOrRight == 2) // Right
            {
                prefabToSpawn = UnityEngine.Random.Range(0f, 1f) < 0.3f
                    ? RightTreeApplePrefab.gameObject
                    : RightWormSpikePrefab.gameObject;
                float randomX = UnityEngine.Random.Range(0f, 4f);
                float randomY = UnityEngine.Random.Range(5.5f, 8f);
                float randomX2 = UnityEngine.Random.Range(-13f, -6f);
                Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
                Vector3 randomPosition2 = new Vector3(randomX2, 9.85f, 0f);
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    pickedPosition = randomPosition;
                }
                else
                {
                    pickedPosition = randomPosition2;
                }

                GameObject instantiatedObject = Instantiate(prefabToSpawn, pickedPosition, Quaternion.identity);
                fallingObject = instantiatedObject.GetComponent<Rigidbody2D>();
            }

            yield return new WaitForSeconds(0.6f);
        }
    }

    public void TrunkAttack() // Attack last 1.70 secs
    {
        PlaySound(trunkAttackSound);

        Rigidbody2D trunkObject = Instantiate(TrunkPrefab, new Vector3(-41.92161f, -17.68358f, 0f), Quaternion.identity);

        Rigidbody2D trunkRigidbody = trunkObject.GetComponent<Rigidbody2D>();

        StartCoroutine(MoveTrunk(trunkRigidbody, new Vector3(8.76f, -17.68358f, 0f), 5));
        Destroy(trunkObject.gameObject, 3);
    }

    public IEnumerator MoveTrunk(Rigidbody2D trunkRigidbody, Vector3 targetPosition, float duration)
    {
        float elapsed_time = 0f;
        Vector3 initialPosition = trunkRigidbody.position;

        while (elapsed_time < duration && trunkRigidbody != null)
        {
            trunkRigidbody.position =
                Vector3.MoveTowards(initialPosition, targetPosition, (elapsed_time / duration) * trunkMoveSpeed);
            elapsed_time += Time.deltaTime;
            yield return null;
        }

        if (trunkRigidbody != null)
        {
            trunkRigidbody.position = targetPosition;
        }
    }
//---------------------------------------------------------------------------------------------------------

    public IEnumerator AOESpikeAttack()
    {
        //Dangeraudio here
        Rigidbody2D Danger1 = Instantiate(DangerPrefab, new Vector3(-30.3f, -15f, 0f), Quaternion.identity);
        Rigidbody2D Danger2 = Instantiate(DangerPrefab, new Vector3(-17.84f, -15f, 0f), Quaternion.identity);
        Rigidbody2D Danger3 = Instantiate(DangerPrefab, new Vector3(-5.51f, -15f, 0f), Quaternion.identity);
        
        Destroy(Danger1.gameObject, 1);
        Destroy(Danger2.gameObject, 1);
        Destroy(Danger3.gameObject, 1);
        
        yield return new WaitForSeconds(1.5f);
        
        //woosh sound here
        Rigidbody2D rootObject1 = Instantiate(RootPrefab, new Vector3(-30.3f, -26.1f, 0f), Quaternion.identity);
        Rigidbody2D rootRigidbody1 = rootObject1.GetComponent<Rigidbody2D>();

        Rigidbody2D rootObject2 = Instantiate(RootPrefab, new Vector3(-17.84f, -26.1f, 0f), Quaternion.identity);
        Rigidbody2D rootRigidbody2 = rootObject2.GetComponent<Rigidbody2D>();

        Rigidbody2D rootObject3 = Instantiate(RootPrefab, new Vector3(-5.51f, -26.1f, 0f), Quaternion.identity);
        Rigidbody2D rootRigidbody3 = rootObject3.GetComponent<Rigidbody2D>();

        StartCoroutine(MoveRoot(rootRigidbody1, new Vector3(-30.3f, -9.7f, 0f),
            new Vector3(-30.3f, -26.1f, 0f), 1));
        StartCoroutine(MoveRoot(rootRigidbody2, new Vector3(-17.84f, -9.7f, 0f),
           new Vector3(-17.84f, -26.1f, 0f), 1));
        StartCoroutine(MoveRoot(rootRigidbody3, new Vector3(-5.51f, -9.7f, 0f),
            new Vector3(-5.51f, -26.1f, 0f), 1));
        
        Destroy(rootObject1.gameObject, 3);
        Destroy(rootObject2.gameObject, 3);
        Destroy(rootObject3.gameObject, 3);

        yield return new WaitForSeconds(1f);

        
        //Dangeraudio here
        
        Rigidbody2D Danger4 = Instantiate(DangerPrefab, new Vector3(-24f, -15f, 0f), Quaternion.identity);
        Rigidbody2D Danger5 = Instantiate(DangerPrefab, new Vector3(-10.3f, -15f, 0f), Quaternion.identity);
        
        Destroy(Danger4.gameObject, 1);
        Destroy(Danger5.gameObject, 1);
        
        yield return new WaitForSeconds(1.5f);
        
        
        //woosh sound here
        Rigidbody2D rootObject4 = Instantiate(RootPrefab, new Vector3(-24f, -26.1f, 0f), Quaternion.identity);
        Rigidbody2D rootRigidbody4 = rootObject4.GetComponent<Rigidbody2D>();
        Rigidbody2D rootObject5 = Instantiate(RootPrefab, new Vector3(-10.3f, -26.1f, 0f), Quaternion.identity);
        Rigidbody2D rootRigidbody5 = rootObject5.GetComponent<Rigidbody2D>();
        
        StartCoroutine(MoveRoot(rootRigidbody4, new Vector3(-24f, -9.7f, 0f),
            new Vector3(-24f, -26.1f, 0f), 1));
        StartCoroutine(MoveRoot(rootRigidbody5, new Vector3(-10.3f, -9.7f, 0f),
            new Vector3(-10.3f, -26.1f, 0f), 1));
        
        Destroy(rootObject4.gameObject, 3);
        Destroy(rootObject5.gameObject, 3);
        
        yield return new WaitForSeconds(1f);
    }



    public IEnumerator MoveRoot(Rigidbody2D rootRigidbody, Vector3 targetPosition, Vector3 initialPosition, float duration)
    {
        float elapsed_time = 0f;

        while (elapsed_time < duration && rootRigidbody != null)
        {
            rootRigidbody.position =
                Vector3.MoveTowards(initialPosition, targetPosition, (elapsed_time / duration) * 1000);
            elapsed_time += Time.deltaTime;
            yield return null;
        }

        if (rootRigidbody != null)
        {
            rootRigidbody.position = targetPosition;

            elapsed_time = 0f;

            while (elapsed_time < duration && rootRigidbody != null)
            {
                rootRigidbody.position =
                    Vector3.MoveTowards(targetPosition, initialPosition, (elapsed_time / duration) * 500);
                elapsed_time += Time.deltaTime;
                yield return null;
            }

            if (rootRigidbody != null)
            {
                rootRigidbody.position = initialPosition;
            }
        }
    }

    public void TreeRushAttack()
    {
        //leave russling or rumbling sounds?
        PlaySound(trunkAttackSound);

        Rigidbody2D treeObject = Instantiate(TreePrefab, new Vector3(-52.96f, -11.37322f, 0f), Quaternion.identity);
        
        Rigidbody2D treeRigidbody = treeObject.GetComponent<Rigidbody2D>();

        StartCoroutine(MoveTree(treeRigidbody, new Vector3(27.55f, -11.37322f, 0f), 6));
        Destroy(treeObject.gameObject, 4);
    }

    public IEnumerator MoveTree(Rigidbody2D treeRigidbody, Vector3 targetPosition, float duration)
    {
        float elapsed_time = 0f;
        Vector3 initialPosition = treeRigidbody.position;

        while (elapsed_time < duration && treeRigidbody != null)
        {
            treeRigidbody.position =
                Vector3.MoveTowards(initialPosition, targetPosition, (elapsed_time / duration) * trunkMoveSpeed);
            elapsed_time += Time.deltaTime;
            yield return null;
        }

        if (treeRigidbody != null)
        {
            treeRigidbody.position = targetPosition;
        }
    }
    
    public IEnumerator P2LSideDropStuff()
    {
        PlaySound(dropObjectSound);

        for (int i = 0; i < P2TreeDropStuffAmount; i++)
        {
            GameObject prefabToSpawn = UnityEngine.Random.Range(0f, 1f) < 0.50f
                ? LeftTreeApplePrefab.gameObject
                : AngrySpikeL.gameObject;
            float randomX = UnityEngine.Random.Range(-37.46f, -25.02f);
            Vector3 randomPosition = new Vector3(randomX, 11.52f, 0f);
            GameObject instantiatedObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
            fallingObject = instantiatedObject.GetComponent<Rigidbody2D>();
            PlaySound(dropObjectSound);

            yield return new WaitForSeconds(1.2f);
        }
    }
    
    public IEnumerator P2RSideDropStuff()
    {
        PlaySound(dropObjectSound);

        for (int i = 0; i < P2TreeDropStuffAmount; i++)
        {
            GameObject prefabToSpawn = UnityEngine.Random.Range(0f, 1f) < 0.50f
                ? RightTreeApplePrefab.gameObject
                : AngrySpikeR.gameObject;
            float randomX = UnityEngine.Random.Range(-13.68f, 1f);
            Vector3 randomPosition = new Vector3(randomX, 11.52f, 0f);

            GameObject instantiatedObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
            fallingObject = instantiatedObject.GetComponent<Rigidbody2D>();

            PlaySound(dropObjectSound);

            yield return new WaitForSeconds(1.2f);
        }
    }
    
    public IEnumerator P2SpinDropAttack()
    {
        PlaySound(dropObjectSound);

        for (int i = 0; i < P2TreeSpinDropStuffAmount; i++)
        {
            int leftOrRight = UnityEngine.Random.Range(1, 3);

            GameObject prefabToSpawn;
            if (leftOrRight == 1) // Left
            {
                prefabToSpawn = UnityEngine.Random.Range(0f, 1f) < 0.3f
                    ? LeftTreeApplePrefab.gameObject
                    : AngryWormL.gameObject;
                float randomX = UnityEngine.Random.Range(-37f, -32.9f);
                float randomY = UnityEngine.Random.Range(5.5f, 8f);
                float randomX2 = UnityEngine.Random.Range(-26.6f, -20.24f);
                Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
                Vector3 randomPosition2 = new Vector3(randomX2, 9.85f, 0f);
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    pickedPosition = randomPosition;
                }
                else
                {
                    pickedPosition = randomPosition2;
                }

                GameObject instantiatedObject = Instantiate(prefabToSpawn, pickedPosition, Quaternion.identity);
                fallingObject = instantiatedObject.GetComponent<Rigidbody2D>();
            }

            if (leftOrRight == 2) // Right
            {
                prefabToSpawn = UnityEngine.Random.Range(0f, 1f) < 0.3f
                    ? RightTreeApplePrefab.gameObject
                    : AngryWormR.gameObject;
                float randomX = UnityEngine.Random.Range(0f, 4f);
                float randomY = UnityEngine.Random.Range(5.5f, 8f);
                float randomX2 = UnityEngine.Random.Range(-13f, -6f);
                Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
                Vector3 randomPosition2 = new Vector3(randomX2, 9.85f, 0f);
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    pickedPosition = randomPosition;
                }
                else
                {
                    pickedPosition = randomPosition2;
                }

                GameObject instantiatedObject = Instantiate(prefabToSpawn, pickedPosition, Quaternion.identity);
                fallingObject = instantiatedObject.GetComponent<Rigidbody2D>();
            }

            yield return new WaitForSeconds(0.9f);
        }
    }
    
    private void PlaySound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
