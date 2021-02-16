$(document).ready(function () {

    $('.mgallery_albume').on('click', 'img', function () {

        var parent = $(this).parent();
        var super_parent = $(parent).parent();

        var sp_class_name = $(super_parent).attr("id");

        gallery_items = $("#" + sp_class_name + " > div");

        var clicked_img_src = $(this).attr("src");
        var clicked_video_src = $(this).attr("video_src");
        var clicked_imgs_title = $(parent).find("p").html();
        var content_type = $(this).attr("content_type");

        gallery_index = -1;

        Show_Gallery_Item(gallery_index, clicked_img_src,clicked_video_src, clicked_imgs_title, content_type);
    });

    var gallery_index = -1;
    var gallery_items = [];

    function Show_Gallery_Item(index, img_src,video_src, img_p, content_type) {

        var _index = -1;

        var _img_src = "";
        var _img_p = "";
        var _content_type = "";
        var _video_src = "";

        if (index == -1) {
            jQuery.each(gallery_items, function () {
                _index++;
                var src = $(this).find('img').attr("src");
                if (src === img_src) {
                    gallery_index = _index;

                    _img_src = img_src;
                    _img_p = img_p;
                    _video_src = video_src;
                    _content_type = content_type;

                    return false;// for break the loop;
                }
            });
        }
        else {
            jQuery.each(gallery_items, function () {
                _index++;

                if (_index == index) {
                    _img_src = $(this).find('img').attr("src");
                    _img_p = $(this).find("p").html();
                    _video_src = $(this).find("img").attr("video_src");
                    _content_type = $(this).find("img").attr("content_type");

                    return false; // for break the loop;
                }
            });
        }

        $('#gallery_stage_img_desktop').attr('src', _img_src);
        $('#gallery_stage_img_mobile').attr('src', _img_src);

        $('#gallery_desktop_video_element').attr('src', _video_src);
        $('#gallery_mobile_video_element').attr('src', _video_src);

        $('#gallery_stage_p_desktop').html(_img_p);
        $('#gallery_stage_p_mobile').html(_img_p);


        if (_content_type === "video") {
            $('#gallery_stage_img_desktop').css('display', "none");
            $('#gallery_stage_img_mobile').css('display', "none");

            $('#gallery_desktop_video_div').css('display', "block");
            $('#gallery_mobile_video_div').css('display', "block");
        } else if (_content_type === "image") {
            $('#gallery_stage_img_desktop').css('display', "block");
            $('#gallery_stage_img_mobile').css('display', "block");

            $('#gallery_desktop_video_div').css('display', "none");
            $('#gallery_mobile_video_div').css('display', "none");
        }


        $('#gallery_stage').css('display', 'block');

        if (gallery_index == 0) {
            $('#gallery_stage_left_button_desktop').attr('src', '/Content/MGallery_Items/none.png');
            $('#gallery_stage_left_button_mobile').attr('src', '/Content/MGallery_Items/none.png');
        }
        else {
            $('#gallery_stage_left_button_desktop').attr('src', '/Content/MGallery_Items/left.png');
            $('#gallery_stage_left_button_mobile').attr('src', '/Content/MGallery_Items/left.png');
        }

        if (gallery_index == gallery_items.length - 1) {
            $('#gallery_stage_right_button_desktop').attr('src', '/Content/MGallery_Items/none.png');
            $('#gallery_stage_right_button_mobile').attr('src', '/Content/MGallery_Items/none.png');
        } else {
            $('#gallery_stage_right_button_desktop').attr('src', '/Content/MGallery_Items/right.png');
            $('#gallery_stage_right_button_mobile').attr('src', '/Content/MGallery_Items/right.png');
        }

    }


    function gallery_stage_left_button_click() {
        if (gallery_index > 0) {
            gallery_index--;
            Show_Gallery_Item(gallery_index, "", "");
        }
    }

    function gallery_stage_right_button_click() {
        if (gallery_index < gallery_items.length - 1) {
            gallery_index++;
            Show_Gallery_Item(gallery_index, "", "");
        }
    }

    function gallery_stage_close_button_click() {
        $('#gallery_stage').css('display', 'none');

        $('#gallery_desktop_video_element').attr('src', "");
        $('#gallery_mobile_video_element').attr('src', "");
    }


    // mobile viewer buttons clicks
    $('#gallery_stage_left_button_mobile').click(function () {
        gallery_stage_left_button_click();
    });

    $('#gallery_stage_right_button_mobile').click(function () {
        gallery_stage_right_button_click();
    });

    $('#gallery_stage_close_button_mobile').click(function () {
        gallery_stage_close_button_click();
    });

    // desktop viewer buttons clicks
    $('#gallery_stage_left_button_desktop').click(function () {
        gallery_stage_left_button_click();
    });

    $('#gallery_stage_right_button_desktop').click(function () {
        gallery_stage_right_button_click();
    });

    $('#gallery_stage_close_button_desktop').click(function () {
        gallery_stage_close_button_click();
    });
});