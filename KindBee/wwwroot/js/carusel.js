


window.onload = function () {
    window.scrollTo(0, +localStorage.getItem('page_scroll'));

   
    document.addEventListener('scroll', function () {
        localStorage.setItem('page_scroll', window.pageYOffset);
    });
}


var angle = 0;
function galleryspin(sign) {
    spinner = document.querySelector("#spinner");
    if (!sign) { angle = angle + 45; } else { angle = angle - 45; }
    spinner.setAttribute("style", "-webkit-transform: rotateY(" + angle + "deg); -moz-transform: rotateY(" + angle + "deg); transform: rotateY(" + angle + "deg);");
}
function sleep(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}
async function fun1() {
    while (true) {
        await sleep(2000);
        angle = angle + 45;
        spinner = document.querySelector("#spinner");
        spinner.setAttribute("style", "-webkit-transform: rotateY(" + angle + "deg); -moz-transform: rotateY(" + angle + "deg); transform: rotateY(" + angle + "deg);");
    }

}

fun1();