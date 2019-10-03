using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplit : MonoBehaviour
{
    #region BulletSplitProperties
    [SerializeField] private float boltSpeed = 5;
    [SerializeField] private int numChildren = 0;
    [SerializeField] private GameObject bolts = null;
    private List<Vector3> EnemyPositions = null;
    private int layer = 0;
    #endregion

    private void Start()
    {
        layer = gameObject.layer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.tag;
        if (tag.Equals("Enemy"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            EnemyPositions = new List<Vector3>();
            foreach (GameObject enemy in enemies)
                EnemyPositions.Add(enemy.transform.position);

            if (EnemyPositions.Count > 0)
            {
                Collider collider = collision.collider;
                GameObject hit = collider.gameObject;

                Transform currentTransform = hit.GetComponent<Transform>();
                EnemyPositions.Remove(currentTransform.position);
                Vector3 currentPosition = currentTransform.Find("Shot Position").position;

                for (int i = 0; i < numChildren; i++)
                {
                    if (EnemyPositions.Count-1 >= i)
                    {
                        GameObject bolt = Instantiate(bolts, currentPosition, gameObject.transform.rotation);
                        Transform boltTransform = bolt.GetComponent<Transform>();
                        bolt.GetComponent<CollisionScript>().SetOwner(hit);
                        bolt.layer = layer;
                        Vector3 target = EnemyPositions[i];
                        boltTransform.LookAt(new Vector3(target.x, currentPosition.y, target.z));
                        Rigidbody rb = bolt.GetComponent<Rigidbody>();

                        rb.velocity = Vector3.Normalize(boltTransform.forward) * boltSpeed;
                        //Zap sound effect could go here
                    }
                }
            }
        }
    }
}
