using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparkle : MonoBehaviour
{

    [Tooltip("The fire prefab which starts after the sparkle ends")]
    public GameObject fire;
    [Tooltip("The lifetime (in seconds) of this sparkle")]
    public float lifetime = 1f;
    [Tooltip("The radius in which this spark can set a fire")]
    public float radius;

    private float timeOfDeath;

    [Tooltip("Draw gizmos for circlecast")]
    public bool drawGizmos;

    // Start is called before the first frame update
    void Start()
    {
        timeOfDeath = Time.time + lifetime; 
    }

    // Update is called once per frame
    void Update()
    {
        Burn();
    }

    /// <summary>
    /// Burns the sparkle until it dies and starts a fire.
    /// </summary>
    public void Burn()
    {
        if (Time.time > timeOfDeath)
        {
            StartFire();
            Die();
        }
    }

    /// <summary>
    /// Draws debug gizmos
    /// </summary>
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }

    /// <summary>
    /// Starts a fire near this sparkle
    /// </summary>
    public void StartFire()
    {
        // Circlecast to find any close colliders
        RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, radius, Vector2.up, 0);
        if (hit)
        {
            //TODO: Remove debug ray
            if (drawGizmos)
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1);
            }

            // Find closest point between hit point and sparkle.
            Vector2 closestPoint = hit.transform.GetComponent<PolygonCollider2D>().ClosestPoint(this.transform.position);
            // Instantiate new fire on closestpoint
            Fire newFire = Instantiate(fire, closestPoint, Quaternion.identity, this.transform.parent).GetComponent<Fire>();
            // Rotate towards hit normal
            newFire.transform.up = hit.normal;
            // Make fire bidirectional
            newFire.Direction = FireDirection.BOTH;
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
