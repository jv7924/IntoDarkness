using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CandleLight : MonoBehaviour
{
    private GameObject Player;
    private Rigidbody2D playerRb;
    private Light2D light2d;
    
    [SerializeField]
    public enum diminishTypeEnum{Constant, Lerp};
    public diminishTypeEnum diminishType;
    [SerializeField]
    public float diminishValue;
    [SerializeField]
    public float outerRadiusGameOver;

    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        playerRb = Player.GetComponent<Rigidbody2D>();
        light2d = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.x != 0){
            switch(diminishType){
                case diminishTypeEnum.Constant:
                    light2d.pointLightInnerRadius -= diminishValue / 1000;
                    light2d.pointLightOuterRadius -= diminishValue / 1000;
                    break;
                case diminishTypeEnum.Lerp:
                    light2d.pointLightInnerRadius *= diminishValue;
                    light2d.pointLightOuterRadius *= diminishValue;
                    break;
            }
        }
        if (light2d.pointLightOuterRadius <= outerRadiusGameOver){
            GameStateManager.GameOver();
            Debug.Log("GAME OVER");
        }
    }
}
