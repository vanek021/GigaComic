function initDragEvents() {
    var app = document.getElementById('app');

    $(".dragdrop__item").each(function () {
        var drag = this;
        drag.addEventListener("dragstart", function (e) {
            var hideDragImage = this.cloneNode(true);
            hideDragImage.id = "hideDragImage-hide"

            var dragImage = this.cloneNode(true);
            dragImage.id = "draggeimage";
            dragImage.style.position = "absolute";

            hideDragImage.style.opacity = 0;
            hideDragImage.style.display = "none";

            dragImage.style.left = "-999px";
            dragImage.style.top = "-999px";

            app.appendChild(hideDragImage);
            app.appendChild(dragImage);
            e.dataTransfer.setDragImage(hideDragImage, 0, 0);
        }, false);
    });

    $(".dragdrop__item").each(function () {
        var drag = this;
        drag.addEventListener("dragend", function (e) {
            var hideDragImage = document.getElementById('hideDragImage-hide');
            var dragImage = document.getElementById('draggeimage');
            hideDragImage.remove()
            dragImage.remove()
        }, false);
    });

    $(".dragdrop__item").each(function () {
        var drag = this;
        drag.addEventListener("drag", function (e) {
            dragImage = document.getElementById('draggeimage');
            if (dragImage) {
                dragImage.style.left = e.pageX + 5 + "px";
                dragImage.style.top = e.pageY + 5 + "px";
            }
        });
    });

    $(".dragdrop__item").each(function () {
        var drag = this;
        drag.addEventListener("dragenter", function (e) {
            e.target.style.opacity = "0.3";
        });
    });

    $(".dragdrop__item").each(function () {
        var drag = this;
        drag.addEventListener("dragleave", function (e) {
            e.target.style.opacity = "1";
        });
    });

    $(".dragdrop__item").each(function () {
        var drag = this;
        drag.addEventListener("drop", function (e) {
            e.target.style.opacity = "1";
        });
    });
}