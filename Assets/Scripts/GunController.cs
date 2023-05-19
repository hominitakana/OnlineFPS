using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    //カメラ
    private Camera cam;

    // 武器の格納リスト
    public List<Gun> guns = new List<Gun>();
    // 選択中の武器管理用数値
    private int selectedGun = 0;

    private float shotTimer;//射撃間隔
    [Tooltip("所持弾薬")]
    public int[] ammunition;
    [Tooltip("最高所持弾薬数")]
    public int[] maxAmmunition;
    [Tooltip("マガジン内の弾数")]
    public int[] ammoClip;
    [Tooltip("マガジンに入る最大の数")]
    public int[] maxAmmoClip;

    UIManager uIManager;//UI管理
    private void Awake()
    {
        //タグからUIManagerを探す
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //カメラ格納
        //タグがついているとこれだけでとれる。
        cam = Camera.main;

        //弾薬テキスト更新
        uIManager.SettingBulletsText(ammoClip[selectedGun], ammunition[selectedGun]);

    }

    // Update is called once per frame
    void Update()
    {
        // 覗き込み関数の呼び出し
        Aim();

        //武器の変更キー検知関数
        SwitchingGuns();

        //射撃関数
        Fire();

        //リロード関数を呼ぶ
        Reload();
    }

    //初期設定では0.02秒ごとに呼ばれる
    private void FixedUpdate()
    {
        //弾薬テキスト更新
        uIManager.SettingBulletsText(ammoClip[selectedGun], ammunition[selectedGun]);
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

    public void Aim(){
        // 右クリックの検知
        if(Input.GetMouseButton(1)){
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,
                                        guns[selectedGun].adsZoom,
                                        guns[selectedGun].adsSpeed* Time.deltaTime);
        }
        else{
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,
                                        60,
                                        guns[selectedGun].adsSpeed* Time.deltaTime);
        }
    }

    /// <summary>
    /// 左クリックの検知
    /// </summary>
    public void Fire()
    {
        
        if (Input.GetMouseButton(0) && ammoClip[selectedGun] > 0 && Time.time > shotTimer)
        {
            FiringBullet();
        }

    }

    /// <summary>
    /// 弾丸の発射
    /// </summary>
    private void FiringBullet()
    {
        //選択中の銃の弾薬減らす
        ammoClip[selectedGun]--;

        //Ray(光線)をカメラの中央からに設定
        Ray ray = cam.ViewportPointToRay(new Vector2(.5f, .5f));//カメラの中心がこの数値


        //レイを飛ばす（開始地点と方向、当たったコライダーの情報格納）
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Debug.Log("当たったオブジェクトは" + hit.collider.gameObject.name);

            // 弾痕を当たった場所に生成する
            GameObject bulletImpactObject = Instantiate(guns[selectedGun].bulletImpactObject,
                hit.point + (hit.normal * 0.02f), //rayが当たった場所 エフェクトがちらつかないように + (hit.normal * 0.02f)
                Quaternion.LookRotation(hit.normal,Vector3.up));// Quaternion LookRotation (Vector3 forward, Vector3 upwards= Vector3.up) 指定された forward と upwards 方向に回転します。
                //hit.normal: rayが当たった面から出る法線
                //Vector3.up: y軸が↑方向

            Destroy(bulletImpactObject,10f);
        }

        //射撃間隔を設定
        shotTimer = Time.time + guns[selectedGun].shootInterval;


    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //リロードで補充する弾数を取得する
            int amountNeed = maxAmmoClip[selectedGun] - ammoClip[selectedGun];

            //必要な弾薬量と所持弾薬量を比較
            int ammoAvailable = amountNeed < ammunition[selectedGun] ? amountNeed : ammunition[selectedGun];

            //弾薬が満タンの時はリロードできない&弾薬を所持しているとき
            if (amountNeed != 0 && ammunition[selectedGun] != 0)
            {
                //所持弾薬からリロードする弾薬分を引く
                ammunition[selectedGun] -= ammoAvailable;
                //銃に装填する
                ammoClip[selectedGun] += ammoAvailable;
            }
        }
    }
}
