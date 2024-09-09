var options = { year: 'numeric', month: 'long', day: 'numeric' };
var mdOptions = { month: 'long', day: 'numeric' };
var wdOptions = { weekday: 'narrow', day: 'numeric' };
var myOptions = { year: 'numeric', month: 'long' };
var dOptions = { day: 'numeric' };
gantt.attachEvent("onBeforeTaskUpdate", function (id, item) {
    //any custom logic here
    zoomToFit();
});
function toggleMode(toggle) {
    gantt.$zoomToFit = !gantt.$zoomToFit;
    if (gantt.$zoomToFit) {
        toggle.innerHTML = "K.thước M.định";
        //Saving previous scale state for future restore
        saveConfig();
        zoomToFit();
    } else {

        toggle.innerHTML = "Kích thước vừa";
        //Restore previous scale state
        restoreConfig();
        gantt.render();
    }
}

var filterEl = document.querySelector("#filter");

var filterValue = "";
console.log(filterEl);

filterEl.addEventListener('input', function (e) {
    filterValue = filterEl.value;
    gantt.refreshData();
});

function filterLogic(task, match) {
    match = match || false;
    // check children
    gantt.eachTask(function (child) {
        if (filterLogic(child)) {
            match = true;
        }
    }, task.id);

    // check task
    if (task.text.toLowerCase().indexOf(filterValue.toLowerCase()) > -1) {
        match = true;
    }
    return match;
}

gantt.attachEvent("onBeforeTaskDisplay", function (id, task) {
    if (!filterValue) {
        return true;
    }
    return filterLogic(task);
});

var cachedSettings = {};

function saveConfig() {
    var config = gantt.config;
    cachedSettings = {};
    cachedSettings.scales = config.scales;
    cachedSettings.start_date = config.start_date;
    cachedSettings.end_date = config.end_date;
    cachedSettings.scroll_position = gantt.getScrollState();
}

function restoreConfig() {
    applyConfig(cachedSettings);
}

function applyConfig(config, dates) {

    gantt.config.scales = config.scales;
    var lowest_scale = config.scales.reverse()[0];

    if (dates && dates.start_date && dates.end_date) {
        gantt.config.start_date = gantt.date.add(dates.start_date, -1, lowest_scale.unit);
        gantt.config.end_date = gantt.date.add(gantt.date[lowest_scale.unit + "_start"](dates.end_date), 2, lowest_scale.unit);
    } else {
        gantt.config.start_date = gantt.config.end_date = null;
    }

    // restore the previous scroll position
    if (config.scroll_position) {
        setTimeout(function () {
            gantt.scrollTo(config.scroll_position.x, config.scroll_position.y)
        }, 4)
    }
}

function zoomToFit() {
    var project = gantt.getSubtaskDates(),
        areaWidth = gantt.$task.offsetWidth,
        scaleConfigs = zoomConfig.levels;

    for (var i = 0; i < scaleConfigs.length; i++) {
        var columnCount = getUnitsBetween(project.start_date, project.end_date, scaleConfigs[i].scales[scaleConfigs[i].scales.length - 1].unit, scaleConfigs[i].scales[0].step);
        if ((columnCount + 2) * gantt.config.min_column_width <= areaWidth) {
            break;
        }
    }


    if (i == scaleConfigs.length) {
        i--;
    }

    gantt.ext.zoom.setLevel(scaleConfigs[i].name);
    applyConfig(scaleConfigs[i], project);
}

// get number of columns in timeline
function getUnitsBetween(from, to, unit, step) {
    var start = new Date(from),
        end = new Date(to);
    var units = 0;
    while (start.valueOf() < end.valueOf()) {
        units++;
        start = gantt.date.add(start, step, unit);
    }
    return units;
}

function zoom_in() {
    gantt.ext.zoom.zoomIn();
    gantt.$zoomToFit = false;
    document.querySelector(".zoom_toggle").innerHTML = "Kích thước vừa";
}
function zoom_out() {
    gantt.ext.zoom.zoomOut();
    gantt.$zoomToFit = false;
    document.querySelector(".zoom_toggle").innerHTML = "Kích thước vừa";
}


var zoomConfig = {
    levels: [
        // hours
        {
            name: "giờ",
            scale_height: 27,
            scales: [
                {
                    unit: "day", step: 1, format: function (date) {
                        return date.toLocaleDateString("vi-VN", mdOptions);
                    }
                },
                { unit: "hour", step: 1, format: "%H:%i" },
            ]
        },
        // days
        {
            name: "ngày",
            scale_height: 27,
            scales: [
                {
                    unit: "day", step: 1, format: function (date) {
                        return date.toLocaleDateString("vi-VN", wdOptions);
                    }
                }
            ]
        },
        // weeks
        {
            name: "tuần",
            scale_height: 50,
            scales: [
                {
                    unit: "week", step: 1, format: function (date) {
                        var endDate = gantt.date.add(date, 6, "day");
                        var weekNum = gantt.date.date_to_str("%W")(date);
                        return "#" + weekNum + ", " + date.toLocaleDateString("vi-VN", mdOptions) + " - " + endDate.toLocaleDateString("vi-VN", mdOptions);
                    }
                },
                {
                    unit: "day", step: 1, format: function (date) {
                        return date.toLocaleDateString("vi-VN", wdOptions);
                    }
                }
            ]
        },
        // months
        {
            name: "tháng",
            scale_height: 50,
            scales: [
                {
                    unit: "month", step: 1, format: function (date) {
                        return date.toLocaleDateString("vi-VN", myOptions);
                    }
                },
                {
                    unit: "week", step: 1, format: function (date) {
                        var endDate = gantt.date.add(gantt.date.add(date, 1, "week"), -1, "day");
                        return date.toLocaleDateString("vi-VN", dOptions) + " - " + endDate.toLocaleDateString("vi-VN", dOptions);
                    }
                }
            ]
        },
        // years
        {
            name: "năm",
            scale_height: 50,
            scales: [
                { unit: "year", step: 1, format: "%Y" }
            ]
        },
        {
            name: "năm",
            scale_height: 50,
            scales: [
                {
                    unit: "year", step: 5, format: function (date) {
                        var dateToStr = gantt.date.date_to_str("%Y");
                        var endDate = gantt.date.add(gantt.date.add(date, 5, "year"), -1, "day");
                        return dateToStr(date) + " - " + dateToStr(endDate);
                    }
                }
            ]
        },
        // decades
        {
            name: "năm",
            scale_height: 50,
            scales: [
                {
                    unit: "year", step: 100, format: function (date) {
                        var dateToStr = gantt.date.date_to_str("%Y");
                        var endDate = gantt.date.add(gantt.date.add(date, 100, "year"), -1, "day");
                        return dateToStr(date) + " - " + dateToStr(endDate);
                    }
                },
                {
                    unit: "year", step: 10, format: function (date) {
                        var dateToStr = gantt.date.date_to_str("%Y");
                        var endDate = gantt.date.add(gantt.date.add(date, 10, "year"), -1, "day");
                        return dateToStr(date) + " - " + dateToStr(endDate);
                    }
                },
            ]
        },
    ],
    element: function () {
        return gantt.$root.querySelector(".gantt_task");
    }
};

gantt.ext.zoom.init(zoomConfig);

gantt.ext.zoom.setLevel("ngày");

gantt.$zoomToFit = false;

gantt.config.scale_height = 50;

//var options = { year: 'numeric', month: 'long', day: 'numeric' };

gantt.config.columns = [
    { name: "text", label: "Tác vụ", align: "left", min_width: 250, width: "*", tree: true },
    { name: "assignee", label: "Đảm nhiệm", align: "center", min_width: 80, width: "*" },
    {
        name: "start_date", label: "Bắt đầu", align: "center", min_width: 160, width: "*", template: function (task) {
            return task.start_date.toLocaleDateString("vi-VN", options)
        }
    },
    {
        name: "end_date", label: "Kết thúc", align: "center", min_width: 160, width: "*", template: function (task) {
            return task.end_date.toLocaleDateString("vi-VN", options)
        }
    },
    { name: "work_remain", label: "Th.lượng", align: "center", min_width: 55, width: "*" },
    {
        name: "progress", label: "Tiến trình", align: "center", min_width: 55, width: "*", template: function (task) {
            return task.progress >= 0 ? Math.round(task.progress * 100) + "%" : 0 + "%"
        }
    },
    { name: "priority", label: "Ưu tiên", align: "center", min_width: 80, width: "*" },
];

gantt.config.open_tree_initially = true;
gantt.config.details_on_create = false;
gantt.config.details_on_dblclick = false;
gantt.config.readonly = true;
gantt.config.sort = true;
gantt.plugins({
    export_api: true,
    tooltip: true
})

gantt.templates.tooltip_text = function (start, end, task) {
    return "<b>Tác vụ:</b> " + task.text +
        "<br/><b>Đảm nhiệm:</b> " + task.assignee +
        "<br/><b>Bắt đầu:</b> " + (task.start_date.toLocaleDateString("vi-VN", options)) +
        "<br/><b>Kết thúc:</b> " + (task.end_date.toLocaleDateString("vi-VN", options)) +
        "<br/><b>Th.lượng:</b> " + (task.work_remain == undefined ? 0 : task.work_remain) +
        "<br/><b>Tiến trình:</b> " + (task.progress >= 0 ? Math.round(task.progress * 100) + "%" : 0 + "%") +
        "<br/><b>Ưu tiên:</b> " + task.priority;
};

function exportData(raw) {
    const previousWidth = gantt.config.grid_width;
    gantt.config.grid_width = 0;
    gantt.config.columns.forEach(function (column, index) {
        const nodes = document.querySelectorAll(`.gantt_cell[data-column-index="${index}"]`)
        let maxWidth = 0;
        nodes.forEach(function (node) {
            if (maxWidth < node.scrollWidth) {
                maxWidth = node.scrollWidth
            }
        })
        gantt.config.grid_width += maxWidth;
        column.previous_width = column.width
        column.width = maxWidth + 12; // 12px is the padding-left + padding right
    })

    gantt.exportToPDF({
        raw: raw
    });

    gantt.config.grid_width = previousWidth;
    gantt.config.columns.forEach(function (column, index) {
        column.width = column.previous_width;
        delete column.previous_width;
    })

    if (raw) {
        gantt.render();
    }
}

function collapseAll() {
    gantt.batchUpdate(function () {
        gantt.eachTask(function (task) {
            gantt.close(task.id)
        })
    })
}

function expandAll() {
    gantt.batchUpdate(function () {
        gantt.eachTask(function (task) {
            gantt.open(task.id)
        })
    })
};
gantt.config.layout = {
    css: "gantt_container",
    cols: [
        {
            width: 600,
            minWidth: 600,
            maxWidth: 700,

            // adding horizontal scrollbar to the grid via the scrollX attribute
            rows: [
                { view: "grid", scrollX: "gridScroll", scrollable: true, scrollY: "scrollVer" },
                { view: "scrollbar", id: "gridScroll" }
            ]
        },
        { resizer: true, width: 1 },
        {
            rows: [
                { view: "timeline", scrollX: "scrollHor", scrollY: "scrollVer" },
                { view: "scrollbar", id: "scrollHor" }
            ]
        },
        { view: "scrollbar", id: "scrollVer" }
    ]
};
gantt.init("gantt_here");


(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
        m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

ga('create', 'UA-11031269-1', 'auto');
ga('send', 'pageview');
