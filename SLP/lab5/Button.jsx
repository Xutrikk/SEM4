import PropTypes from "prop-types";

const Button = ({ title, onClick, disabled }) => {
  return (
    <button className="btn" onClick={onClick} disabled={disabled}>
      {title}
    </button>
  );
};

Button.propTypes = {
  title: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
  disabled: PropTypes.bool
};

Button.defaultProps = {
  disabled: false
};

export default Button;
