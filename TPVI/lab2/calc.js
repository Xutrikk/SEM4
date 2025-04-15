document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("sum").textContent = Sum(7, 3);
    document.getElementById("sub").textContent = Sub(7, 3);
    document.getElementById("mul").textContent = Mul(7, 3);
    document.getElementById("div").textContent = Div(7, 3).toFixed(2);
});

function Sum(a, b) { return a + b; }
function Sub(a, b) { return a - b; }
function Mul(a, b) { return a * b; }
function Div(a, b) { return (b !== 0) ? (a / b) : "Ошибка"; }
