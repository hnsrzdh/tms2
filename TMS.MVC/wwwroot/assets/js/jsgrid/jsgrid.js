(function($) {
    "use strict";
    $("#basicScenario").jsGrid({
        width: "100%",
        filtering: true,
        editing: true,
        inserting: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 15,
        pageButtonCount: 5,
        deleteConfirm: "آیا واقعاً می‌خواهید مشتری را حذف کنید؟",
        controller: db,
        fields: [
            { name: "وظیفه", type: "text", width: 150 },
            { name: "ایمیل", type: "text", width: 200 },
            { name: "شماره تماس", type: "text", width: 150 },
            { name: "مسئول", type: "text", width: 160 },
            { name: "تاریخ", type: "text", width: 150 },
            { name: "قیمت", type: "text", width: 100 },
            { name: "وضعیت", type: "html", width: 150 },
            { name: "پیشرفت", type: "text", width: 100 },
            { type: "control" , width: 80 },
        ]
    });
    $("#sorting-table").jsGrid({
        height:"400px",
        width: "100%",
        autoload: true,
        selecting: false,
        controller: db,
        fields: [
            { name: "شناسه", type: "text", width: 50 },
            {  name: "محصول", type: "text", width: 150 },
            {  name: "شناسه سفارش", type: "text", width: 100 },
            { name: "قیمت", type: "text", width: 100 },
            { name: "تعداد", type: "text", title: "تعداد", width: 90 },
            { name: "وضعیت ارسال", type: "text", width: 150 },
            { name: "مجموع", type: "text", width: 100 },
        ]
    });
    $("#sort").click ( function() {
        var field = $("#sortingField").val();
        $("#sorting-table").jsGrid("sort", field);
    });
    $("#batchDelete").jsGrid({
        width: "100%",
        autoload: true,
        confirmDeleting: false,
        paging: true,
        controller: {
            loadData: function() {
                return db.clients;
            }
        },
        fields: [
            {
                headerTemplate: function() {
                    return $("<button>").attr("type", "button").text("حذف") .addClass("btn btn-danger btn-sm btn-delete mb-0")
                        .click( function () {
                            deleteSelectedItems();
                        });
            },
            itemTemplate: function(_, item) {
                return $("<input>").attr("type", "checkbox")
                        .prop("checked", $.inArray(item, selectedItems) > -1)
                        .on("change", function () {
                            $(this).is(":checked") ? selectItem(item) : unselectItem(item);
                        });
            },
            align: "center",
            width: 80
            },
            { name: "شناسه", type: "text", width: 50 },
            { name: "نام کارمند", type: "Text", width: 150 },
            { name: "حقوق", type: "text", width: 100 },
            { name: "مهارت", type: "text", width: 60 },
            { name: "دفتر", type: "text", width: 100 },
            { name: "ساعت", type: "text", width: 80 },
            { name: "سابقه کاری", type: "text", width: 110 },
        ]
    });
    var selectedItems = [];
    var selectItem = function(item) {
        selectedItems.push(item);
    };
    var unselectItem = function(item) {
        selectedItems = $.grep(selectedItems, function(i) {
            return i !== item;
        });
    };
    var deleteSelectedItems = function() {
        if(!selectedItems.length || !confirm("مطمئن هستید؟"))
            return;
        deleteClientsFromDb(selectedItems);
        var $grid = $("#batchDelete");
        $grid.jsGrid("option", "pageIndex", 1);
        $grid.jsGrid("loadData");
        selectedItems = [];
    };
    var deleteClientsFromDb = function(deletingClients) {
        db.clients = $.map(db.clients, function(client) {
            return ($.inArray(client, deletingClients) > -1) ? null : client;
        });
    };
})(jQuery);