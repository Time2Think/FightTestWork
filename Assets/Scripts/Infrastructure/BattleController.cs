using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    [SerializeField]
    private LevelConfig Config;
    public List<Enemy> Enemies;
    [SerializeField]
    private int currWave;
    [SerializeField]
    private TextMeshProUGUI _waveView;
    public GameObject Lose;
    public GameObject Win;
    
    
    private void Start()
    {
        SpawnWave();
    }

    public void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        if(Enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        UpdateStatusWave();
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return;
        }

        var wave = Config.Waves[currWave];
        foreach (var character in wave.Enemies)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(character, pos, Quaternion.identity);
        }
        currWave++;
    }

    private void UpdateStatusWave()
    {
        _waveView.text = $" Wave: {currWave} / {Config.Waves.Length}";
    }
    
    public void GameOver()
    {
        Lose.SetActive(true);
    }
    
    public void Reset()
    {
       SceneManager.LoadScene(0);
    }
}
