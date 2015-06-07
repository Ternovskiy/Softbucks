function LoadMerchandise(i) {
    $('#myCoffeLoad').load('@Url.Action("ViewMyMerchandise")', { 'productId': i });
}

function MerchandiseKick(i) {
    $('#myCoffeLoad').load('@Url.Action("MyMerchandiseKick")', { 'productId': i });
}

function MerchandiseKickAll() {
    $('#myCoffeLoad').load('@Url.Action("MyMerchandiseKickAll")');
}

function LoadProduct() {
    var name = $('#search').val();
    name = name.replace(new RegExp(" ", 'g'), "%20");
    var tp = $('#typePr').val();
    var pp = $("#PrPage").val();
    $('#resultsPrSearch').load('@Url.Action("ProductSearch")', { 'name': name, 'typeProduct': tp, prPage: pp });
}


$(document).ready(function () {



    LoadProduct();
    LoadMerchandise();
    $('#searchProduct').click(LoadProduct);

    $('#typePr').change(LoadProduct);

    $('#PrPage').change(LoadProduct);


});