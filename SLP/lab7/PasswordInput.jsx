import React, { useState } from 'react';
import PropTypes from 'prop-types';
import InputField from './InputField';

const PasswordInput = ({ label, name, value, error, onChange }) => {
  const [showPassword, setShowPassword] = useState(false);

  return (
    <div className="form-group">
      <label htmlFor={name}>{label}</label>
      <div className="password-container">
        <InputField
          type={showPassword ? 'text' : 'password'}
          name={name}
          value={value}
          error={error}
          onChange={onChange}
        />
        <button
          type="button"
          className="toggle-password"
          onClick={() => setShowPassword(!showPassword)}
        >
          {showPassword ? '🙈' : '👁️'}
        </button>
      </div>
    </div>
  );
};

PasswordInput.propTypes = {
  label: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  value: PropTypes.string.isRequired,
  error: PropTypes.string,
  onChange: PropTypes.func.isRequired
};

export default PasswordInput;