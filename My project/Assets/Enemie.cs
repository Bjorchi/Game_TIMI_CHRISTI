using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    public float speed = 3f;
    public float rechts = 5f; // Beispielwert, du kannst ihn anpassen
    public float links = -5f; // Beispielwert, du kannst ihn anpassen
    private Vector3 rotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rechts = transform.position.x + rechts;
        links = transform.position.x - links;
        rotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < links)
        {
            transform.eulerAngles = rotation - new Vector3(0, 180, 0); // Drehen, um in die andere Richtung zu gehen
            speed = -speed; // Richtung ändern
        }
        else if (transform.position.x > rechts)
        {
            transform.eulerAngles = rotation;
            speed = -speed; // Richtung wieder ändern
        }
    }
}
