using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;

    public GameObject player;

    public GameObject[] asteroidModels;

    string[][] asteroidData;

    // Start is called before the first frame update
    void Start()
    {
        asteroidData = ReadCSV("asteroids");
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string[][] ReadCSV(string path) {
        List<string[]> data = new List<string[]>();
        TextAsset csv = Resources.Load(path) as TextAsset;
        // Debug.Log(Resources.Load(path));
        StringReader reader = new StringReader(csv.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            string[] items = line.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            // Debug.Log(items);
            data.Add(items);
        }
        return data.ToArray();
    }

    IEnumerator<UnityEngine.WaitForSeconds> Spawn() {
        while (true) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 4));

            GameObject a = Instantiate(asteroidPrefab);

            string[] row = asteroidData[UnityEngine.Random.Range(0, asteroidData.Length)];
            a.GetComponent<Asteroid>().name = row[1];
            Debug.Log(row[1]);
            a.GetComponent<Asteroid>().size = float.Parse(row[2]);

            GameObject b = Instantiate(asteroidModels[UnityEngine.Random.Range(0, asteroidModels.Length)]);
            a.GetComponent<MeshFilter>().sharedMesh = b.GetComponentInChildren<MeshFilter>().sharedMesh;
            Destroy(b);

            switch(UnityEngine.Random.Range(0, 3)) {
            case 0: // left
                a.transform.position = new Vector3(-75, 0, UnityEngine.Random.Range(-43.5f, 43.5f));
                break;
            case 1: // right
                a.transform.position = new Vector3(75, 0, UnityEngine.Random.Range(-43.5f, 43.5f));
                break;
            case 2: // top
                a.transform.position = new Vector3(UnityEngine.Random.Range(-75f, 75f), 0, 43.5f);
                break;
            case 3: // top
                a.transform.position = new Vector3(UnityEngine.Random.Range(-75f, 75f), 0, -43.5f);
                break;
            }

            a.GetComponent<Asteroid>().velocity = (player.transform.position - a.transform.position) / UnityEngine.Random.Range(4.5f, 6.5f);
            a.GetComponent<Asteroid>().startNow = true;
        }
    }
}
