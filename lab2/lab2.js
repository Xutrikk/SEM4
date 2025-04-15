var array = [
    { id: 1, name: 'Vasya', group: 10 },
    { id: 2, name: 'Ivan', group: 11 },
    { id: 3, name: 'Masha', group: 12 },
    { id: 4, name: 'Petya', group: 10 },
    { id: 5, name: 'Kira', group: 11 },
];
var car = {}; // Используем type assertion
car.manufacturer = "manufacturer";
car.model = 'model';
var car1 = {};
car1.manufacturer = "manufacturer";
car1.model = 'model';
var car2 = {};
car2.manufacturer = "manufacturer";
car2.model = 'model';
var arrayCars = [{
        cars: [car1, car2]
    }];
// Данные о студентах
var students = [
    {
        id: 1,
        name: "Vasya",
        group: 10,
        marks: [
            { subject: "Math", mark: 8, done: true },
            { subject: "Physics", mark: 7, done: false },
        ],
    },
    {
        id: 2,
        name: "Masha",
        group: 12,
        marks: [
            { subject: "Math", mark: 9, done: true },
            { subject: "Physics", mark: 10, done: true },
        ],
    },
    {
        id: 3,
        name: "Ivan",
        group: 10,
        marks: [
            { subject: "Math", mark: 6, done: false },
            { subject: "Physics", mark: 5, done: true },
        ],
    },
];
// Создаем объект группы с методами
var group = {
    students: students,
    // Фильтрация студентов по номеру группы
    studentsFilter: function (group) {
        return this.students.filter(function (student) { return student.group === group; });
    },
    // Фильтрация студентов по оценке (если хотя бы одна оценка соответствует)
    marksFilter: function (mark) {
        return this.students.filter(function (student) {
            return student.marks.some(function (m) { return m.mark === mark; });
        });
    },
    // Удаление студента по id
    deleteStudent: function (id) {
        this.students = this.students.filter(function (student) { return student.id !== id; });
    }
};
// Примеры использования
console.log("Студенты группы 10:", group.studentsFilter(10));
console.log("Студенты с оценкой 9:", group.marksFilter(9));
// Удаление студента с id = 1
group.deleteStudent(1);
console.log("После удаления студента с id = 1:", group.students);
