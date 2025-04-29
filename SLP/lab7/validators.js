export const validateName = (name) => {
    if (!name) return 'Имя обязательно';
    if (!/^[a-zA-Zа-яА-Я\s]{2,50}$/.test(name)) return 'Некорректное имя';
    return '';
  };
  
export const validateEmail = (email) => {
  if (!email) return 'Email обязателен';
  if (/\s/.test(email)) return 'Email не должен содержать пробелы';
  if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(email)) 
    return 'Некорректный email';
  return '';
};

export const validatePassword = (password) => {
  if (!password) return 'Пароль обязателен';
  if (password.length < 8) return 'Минимум 8 символов';
  if (!/[A-Z]/.test(password)) return 'Должна быть хотя бы одна заглавная буква';
  if (!/[0-9]/.test(password)) return 'Должна быть хотя бы одна цифра';
  return '';
};