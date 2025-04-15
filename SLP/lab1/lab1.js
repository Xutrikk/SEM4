/*1.Напишите функцию, которая принимает массив из 10 целых чисел (от 0 до 9) и возвращает строку этих чисел в виде номера телефона. 
Формат номера телефона должен соответствовать "(xxx) xxx-xxxx".*/

function createNumber(numberArr) {
    if (numberArr.length != 10)
        return "Неверный формат номера";
    var phoneNum;
    phoneNum = String("(" + numberArr[0] + numberArr[1] + numberArr[2] + ") " + numberArr[3] + numberArr[4] + numberArr[5] + "-" + numberArr[6] + numberArr[7] + "-" + numberArr[8] + numberArr[9]);
    return phoneNum;
}
var num_arr = [3, 7, 5, 2, 9, 1, 2, 3, 4, 5];
console.log("Задание 1\n" + "Номер телефона после обработки: " + createNumber(num_arr));

/*2.Если перечислить все натуральные числа до 10, кратные 3 или 5, то получим 3, 5, 6 и 9. Сумма этих чисел равна 23.
Завершите метод так, чтобы он возвращал сумму всех чисел, кратных 3 или 5, меньше переданного числа. Кроме того, если число отрицательное, верните 0.
Примечание. Если число кратно и 3, и 5, считайте его только один раз. */

var Challenge = /** @class */ (function () {
    function Challenge() {
    }
    Challenge.solution = function (number) {
        if (number < 0) {
            return 0;
        }
        else {
            var sum = 0;
            for (var i = 1; i < number; i++) {
                if (i % 3 == 0 || i % 5 == 0) {
                    sum += i;
                }
            }
            return sum;
        }
    };
    return Challenge;
}());
console.log("Задание 2\n" + "Сумма /5 и /3: " + Challenge.solution(10));

/*3.Дан целочисленный массив nums, поверните массив вправо на k шагов, где k неотрицательно.*/

function swapArray(arr, k) {
    if (k == null) {
        return arr;
    }
    if (k > arr.length) {
        return null;
    }
    if (arr.length == 0) {
        return null;
    }
    var newArr = [];
    for (var i = 0; i < k; i++) {
        newArr[i] = arr[arr.length - (k - i)];
    }
    for (var i = 0; i < arr.length - k; i++) {
        newArr[k + i] = arr[i];
    }
    return newArr;
}
console.log("Задание 3\n" + "Массив после обработки: " + swapArray([1, 2, 3, 4, 5, 6, 7, 8, 9], 3));

/*4.Есть два отсортированных массива nums1 и nums2 размера m и n соответственно, вернуть медиану двух отсортированных массивов. 
Медиана число (два числа) находящееся в середине массива. */

function GetMedian(arrnum1, arrnum2) {
    if (arrnum1.length == 0 && arrnum2.length == 0) {
        return null;
    }
    var concatTwoArr = []; 
    for (var i = 0; i < arrnum1.length; i++) 
     {
        concatTwoArr[i] = arrnum1[i];
    }
    var j = 0;
    for (var i = arrnum1.length; i < arrnum1.length + arrnum2.length; i++, j++) 
     {
        concatTwoArr[i] = arrnum2[j];
    }
    concatTwoArr.sort(); 
    if (concatTwoArr.length % 2 == 0) {
        return (concatTwoArr[(concatTwoArr.length / 2) - 1] + concatTwoArr[concatTwoArr.length / 2]) / 2;
    }
    else {
        return concatTwoArr[Math.floor(concatTwoArr.length / 2)];
    }
}
console.log("Задание 4\n" + "Медиана: " + GetMedian([1, 2], [3, 4]));
console.log("Медиана: " + GetMedian([1, 3], [2]));
