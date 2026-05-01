document.addEventListener("DOMContentLoaded", function () {
 //config
 var singleNoSearch = new Choices("#choices-single-no-search", {
  allowHTML: true,
  searchEnabled: false,
  removeItemButton: true,
  choices: [
    { value: "One", label: "گزینه 1" },
    { value: "Two", label: "گزینه 2", disabled: true },
    { value: "Three", label: "گزینه 3" },
  ],
}).setChoices(
  [
    { value: "Four", label: "گزینه 4", disabled: true },
    { value: "Five", label: "گزینه 5" },
    { value: "Six", label: "کزینه 6", selected: true },
  ],
  "value",
  "label",
  false  
);
});
  //Multiple select input
  var multipleCancelButton = new Choices("#choices-multiple-remove-button", {
    allowHTML: true,
    removeItemButton: true,
  });
  // Select one inputs
  new Choices("#choices-scrolling-dropdown", {
    allowHTML: true,
    shouldSort: false,
  });
  //Multiple sections with headers
  var multipleDefault = new Choices(
    document.getElementById("choices-multiple-groups"),
    { allowHTML: true }
  );
  //Basic typeahead
  var cities = new Choices(document.getElementById("cities"), {
    allowHTML: true,
  });
  //rtl
  var rtl = new Choices(document.getElementById("rtl"), {
    allowHTML: true,
  });
// });