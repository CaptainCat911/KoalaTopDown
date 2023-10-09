using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    Transform player;
    Camera cam;
    [SerializeField] float threshold;

    private void Awake()
    {
        player = GameManager.instance.player.transform;
        cam = Camera.main;
    }

    private void Start()
    {
        if (GameManager.instance.forAndroid)
        {
            GameManager.instance.cameraOnPlayer = true;
        }
    }

    void Update()
    {
        if (!GameManager.instance.cameraOnPlayer)       
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);     
            Vector3 targetPos = (player.position + mousePos) / 2f;

            targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);

            transform.position = targetPos;
        }
        else                // в синематиках смотрим на игрока
        {
            transform.position = player.position;
        }
    }
}
