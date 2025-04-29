import React from "react";
import styles from "./Button.module.css";

interface ButtonProps {
  onClick: () => void;
  children: React.ReactNode;
  variant?: "primary" | "danger" | "reset";
}

export const Button: React.FC<ButtonProps> = ({ 
  onClick, 
  children, 
  variant = "primary" 
}) => (
  <button
    onClick={onClick}
    className={`${styles.button} ${styles[variant]}`}
  >
    {children}
  </button>
);