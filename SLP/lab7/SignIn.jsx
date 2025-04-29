import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import PasswordInput from '../AuthForm/PasswordInput';
import InputField from '../AuthForm/InputField';
import { validateEmail, validatePassword } from '../../utils/validators';
import './signin.css';

const SignIn = () => {
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });
  const [errors, setErrors] = useState({});
  const [success, setSuccess] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const newErrors = {
      email: validateEmail(formData.email),
      password: validatePassword(formData.password)
    };
    
    setErrors(newErrors);
    
    if (Object.values(newErrors).every(error => !error)) {
      const users = JSON.parse(localStorage.getItem('users') || '[]');
      const user = users.find(u => u.email === formData.email);
      
      if (user && user.password === formData.password) {
        setSuccess(true);
        setTimeout(() => {
          setSuccess(false);
          setFormData({ email: '', password: '' });
        }, 3000);
      } else {
        setErrors({ ...errors, auth: 'Неверные учетные данные' });
      }
    }
  };

  return (
    <div className="auth-container">
      <h2>Вход</h2>
      {success && <div className="success-message">Авторизация успешна!</div>}
      {errors.auth && <div className="error">{errors.auth}</div>}

      <form onSubmit={handleSubmit}>
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
        
        <button type="submit">Войти</button>
      </form>
      
      <div className="links">
        <Link to="/sign-up">Создать аккаунт</Link> <br />
        <Link to="/reset-password">Забыли пароль?</Link>
      </div>
    </div>
  );
};

export default SignIn;