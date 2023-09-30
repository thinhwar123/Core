using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABIPlugins;
public class TemplatePopup : SingletonPopup<TemplatePopup> { 
    // Kéo prefab PopupManager vào Scene đầu tiên của gme
    // Step 1: Kế thừa SingletonPopup

	// Step 2: Triển khai hàm Show với tham số bất kỳ. 
	public void Show(object anyType1, object anyType2, object anyTypeN,  bool overlay = true) {
		// Implement here ....

		base.Show(null, overlay); //Bắt buộc 
	}

    // Step 3: Sau khi thiết kế xong UI thì kéo vào thư mục Prefabs.

	// Step 4: Kéo thả popup ref vào Prefabs của PopupManager

	// Step 5: Nếu muốn thay thế Animation mặc định lúc hiển thị và ẩn popup. Thì thiết kế Animator 
    // và khai báo lại tên showAnimationName, hideAnimationName trên editor và lưu vào prefab.
}
