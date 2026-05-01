// Tagify JS
("use strict");
// 1. Basic Select

// The DOM element you wish to replace with Tagify
var input = document.querySelector("input[name=basic]");

// initialize Tagify on the above input node reference
new Tagify(input);

// 2. Random suggestions

var input = document.querySelector('input[name="input-custom-dropdown"]'),
  // init Tagify script on the above inputs
  tagify = new Tagify(input, {
           whitelist: [
          "A# .NET",
          "A# (Axiom)",
          "سیستم A-0",
          "A+",
          "Mortran",
          "ماوس",
          "MPD",
          "Mathcad",
          "MSIL – نام منسوخ CIL",
          "MSL",
          "MUMPS",
          "Mystic Programming L",
        ],
    maxTags: 10,
    dropdown: {
      maxItems: 20, // <- mixumum allowed rendered suggestions
      classname: "tags-look", // <- custom classname for this dropdown, so it could be targeted
      enabled: 0, // <- show suggestions on focus
      closeOnSelect: false, // <- do not hide the suggestions dropdown once an item has been selected
    },
  });

// 3. Movie suggestions

var input = document.querySelector("textarea[name=tags2]"),
  tagify = new Tagify(input, {
    enforceWhitelist: true,
    delimiters: null,
    whitelist: [
          "رستگاری در شاوشنک",
          "پدرخوانده",
          "شوالیه تاریکی",
          "۱۲ مرد خشمگین",
          "صندلی داغ",
          "انتقام‌جویان: پایان بازی",
          "آرواره‌ها",
          "بهترین سال‌های زندگی ما",
          "امور پنهان",
          "اودان",
          "نبرد الجزایر",
          "غریبه‌ها در قطار",
          "عصر سگ",
          "شهر گناه",
          "دل‌های مهربان و تاج‌ها",
          "دار و دسته‌های واسایپور",
          "کمک",
        ],
    callbacks: {
      add: console.log, // callback when adding a tag
      remove: console.log, // callback when removing a tag
    },
  });

// 4.Render Suggestions List Manually

var input = document.querySelector("input[name=tags-manual-suggestions]"),
  // init Tagify script on the above inputs
  tagify = new Tagify(input, {
            whitelist: [
          ".NET",
          "مهندس نرم‌افزار",
          "طراح UI/UX",
          "A+",
          "A++",
          "مهندس برق",
          "مهندس شیمی",
          "مهندس عمران",
          "حوزه IT",
          "C#",
          "C++",
          "رمزنگاری",
          "DBMS",
          "JS",
          "نقاش",
          "CTC",
          "Typescript",
          "Vuejs",
          "React",
          "آزمون‌های دولتی",
          "Agda",
          "Agilent VEE",
          "MSIL – نام منسوخ CIL",
          "MSL",
          "MUMPS",
          "Mystic Programming L",
        ],
    dropdown: {
      position: "manual",
      maxItems: Infinity,
      enabled: 0,
      classname: "customSuggestionsList",
    },
    templates: {
      dropdownItemNoMatch() {
        return `<div class='empty'>Nothing Found</div>`;
      },
    },
    enforceWhitelist: true,
  });

tagify
  .on("dropdown:show", onSuggestionsListUpdate)
  .on("dropdown:hide", onSuggestionsListHide)
  .on("dropdown:scroll", onDropdownScroll);

renderSuggestionsList(); // defined down below

// ES2015 argument destructuring
function onSuggestionsListUpdate({ detail: suggestionsElm }) {
  console.log(suggestionsElm);
}

function onSuggestionsListHide() {
  console.log("hide dropdown");
}

function onDropdownScroll(e) {
  console.log(e.detail);
}

// https://developer.mozilla.org/en-US/docs/Web/API/Element/insertAdjacentElement
function renderSuggestionsList() {
  tagify.dropdown.show(); // load the list
  tagify.DOM.scope.parentNode.appendChild(tagify.DOM.dropdown);
}

// 5. Colors select options

var input = document.querySelector("input[name=tags3]"),
  tagify = new Tagify(input, {
    pattern: /^.{0,20}$/, // Validate typed tag(s) by Regex. Here maximum chars length is defined as "20"
    delimiters: ",| ", // add new tags when a comma or a space character is entered
    trim: false, // if "delimiters" setting is using space as a delimeter, then "trim" should be set to "false"
    keepInvalidTags: true, // do not remove invalid tags (but keep them marked as invalid)
    // createInvalidTags: false,
    editTags: {
      clicks: 2, // single click to edit a tag
      keepInvalid: false, // if after editing, tag is invalid, auto-revert
    },
    maxTags: 6,
    blacklist: ["foo", "bar", "baz"],
    whitelist: [
          "معبد",
          "فلج کردن / شوکه کردن",
          "کارآگاه",
          "علامت",
          "اشتیاق",
          "روتین / برنامه روزانه",
          "دک / عرشه",
          "تمیز قضاوت کردن / تبعیض قائل شدن",
          "استراحت / آرامش",
          "تقلب / کلاه‌برداری",
          "جذاب",
          "نرم / لطیف",
          "پیش‌بینی",
          "نقطه / نکته",
          "تشکر",
          "صحنه / مرحله",
          "حذف کردن",
          "موثر",
          "سیل",
          "شخصیت",
          "خاله",
          "سگ",
        ],
    transformTag: transformTag,
    backspace: "edit",
    placeholder: "Type something",
    dropdown: {
      enabled: 1, // show suggestion after 1 typed character
      fuzzySearch: false, // match only suggestions that starts with the typed characters
      position: "text", // position suggestions list next to typed text
      caseSensitive: true, // allow adding duplicate items if their case is different
    },
    templates: {
      dropdownItemNoMatch: function (data) {
        return `<div class='${this.settings.classNames.dropdownItem}' value="noMatch" tabindex="0" role="option">
                    No suggestion found for: <strong>${data.value}</strong>
                </div>`;
      },
    },
  });

// generate a random color (in HSL format, which I like to use)
function getRandomColor() {
  function rand(min, max) {
    return min + Math.random() * (max - min);
  }

  var h = rand(1, 360) | 0,
    s = rand(40, 70) | 0,
    l = rand(65, 72) | 0;

  return "hsl(" + h + "," + s + "%," + l + "%)";
}

function transformTag(tagData) {
  tagData.color = getRandomColor();
  tagData.style = "--tag-bg:" + tagData.color;

  if (tagData.value.toLowerCase() == "shit") tagData.value = "s✲✲t";
}

tagify.on("add", function (e) {
  console.log(e.detail);
});

tagify.on("invalid", function (e) {
  console.log(e, e.detail);
});

var clickDebounce;

tagify.on("click", function (e) {
  const { tag: tagElm, data: tagData } = e.detail;

  // a delay is needed to distinguish between regular click and double-click.
  // this allows enough time for a possible double-click, and noly fires if such
  // did not occur.
  clearTimeout(clickDebounce);
  clickDebounce = setTimeout(() => {
    tagData.color = getRandomColor();
    tagData.style = "--tag-bg:" + tagData.color;
    tagify.replaceTag(tagElm, tagData);
  }, 200);
});

tagify.on("dblclick", function (e) {
  // when souble clicking, do not change the color of the tag
  clearTimeout(clickDebounce);
});
CSS;

// 6. Flag Selections

var tagify = new Tagify(document.querySelector("input[name=tags3-1]"), {
  delimiters: null,
  templates: {
    tag: function (tagData) {
      try {
        return `<tag title='${
          tagData.value
        }' contenteditable='false' spellcheck="false" class='tagify__tag ${
          tagData.class ? tagData.class : ""
        }' ${this.getAttributes(tagData)}>
                      <x title='remove tag' class='tagify__tag__removeBtn'></x>
                      <div>
                          ${
                            tagData.code
                              ? `<img onerror="this.style.visibility='hidden'" src='https://flagicons.lipis.dev/flags/4x3/${tagData.code.toLowerCase()}.svg'>`
                              : ""
                          }
                          <span class='tagify__tag-text'>${tagData.value}</span>
                      </div>
                  </tag>`;
      } catch (err) {}
    },

    dropdownItem: function (tagData) {
      try {
        return `<div ${this.getAttributes(
          tagData
        )} class='tagify__dropdown__item ${
          tagData.class ? tagData.class : ""
        }' >
                          <img onerror="this.style.visibility = 'hidden'"
                              src='https://flagicons.lipis.dev/flags/4x3/${tagData.code.toLowerCase()}.svg'>
                          <span>${tagData.value}</span>
                      </div>`;
      } catch (err) {
        console.error(err);
      }
    },
  },
  enforceWhitelist: true,
            whitelist: [
              { value: "افغانستان", code: "AF" },
              { value: "جزایر آلاند", code: "AX" },
              { value: "آلبانی", code: "AL" },
              { value: "الجزایر", code: "DZ" },
              { value: "ساموآی آمریکا", code: "AS" },
              { value: "آندورا", code: "AD" },
              { value: "آنگولا", code: "AO" },
              { value: "آنگویلا", code: "AI" },
              { value: "قطب جنوب", code: "AQ" },
              { value: "آنتیگوا و باربودا", code: "AG" },
              { value: "آرژانتین", code: "AR" },
              { value: "ارمنستان", code: "AM" },
              { value: "آروبا", code: "AW" },
              { value: "استرالیا", code: "AU", searchBy: "ساحل, نیمه‌گرمسیری" },
              { value: "اتریش", code: "AT" },
              { value: "آذربایجان", code: "AZ" },
              { value: "باهاماس", code: "BS" },
              { value: "بحرین", code: "BH" },
              { value: "بنگلادش", code: "BD" },
              { value: "باربادوس", code: "BB" },
              { value: "بلاروس", code: "BY" },
              { value: "بلژیک", code: "BE" },
              { value: "بلیز", code: "BZ" },
              { value: "بنین", code: "BJ" },
              { value: "برمودا", code: "BM" },
              { value: "بوتان", code: "BT" },
              { value: "بولیوی", code: "BO" },
              { value: "بوسنی و هرزگوین", code: "BA" },
              { value: "بوتسوانا", code: "BW" },
              { value: "جزیره بووه", code: "BV" },
              { value: "برزیل", code: "BR" },
              { value: "قلمرو بریتانیایی اقیانوس هند", code: "IO" },
              { value: "برونئی دارالسلام", code: "BN" },
              { value: "بلغارستان", code: "BG" },
              { value: "بورکینافاسو", code: "BF" },
              { value: "بوروندی", code: "BI" },
              { value: "کامبوج", code: "KH" },
              { value: "کامرون", code: "CM" },
              { value: "کانادا", code: "CA" },
              { value: "کیپ ورد", code: "CV" },
              { value: "جزایر کیمن", code: "KY" },
              { value: "جمهوری آفریقای مرکزی", code: "CF" },
              { value: "چاد", code: "TD" },
              { value: "شیلی", code: "CL" },
              { value: "چین", code: "CN" },
              { value: "جزیره کریسمس", code: "CX" },
              { value: "جزایر کوکوس (کیلینگ)", code: "CC" },
              { value: "کلمبیا", code: "CO" },
              { value: "کومور", code: "KM" },
              { value: "کنگو", code: "CG" },
              { value: "جمهوری دموکراتیک کنگو", code: "CD" },
              { value: "جزایر کوک", code: "CK" },
              { value: "کاستاریکا", code: "CR" },
              { value: "ساحل عاج", code: "CI" },
              { value: "کرواسی", code: "HR" },
              { value: "کوبا", code: "CU" },
              { value: "قبرس", code: "CY" },
              { value: "جمهوری چک", code: "CZ" },
              { value: "دانمارک", code: "DK" },
              { value: "جیبوتی", code: "DJ" },
              { value: "دومینیکا", code: "DM" },
              { value: "جمهوری دومینیکن", code: "DO" },
              { value: "اکوادور", code: "EC" },
              { value: "مصر", code: "EG" },
              { value: "السالوادور", code: "SV" },
              { value: "گینه استوایی", code: "GQ" },
              { value: "اریتره", code: "ER" },
              { value: "استونی", code: "EE" },
              { value: "اتیوپی", code: "ET" },
              { value: "جزایر فالکلند (مالویناس)", code: "FK" },
              { value: "جزایر فارو", code: "FO" },
              { value: "فیجی", code: "FJ" },
              { value: "فنلاند", code: "FI" },
              { value: "فرانسه", code: "FR" },
              { value: "گویان فرانسه", code: "GF" },
              { value: "پلی‌نزی فرانسه", code: "PF" },
              { value: "قلمروهای جنوبی فرانسه", code: "TF" },
              { value: "گابن", code: "GA" },
              { value: "گامبیا", code: "GM" },
              { value: "گرجستان", code: "GE" },
              { value: "آلمان", code: "DE" },
              { value: "غانا", code: "GH" },
              { value: "جبل‌الطارق", code: "GI" },
              { value: "یونان", code: "GR" },
              { value: "گرینلند", code: "GL" },
              { value: "گرنادا", code: "GD" },
              { value: "گوادلوپ", code: "GP" },
              { value: "گوام", code: "GU" },
              { value: "گواتمالا", code: "GT" },
              { value: "گرنزی", code: "GG" },
              { value: "گینه", code: "GN" },
              { value: "گینه بیسائو", code: "GW" },
              { value: "گویان", code: "GY" },
              { value: "هائیتی", code: "HT" },
              { value: "جزیره هرد و مک‌دونالد", code: "HM" },
              { value: "دولت کلیسای مقدس (واتیکان)", code: "VA" },
              { value: "هندوراس", code: "HN" },
              { value: "هنگ‌کنگ", code: "HK" },
              { value: "مجارستان", code: "HU" },
              { value: "ایسلند", code: "IS" },
              { value: "هند", code: "IN" },
              { value: "اندونزی", code: "ID" },
              { value: "ایران", code: "IR" },
              { value: "عراق", code: "IQ" },
              { value: "ایرلند", code: "IE" },
              { value: "جزیره من", code: "IM" },
              { value: "اسرائیل", code: "IL", searchBy: "سرزمین مقدس, صحرا" },
              { value: "ایتالیا", code: "IT" },
              { value: "جامائیکا", code: "JM" },
              { value: "ژاپن", code: "JP" },
              { value: "جرزی", code: "JE" },
              { value: "اردن", code: "JO" },
              { value: "قزاقستان", code: "KZ" },
              { value: "کنیا", code: "KE" },
              { value: "کیریباتی", code: "KI" },
              { value: "کره شمالی", code: "KP" },
              { value: "کره جنوبی", code: "KR" },
              { value: "کویت", code: "KW" },
              { value: "قرقیزستان", code: "KG" },
              { value: "لائوس", code: "LA" },
              { value: "لتونی", code: "LV" },
              { value: "لبنان", code: "LB" },
              { value: "لسوتو", code: "LS" },
              { value: "لیبریا", code: "LR" },
              { value: "لیبی", code: "LY" },
              { value: "لیختن‌اشتاین", code: "LI" },
              { value: "لیتوانی", code: "LT" },
              { value: "لوکزامبورگ", code: "LU" },
              { value: "ماکائو", code: "MO" },
              { value: "مقدونیه شمالی", code: "MK" },
              { value: "ماداگاسکار", code: "MG" },
              { value: "مالاوی", code: "MW" },
              { value: "مالزی", code: "MY" },
              { value: "مالدیو", code: "MV" },
              { value: "مالی", code: "ML" },
              { value: "مالتا", code: "MT" },
              { value: "جزایر مارشال", code: "MH" },
              { value: "مارتینیک", code: "MQ" },
              { value: "موریتانی", code: "MR" },
              { value: "موریس", code: "MU" },
              { value: "مایوت", code: "YT" },
              { value: "مکزیک", code: "MX" },
              { value: "ایالات فدرال میکرونزی", code: "FM" },
              { value: "مولداوی", code: "MD" },
              { value: "موناکو", code: "MC" },
              { value: "مغولستان", code: "MN" },
              { value: "مونتسرات", code: "MS" },
              { value: "مراکش", code: "MA" },
              { value: "موزامبیک", code: "MZ" },
              { value: "میانمار", code: "MM" },
              { value: "نامیبیا", code: "NA" },
              { value: "نائورو", code: "NR" },
              { value: "نپال", code: "NP" },
              { value: "هلند", code: "NL" },
              { value: "آنتیل هلند", code: "AN" },
              { value: "کالدونیای جدید", code: "NC" },
              { value: "نیوزیلند", code: "NZ" },
              { value: "نیکاراگوئه", code: "NI" },
              { value: "نیجر", code: "NE" },
              { value: "نیجریه", code: "NG" },
              { value: "نیوئه", code: "NU" },
              { value: "جزیره نورفولک", code: "NF" },
              { value: "جزایر ماریانای شمالی", code: "MP" },
              { value: "نروژ", code: "NO" },
              { value: "عمان", code: "OM" },
              { value: "پاکستان", code: "PK" },
              { value: "پالائو", code: "PW" },
              { value: "سرزمین‌های فلسطینی", code: "PS" },
              { value: "پاناما", code: "PA" },
              { value: "پاپوآ گینه نو", code: "PG" },
              { value: "پاراگوئه", code: "PY" },
              { value: "پرو", code: "PE" },
              { value: "فیلیپین", code: "PH" },
              { value: "پیت‌کرن", code: "PN" },
              { value: "لهستان", code: "PL" },
              { value: "پرتغال", code: "PT" },
              { value: "پورتوریکو", code: "PR" },
              { value: "قطر", code: "QA" },
              { value: "رئونیون", code: "RE" },
              { value: "رومانی", code: "RO" },
              { value: "فدراسیون روسیه", code: "RU" },
              { value: "رواندا", code: "RW" },
              { value: "سنت هلنا", code: "SH" },
              { value: "سنت کیتس و نویس", code: "KN" },
              { value: "سنت لوسیا", code: "LC" },
              { value: "سنت پیر و میکلون", code: "PM" },
              { value: "سنت وینسنت و گرنادین‌ها", code: "VC" },
              { value: "ساموآ", code: "WS" },
              { value: "سان مارینو", code: "SM" },
              { value: "ساو تومه و پرینسیپ", code: "ST" },
              { value: "عربستان سعودی", code: "SA" },
              { value: "سنگال", code: "SN" },
              { value: "صربستان و مونته‌نگرو", code: "CS" },
              { value: "سیشل", code: "SC" },
              { value: "سیرالئون", code: "SL" },
              { value: "سنگاپور", code: "SG" },
              { value: "اسلواکی", code: "SK" },
              { value: "اسلوونی",
            code: "SI" },
            { value: "جزایر سلیمان", code: "SB" },
            { value: "سومالی", code: "SO" },
            { value: "آفریقای جنوبی", code: "ZA" },
            { value: "جزایر جورجیای جنوبی و ساندویچ جنوبی", code: "GS" },
            { value: "اسپانیا", code: "ES" },
            { value: "سریلانکا", code: "LK" },
            { value: "سودان", code: "SD" },
            { value: "سورینام", code: "SR" },
            { value: "سوالبارد و جان ماین", code: "SJ" },
            { value: "سوازیلند", code: "SZ" },
            { value: "سوئد", code: "SE" },
            { value: "سوئیس", code: "CH" },
            { value: "جمهوری عربی سوریه", code: "SY" },
            { value: "تایوان", code: "TW" },
            { value: "تاجیکستان", code: "TJ" },
            { value: "تانزانیا", code: "TZ" },
            { value: "تایلند", code: "TH" },
            { value: "تیمور-لسته", code: "TL" },
            { value: "توگو", code: "TG" },
            { value: "توکلائو", code: "TK" },
            { value: "تونگا", code: "TO" },
            { value: "ترینیداد و توباگو", code: "TT" },
            { value: "تونس", code: "TN" },
            { value: "ترکیه", code: "TR" },
            { value: "ترکمنستان", code: "TM" },
            { value: "جزایر تورکس و کایکوس", code: "TC" },
            { value: "تووالو", code: "TV" },
            { value: "اوگاندا", code: "UG" },
            { value: "اوکراین", code: "UA" },
            { value: "امارات متحده عربی", code: "AE" },
            { value: "بریتانیا", code: "GB" },
            { value: "ایالات متحده آمریکا", code: "US" },
            { value: "جزایر کوچک دورافتاده ایالات متحده", code: "UM" },
            { value: "اروگوئه", code: "UY" },
            { value: "ازبکستان", code: "UZ" },
            { value: "وانواتو", code: "VU" },
            { value: "ونزوئلا", code: "VE" },
            { value: "ویتنام", code: "VN" },
            { value: "جزایر ویرجین بریتانیا", code: "VG" },
            { value: "جزایر ویرجین آمریکا", code: "VI" },
            { value: "والیس و فوتونا", code: "WF" },
            { value: "صحرای غربی", code: "EH" },
            { value: "یمن", code: "YE" },
            { value: "زامبیا", code: "ZM" },
            { value: "زیمبابوه", code: "ZW" },
            ],
  dropdown: {
    enabled: 1, // suggest tags after a single character input
    classname: "extra-properties", // custom class for the suggestions dropdown
  }, // map tags' values to this property name, so this property will be the actual value and not the printed value on the screen
});

tagify.on("click", function (e) {
  console.log(e.detail);
});

tagify.on("remove", function (e) {
  console.log(e.detail);
});

tagify.on("add", function (e) {
  console.log("original Input:", tagify.DOM.originalInput);
  console.log("original Input's value:", tagify.DOM.originalInput.value);
  console.log("event detail:", e.detail);
});

// add the first 2 tags and makes them readonly
var tagsToAdd = tagify.whitelist.slice(0, 3);
tagify.addTags(tagsToAdd);

// 7. Readonly options
var input = document.querySelector("input[name=tags4]"),
  tagify = new Tagify(input);

// 8. Read and write options
var input = document.querySelector("input[name=tags-readonly-mix]"),
  tagify = new Tagify(input);

// 9. Disabled readonly
var input = document.querySelector("input[name=disabledInput]"),
  tagify = new Tagify(input);

// 10. Single-Value Select
var input = document.querySelector("input[name=tags-select-mode]");
var input1 = document.querySelector("input[name=tags-select-value-mode");
var input2 = document.querySelector("input[name=tags-select-v-mode");
var input3 = document.querySelector("input[name=tags-s-value-mode");

tagify = new Tagify(input, {
  enforceWhitelist: true,
    mode: "select",
    whitelist: ["آدمیرو", "روکسو", "تیوو"],
    blacklist: ["فو", "بار"],
});

tagify = new Tagify(input1, {
  enforceWhitelist: true,
    mode: "select",
     whitelist: ["طراح وب", "طراح UI/UX", "توسعه‌دهنده وب"],
     blacklist: ["فو", "بار"],

});

tagify = new Tagify(input2, {
  enforceWhitelist: true,
    mode: "select",
    whitelist: ["کیت‌های UI", "UI جایزه", "نشانه‌گذاری‌ها"],
    blacklist: ["فو", "بار"],
});

tagify = new Tagify(input3, {
  enforceWhitelist: true,
     mode: "select",
    whitelist: ["فرم‌ها", "چارت‌ها", "وبلاگ"],
    blacklist: ["فو", "بار"],
});
// bind events
tagify.on("add", onAddTag);
tagify.DOM.input.addEventListener("focus", onSelectFocus);

function onAddTag(e) {
  console.log(e.detail);
}

function onSelectFocus(e) {
  console.log(e);
}

// 11. Customize emails

// generate random whilist items
var randomStringsArr = Array.apply(null, Array(100)).map(function () {
  return (
    Array.apply(null, Array(~~(Math.random() * 10 + 3)))
      .map(function () {
        return String.fromCharCode(Math.random() * (123 - 97) + 97);
      })
      .join("") + "@gmail.com"
  );
});

var emailsearch = document.querySelector(".customLook"),
  tagify = new Tagify(emailsearch, {
    // email address validation (https://stackoverflow.com/a/46181/104380)
    pattern:
      /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
    whitelist: randomStringsArr,
    callbacks: {
      invalid: onInvalidTag,
    },
    dropdown: {
      position: "text",
      enabled: 1, // show suggestions dropdown after 1 typed character
    },
  }),
  button = emailsearch.nextElementSibling; // "add new tag" action-button

button.addEventListener("click", onAddButtonClick);

function onAddButtonClick() {
  tagify.addEmptyTag();
}

function onInvalidTag(e) {
  console.log("invalid", e.detail);
}