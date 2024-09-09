(function () {
    window.PusherFunctions = {
        initPusher: function (userId) {
            //Setup and get handle process pusher event
            var pusher = new Pusher('', {
                cluster: ''
            });
            var channel = pusher.subscribe(userId);
            channel.bind('notification', function (data) {
                notiToast(data);
                updateBagdeNoti();
            });
        }
    };
})();
