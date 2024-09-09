// toast settings
iziToast.settings({
    timeout: 5000, // default timeout
    resetOnHover: true,
    // icon: '', // icon class
    transitionIn: 'flipInX',
    transitionOut: 'flipOutX',
    position: 'topRight', // bottomRight, bottomLeft, topRight, topLeft, topCenter, bottomCenter, center
    onOpen: function () {
        console.log('callback abriu!');
    },
    onClose: function () {
        console.log("callback fechou!");
    }
});
// custom toast
function notiToast(data) {
    console.log(localStorage.getItem("culture"));
    iziToast.show({
        color: 'info',
        icon: 'fa fa-bell-o',
        title: 'New',
        message: localStorage.getItem("culture").localeCompare('en-US') == 0 ? data.NotificationInput.EnglishTitle : data.NotificationInput.VietnameseTitle,
        position: 'topRight', // bottomRight, bottomLeft, topRight, topLeft, topCenter, bottomCenter
        progressBarColor: 'rgb(151, 255, 255)',
        buttons: [
            [
                '<button>Detail</button>',
                function (instance, toast) {
                    window.location.href = data.NotificationInput.RedirectAt;
                }
            ]
        ]
    });
};

function updateBagdeNoti() {
    const notiBagde = document.getElementById("badge-noti");
    notiBagde.style.visibility = "visible";
}

//Noti
var navtoggle = document.querySelectorAll('#navtoggle')[0]
var deltoggle = document.querySelectorAll('#deletetoggle')[0]

