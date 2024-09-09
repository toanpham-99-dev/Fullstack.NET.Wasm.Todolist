(function () {
    window.CalendarFunctions = {
        initCalendar: function (culture, events) {

            var calendarEl = document.getElementById('calendar');
            console.log(events);
            var calendar = new FullCalendar.Calendar(calendarEl, {
                eventMouseEnter: function (info) {
                    var options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
                    var cultureLabels = [
                        {
                            culture: 'en-US',
                            labels: {
                                title: 'Title',
                                start: 'Start',
                                end: 'End'
                            }
                        },
                        {
                            culture: 'vi-VN',
                            labels: {
                                title: 'T.Đề',
                                start: 'B.Đầu',
                                end: 'K.Thúc'
                            }
                        }
                    ];
                    var foundlabel;
                    for (var i = 0; i < cultureLabels.length; i++) {
                        if (cultureLabels[i].culture == culture) {
                            foundlabel = cultureLabels[i];
                            break;
                        }
                    };
                    var eventObj = info.event;
                    tippy(info.el, {
                        content: '<strong>' + foundlabel.labels.title + ': <span style="color: aqua;">' + eventObj.title + '</span></strong>' +
                            '<br/>' +
                            '<strong>' + foundlabel.labels.start + ': <span style="color: aqua;">' + eventObj.start.toLocaleDateString(culture, options) + '</span></strong>' +
                            '<br/>' +
                            '<strong>' + foundlabel.labels.end + ': <span style="color: aqua;">' + eventObj.end.toLocaleDateString(culture, options) + '</span></strong>',
                        allowHTML: true,
                    });
                },
                eventClick: function (info) {
                    var eventObj = info.event;

                    if (eventObj.url) {
                        info.jsEvent.preventDefault(); // prevents browser from following link in current tab.
                        window.open(eventObj.url);
                    }
                },
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'multiMonthYear,dayGridMonth,timeGridWeek,timeGridDay,listMonth'
                },
                locale: culture,
                buttonIcons: false, // show the prev/next text
                weekNumbers: false,
                eventDisplay: 'block',
                eventBackgroundColor: '#198754',
                navLinks: true, // can click day/week names to navigate views
                editable: false,
                dayMaxEvents: true, // allow "more" link when too many events
                events: events
            });
            calendar.setOption('contentHeight', 650);
            calendar.render();
        }
    };
})();
