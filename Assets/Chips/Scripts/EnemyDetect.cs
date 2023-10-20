using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


enum EnemyDetectEnum {
    detect,
    notDetect,
    wait,
}

enum EnemyGoingToEnum {
    up,
    down,
}


public class Enemy_Detect : MonoBehaviour {
    [SerializeField] float Timer;
    [SerializeField] float DetectTime;
    [SerializeField] float speed;
    [SerializeField] private Transform position1, position2;
    [SerializeField] Image detect_image;
    [SerializeField] GameObject enemy;
    [SerializeField] float normalSpeed;
    [SerializeField] float EnemyStayTimeMax;

    private bool _switch = false;
    EnemyDetectEnum currentEnemyState = EnemyDetectEnum.notDetect;
    EnemyGoingToEnum currentEnemyGoingToState = EnemyGoingToEnum.up;
    public bool lose = false;

    float percent;
    float currentPosition;
  
    float zeroSpeed = 0;
    float EnemyStayTimer = 0;    
    bool move_pull = true; 

    private void Update() {
        float enemySpeed = speed * Time.deltaTime;
        if (currentEnemyState == EnemyDetectEnum.notDetect) {
            switch(currentEnemyGoingToState) {
                case EnemyGoingToEnum.up:
                    EnemyTransformPosition(enemy.transform.position, position1.position, enemySpeed);
                    EnemyEndpointChange(enemy.transform.position, position1.position, EnemyGoingToEnum.down);
                    break;
                case EnemyGoingToEnum.down:
                    EnemyTransformPosition(enemy.transform.position, position2.position, enemySpeed);
                    EnemyEndpointChange(enemy.transform.position, position2.position, EnemyGoingToEnum.up);
                    break;
            }
        }
        
        ImageFill1();

        if (currentEnemyState == EnemyDetectEnum.detect) {
            Detected();
            DetectUp();
        }
        else {
            DetectDown();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<PlayerController>(out PlayerController pla)) {
            currentEnemyState = EnemyDetectEnum.wait;            
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.TryGetComponent<PlayerController>(out PlayerController pla)) {
            currentEnemyState = EnemyDetectEnum.notDetect;
        }
    }

    private void DetectUp() {
        DetectTime += 0.1f;   
    }

    private void DetectDown() {
        if (DetectTime > 0){
            DetectTime -= 0.1f;
            if (DetectTime <= 0) {
                DetectTime = 0;
                currentEnemyState = EnemyDetectEnum.notDetect;
            }
        }
    }

    private void Detected() {
        if (DetectTime >= Timer) {
            Debug.Log("Detected");
            UIAdministrator.Menu.LoseMenu.active = true;
            Time.timeScale = 0;
        }
    }

    void ImageFill1() {
        percent = DetectTime / Timer;
        detect_image.fillAmount = percent;
    }

    void EnemyStay() {
        EnemyStayTimer += 0.1f;
        speed = zeroSpeed;
        if(EnemyStayTimer >= EnemyStayTimeMax) {
            speed = normalSpeed;
        }
    }

    void EnemyMove() {
        if(EnemyStayTimer >= 0) {
            EnemyStayTimer = 0;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(position1.position, position2.position);
    }

    private void EnemyTransformPosition(
        Vector3 current,
        Vector3 target,
        float maxDistanceDelta
    ) {
        enemy.transform.position = Vector3.MoveTowards(current, target, maxDistanceDelta);
        EnemyStay();
    }

    private void EnemyEndpointChange(
        Vector3 currentPosition,
        Vector3 targetPosition,
        EnemyGoingToEnum goingTo
    ) {
        if(currentPosition == targetPosition) {
            currentEnemyGoingToState = goingTo;
            EnemyMove();
        }
    }
}
