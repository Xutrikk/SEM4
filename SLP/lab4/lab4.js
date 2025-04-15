// 2
const myPromise = new Promise((resolve) => {
    setTimeout(() => {
        const randomNumber = Math.random();
        resolve(randomNumber);
    }, 3000);
});

myPromise.then((result) => {
    console.log("Случайное число:", result);
});

// 3
function createPromise(delay) {
    return new Promise((resolve) => {
        setTimeout(() => {
            const randomNumber = Math.random();
            resolve(randomNumber);
        }, delay);
    });
}
const promises = [
    createPromise(1000),
    createPromise(2000),
    createPromise(3000)
];

Promise.all(promises)
    .then((results) => {
        console.log("Все числа:", results);
    });

// 4
let pr = new Promise((res, rej) => {
    rej('ku');
});

pr
    .then(() => console.log(1))  
    .catch(() => console.log(2)) 
    .catch(() => console.log(3)) 
    .then(() => console.log(4)) 
    .then(() => console.log(5)); 

// 5
const promise = Promise.resolve(21);

promise
    .then((result) => {
        console.log(result); 
        return result;
    })
    .then((result) => {
        console.log(result * 2); 
    });

// 6
async function process() {
    const result = await Promise.resolve(21);
    console.log(result); 
    const doubled = result * 2;
    console.log(doubled); 
}

process();   

// 7
let promise7 = new Promise((res, rej) => { 
    res('Resolved promise - 1'); 
});

promise7
    .then((res) => { 
        console.log("Resolved promise -2"); 
    })
    .then((res) => { 
        console.log(res); 
    });
// Вывод: "Resolved promise -2", "undefined"

// 8
let promise8 = new Promise((res, rej) => { 
    res('Resolved promise -1'); 
});

promise8
    .then((res) => { 
        console.log(res); 
        return "OK"; 
    })
    .then((res1) => { 
        console.log(res1); 
    });
// Вывод: "Resolved promise -1", "OK"

// 9
let promise9 = new Promise((res, rej) => {
    res('Resolved promise - 1')
})

promise9
    .then((res) => {
        console.log(res); 
        return res;
    })
    .then((res1) => {
        console.log('Resolved promise - 2'); 
    });
// Вывод: "Resolved promise - 1", "Resolved promise - 2"

// 10
let promise10 = new Promise((res, rej) => {
    res('Resolved promise - 1')
})

promise10
    .then((res) => {
        console.log(res); 
        return res;
    })
    .then((res1) => {
        console.log(res1); 
    });
// Вывод: "Resolved promise - 1", "Resolved promise - 1"

// 11
let promise11 = new Promise((res, rej) => {
    res('Resolved promise - 1')
})

promise11
    .then((res) => {
        console.log(res); 
    })
    .then((res1) => {
        console.log(res1); 
    });
// Вывод: "Resolved promise - 1", undefined


