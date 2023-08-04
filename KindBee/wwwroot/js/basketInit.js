function ReCountTotalSum() {

    var total = 0;

    let strings = $("#tableBody").children();

    for (let i = 0; i < strings.length; i++) {

        try {
            let price = parseFloat(strings[i].children[1].textContent);
            let quantity = Number(strings[i].children[2].textContent);
            total += price * quantity;
        } catch (e) {

        }
    }

    document.querySelector('.totalSum').textContent = total;
    return total;
    /*
   //alert(total);
   // e = document.getElementByName('totalSum');
   //alert($("#totalSum").content);
   //let strings = document.getElementsByTagName('tr');
   //document.querySelector('#totalSum')[0].textContent = 'asdfhgfdgf';
   //document.getElementByName('totalSum').textContent = 'asdfhgfdgf';

   //$("#tableBody").children[$("#tableBody").children().length - 1].children[0].textContent = total;
   //$("#totalSum").textContent = 'asdfasdfawsd';
   //$("#tableBody").children()[0].
   // alert(t);
   //t.children().forEach(function (elem) {
   //    alert(elem.tagName); // HEAD, текст, BODY
   //});

   //$("#tableBody").children().$each(function (index, object) {
   //    var Input = $(object);
   //    total += Input.children("#m").children("#quantityInBasket").value;
   //    alert(total);
   //});

   // alert($("#tableBody").children());
   //for (var t of $("#tableBody").children()) {
   //    total += t.children("#m").children("#quantityInBasket").value;

   //}
   */
}

function AddOneProductInBasket(Id, el) {
    var product = el.parentNode.parentNode;
    var count = Number(product.children[2].textContent);


    if (product.children[0].max < count) {
        return;
    }
    $.post("/Basket/AddOneProductInBasket", { id: Id })
        .done(function (data) {
            if (data == 200) {
                count++;
                product.children[2].textContent = count;
                ReCountTotalSum();
            }
            else {
                alert("Не удалось добавить товар из-за проблемы на сервере. Повторите операцию позже");
                location.reload();
            }
        });
};

function DeleteOneProductFromBasket(Id, el) {
    var product = el.parentNode.parentNode;
    var count = Number(product.children[2].textContent);
    $.post("/Basket/DeleteOneProductFromBasket", { id: Id })
        .done(function (data) {
            if (data == 204) {
                var row = el.parentNode.parentNode;
                row.remove();
                return;
            }
            else if (data == 200) {
                count--;
                product.children[2].textContent = count;
                if (count == 0) {
                    var str = el.parentNode.parentNode;
                    str.remove();
                }

                var totalSum = ReCountTotalSum();
                if (totalSum <= 0) {
                    $("#BasketTable").remove();
                    $("#deleteButton").remove();
                    let emptyBasketMessageBlock = document.createElement('div');
                    emptyBasketMessageBlock.innerHTML = "<h3 style=\"margin:100px\">Вы очистили корзину</h3>";
                    orderButton.before(emptyBasketMessageBlock);
                    $("#orderButton").remove();
                }
            }
            else {
                alert("Не удалось удалить из-за проблемы на сервере. Повторите операцию позже");
                location.reload();
            }
        });
};


function DeletePositionFromBasket(Id, el) {
    setTimeout(function () {
        $.post("/Basket/DeletePosition", { id: Id })
            .done(function (data) {
                if (data == 200) {
                    var str = el.parentNode.parentNode;
                    str.remove();
                    var totalSum = ReCountTotalSum();
                    if (totalSum <= 0) {
                        $("#BasketTable").remove();
                        $("#deleteButton").remove();
                        let emptyBasketMessageBlock = document.createElement('div');
                        emptyBasketMessageBlock.innerHTML = "<h3 style=\"margin:100px\">Вы очистили корзину</h3>";
                        orderButton.before(emptyBasketMessageBlock);
                        $("#orderButton").remove();
                    }
                    return;
                }
                else {
                    alert("Не удалось удалить из-за проблемы на сервере. Повторите операцию позже");
                    location.reload();
                }
            });
    }, 500);


};

function DeleteAllPositions() {
    $.post("/Basket/DeleteAllPositions")
        .done(function (data) {
            if (data == 200) {
                /* $("#tableBody").children().remove();*/
                $("#BasketTable").remove();
                $("#deleteButton").remove();
                let emptyBasketMessageBlock = document.createElement('div');
                emptyBasketMessageBlock.innerHTML = "<h3 style=\"margin:100px\">Вы очистили корзину</h3>";
                orderButton.before(emptyBasketMessageBlock);
                $("#orderButton").remove();
                return;
            }
            else {
                alert("Не удалось удалить из-за проблемы на сервере. Повторите операцию позже");
                location.reload();
            }
        });
};


function InitOrder() {
    setTimeout(function () {
        window.location.href = '/Order/Init';
    }, 5 * 1000);
};
