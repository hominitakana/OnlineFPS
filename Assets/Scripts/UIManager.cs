using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //弾数の表示を行うテキスト
    public Text bulletText;


    public void SettingBulletsText(int ammoClip, int ammunition)
    {
        //テキストに装備中の銃の　マガジン内弾数/所持弾数　を表示
        bulletText.text = ammoClip + "/" + ammunition;
    }

}