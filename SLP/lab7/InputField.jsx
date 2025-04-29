import React from 'react';
import PropTypes from 'prop-types';

const InputField = ({ label, type, name, value, error, onChange }) => (
  <div className="form-group">
    <label htmlFor={name}>{label}</label>
    <input
      id={name}
      type={type}
      name={name}
      value={value}
      onChange={onChange}
      className={error ? 'input-error' : ''}
    />
    {error && <span className="error">{error}</span>}
  </div>
);

InputField.propTypes = {
  label: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  value: PropTypes.string.isRequired,
  error: PropTypes.string,
  onChange: PropTypes.func.isRequired
};

export default InputField;