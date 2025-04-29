import { useSelector, useDispatch } from "react-redux";
import { RootState } from "../redux/store";
import { increment, decrement, reset } from "../redux/actions";
import { Button } from "./Button";
import styles from "./Counter.module.css";

export const Counter: React.FC = () => {
  const count = useSelector((state: RootState) => state.counter.count);
  const dispatch = useDispatch();

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Count: {count}</h2>
      <div className={styles.buttonsWrapper}>
        <Button variant="primary" onClick={() => dispatch(increment())}>
          +
        </Button>
        <Button variant="danger" onClick={() => dispatch(decrement())}>
          -
        </Button>
        <Button variant="reset" onClick={() => dispatch(reset())}>
          Reset
        </Button>
      </div>
    </div>
  );
};