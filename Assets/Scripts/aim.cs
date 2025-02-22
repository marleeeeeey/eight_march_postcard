using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour
{
    public float driftRadius = 5f;
    public float driftSpeed = 1f;
    public float calculationPeriod = 0.1f;
    public GameObject explosionPrefab;
    public Score scoreObject;
    
    private int score = 0;
    private Vector3 targetPosition;
    private AudioSource shootSound;

    void Start()
    {
        Cursor.visible = false;
        shootSound = GetComponent<AudioSource>();
        StartCoroutine(UpdateTargetPosition());
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, driftSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0)) // Check if the left mouse button is pressed
        {
            Shoot();
        }
    }


    void Shoot()
    {
        if (shootSound)
        {
            shootSound.Play();
        }

        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (hit && hit.CompareTag("Sweet"))
        {
            Destroy(hit.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            score++;
            scoreObject.SetScore(score);
        }
    }

    IEnumerator UpdateTargetPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(calculationPeriod);
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0f;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldMousePosition.z = 0f;

            Vector3 newTarget = worldMousePosition + new Vector3(
                Random.Range(-driftRadius, driftRadius),
                Random.Range(-driftRadius, driftRadius),
                0);

            if (Vector3.Distance(worldMousePosition, newTarget) > driftRadius)
            {
                newTarget = Vector3.Lerp(newTarget, worldMousePosition, 0.5f);
            }

            targetPosition = newTarget;
        }
    }
}