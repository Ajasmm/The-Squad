using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GunInfo_UI : MonoBehaviour
{
    [SerializeField] GunInfo gunInfo;
    [SerializeField] TMP_Text infoText;

    private void Start()
    {
        gunInfo.AddChangeListener(UpdateInfo);
    }
    private void OnDestroy()
    {
        gunInfo.RemoveChangeListener(UpdateInfo);
    }

    private void UpdateInfo(int bulletInMag, int bulletInHand)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(bulletInMag.ToString());
        sb.Append(" / ");
        sb.Append(bulletInHand.ToString());
        this.infoText.text = sb.ToString();
    }
}
