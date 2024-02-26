$(document).ready(function () {
    $('#currencySwitch').change(function () {
        var isChecked = $(this).is(":checked");

        if (isChecked) {
            $('.stockValues').hide();
            $('.stockValuesInWalletCurrency').show();
        } else {
            $('.stockValues').show();
            $('.stockValuesInWalletCurrency').hide();
        }
    });

    $('#currencySwitch').trigger('change');
});