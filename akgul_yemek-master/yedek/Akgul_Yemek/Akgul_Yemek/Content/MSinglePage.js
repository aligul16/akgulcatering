$(document).ready(function () {
    
    $("#navbar_head").css("display", "none");

    // initalize stage elements
    var gallery_items = $("#MSinglePage_Container > div");
    var flag = true;
    jQuery.each(gallery_items, function () {
        if (flag) { // skip for first element
            flag = false;
            return true;// for continue the loop
        }
        var src = $(this).css("display", "none");
    });

    var before_showed_element_index = 0;

    $('.navigation_menu').on('click', 'li', function () {
        
        var index = $(this).attr("id");

        if (index < 0) {
            return;
        }

        if (index == 1)
        {
            $("#hakkimizda").css("display","none");
            $("#sertifikalarimiz").css("display","none");
            $("#hizmetlerimiz").css("display","none");
            $("#insankaynaklari").css("display","none");

            var target = $(this).attr("target");
            $("#" + target).css("display", "");
        }

        // if clicked to the same element
        if (index == before_showed_element_index)
            return;

        $(gallery_items[index]).css("display", "block");


        // diplaying/hiddening navbar
        if (index == 0) {
            $("#navbar_head").css("display", "none");
            $("#navbar_homepage").css("display", "block");
        } else {
            $("#navbar_homepage").css("display", "none");
            $("#navbar_head").css("display", "block");
        }


        if (index > before_showed_element_index) // if shift direction is right to left
        {
                var previous_element_width = $(gallery_items[before_showed_element_index]).css("width");
                var previous_element_top = $(gallery_items[before_showed_element_index]).css("top");

               if (before_showed_element_index == 0)
                    previous_element_top += 150;
                else if (index == 0)
                    previous_element_top -= 0;

                $(gallery_items[index]).css("left", previous_element_width);
                $(gallery_items[index]).css("top", previous_element_top);



                $(gallery_items[before_showed_element_index]).animate({
                    opacity: 1,
                    left: -1 * previous_element_width,
                }, 1000, function () {
                    // Animation complete.

                    // hide previous element
                    $(gallery_items[before_showed_element_index]).css("display", "none");
                    before_showed_element_index = index;
                });

                $(gallery_items[index]).animate({
                    opacity: 1,
                    left: 0,
                }, 1000, function () {
                    // Animation complete.

                    // #BUG# // at mobile
                    // locating element to own place
                    //$(gallery_items[before_showed_element_index]).css("position", "");

                    
                });

        }
        else // if shift direction is left to right
        {

            var previous_element_width = $(gallery_items[before_showed_element_index]).css("width");
            var previous_element_top = $(gallery_items[before_showed_element_index]).css("top");

            if (before_showed_element_index == 0)
                previous_element_top += 150;
            else if (index == 0)
                previous_element_top -= 0;

            $(gallery_items[index]).css("left", -previous_element_width);
            $(gallery_items[index]).css("top", previous_element_top);
            

            $(gallery_items[before_showed_element_index]).animate({
                opacity: 1,
                left: previous_element_width,
            }, 1000, function () {
                // Animation complete.

                // hide previous element
                $(gallery_items[before_showed_element_index]).css("display", "none");
                before_showed_element_index = index;
            });

            $(gallery_items[index]).animate({
                opacity: 1,
                left: 0,
            }, 1000, function () {
                // Animation complete.

                // #BUG# // at mobile
                // locating element to own place
                // $(gallery_items[before_showed_element_index]).css("position", "");

               
            });

        }
        // closing collapseable navbar
        $(".collapse").collapse('hide');

    });

});