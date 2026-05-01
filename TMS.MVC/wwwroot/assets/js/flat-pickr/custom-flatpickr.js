// Custom-flatpickr JS

flatpickr("#datetime-local", {});

flatpickr("#human-friendly", {
  altInput: true,
  altFormat: "F j, Y",
  dateFormat: "Y-m-d",
});

flatpickr("#min-max", {
  dateFormat: "d.m.Y",
  maxDate: "15.12.2017",
});

flatpickr("#disabled-date", {
  disable: ["2025-01-30", "2025-02-21", "2025-03-08", new Date(2025, 4, 9)],
  dateFormat: "Y-m-d",
});

flatpickr("#multiple-date", {
  mode: "multiple",
  dateFormat: "Y-m-d",
});

flatpickr("#customize-date", {
  mode: "multiple",
  dateFormat: "Y-m-d",
  conjunction: " :: ",
});

flatpickr("#range-date", {
  mode: "range",
});

flatpickr("#preloading-date", {
  mode: "multiple",
  dateFormat: "Y-m-d",
  defaultDate: ["2016-10-20", "2016-11-04"],
});


flatpickr("#time-picker", {
  enableTime: true,
  noCalendar: true,
  dateFormat: "H:i",
});

flatpickr("#twenty-four-hour", {
  enableTime: true,
  noCalendar: true,
  dateFormat: "H:i",
  time_24hr: true,
});

flatpickr("#limit-time", {
  enableTime: true,
  noCalendar: true,
  dateFormat: "H:i",
  minTime: "16:00",
  maxTime: "22:30",
});

flatpickr("#preloading-time", {
  enableTime: true,
  noCalendar: true,
  dateFormat: "H:i",
  defaultDate: "13:45",
});

flatpickr("#limit-time-range", {
  enableTime: true,
  minTime: "09:00",
});

flatpickr("#limit-min-max-range", {
  enableTime: true,
  minTime: "16:00",
  maxTime: "22:00",
});

flatpickr("#inline-calender", {
  inline: true,
});
