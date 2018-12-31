using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
[RequireComponent(typeof(AudioSource))]

public class Move : MonoBehaviour
{
    // move keys (can be bound in the UI)
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode rightKey;
    public KeyCode leftKey;

    public float speed = 1;
    public float lineSize = 3;

    public GameObject wallPrefab;
    public GameObject deathParticlesPrefab;

    //private AudioSource sound;
    public Color lightwallColor;


    float shakeMagnitude = 20f;
    float shakeRoughness = .1f;
    float shakeFadeIn = .1f;
    float shakeFadeOut = .2f;


    public AudioClip turnSound; //set this in ispector with audiofile
    public AudioClip deathSound; //set this in ispector with audiofile


    KeyCode lastKey;
    Vector2 lastWallPos;

    Collider2D wall;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        spawnWall();
        lastKey = upKey;

        //sound = GetComponent<AudioSource>();
        //particle = GetComponent<ParticleSystem>();
        //print(particle + "a");
        lastWallPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(upKey) && lastKey != upKey && lastKey != downKey)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
            transform.eulerAngles = new Vector3(0, 0, 0);


            spawnWall();

            lastKey = upKey;
            lastWallPos = transform.position;

            CameraShaker.Instance.ShakeOnce(shakeMagnitude, shakeRoughness, shakeFadeIn, shakeFadeOut, Vector3.down, Vector3.zero);
            AudioSource.PlayClipAtPoint(turnSound, transform.position);
        }
        else if (Input.GetKeyDown(downKey) && lastKey != upKey && lastKey != downKey)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
            transform.eulerAngles = new Vector3(0,0,180);

            spawnWall();

            lastKey = downKey;
            lastWallPos = transform.position;

            CameraShaker.Instance.ShakeOnce(shakeMagnitude, shakeRoughness, shakeFadeIn, shakeFadeOut, Vector3.up, Vector3.zero);
            AudioSource.PlayClipAtPoint(turnSound, transform.position);
        }
        else if (Input.GetKeyDown(leftKey) && lastKey != rightKey && lastKey != leftKey)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
            transform.eulerAngles = new Vector3(0, 0, 90);

            spawnWall();

            lastKey = leftKey;
            lastWallPos = transform.position;

            CameraShaker.Instance.ShakeOnce(shakeMagnitude, shakeRoughness, shakeFadeIn, shakeFadeOut, Vector3.right, Vector3.zero);
            AudioSource.PlayClipAtPoint(turnSound, transform.position);
        }
        else if (Input.GetKeyDown(rightKey) && lastKey != rightKey && lastKey != leftKey)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            transform.eulerAngles = new Vector3(0, 0, 270);

            spawnWall();

            lastKey = rightKey;
            lastWallPos = transform.position;

            CameraShaker.Instance.ShakeOnce(shakeMagnitude, shakeRoughness, shakeFadeIn, shakeFadeOut, Vector3.left, Vector3.zero);
            AudioSource.PlayClipAtPoint(turnSound, transform.position);
        }

        fitColliderBetween(wall, lastWallPos, transform.position);

    }

    void spawnWall()
    {
        GameObject g = (GameObject)Instantiate(wallPrefab, transform.position, Quaternion.identity);

        g.GetComponent<SpriteRenderer>().color = lightwallColor;

        print(transform.position);
        wall = g.GetComponent<Collider2D>();
        //print(wall.);

    }

    void fitColliderBetween(Collider2D co, Vector3 a, Vector3 b)
    {

        float dist = Vector2.Distance(a, b);
        if (Mathf.Approximately(a.y, b.y))
        {
            co.transform.position = a + (b - a) * 0.5f ;
            co.transform.localScale = new Vector3(dist + lineSize, lineSize);
        }

        else if(Mathf.Approximately(a.x, b.x))
        {
            co.transform.position = a + (b - a) * 0.5f;
            co.transform.localScale = new Vector3(lineSize, dist + lineSize);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision != wall)
        {
            if (deathParticlesPrefab)
            {
                GameObject deathParticles = (GameObject)Instantiate(deathParticlesPrefab, transform.position, deathParticlesPrefab.transform.rotation);
                Destroy(deathParticles, deathParticles.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
            }

            CameraShaker.Instance.ShakeOnce(4f, 5f, .1f, 2f);

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 100f);


            print("Player out:" + name);
            FindObjectOfType<GameManager>().RemovePlayer(gameObject);
            gameObject.transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
