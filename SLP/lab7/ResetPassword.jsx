import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import InputField from '../AuthForm/InputField';
import { validateEmail } from '../../utils/validators';
import './reset.css';

const ResetPassword = () => {
  const [email, setEmail] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const generateSecurePassword = () => {
    const uppercase = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const numbers = '0123456789';
    const allChars = uppercase + 'abcdefghijklmnopqrstuvwxyz' + numbers + '!@#$%^&*';
    
    let passwordArray = Array(12);
    
    let uppercasePos, numberPos;
    do {
      uppercasePos = Math.floor(Math.random() * 12);
      numberPos = Math.floor(Math.random() * 12);
    } while (uppercasePos === numberPos);
  
    passwordArray[uppercasePos] = uppercase[Math.floor(Math.random() * uppercase.length)];
    passwordArray[numberPos] = numbers[Math.floor(Math.random() * numbers.length)];
  
    for (let i = 0; i < 12; i++) {
      if (!passwordArray[i]) {
        passwordArray[i] = allChars[Math.floor(Math.random() * allChars.length)];
      }
    }
  
    for (let i = passwordArray.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [passwordArray[i], passwordArray[j]] = [passwordArray[j], passwordArray[i]];
    }
  
    return passwordArray.join('');
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const emailError = validateEmail(email);
    setError(emailError);

    if (!emailError) {
      const users = JSON.parse(localStorage.getItem('users') || '[]');
      const userIndex = users.findIndex(u => u.email === email);
      
      if (userIndex === -1) {
        setError('Пользователь с таким email не найден');
        return;
      }

      const newPassword = generateSecurePassword();
      users[userIndex].password = newPassword;
      
      localStorage.setItem('users', JSON.stringify(users));
      setSuccess(`Новый пароль отправлен на ${email}: ${newPassword}`);
      setTimeout(() => {
        setSuccess('');
        setEmail('');
      }, 5000);
    }
  };

  return (
    <div className="auth-container reset-form">
      <h2>Восстановление пароля</h2>
      {success && <div className="success-message">{success}</div>}
      
      <form onSubmit={handleSubmit}>
        <InputField
          label="Email"
          type="email"
          name="email"
          value={email}
          error={error}
          onChange={(e) => setEmail(e.target.value)}
        />
        
        <button type="submit">Сбросить пароль</button>
      </form>
      
      <div className="links">
        <Link to="/sign-in">Войти</Link>
      </div>
    </div>
  );
};

export default ResetPassword;