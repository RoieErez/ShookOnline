$(function () {
    var btn = $(".slider__btn");
    btn.on("click", function () {
        $(".slider__item").first().clone().appendTo(".slider");
        $(".slider__image").first().css({ transform: "rotateX(-180deg)", opacity: 0 });
        setTimeout(function () {
            $(".slider__item").first().remove();
        }, 200);
    });
});

$(function () {

    $('#login-form-link').click(function (e) {
        $("#login-form").delay(100).fadeIn(100);
        $("#register-form").fadeOut(100);
        $('#register-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });
    $('#register-form-link').click(function (e) {
        $("#register-form").delay(100).fadeIn(100);
        $("#login-form").fadeOut(100);
        $('#login-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });

});