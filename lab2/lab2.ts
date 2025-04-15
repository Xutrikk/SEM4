// 1. 
interface Student {
    id: number;
    name: string;
    group: number;
  }
  
  const array: Student[] = [
    {id: 1, name: 'Vasya', group: 10}, 
    {id: 2, name: 'Ivan', group: 11},
    {id: 3, name: 'Masha', group: 12},
    {id: 4, name: 'Petya', group: 10},
    {id: 5, name: 'Kira', group: 11},
  ];
  
  // 2. 
  interface CarsType {
    manufacturer: string;
    model: string;
  }
  
  let car: CarsType = {} as CarsType; 
  car.manufacturer = "manufacturer";
  car.model = 'model';
  
  // 3. 
  interface ArrayCarsType {
    cars: CarsType[];
  }
  
  const car1: CarsType = {} as CarsType;
  car1.manufacturer = "manufacturer";
  car1.model = 'model';
  
  const car2: CarsType = {} as CarsType;
  car2.manufacturer = "manufacturer";
  car2.model = 'model';
  
  const arrayCars: ArrayCarsType[] = [{
    cars: [car1, car2]
  }];
//4.Имеются следующие типы. Создать недостающие типы, необходимые структуры данных и дописать функции.
type MarkFilterType = 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10;

type GroupFilterType = 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12;

type DoneType = boolean ;

type MarkType = {
    subject: string;
    mark: MarkFilterType; 
    done: DoneType;
};

type StudentType = {
    id: number;
    name: string;
    group: GroupFilterType; 
    marks: Array<MarkType>;
};

type GroupType = {
    students: Array<StudentType>; 
    studentsFilter: (group: GroupFilterType) => Array<StudentType>; 
    marksFilter: (mark: MarkFilterType) => Array<StudentType>;
    deleteStudent: (id: number) => void; 
};

const students: Array<StudentType> = [
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

const group: GroupType = {
    students: students,

    studentsFilter: function (group: GroupFilterType): Array<StudentType> {
        return this.students.filter(student => student.group === group);
    },

    marksFilter: function (mark: MarkFilterType): Array<StudentType> {
        return this.students.filter(student => 
            student.marks.some(m => m.mark === mark)
        );
    },

    deleteStudent: function (id: number): void {
        this.students = this.students.filter(student => student.id !== id);
    }
};

console.log("Студенты группы 10:", group.studentsFilter(10));
console.log("Студенты с оценкой 9:", group.marksFilter(9));

group.deleteStudent(1);
console.log("После удаления студента с id = 1:", group.students);
