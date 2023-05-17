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

    //入力された移動値格納
    private Vector3 moveDir;

    //進む方向格納
    private Vector3 movement;

    //移動速度
    public float activeMoveSpeed = 4f;

    //ジャンプ力
    public Vector3 jumpForce = new Vector3(0, 6, 0);

    //レイを飛ばすオブジェクトの位置
    public Transform groundCheckPoint;

    //地面レイヤー
    public LayerMask groundLayers;

    //剛体
    private Rigidbody rb;

    //歩きの速度
    public float walkSpeed = 4f;

    //走りの速度
    public float runSpeed = 8f;


    //カーソルの表示判定
    private bool cursorLock = true;

    // 武器の格納リスト
    public List<Gun> guns = new List<Gun>();
    // 選択中の武器管理用数値
    private int selectedGun = 0;



    void Start()
    {
        //カメラ格納
        //タグがついているとこれだけでとれる。
        cam = Camera.main;

        rb = GetComponent<Rigidbody>();

        UpdateCursorLock();

        
    }

    // Update is called once per frame
    void Update()
    {
        //視点移動関数の呼び出し
        PlayertRotate();

        //移動関数の呼び出し
        PlayerMove();
        
        if(IsGround()){
            //ジャンプ関数の呼び出し
            Jump();
            //走り関数の呼び出し
            Run();
            
        }

        //武器の変更キー検知関数
        SwitchingGuns();

        UpdateCursorLock();
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

    //移動関数
    public void PlayerMove(){

        //移動用キーの入力を検知して値を格納する
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0,
            Input.GetAxisRaw("Vertical"));


        //正規化
        //ベクトルの向きを変えずに値を小さくする。
        //正規化して指定の数値をかければ、移動スピードの制御が簡単にできるため。
        //進む方向を出して変数に格納
        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized;

        transform.position += movement * activeMoveSpeed* Time.deltaTime;
        //Time.deltaTimeをかけるのはパソコン間でスペックの差がでないようにするため


    }

    //Updateが呼び出された後に呼び出される
    private void  LateUpdate()
    {   
        //カメラの位置調整
        cam.transform.position = viewPoint.position;
        //回転
        cam.transform.rotation = viewPoint.rotation;
    }


    //ジャンプ関数
    public void Jump(){
        //地面についているかつスペースが押されたときにジャンプ
        if(IsGround() && Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(jumpForce, ForceMode.Impulse);
        }
    }

    //地面についていればTrue
    public bool IsGround(){
        return Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.25f, groundLayers);
    }

    //Run関数
    public void Run(){
        if (Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        }
        else
        {
            activeMoveSpeed = walkSpeed;
        }
    }
    //カーソルのロックの入り切り関数
    public void UpdateCursorLock(){
        //boolを切り替える
        if(Input.GetKeyDown(KeyCode.Escape)){
            cursorLock = false;//表示
        }else if (Input.GetMouseButton(0)){
            cursorLock = true;//非表示
        }

        if(cursorLock){
            Cursor.lockState = CursorLockMode.Locked;
        }else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //武器の変更キー検知関数
    public void SwitchingGuns(){
        // ホイールくるくるで銃の切り替え
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f){
            selectedGun++;

            // リストより大きい数値になっていないか確認
            if (selectedGun >= guns.Count)
            {
                selectedGun = 0;
            }

            switchGun();
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f){
            selectedGun--;

            if(selectedGun < 0){

                //0より小さければリストの最大値-1に設定する
                selectedGun = guns.Count -1;
            }

            switchGun();
        }

        //数値キーの入力検知で武器を切り替える
        for (int i = 0; i < guns.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))//ループの数値＋１をして文字列に変換。その後、押されたか判定
            {
                selectedGun = i;//銃を扱う数値を設定

                //実際に武器を切り替える関数
                switchGun();

            }
        }
    }

    void switchGun(){
        for (int i = 0; i < guns.Count; i++)
        {
            guns[i].gameObject.SetActive(i == selectedGun);

        }
    }


}