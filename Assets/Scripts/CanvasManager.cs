using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager _instance;

    public static CanvasManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    int score;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject labelPrefab;

    public void NewLabel(Asteroid a) {
        GameObject l = Instantiate(labelPrefab);
        l.transform.SetParent(transform);

        l.GetComponent<Label>().target = a.transform;
        l.GetComponent<Label>().name = a.name;
    }

    public void ReloadScene() {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void UpdateScore(int delta) {
        score += delta;

        scoreText.text = "SCORE: " + score.ToString();
    }
}
