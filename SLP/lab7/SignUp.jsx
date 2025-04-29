import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import InputField from '../AuthForm/InputField';
import PasswordInput from '../AuthForm/PasswordInput';
import { validateName, validateEmail, validatePassword } from '../../utils/validators';
import './signup.css';

const SignUp = () => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  });
  const [errors, setErrors] = useState({});
  const [success, setSuccess] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const validateForm = () => {
    const newErrors = {
      name: validateName(formData.name),
      email: validateEmail(formData.email),
      password: validatePassword(formData.password),
      confirmPassword: formData.password === formData.confirmPassword 
        ? '' 
        : 'Пароли не совпадают'
    };
    setErrors(newErrors);
    return Object.values(newErrors).every(error => !error);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (validateForm()) {
      const users = JSON.parse(localStorage.getItem('users') || '[]');
      
      if (users.some(u => u.email === formData.email)) {
        setErrors({ email: 'Пользователь с таким email уже существует' });
        return;
      }

      const newUser = {
        name: formData.name,
        email: formData.email,
        password: formData.password
      };

      users.push(newUser);
      localStorage.setItem('users', JSON.stringify(users));
      setSuccess(true);
      setTimeout(() => {
        setSuccess(false);
        setFormData({ name: '', email: '', password: '', confirmPassword: '' });
      }, 3000);
    }
  };

  return (
    <div className="auth-container signup-container">
      <h2>Регистрация</h2>
      {success && <div className="success-message">Успешная регистрация!</div>}
      
      <form className="signup-form" onSubmit={handleSubmit}>
        <InputField
          label="Имя"
          type="text"
          name="name"
          value={formData.name}
          error={errors.name}
          onChange={handleChange}
        />
        
        <InputField
          label="Email"
          type="email"
          name="email"
          value={formData.email}
          error={errors.email}
          onChange={handleChange}
        />
        
        <PasswordInput
          label="Пароль"
          name="password"
          value={formData.password}
          error={errors.password}
          onChange={handleChange}
        />
        
        <PasswordInput
          label="Подтвердите пароль"
          name="confirmPassword"
          value={formData.confirmPassword}
          error={errors.confirmPassword}
          onChange={handleChange}
        />
        
        <button type="submit">Зарегистрироваться</button>
      </form>
      
      <div className="links">
        Уже есть аккаунт? <Link to="/sign-in">Войти</Link>
      </div>
    </div>
  );
};

export default SignUp;