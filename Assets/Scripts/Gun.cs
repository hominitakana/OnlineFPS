
using UnityEngine;

public class Gun : MonoBehaviour
{   
    //変数
    //射撃間隔
    [Tooltip("射撃間隔")]//補足説明
    public float shootInterval = 0.1f;

    //威力
    [Tooltip("威力")]//補足説明
    public float shotDamage;

    //覗き込む時のズーム
    [Tooltip("覗き込む時のズーム")]//補足説明
    public float adsZoom;

    //覗き込む時の速度
    [Tooltip("覗き込む時の速度")]//補足説明
    public float adsSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
