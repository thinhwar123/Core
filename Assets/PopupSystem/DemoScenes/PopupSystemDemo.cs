using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABIPlugins;


public class PopupSystemDemo : MonoBehaviour
{

    void Awake()
    {
        Invoke("ShowExamplePopup", 1);
    }
    void TestShowInterAd()
    {

    }

    void ShowExamplePopup()
    {
        PopupManager.CreateNewInstance<ExamplePopup>().Show("This is example popup");
    }

    public void ShowFistInfoPopup()
    {
        PopupManager.CreateNewInstance<InfoPopup>().Show("Info Dialog", "Đây là Info Popup xuất hiện lần đầu tiên", true);
    }

    public void ShowInfoPopupNewInstance()
    {
        PopupManager.CreateNewInstance<InfoPopup>().Show("Info Dialog", "Đây là Info Popup nhưng tạo thêm một thể hiện mới", true);
    }

    public void ShowInfoPopupOnlyOneInstance()
    {
        InfoPopup.Instance.Show("Info Dialog", "Đây là Info Popup sử dụng lại", true);
    }

    void ShowConfirmPopup()
    {
        PopupManager.CreateNewInstance<ConfirmPopup>().Show("Confirm Dialog", "Đây là Confirm dialog", true);
    }

    void ShowQuestionPopup()
    {
        PopupManager.CreateNewInstance<QuestionPopup>().Show("Question Dialog", "Đây là Question dialog", true);
    }
}
