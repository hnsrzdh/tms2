(function () {
    var editor2 = new Quill("#editor2", {
      modules: { toolbar: "#toolbar2" },
      theme: "snow",
      placeholder: "متن خود را وارد کنید",
    });
  
    var editor3 = new Quill("#editor3", {
      modules: { toolbar: "#toolbar3" },
      theme: "snow",
      placeholder: "متن خود را وارد کنید",
    });
  
    var editor4 = new Quill("#editor4", {
      modules: { toolbar: "#toolbar4" },
      theme: "snow",
      placeholder: "متن خود را وارد کنید",
    });
    // =====================================================================
    function openAlert() {
      var alertBox = document.getElementById("alertBox");
      alertBox.style.display = "block";
    }
  
    function closeAlert() {
      var alertBox = document.getElementById("alertBox");
      alertBox.style.display = "none";
    }
  
    var form = document.getElementById("advance-tab");
  
    var my_func = function (event) {
      event.preventDefault();
    };
  
    form.addEventListener("submit", my_func, true);
  })();
  