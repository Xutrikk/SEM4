var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
//1. Система управления пользователями
var BaseUser = /** @class */ (function () {
    function BaseUser(id, name) {
        this.id = id;
        this.name = name;
    }
    return BaseUser;
}());
var Admin = /** @class */ (function (_super) {
    __extends(Admin, _super);
    function Admin() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Admin.prototype.getRole = function () {
        return "Admin";
    };
    Admin.prototype.getPermissions = function () {
        return ["Просмотр контента",
            "Добавление комментариев",
            "Удаление комментариев",
            "Управление пользователями"];
    };
    return Admin;
}(BaseUser));
var Guest = /** @class */ (function (_super) {
    __extends(Guest, _super);
    function Guest() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Guest.prototype.getRole = function () {
        return "Guest";
    };
    Guest.prototype.getPermissions = function () {
        return ["Просмотр контента"];
    };
    return Guest;
}(BaseUser));
var User = /** @class */ (function (_super) {
    __extends(User, _super);
    function User() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    User.prototype.getRole = function () {
        return "User";
    };
    User.prototype.getPermissions = function () {
        return ["Просмотр контента", "Добавление комментариев"];
    };
    return User;
}(BaseUser));
console.log("Задание 1:");
var admin = new Admin(52, "Огник");
console.log(admin.getPermissions());
var guest = new Guest(42, "Симка");
console.log(guest.getPermissions());
var user = new User(55, "Хутрик");
console.log(user.getPermissions());
var HTMLReport = /** @class */ (function () {
    function HTMLReport(title, content) {
        this.title = title;
        this.content = content;
    }
    HTMLReport.prototype.getGenerate = function () {
        return "<h1>".concat(this.title, "</h1><p>").concat(this.content, "</p>");
    };
    return HTMLReport;
}());
var JSONReport = /** @class */ (function () {
    function JSONReport(title, content) {
        this.title = title;
        this.content = content;
    }
    JSONReport.prototype.getGenerate = function () {
        return "title: ".concat(this.title, ", content: ").concat(this.content);
    };
    return JSONReport;
}());
console.log("Задание 2:");
var report1 = new HTMLReport("Отчет 1", "Содержание отчета");
console.log(report1.getGenerate());
var report2 = new JSONReport("Отчет 2", "Содержание отчета");
console.log(report2.getGenerate());
//3. Обобщенный кеш данных
var DataCache = /** @class */ (function () {
    function DataCache() {
        this.items = {};
    }
    DataCache.prototype.add = function (key, value, ttl) {
        var expiresAt = Date.now() + ttl;
        this.items[key] = { value: value, expiresAt: expiresAt };
    };
    DataCache.prototype.get = function (key) {
        var item = this.items[key];
        if (!item || Date.now() > item.expiresAt) {
            delete this.items[key];
            return null;
        }
        return item.value;
    };
    DataCache.prototype.clearExpired = function () {
        var _this = this;
        var now = Date.now();
        Object.keys(this.items).forEach(function (key) {
            if (now > _this.items[key].expiresAt) {
                delete _this.items[key];
            }
        });
    };
    return DataCache;
}());
console.log("Задание 3:");
var cache = new DataCache();
cache.add("price", 100, 5000);
console.log(cache.get("price")); // 100
setTimeout(function () { return console.log(cache.get("price")); }, 6000); // null
//4. Дженерик-фабрика объектов
function createInstance(cls) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    return new (cls.bind.apply(cls, __spreadArray([void 0], args, false)))();
}
var Product = /** @class */ (function () {
    function Product(name, price) {
        this.name = name;
        this.price = price;
    }
    return Product;
}());
console.log("Задание 4:");
var p = createInstance(Product, "Телефон", 50000);
console.log(p); // Product { name: "Телефон", price: 50000 }
//5. Логирование событий с кортежами
var LogLevel;
(function (LogLevel) {
    LogLevel["INFO"] = "INFO";
    LogLevel["WARNING"] = "WARNING";
    LogLevel["ERROR"] = "ERROR";
})(LogLevel || (LogLevel = {}));
function logEvent(event) {
    var timestamp = event[0], level = event[1], message = event[2];
    console.log("[".concat(timestamp.toISOString(), "] [").concat(level, "]: ").concat(message));
}
console.log("Задание 5:");
logEvent([new Date(), LogLevel.INFO, "Система запущена"]);
//6. Тип безопасных API-ответов
var HttpStatus;
(function (HttpStatus) {
    HttpStatus[HttpStatus["OK"] = 200] = "OK";
    HttpStatus[HttpStatus["BAD_REQUEST"] = 400] = "BAD_REQUEST";
    HttpStatus[HttpStatus["UNAUTHORIZED"] = 401] = "UNAUTHORIZED";
    HttpStatus[HttpStatus["NOT_FOUND"] = 404] = "NOT_FOUND";
    HttpStatus[HttpStatus["INTERNAL_SERVER_ERROR"] = 500] = "INTERNAL_SERVER_ERROR";
})(HttpStatus || (HttpStatus = {}));
function success(data) {
    return [HttpStatus.OK, data];
}
function error(message, status) {
    return [status, null, message];
}
console.log("Задание 6:");
var res1 = success({ user: "Андрей" });
console.log(res1);
var res2 = error("Не найдено", HttpStatus.NOT_FOUND);
console.log(res2);
