function initDragEvent(drag) {
    var app = document.getElementById('app');

    drag.addEventListener("dragstart", function (e) {
        e.stopPropagation();
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

        //$('body').not('.dragdrop').css("filter", "blur(3px)");
    }, false);

    drag.addEventListener("dragend", function (e) {
        e.stopPropagation();
        var hideDragImage = document.getElementById('hideDragImage-hide');
        var dragImage = document.getElementById('draggeimage');
        hideDragImage.remove()
        dragImage.remove()
    }, false);

    drag.addEventListener("drag", function (e) {
        dragImage = document.getElementById('draggeimage');
        if (dragImage) {
            dragImage.style.left = e.pageX + 5 + "px";
            dragImage.style.top = e.pageY + 5 + "px";
        }
    });

    drag.addEventListener("dragenter", function (e) {
        e.target.style.opacity = "0.3";
    });

    drag.addEventListener("dragleave", function (e) {
        e.target.style.opacity = "1";
    });

    drag.addEventListener("drop", function (e) {
        e.target.style.opacity = "1";
    });
}

function initDragEvents() {
    $(".dragdrop__item").each(function () {
        initDragEvents(this);
    });
}

function initDragEventById(id) {
    initDragEvent($(`#${id}`)[0]);
}