import React, { useState, useEffect, useCallback } from "react";
import Display from "./Display";
import Button from "./Button";
import History from "./History";

const buttons = [
  "C", "⌫", "±", "/",
  "7", "8", "9", "*",
  "4", "5", "6", "-",
  "1", "2", "3", "+",
  "0", ".", "="
];

const safeEval = (expression: string): number => {
  if (expression.includes('/0')) throw new Error("Деление на ноль");
  return Function(`'use strict'; return (${expression})`)();
}

const Calculator: React.FC = () => {
  const [expression, setExpression] = useState<string>("");
  const [history, setHistory] = useState<string[]>([]);
  const [theme, setTheme] = useState<"light" | "dark">("light");

  const handleClick = useCallback((value: string) => {
    if (expression === "Ошибка" && value !== "C") {
      setExpression("");
    }
  
    if (value === "=") {
      try {
        if (expression === "") return;
        
        if (/(\/0)(?!\.)/.test(expression)) {
          throw new Error("Деление на ноль");
        }
        
        const result = safeEval(expression);
        setHistory(prev => [...prev.slice(-4), `${expression} = ${result}`]);
        setExpression(result.toString());
      } catch (e) {
        setExpression(e instanceof Error ? e.message : "Ошибка");
      }
      return;
    }
  
    if (value === "C") {
      setExpression("");
      return;
    }
  
    if (value === "⌫") {
      setExpression(prev => prev.slice(0, -1));
      return;
    }
  
    if (value === "±") {
      setExpression(prev => {
        if (prev === "") return prev;
  
        const parts = prev.split(/([+*-/])/).filter(p => p !== "");
        let lastOperatorIndex = -1;
        let lastOperator = "";
  
        for (let i = parts.length - 2; i >= 0; i--) {
          if (["+", "-", "*", "/"].includes(parts[i])) {
            lastOperatorIndex = i;
            lastOperator = parts[i];
            break;
          }
        }
  
        let numberPart;
        if (lastOperatorIndex === -1) {
          numberPart = parts.join("");
        } else {
          numberPart = parts.slice(lastOperatorIndex + 1).join("");
        }
  
        if (!numberPart || isNaN(Number(numberPart))) return prev;
  
        const newNumber = (-parseFloat(numberPart)).toString();
  
        if (lastOperatorIndex === -1) {
          return newNumber;
        } else {
          const newParts = [...parts];
          if (lastOperator === "+" || lastOperator === "-") {
            newParts[lastOperatorIndex] = lastOperator === "+" ? "-" : "+";
            newParts[lastOperatorIndex + 1] = Math.abs(parseFloat(numberPart)).toString();
          } else {
            newParts[lastOperatorIndex + 1] = newNumber;
          }
          return newParts.join("");
        }
      });
      return;
    }
  
    setExpression(prev => {
      if (prev.length >= 16) return prev;
      const lastChar = prev.slice(-1);
      const operators = ["+", "-", "*", "/"];
      const currentNumber = prev.split(/[+*-/]/).pop() || "";
  
      if (/\d/.test(value)) {
        if (currentNumber === "0" && value !== ".") {
          return prev.slice(0, -1) + value;
        }
      }
  
      if (value === "." && currentNumber.includes(".")) return prev;
  
      if (operators.includes(value) && prev === "" && value !== "-") return prev;
  
      if (operators.includes(value) && operators.includes(lastChar)) {
        return prev.slice(0, -1) + value;
      }
  
      if (value === "." && (prev === "" || operators.includes(lastChar))) {
        return prev + "0.";
      }
      if (value === "0" && currentNumber === "0") return prev;
  
      return prev + value;
    });
  }, [expression]);
  
  const handleKeyPress = useCallback((event: KeyboardEvent) => {
    const { key } = event;
  
    if (/[0-9+*-/.]/.test(key)) {
      handleClick(key);
    } else if (key === "Enter") {
      handleClick("=");
    } else if (key === "Backspace") {
      handleClick("⌫");
    } else if (key === "Escape") {
      handleClick("C");
    }
    else if (event.key === '_' && event.shiftKey) { 
      handleClick("±");
    }
  }, [handleClick]);  
 
  useEffect(() => {
    window.addEventListener("keydown", handleKeyPress);
    return () => {
      window.removeEventListener("keydown", handleKeyPress);
    };
  }, [handleKeyPress]); 

  const toggleTheme = () => {
    setTheme(theme === "light" ? "dark" : "light");
  };

  return (
    <div className={`calculator ${theme}`}>
      <button onClick={toggleTheme} className="theme-toggle">
        {theme === "light" ? "🌙" : "☀️"}
      </button>
      <Display value={expression} />
      <div className="buttons">
        {buttons.map((btn) => (
          <Button key={btn} label={btn} onClick={() => handleClick(btn)} />
        ))}
      </div>
      <History history={history} />
    </div>
  );
};

export default Calculator;
