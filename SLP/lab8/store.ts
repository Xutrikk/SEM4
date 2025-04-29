import { createStore, combineReducers } from "redux";
import { counterReducer } from "./reducer";
import { CounterState } from "./types";

export interface RootState {
  counter: CounterState;
}

const rootReducer = combineReducers({
  counter: counterReducer,
});

export const store = createStore(rootReducer);