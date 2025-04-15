//1. Система управления пользователями
abstract class BaseUser{
    constructor(public readonly id:number,public readonly name:string){}

    abstract getRole():string;

    abstract getPermissions():string[];
}

class Admin extends BaseUser{
    getRole():string{
        return "Admin"
     }

     getPermissions(): string[] {
         return ["Просмотр контента",
            "Добавление комментариев",
            "Удаление комментариев",
            "Управление пользователями"];
     }
}

class Guest extends BaseUser{
    getRole():string{
        return "Guest"
     }

     getPermissions(): string[] {
         return ["Просмотр контента"];
     }
}

class User extends BaseUser{
    getRole():string{
        return "User"
     }

     getPermissions(): string[] {
         return ["Просмотр контента","Добавление комментариев"];
     }
}
console.log("Задание 1:")
const admin = new Admin(52,"Огник");
console.log(admin.getPermissions());

const guest = new Guest(42,"Симка");
console.log(guest.getPermissions());

const user = new User(55,"Хутрик");
console.log(user.getPermissions());

//2. Полиморфизм и интерфейсы
interface IReport{
    title:string;
    content: string;
    getGenerate():string | object;
}

class HTMLReport implements IReport{
    constructor(
       public title:string,
       public content: string
    ){}
    getGenerate():string{
        return `<h1>${this.title}</h1><p>${this.content}</p>`;
    }
}

class JSONReport implements IReport{
 constructor(
    public title:string,
    public content: string
 ){}
 getGenerate():string{
     return `title: ${this.title}, content: ${this.content}`;
 }
}
console.log("Задание 2:")
const report1 = new HTMLReport("Отчет 1", "Содержание отчета");
console.log(report1.getGenerate()); 

const report2 = new JSONReport("Отчет 2", "Содержание отчета");
console.log(report2.getGenerate()); 

//3. Обобщенный кеш данных
class DataCache<T> {
    private items: {
        [key: string]: {
            value: T;
            expiresAt: number;
        };
    } = {};

    add(key: string, value: T, ttl: number): void {
        const expiresAt = Date.now() + ttl;
        this.items[key] = { value, expiresAt };
    }

    get(key: string): T | null {
        const item = this.items[key];
        if (!item || Date.now() > item.expiresAt) {
            delete this.items[key]; 
            return null;
        }
        return item.value;
    }

    clearExpired(): void {
        const now = Date.now();
        Object.keys(this.items).forEach(key => {
            if (now > this.items[key].expiresAt) {
                delete this.items[key];
            }
        });
    }
}
console.log("Задание 3:")
const cache = new DataCache<number>();
cache.add("price", 100, 5000);
console.log(cache.get("price")); // 100
setTimeout(() => console.log(cache.get("price")), 6000); // null

//4. Дженерик-фабрика объектов
function createInstance<T>(cls: new (...args: any[]) => T, ...args: any[]): T {
    return new cls(...args);
}

class Product {
    constructor(public name: string, public price: number) {}
}
console.log("Задание 4:")
const p = createInstance(Product, "Телефон", 50000);
console.log(p); // Product { name: "Телефон", price: 50000 }

//5. Логирование событий с кортежами
enum LogLevel {
    INFO = "INFO",
    WARNING = "WARNING",
    ERROR = "ERROR"
}

type LogEntry = [Date, LogLevel, string];

function logEvent(event: LogEntry): void {
    const [timestamp, level, message] = event;
    console.log(`[${timestamp.toISOString()}] [${level}]: ${message}`);
}
console.log("Задание 5:")
logEvent([new Date(), LogLevel.INFO, "Система запущена"]);

//6. Тип безопасных API-ответов
enum HttpStatus {
    OK = 200,
    BAD_REQUEST = 400,
    UNAUTHORIZED = 401,
    NOT_FOUND = 404,
    INTERNAL_SERVER_ERROR = 500
}

type ApiResponse<T> = [HttpStatus, T | null, string?];

function success<T>(data: T): ApiResponse<T> {
    return [HttpStatus.OK, data]; 
}

function error(message: string, status: HttpStatus): ApiResponse<null> {
    return [status, null, message]; 
}
console.log("Задание 6:")
const res1 = success({ user: "Андрей" });
console.log(res1); 

const res2 = error("Не найдено", HttpStatus.NOT_FOUND);
console.log(res2); 
// инкапсуляция any generic-фабрика


