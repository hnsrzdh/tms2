/*-----------------------------------------------------------------------------------
 Template Name: Admiro
 Description: This is Admin Template
 ----------------------------------------------------------------------------------- */
// 01. Loader js
// 02. Tap to top js
// 03. Header DropDown Toggle js
// 04. Full screen js
// 05. Header search js
document.addEventListener("DOMContentLoaded", function () {
  const body = document.querySelector("body");
  const html = document.querySelector("html");
    /*=====================
        01 Loader Js
    ==========================*/
    $(".loader-wrapper").fadeOut("slow", function () {
      $(this).remove();
    });
  /*=====================
        02 Tap to top js
    ==========================*/
  const button = document.querySelector(".tap-top");
  const displayButton = () => {
    window.addEventListener("scroll", () => {
      if (window.scrollY > 100) {
        button.style.display = "block";
      } else {
        button.style.display = "none";
      }
    });
  };
  const scrollToTop = () => {
    button.addEventListener("click", () => {
      window.scroll({
        top: 0,
        left: 0,
        behavior: "smooth",
      });
      console.log(event);
    });
  };
  displayButton();
  scrollToTop();
  /*=====================
      03 Header DropDown Toggle
  ==========================*/
  body.addEventListener("click", function (event) {
    const headerDropdownMenu = document.querySelectorAll(".custom-menu");
    const dropdownEl = event.target.closest(".custom-dropdown");
    const visible = dropdownEl
      ?.querySelector(".custom-menu")
      .classList.contains("show");
    const dropdownMenuElement = event.target.closest(".custom-menu");
    if (!dropdownMenuElement) {
      headerDropdownMenu.forEach((item) => {
        item.classList.remove("show");
      });
    }
    if (!dropdownEl) return;
    const dropdownMenu = dropdownEl.querySelector(".custom-menu");
    if (!visible) dropdownMenu.classList.add("show");
  });
  /*=====================
      04 Full screen js
  ==========================*/
  $(document).ready(function () {
    $(".full-screen").click(function (event) {
      var elem = document.documentElement;

      if (
        (document.fullScreenElement && document.fullScreenElement !== null) ||
        (!document.mozFullScreen && !document.webkitIsFullScreen)
      ) {
        if (elem.requestFullScreen) {
          elem.requestFullScreen();
        } else if (elem.mozRequestFullScreen) {
          elem.mozRequestFullScreen();
        } else if (elem.webkitRequestFullScreen) {
          elem.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
        }
      } else {
        if (document.cancelFullScreen) {
          document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
          document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
          document.webkitCancelFullScreen();
        }
      }
    });
  });

  /*=====================
       05. Header search js
     ==========================*/
  const filterSidebarToggle = document.querySelector(".md-sidebar-toggle");
  const filterSidebarAside = document.querySelector(".md-sidebar-aside");
  filterSidebarToggle?.addEventListener("click", function () {
    filterSidebarAside.classList.toggle("open");
  });
  $(".search").click(function () {
    $(".search-full").addClass("open");
  });
  $(".close-search").click(function () {
    $(".search-full").removeClass("open");
    $("body").removeClass("offcanvas");
  });
  /*=====================
       05. Dark Mode js
    ==========================*/
    if(window.location.pathname.includes("layout-dark.html")){
      $("body").removeClass("light");
      $("body").addClass("dark-only");
      }else{
      $(".dark-mode").on("click", function () {
      const bodyModeDark = $("body").hasClass("dark-only");
      if(!window.location.pathname.includes("layout-dark.html")){
      if (!bodyModeDark) {
      $(".dark-mode").addClass("active");
      localStorage.setItem("mode", "dark-only");
      $("body").addClass("dark-only");
      $("body").removeClass("light");
      }
      else{
      $(".dark-mode").removeClass("active");
      localStorage.setItem("mode", "light");
      $("body").removeClass("dark-only");
      $("body").addClass("light");
      }
      }
      });
      $("body").addClass(
      localStorage.getItem("mode")
      ? localStorage.getItem("mode")
      : "light"
      );
      $(".dark-mode").addClass(
      localStorage.getItem("mode") === "dark-only" ? "active" : " "
      )}
  // product-page-js-start
  var toggleDataElements = document.querySelectorAll(".toggle-data");
  toggleDataElements.forEach(function (element) {
    element.addEventListener("click", function () {
      var productWrapperElements =
        document.querySelectorAll(".product-wrapper");
      productWrapperElements.forEach(function (wrapperElement) {
        wrapperElement.classList.toggle("sidebaron");
      });
    });
  });
  // product-page-js-end
  $(".prooduct-details-box .close").on("click", function (e) {
    var tets = $(this).parent().parent().parent().parent().addClass("d-none");
    console.log(tets);
  });
  /*=====================
    00. Background Image js
    ==========================*/
  $(".bg-center").parent().addClass("b-center");
  $(".bg-img-cover").parent().addClass("bg-size");
  $(".bg-img-cover").each(function () {
    var el = $(this),
      src = el.attr("src"),
      parent = el.parent();
    parent.css({
      "background-image": "url(" + src + ")",
      "background-size": "cover",
      "background-position": "center",
      display: "block",
    });
    el.hide();
  });
  /*=====================
    00. Language js
    ==========================*/
  var tnum = "en";

  $(document).ready(function () {
    if (localStorage.getItem("primary") != null) {
      var primary_val = localStorage.getItem("primary");
      $("#ColorPicker1").val(primary_val);
      var secondary_val = localStorage.getItem("secondary");
      $("#ColorPicker2").val(secondary_val);
    }

    $(document).click(function (e) {
      $(".translate_wrapper, .more_lang").removeClass("active");
    });
    $(".translate_wrapper .current_lang").click(function (e) {
      e.stopPropagation();
      $(this).parent().toggleClass("active");

      setTimeout(function () {
        $(".more_lang").toggleClass("active");
      }, 5);
    });

    /*TRANSLATE*/
    translate(tnum);

    $(".more_lang .lang").click(function () {
      $(this).addClass("selected").siblings().removeClass("selected");
      $(".more_lang").removeClass("active");

      var i = $(this).find("i").attr("class");
      var lang = $(this).attr("data-value");
      var tnum = lang;
      translate(tnum);

      $(".current_lang .lang-txt").text(lang);
      $(".current_lang i").attr("class", i);
    });
  });

  function translate(tnum) {
    $(".lan-1").text(trans[0][tnum]);
    $(".lan-2").text(trans[1][tnum]);
    $(".lan-3").text(trans[2][tnum]);
  }

  var trans = [
    {
      en: "اصلی",
      es: "اصلی",
      fr: "اصلی",
    },
    {
      en: "ویجت ها",
      es: "ویجت ها",
      fr: "ویجت ها",
    },
    {
      en: "طرح صفحه",
      es: "طرح صفحه",
      fr: "طرح صفحه",
    },
  ];

  /*=====================
    Tractor Assignment Loading Modal
    ==========================*/
  (function enhanceTractorAssignmentLoadingModal() {
    const loadingModal = document.getElementById("loadingModal");
    if (!loadingModal) return;

    const form = loadingModal.querySelector("form");
    if (!form) return;

    if (form.dataset.loadingEnhanced === "true") return;
    form.dataset.loadingEnhanced = "true";

    form.setAttribute("enctype", "multipart/form-data");

    const currentAction = form.getAttribute("action") || "";
    if (currentAction) {
      form.setAttribute("action", currentAction.replace("ConfirmLoading", "ConfirmLoadingWithDocuments"));
    } else {
      form.setAttribute("action", "/TractorAssignment/ConfirmLoadingWithDocuments");
    }

    const amountInput = form.querySelector('input[name="loadedAmount"], input[name="LoadedAmount"]');
    if (amountInput) {
      amountInput.setAttribute("required", "required");
      amountInput.setAttribute("min", "0.001");
      amountInput.setAttribute("step", "0.001");
      amountInput.setAttribute("placeholder", "وزن خالص بارگیری");
      const amountLabel = amountInput.closest(".mb-3, .col-md-6, .col-md-12, div")?.querySelector("label");
      if (amountLabel) amountLabel.textContent = "وزن خالص بارگیری (ضروری)";
    }

    const insertAfter = amountInput?.closest(".mb-3, .col-md-6, .col-md-12, div") || amountInput?.parentElement || form.querySelector(".modal-body") || form;

    const extraFieldsWrapper = document.createElement("div");
    extraFieldsWrapper.className = "loading-extra-fields";
    extraFieldsWrapper.innerHTML = `
      <div class="mb-3">
        <label class="form-label">حجم خالص بارگیری (اختیاری)</label>
        <input type="number"
               name="loadingNetVolume"
               class="form-control form-control-sm"
               step="0.001"
               min="0"
               placeholder="مثلاً 32000" />
      </div>

      <div class="mb-3">
        <label class="form-label">شماره بارنامه (اختیاری)</label>
        <input type="text"
               name="loadingBillOfLadingNumber"
               class="form-control form-control-sm"
               maxlength="100"
               placeholder="شماره بارنامه" />
      </div>

      <div class="mb-2">
        <label class="form-label">مدارک بارگیری (ضروری)</label>
        <input type="file"
               name="loadingDocuments"
               class="form-control form-control-sm"
               multiple
               required />
        <div class="form-text small">
          حداقل یک فایل مدرک بارگیری باید انتخاب شود. برای چند مدرک، چند فایل را همزمان انتخاب کنید.
        </div>
      </div>
    `;

    insertAfter.insertAdjacentElement("afterend", extraFieldsWrapper);

    form.addEventListener("submit", function (event) {
      const weightInput = form.querySelector('input[name="loadedAmount"], input[name="LoadedAmount"]');
      const docsInput = form.querySelector('input[name="loadingDocuments"]');

      const weight = weightInput ? parseFloat(weightInput.value || "0") : 0;
      if (!weight || weight <= 0) {
        event.preventDefault();
        alert("وزن خالص بارگیری ضروری است و باید بزرگتر از صفر باشد.");
        weightInput?.focus();
        return;
      }

      if (!docsInput || !docsInput.files || docsInput.files.length === 0) {
        event.preventDefault();
        alert("آپلود حداقل یک مدرک بارگیری الزامی است.");
        docsInput?.focus();
      }
    });
  })();
});
