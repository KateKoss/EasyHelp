$(".nav-tabs a").click(navclick);

function navclick() {
    var position = $(".nav-tabs .active").width();
    var width = $(".nav-tabs .active").position();
    $(".slider").css({ "left": +position.left, "width": width });
}
    
var actWidth = $(".nav-tabs .active").width();
var actPosition = $(".nav-tabs .active").position();
    
$(".slider").css({ "left": +actPosition.left, "width": actWidth });
