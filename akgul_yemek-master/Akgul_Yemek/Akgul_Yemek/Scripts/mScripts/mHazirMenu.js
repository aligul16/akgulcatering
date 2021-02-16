
$(document).ready(function () {

    $(".mJS").val(-1).change();

    $(".mJS").change(function(){
        var category_ids = $(this).attr("id").split("-");
        var category_id = category_ids[1];
        var food_id = $(this).val();

        var category;
        for (var i = 0; i < categories_and_foods.length; i++) {
            if(categories_and_foods[i].id==category_id){
                category = categories_and_foods[i];

                for (var j = 0; j < category.foods.length; j++) {
                    var _food = category.foods[j];
                    if(_food.id==food_id){
                        $("#price_"+category.id).val(_food.price);
                        $("#quantity_"+category.id).val(_food.quantity);
                        $("#unit_"+category.id).val(_food.unit);
                    }
                }
            }
        }

    });



    $(".remenu_item").click(function () {
        var remenuId = $(this).attr('id').split('_')[1];
        
        var remenuandFoodId = $("#remenuAndFoodId_" + remenuId).text();
        var foodId = $("#foodId_" + remenuId).text();
        var foodName = $("#foodName_" + remenuId).text();
        var unit = $("#unit_" + remenuId).text();
        var quantity = $("#quantity_" + remenuId).text();
        var price = $("#price_" + remenuId).text();

        $("#modalRemenuAndFoodId").val(remenuandFoodId);
        $("#modalFoodName").text("Yemek: " + foodName);
        $("#modalFoodId").val(foodId);
        $("#modalUnit").val(unit);
        $("#modalQuantity").val(quantity);
        $("#modalPrice").val(price);
    });






});