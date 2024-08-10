//window.dragDropHandler = {
//    preventDefault: function (e) {
//        e.preventDefault();
//    },
//    handleDrop: function (e, dotnetHelper) {
//        e.preventDefault();
//        var files = e.dataTransfer.files;
//        if (files.length > 0) {
//            dotnetHelper.invokeMethodAsync('HandleFileDrop', files[0]);
//        }
//    }
//};

window.clearFileInput = function (elementId) {
    var fileInput = document.getElementById(elementId);
    if (fileInput) {
        fileInput.value = ""; // Clear the file input
    }
};