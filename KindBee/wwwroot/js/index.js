
function AddOneProductInBasket(Id, el) {
    var product = el.parentNode;
    var count = product.children[0].value;
    count++;
    if (product.children[0].max < count) {
        return;
    }
    $.post("/Basket/AddOneProductInBasket", { id: Id })
        .done(function (data) {
            //анализируем ответ от сервера. Если больше продуктов не осталось, то деактивируем кнопку добавления и выводим сообщение
            if (data == 204) {
                product.children[0].value = 0;
            }
            if (data == 403) { //если пользователь не авторизован - редирект на форму Login
                alert("Чтобы оформить заказ необходимо авторизоваться");
                window.location.href = '/Account/Login';
                //setTimeout(function () {
                //    window.location.href = '/Account/Login';
                //}, 3 * 1000);
            }
            if (data==200) { //ok
                product.children[0].value = count;
            }
        }).fail(function (data) {
             //если что то не так - выводим сообщение об ошибке
            alert("error");
            alert(data);
        });
};

function DeleteOneProductFromBasket(Id, el) {
    var product = el.parentNode;
    var count = product.children[0].value;
    if (product.children[0].value == 0) {
        return;
    }
    $.post("/Basket/DeleteOneProductFromBasket", { id: Id })
        .done(function (data) {
            if (data == 403) { //если пользователь не авторизован - редирект на форму Login
                alert("Чтобы оформить заказ необходимо авторизоваться");
                window.location.href = '/Account/Login';
            }
            count--;
            product.children[0].value = count;
        }).fail(function (data) {
            alert("error");
            alert(data);
        });

    /*.fail(function () {
            alert("error");
                statusCode: {
                    404: function () {
                        alert("page not found");
                    }
                    401: function () {
                        alert("unauthor");
                    }
                }
        })
        .always(function () {
            alert("complete");
        }); */
};
