import { Counter } from "./components/Counter";
import styles from "./App.module.css";

export const App = () => (
  <div className={styles.app}>
    <Counter />
  </div>
);

export default App;