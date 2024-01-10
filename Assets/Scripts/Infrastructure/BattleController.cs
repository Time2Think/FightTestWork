using System.Collections.Generic;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    [SerializeField]
    private LevelConfig Config;
    [SerializeField]
    private GameObject _littleEnemyPrefab;
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
        if (enemy.TypeEnemy == EnemyType.Big)
        {
            for (int i = 0; i < 2; i++)
            {
                SpawnEnemy(_littleEnemyPrefab, enemy.transform.position);
            }
        }
        Enemies.Remove(enemy);
        if(Enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        Enemies.Clear();
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return;
        }

        var wave = Config.Waves[currWave];
        foreach (var character in wave.Enemies)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            SpawnEnemy(character,pos);
        }
        currWave++;
        UpdateStatusWave();
    }

    private void SpawnEnemy(GameObject character, Vector3 position)
    {
       GameObject newEnemy =  Instantiate(character, position, Quaternion.identity);
        var enemy = newEnemy.GetComponent<Enemy>();
        AddEnemy(enemy);
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
