$(function () {
    //var selectize = $("form select, .modal select").selectize();

    $("form select, .modal select").select2({ width: "100%" });

    $("select[data-child]").each(function () {
        var thisId = $(this).attr("id");
        var childId = $(this).attr("data-child");
        var childUrl = $(this).attr("data-childUrl");
        var childUrlParm = $(this).attr("data-childUrlParm");

        var cascadLoading = new Select2Cascade(thisId, childId, childUrl, childUrlParm);
        cascadLoading.then(function (parent, child, url, childUrlParm) {
            var i = childUrlParm.split(",");

            if (i.length !== 0) {
                url += "?";

                console.log(i);
                $.each(i, function (index, value) {
                    url += value + "=" + $("#" + value).select2("val") + "&";
                });
                url = url.slice(0, -1);
                    console.log(url);
            }
            //console.log(url);
            $.getJSON(url, function (items) {
                var newOptions = "<option value=\"\">-- Select --</option>";
                $.each(items.result, function (index, item) {
                    if (item.id !== null && item.txt !== null) {
                        newOptions += "<option value=\"" + item.id + "\">" + item.txt + "</option>";
                    } else {
                        newOptions += "<option value=\"" + item + "\">" + items[item] + "</option>";
                    }
                });
                child.select2("destroy")
                    .html(newOptions)
                    .prop("disabled", false)
                    .select2({ width: "100%", placeholder: "-- Select --" });
            });
        });
    });

    var validobj = $("form").validate({
        ignore: ":hidden:not([class~=selectized]),:hidden > .selectized, .selectize-control .selectize-input input",
        onkeyup: false,
        errorClass: "has-error",
        errorPlacement: function (error, element) {
            //var elem = $(element);
            //error.insertAfter(element);
            return true;
        },
        highlight: function (element, errorClass, validClass) {
            var elem = $(element);
            if (elem.hasClass("select2-offscreen")) {
                $("#s2id_" + elem.attr("id") + " ul").addClass(errorClass);
            }
            else if (elem.hasClass("selectized")) {
                $("#" + elem.attr("id") + "-selectized").parent().addClass(errorClass);
            } else {
                elem.addClass(errorClass);
            }
        },
        unhighlight: function (element, errorClass, validClass) {
            var elem = $(element);
            if (elem.hasClass("select2-offscreen")) {
                $("#s2id_" + elem.attr("id") + " ul").removeClass(errorClass);
            } else
                if (elem.hasClass("select2-offscreen")) {
                    $("#s2id_" + elem.attr("id") + " ul").removeClass(errorClass);
                } else {
                    elem.removeClass(errorClass);
                }
        },
        //submitHandler: function (form) {
        //    $.ajax({
        //        url: $("#ItemCreate").attr("action"),
        //        data: $(form).serialize(),
        //        dataType: 'json'
        //    });
        //    return false;
        //}
    });

    $(document).on("change", ".select2-offscreen", function () {
        if (!$.isEmptyObject(validobj.submitted)) {
            validobj.form();
        }
    });

    $(document).on("dropdown_open", function (arg) {
        var elem = $(arg.target);
        if ($("#s2id_" + elem.attr("id") + " ul").hasClass("has-error")) {
            $(".select2-drop ul").addClass("has-error");
        } else {
            $(".select2-drop ul").removeClass("has-error");
        }
    });

    $(document).on("select2-opening", function (arg) {
        var elem = $(arg.target);
        if ($("#s2id_" + elem.attr("id") + " ul").hasClass("has-error")) {
            //jquery checks if the class exists before adding.
            $(".select2-drop ul").addClass("has-error");
        } else {
            $(".select2-drop ul").removeClass("has-error");
        }
    });
});

var Select2Cascade = (function (window, $) {
    function select2Cascade(parentName, childName, url, childUrlParm) {

        var parent = $("#" + parentName);
        var child = $("#" + childName);
        var afterActions = [];

        this.then = function (callback) {// Register functions to be called after cascading data loading done
            afterActions.push(callback);
            return this;
        };
        parent.select2({ width: "100%" }).on("change", function () {
            child.prop("disabled", true);

            afterActions.forEach(function (callback) {
                callback(parent, child, url, childUrlParm);
            });
        });
        //parent.select2({ width: "100%" }).on("change", function () {
        //    child.prop("disabled", true);
        //    $.getJSON(url, function (items) {
        //        var newOptions = "<option value=\"\">-- Select --</option>";
        //        for (var id in items) {
        //            if (items.hasOwnProperty(id)) {
        //                newOptions += "<option value=\"" + id + "\">" + items[id] + "</option>";
        //            }
        //        }
        //        child.select2("destroy")
        //            .html(newOptions)
        //            .prop("disabled", false)
        //            .select2({ width: "100%", placeholder: "-- Select --" });
        //        afterActions.forEach(function (callback) {
        //            callback(parent, child, items);
        //        });
        //    });
        //});
    }

    return select2Cascade;

})(window, $);
