import { useState } from "react";
import Button from "./Button";
import "./App.css";

const Counter = () => {
  const [count, setCount] = useState(0);

  return (
    <div className="counter-container">
      <h1 className="counter-value">{count}</h1>
      <div className="buttons-container">
        <Button 
          title="Increase" 
          onClick={() => setCount(count + 1)} 
          disabled={count === 5} 
        />
        <Button 
          title="Reset" 
          onClick={() => setCount(0)} 
          disabled={count === 0} 
        />
      </div>
    </div>
  );
};

export default Counter;
