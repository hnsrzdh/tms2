/**=====================
    Tree JS Start
==========================**/
let data = [
    {
      id: "0",
      text: "برنامه‌ها",
      children: [
        {
          id: "0-0",
          text: "فروشگاه اینترنتی",
          children: [
            { id: "0-0-0", text: "محصول" },
            { id: "0-0-1", text: "سبد خرید" },
            { id: "0-0-2", text: "فاکتور" },
          ],
        },
        {
          id: "0-1",
          text: "کاربران",
          children: [
            { id: "0-1-0", text: "پروفایل کاربر" },
            { id: "0-1-1", text: "ویرایش کاربر" },
            { id: "0-1-2", text: "کارت‌های کاربر" },
          ],
        },
        {
          id: "0-2",
          text: "گفتگو",
          children: [
            { id: "0-2-0", text: "اپلیکیشن گفتگو" },
            { id: "0-2-1", text: "تماس تصویری" },
          ],
        },
      ],
    },
    {
      id: "1",
      text: "اجزا",
      children: [
        {
          id: "1-0",
          text: "کیت‌های UI",
          children: [
            { id: "1-0-0", text: "تایپوگرافی" },
            { id: "1-0-1", text: "آواتارها" },
            { id: "1-0-2", text: "گرید" },
          ],
        },
        {
          id: "1-1",
          text: "رابط کاربری اضافی",
          children: [
            { id: "1-1-0", text: "اعلان‌ها" },
            { id: "1-1-1", text: "امتیازدهی" },
            { id: "1-1-2", text: "صفحه‌بندی" },
          ],
        },
        {
          id: "1-2",
          text: "نمودارها",
          children: [
            { id: "1-2-0", text: "نمودار Apex" },
            { id: "1-2-1", text: "نمودار Google" },
            { id: "1-2-2", text: "Echarts" },
          ],
        },
      ],
    },
    {
      id: "2",
      text: "متفرقه",
      children: [
        {
          id: "2-0",
          text: "گالری",
          children: [
            { id: "2-0-0", text: "گرید گالری" },
            { id: "2-0-1", text: "توضیحات گرید گالری" },
            { id: "2-0-2", text: "گالری ماسونری" },
          ],
        },
        {
          id: "2-1",
          text: "وبلاگ",
          children: [
            { id: "2-1-0", text: "جزئیات وبلاگ" },
            { id: "2-1-1", text: "وبلاگ تکی" },
            { id: "2-1-2", text: "افزودن پست" },
          ],
        },
        {
          id: "2-2",
          text: "ویرایشگرها",
          children: [
            { id: "2-2-0", text: "ویرایشگر Summer-note" },
            { id: "2-2-1", text: "ویرایشگر CK" },
            { id: "2-2-2", text: "ویرایشگر MDE" },
          ],
        },
      ],
    },
  ];
  
  let tree = new Tree(".tree-container", {
    data: [{ id: "-1", text: "ریشه", children: data }],
    closeDepth: 3,
    loaded: function () {
      this.values = ["0-0-0", "0-1-1"];
      console.log(this.selectedNodes);
      console.log(this.values);
    },
    onChange: function () {
      console.log(this.values);
    },
  });
  
  // درخت غیرفعال
  let data1 = [
    {
      id: "0",
      text: "برنامه‌ها",
      children: [
        {
          id: "0-0",
          text: "فروشگاه اینترنتی",
          children: [
            { id: "0-0-0", text: "محصول" },
            { id: "0-0-1", text: "سبد خرید" },
            { id: "0-0-2", text: "فاکتور" },
          ],
        },
        {
          id: "0-1",
          text: "کاربران",
          children: [
            { id: "0-1-0", text: "پروفایل کاربر" },
            { id: "0-1-1", text: "ویرایش کاربر" },
            { id: "0-1-2", text: "کارت‌های کاربر" },
          ],
        },
        {
          id: "0-2",
          text: "گفتگو",
          children: [
            { id: "0-2-0", text: "اپلیکیشن گفتگو" },
            { id: "0-2-1", text: "تماس تصویری" },
          ],
        },
      ],
    },
    {
      id: "1",
      text: "اجزا",
      children: [
        {
          id: "1-0",
          text: "کیت‌های UI",
          children: [
            { id: "1-0-0", text: "تایپوگرافی" },
            { id: "1-0-1", text: "آواتارها" },
            { id: "1-0-2", text: "گرید" },
          ],
        },
        {
          id: "1-1",
          text: "رابط کاربری اضافی",
          children: [
            { id: "1-1-0", text: "اعلان‌ها" },
            { id: "1-1-1", text: "امتیازدهی" },
            { id: "1-1-2", text: "صفحه‌بندی" },
          ],
        },
        {
          id: "1-2",
          text: "نمودارها",
          children: [
            { id: "1-2-0", text: "نمودار Apex" },
            { id: "1-2-1", text: "نمودار Google" },
            { id: "1-2-2", text: "Echarts" },
          ],
        },
      ],
    },
    {
      id: "2",
      text: "متفرقه",
      children: [
        {
          id: "2-0",
          text: "گالری",
          children: [
            { id: "2-0-0", text: "گرید گالری" },
            { id: "2-0-1", text: "توضیحات گرید گالری" },
            { id: "2-0-2", text: "گالری ماسونری" },
          ],
        },
        {
          id: "2-1",
          text: "وبلاگ",
          children: [
            { id: "2-1-0", text: "جزئیات وبلاگ" },
            { id: "2-1-1", text: "وبلاگ تکی" },
            { id: "2-1-2", text: "افزودن پست" },
          ],
        },
        {
          id: "2-2",
          text: "ویرایشگرها",
          children: [
            { id: "2-2-0", text: "ویرایشگر Summer-note" },
            { id: "2-2-1", text: "ویرایشگر CK" },
            { id: "2-2-2", text: "ویرایشگر MDE" },
          ],
        },
      ],
    },
    {
      id: "1",
      text: "گره-۱",
      children: [
        {
          id: "1-0",
          text: "گره-۱-۰",
          children: [
            { id: "1-0-0", text: "گره-۱-۰-۰" },
            { id: "1-0-1", text: "گره-۱-۰-۱" },
            { id: "1-0-2", text: "گره-۱-۰-۲" },
          ],
        },
        {
          id: "1-1",
          text: "گره-۱-۱",
          children: [
            { id: "1-1-0", text: "گره-۱-۱-۰" },
            { id: "1-1-1", text: "گره-۱-۱-۱" },
            { id: "1-1-2", text: "گره-۱-۱-۲" },
          ],
        },
        {
          id: "1-2",
          text: "گره-۱-۲",
          children: [
            { id: "1-2-0", text: "گره-۱-۲-۰" },
            { id: "1-2-1", text: "گره-۱-۲-۱" },
            { id: "1-2-2", text: "گره-۱-۲-۲" },
          ],
        },
      ],
    },
    {
      id: "2",
      text: "گره-۲",
      children: [
        {
          id: "2-0",
          text: "گره-۲-۰",
          children: [
            { id: "2-0-0", text: "گره-۲-۰-۰" },
            { id: "2-0-1", text: "گره-۲-۰-۱" },
            { id: "2-0-2", text: "گره-۲-۰-۲" },
          ],
        },
        {
          id: "2-1",
          text: "گره-۲-۱",
          children: [
            { id: "2-1-0", text: "گره-۲-۱-۰" },
            { id: "2-1-1", text: "گره-۲-۱-۱" },
            { id: "2-1-2", text: "گره-۲-۱-۲" },
          ],
        },
        {
          id: "2-2",
          text: "گره-۲-۲",
          children: [
            { id: "2-2-0", text: "گره-۲-۲-۰" },
            { id: "2-2-1", text: "گره-۲-۲-۱" },
            { id: "2-2-2", text: "گره-۲-۲-۲" },
          ],
        },
      ],
    },
  ];
  
  let tree1 = new Tree(".disabled-container", {
    data: [{ id: "-1", text: "ریشه", children: data }],
    closeDepth: 3,
    loaded: function () {
      this.values = ["0-0-0", "0-1-1"];
      console.log(this.selectedNodes);
      console.log(this.values);
      this.disables = ["0", "0-0", "0-0-0", "0-0-1", "0-0-2"];
    },
    onChange: function () {
      console.log(this.values);
    },
  });
  
  /**=====================
      Tree JS Ends
  ==========================**/
