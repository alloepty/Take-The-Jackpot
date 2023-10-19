using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyDetectEnum{
    detected,
    notDetected,
}



public class Enemy_Detect : MonoBehaviour
{
    [SerializeField] float Timer;
    [SerializeField] Collider col;

    EnemyDetectEnum enemyCurrentState = EnemyDetectEnum.notDetected;

    private void Update()
    {
        if (enemyCurrentState == EnemyDetectEnum.detected)
        {
            StartCoroutine(Detect_Player());
           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Detect_Player());
        if(other.TryGetComponent<PlayerController>(out PlayerController pla))
        {
           enemyCurrentState = EnemyDetectEnum.detected;
        }
    }
    IEnumerator Detect_Player()
    {
        yield return new WaitForSeconds(Timer);
    }
}
