
$(document).ready(function () {

    // REFRESING PAGE WHEN A DIFFERENT READY_MENU CHOOSED
    $("#hazir_menu").change(function () {
        var menu_id = $(this).val();
        var org_info_id = $("#organization_information_id").val();

        window.location = "/Admin_Sistem/SiparisAl_Yemek/" + org_info_id + "_" + menu_id;
    });


    // ADDING FOODS TO SELECT ELEMENT WHICS ARE BELONG TO CHOOSED CATEGORY
    $("#select_category").val(-1).change();

    $("#select_category").change(function () {
        var category_id = $(this).val();

        var category;
        for (var i = 0; i < categories_and_foods.length; i++) {
            if(categories_and_foods[i].id==category_id){
                category = categories_and_foods[i];

                $("#food_id").empty();

                for (var j = 0; j < category.foods.length; j++) {
                    var _food = category.foods[j];
                    $("#food_id").append("<option value=" + _food.id + ">" + _food.name + "</option>")
                }
            }
        }

        var deger = _remenu_and_foods.length;
        var remenu_food;
        for (var i = 0; i < _remenu_and_foods.length; i++) {
            var _food_ = _remenu_and_foods[i];
            if (_food_.category_id == category_id) {
                $("#food_id").val(_food_.food_id).change();
            }
        }

    });



    // FILLING FOOD'S FEATURES BOX
    $("#food_id").val(-1).change();

    $("#food_id").change(function () {
        var category_id = $("#select_category").val();
        var food_id = $(this).val();

        var category;
        for (var i = 0; i < categories_and_foods.length; i++) {
            if (categories_and_foods[i].id == category_id) {
                category = categories_and_foods[i];

                for (var j = 0; j < category.foods.length; j++) {
                    var _food = category.foods[j];
                    if (_food.id == food_id) {
                        $("#price").val(_food.price);
                        $("#quantity").val(_food.quantity);
                        $("#unit").val(_food.unit);

                        var price = $("#price").val();
                        var people_count = $("#people_count").val();
                        var total_price = price * people_count;
                        $("#total_price").val(total_price);
                        
                    }
                }
            }
        }

    });
    

    // CALCULATING FOOD PRICE WHEN Quantitiy Changed
    $('#quantity').bind('input', function () {
        YemekFiyatiHesapla("calculate_price_by_quantity");
        YemekFiyatiHesapla("calculate_totalPrice_by_price");
    });
    // CALCULATING FOOD PRICE WHEN People Count Changed
    $('#people_count').bind('input', function() {
        YemekFiyatiHesapla("calculate_totalPrice_by_peopleCount");
    });
    // CALCULATING FOOD PRICE WHEN Price Changed
    $('#price').bind('input', function () {
        YemekFiyatiHesapla("calculate_totalPrice_by_price");
    });



    var YemekFiyatiHesapla = function (caller) {

        var category_id = $("#select_category").val();
        var food_id = $("#food_id").val();

        var category;
        for (var i = 0; i < categories_and_foods.length; i++) {
            if (categories_and_foods[i].id == category_id) {
                category = categories_and_foods[i];

                for (var j = 0; j < category.foods.length; j++) {
                    var _food = category.foods[j];
                    if (_food.id == food_id) {

                        if (caller == "calculate_price_by_quantity") {
                            var quantity_price_rate = _food.price / _food.quantity;
                            var quantity = $("#quantity").val();
                            $("#price").val(quantity * quantity_price_rate);


                        }
                        else if (caller == "calculate_totalPrice_by_peopleCount" || caller == "calculate_totalPrice_by_price") {
                            var price = $("#price").val();
                            var people_count = $("#people_count").val();
                            var total_price = price * people_count;
                            $("#total_price").val(total_price);
                        }

                        
                    }
                }
            }
        }
    }



    // new



    $(".food_item").click(function () {
        var remenuId = $(this).attr('id').split('_')[1];

        var orgAndFoodId = $("#orgAndFoodId_" + remenuId).text();
        var foodName = $("#foodName_" + remenuId).text();
        var unit = $("#unit_" + remenuId).text();
        var quantity = $("#quantity_" + remenuId).text();
        var price = $("#price_" + remenuId).text().split(' ')[0];
        var peopleCount = $("#peopleCount_" + remenuId).text().split(' ')[0];
        var totalPrice = $("#totalPrice_" + remenuId).text().split(' ')[0];


        $("#modalOrgAndFoodId").val(orgAndFoodId);
        $("#modalFoodName").text("Yemek: " + foodName);
        $("#modalUnit").val(unit);
        $("#modalQuantity").val(quantity);
        $("#modalPrice").val(price);
        $("#modalPeopleCount").val(peopleCount);
        $("#modalTotalPrice").val(totalPrice);

    });



});