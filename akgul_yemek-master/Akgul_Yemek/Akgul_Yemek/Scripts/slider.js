//Based on https://codepen.io/zuraizm/pen/vGDHl pen by zuraiz
jQuery(document).ready(function ($) {

    startSlider($('#slider'), 30); // Slide container ID, SlideShow interval 

    function startSlider(obj, timer) {

        var obj, timer;
        var id = "#" + obj.attr("id");
        var slideCount = obj.find('ul li').length;
        slideWidth = obj.attr("data-width");
        var sliderUlWidth = (slideCount + 1) * slideWidth;
        var time = 2;
        var $bar,

        isPause,
        tick,
        percentTime;
        isPause = false;

        $bar = obj.find('.progress .bar');

        function startProgressbar() {
            resetProgressbar();
            percentTime = 0;
            tick = setInterval(interval, timer);
        }

        function interval() {
            if (isPause === false) {
                percentTime += 1 / (time + 0.1);
                $bar.css({
                    width: percentTime + "%"
                });
                if (percentTime >= 100) {
                    moveRight();
                    startProgressbar();
                }
            }
        }

        function resetProgressbar() {
            $bar.css({
                width: 0 + '%'
            });
            clearTimeout(tick);
        }

        function startslide() {


            obj.find('ul').css({ width: sliderUlWidth + 'vw', marginLeft: -slideWidth + 'vw' });

            obj.find('ul li:last-child').appendTo(obj.attr('id') + ' ul');

        }

        if (slideCount > 1) {
            startslide();
            startProgressbar();
        }
        else { // hade navigation buttons for 1 slide only
            $(id + ' button.control_prev').hide();
            $(id + ' button.control_next').hide();
        }




        function moveLeft() {
            $(id + ' ul').animate({
                left: +slideWidth + 'vw'
            }, 1000, function () {
                $(id + ' ul li:last-child').prependTo(id + ' ul');
                $(id + ' ul').css('left', '0');
            });

        }

        function moveRight2() { // fix for only 2 slades
            $(id + ' ul li:last-child').prependTo(id + ' ul');
            $(id + ' ul').animate({ left: '100vw' }, 0);
            $(id + ' ul').animate({
                left: 0 + 'vw'
            }, 1000, function () {


                $(id + ' ul').css('left', '0');
            });
        }

        function moveRight() {
            if (slideCount > 2) {
                $(id + ' ul').animate({
                    left: -slideWidth + 'vw'
                }, 1000, function () {
                    $(id + ' ul li:first-child').appendTo(id + ' ul');
                    $(id + ' ul').css('left', '');
                });
            }
            else {
                moveRight2();
            }
        }

        $(id + ' button.control_prev').click(function () {
            moveLeft();
            startProgressbar();
        });

        $(id + ' button.control_next').click(function () {

            moveRight();

            startProgressbar();
        });

        $(id + ' .progress').click(function () {
            if (isPause === false) {
                isPause = true;
            }
            else {
                isPause = false;
            }
        });
    };
});