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



    // Start is called before the first frame update
    void Start()
    {
        //カメラ格納
        //タグがついているとこれだけでとれる。
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        // 覗き込み関数の呼び出し
        Aim();

        //武器の変更キー検知関数
        SwitchingGuns();
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
}
