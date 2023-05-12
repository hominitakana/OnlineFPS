using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    //カメラの親オブジェクト
    public Transform viewPoint;

    //視点移動の速度
    public float mouseSensitivity = 1f;

    //ユーザーのマウス入力格納
    private Vector2 mouseInput;

    //y軸の回転格納
    private float verticalMouseInput;
    //カメラ
    private Camera cam;


    void Start()
    {
        //カメラ格納
        //タグがついているとこれだけでとれる。
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //視点移動関数の呼び出し
        PlayertRotate();
    }

    //視点移動関数
    public void PlayertRotate()
    {
        //変数にユーザーのマウスの動きを格納
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X")* mouseSensitivity,
            Input.GetAxisRaw("Mouse Y")* mouseSensitivity);
        
        //マウスのX軸の動きを反映
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
            transform.eulerAngles.y + mouseInput.x,
            transform.eulerAngles.z);

        //y軸の値に現在の値を足す
        verticalMouseInput += mouseInput.y;

        //数値を丸める
        verticalMouseInput = Mathf.Clamp(verticalMouseInput, -60f, 60f);

        viewPoint.rotation = Quaternion.Euler(-verticalMouseInput,
            viewPoint.transform.rotation.eulerAngles.y,
            viewPoint.transform.rotation.eulerAngles.z);
            
    }

    private void  LateUpdate()
    {   
        //カメラの位置調整
        cam.transform.position = viewPoint.position;

        //回転
        cam.transform.rotation = viewPoint.rotation;
    }


}
